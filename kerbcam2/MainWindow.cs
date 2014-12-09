using KSPPluginFramework;
using UnityEngine;

namespace kerbcam2 {
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    class MainWindow : MonoBehaviourWindow {
        private SimpleStockToolbarButton launcherButton;
        private WindowResizer resizer;

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

        internal override void DrawWindow(int id) {
            using (GU.Vertical()) {
                // TODO: Main GUI stuff here. This spacer is a placeholder.
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
