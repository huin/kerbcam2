using KSPPluginFramework;
using UnityEngine;

namespace kerbcam2 {
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    class MainWindow : MonoBehaviourWindow {
        private SimpleStockToolbarButton launcherButton;
        private WindowResizer resizer;

        private Story story;
        private StoryEditor storyEditor;

        #region Lifetime
        internal override void Awake() {
            resizer = new WindowResizer(
                new Rect(20, 100, 600, 500),
                new Vector2(400, 300));
            launcherButton = new SimpleStockToolbarButton(
                delegate() { Visible = true; },
                delegate() { Visible = false; },
                ApplicationLauncher.AppScenes.FLIGHT,
                "icon-36x36.png");

            NewStory();

            // MonoBehaviourWindow settings.
            Visible = false;
            DragEnabled = true;
            ClampToScreen = true;
            TooltipsEnabled = true;
            WindowCaption = "KerbCam2";
            WindowOptions = new GUILayoutOption[]{
                resizer.LayoutMinHeight(),
                resizer.LayoutMinWidth(),
            };
        }

        internal override void OnDestroy() {
            if (launcherButton != null) {
                launcherButton.Dispose();
                launcherButton = null;
            }
        }
        #endregion

        private void NewStory() {
            story = new Story();
            storyEditor = new StoryEditor(story);
            // TODO: Remove these time keys, at least from MainWindow.
            TimeKey tk0 = story.Timeline.NewTimeKey("Start", 0f);
            TimeKey tk1 = story.Timeline.NewTimeKey("", 3.15f);
            TimeKey tk2 = story.Timeline.NewTimeKey("", 7.5f);
            TimeKey tk3 = story.Timeline.NewTimeKey("Engines start", 10.23f);
            TimeKey tk4 = story.Timeline.NewTimeKey("", 12.73f);
            TimeKey tk5 = story.Timeline.NewTimeKey("", 20.28f);

            var dummyOp = new DummyOperation(story.Timeline, "test");
            story.AddOperation(dummyOp);
            dummyOp.AddKey(tk0).Name = "p0";
            dummyOp.AddKey(tk1).Name = "p1";
            dummyOp.AddKey(tk3).Name = "p3";
        }

        internal override void DrawWindow(int id) {
            using (GU.Vertical()) {
                using (GU.Horizontal()) {
                    GUILayoutOption playbackWidth = GUILayout.Width(30);
                    if (GUILayout.Button("\u25a0", Styles.playbackButton, playbackWidth)) {
                        // TODO
                    }
                    if (GUILayout.Button("\u25b6", Styles.playbackButton, playbackWidth)) {
                        // TODO
                    }
                }

                if (storyEditor != null) {
                    storyEditor.DrawUI();
                }

                GUILayout.FlexibleSpace();

                // Bottom row for window controls.
                using (GU.Horizontal()) {
                    GUILayout.FlexibleSpace();
                    // Handle resizing.
                    resizer.Position = WindowRect;
                    resizer.HandleResize();
                    WindowRect = resizer.Position;
                }
            }
        }
    }
}
