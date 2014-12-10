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
            story.Timeline.NewTimeKey(new TimeKey("Start", 0f));
            story.Timeline.NewTimeKey(new TimeKey("", 3.15f));
            story.Timeline.NewTimeKey(new TimeKey("", 7.5f));
            story.Timeline.NewTimeKey(new TimeKey("Engines start", 10.23f));
            story.Timeline.NewTimeKey(new TimeKey("", 12.73f));
            story.Timeline.NewTimeKey(new TimeKey("", 20.28f));
        }

        internal override void DrawWindow(int id) {
            using (GU.Vertical()) {
                // TODO: GUI components other than story editor.
                if (storyEditor != null) {
                    storyEditor.DrawUI();
                }

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
