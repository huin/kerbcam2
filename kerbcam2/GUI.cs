using KSPPluginFramework;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

/// Generic GUI utilities.
namespace kerbcam2 {
    /// <summary>
    /// GUI layout helper class.
    /// </summary>
    /// Provides methods that start a GUI layout section that work as
    /// disposables to close the section at the end of the `using` scope.
    static class GU {
        #region Horizontal
        public static GUICloser Horizontal() {
            GUILayout.BeginHorizontal();
            return new GUICloser(GUILayout.EndHorizontal);
        }
        public static GUICloser Horizontal(params GUILayoutOption[] options) {
            GUILayout.BeginHorizontal(options);
            return new GUICloser(GUILayout.EndHorizontal);
        }
        public static GUICloser Horizontal(GUIStyle style, params GUILayoutOption[] options) {
            GUILayout.BeginHorizontal(style, options);
            return new GUICloser(GUILayout.EndHorizontal);
        }
        public static GUICloser Horizontal(string text, GUIStyle style, params GUILayoutOption[] options) {
            GUILayout.BeginHorizontal(text, style, options);
            return new GUICloser(GUILayout.EndHorizontal);
        }
        public static GUICloser Horizontal(Texture image, GUIStyle style, params GUILayoutOption[] options) {
            GUILayout.BeginHorizontal(image, style, options);
            return new GUICloser(GUILayout.EndHorizontal);
        }
        public static GUICloser Horizontal(GUIContent content, GUIStyle style, params GUILayoutOption[] options) {
            GUILayout.BeginHorizontal(content, style, options);
            return new GUICloser(GUILayout.EndHorizontal);
        }
        #endregion

        #region Vertical
        public static GUICloser Vertical() {
            GUILayout.BeginVertical();
            return new GUICloser(GUILayout.EndVertical);
        }
        public static GUICloser Vertical(params GUILayoutOption[] options) {
            GUILayout.BeginVertical(options);
            return new GUICloser(GUILayout.EndVertical);
        }
        public static GUICloser Vertical(GUIStyle style, params GUILayoutOption[] options) {
            GUILayout.BeginVertical(style, options);
            return new GUICloser(GUILayout.EndVertical);
        }
        public static GUICloser Vertical(string text, GUIStyle style, params GUILayoutOption[] options) {
            GUILayout.BeginVertical(text, style, options);
            return new GUICloser(GUILayout.EndVertical);
        }
        public static GUICloser Vertical(Texture image, GUIStyle style, params GUILayoutOption[] options) {
            GUILayout.BeginVertical(image, style, options);
            return new GUICloser(GUILayout.EndVertical);
        }
        public static GUICloser Vertical(GUIContent content, GUIStyle style, params GUILayoutOption[] options) {
            GUILayout.BeginVertical(content, style, options);
            return new GUICloser(GUILayout.EndVertical);
        }
        #endregion

        #region ScrollView
        public static GUICloser ScrollView(ref Vector2 scrollPosition, params GUILayoutOption[] options) {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, options);
            return new GUICloser(GUILayout.EndScrollView);
        }
        public static GUICloser ScrollView(ref Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options) {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, options);
            return new GUICloser(GUILayout.EndScrollView);
        }
        public static GUICloser ScrollView(ref Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options) {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, horizontalScrollbar, verticalScrollbar, options);
            return new GUICloser(GUILayout.EndScrollView);
        }
        public static GUICloser ScrollView(ref Vector2 scrollPosition, GUIStyle style) {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, style);
            return new GUICloser(GUILayout.EndScrollView);
        }
        public static GUICloser ScrollView(ref Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options) {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, style, options);
            return new GUICloser(GUILayout.EndScrollView);
        }
        public static GUICloser ScrollView(ref Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options) {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, options);
            return new GUICloser(GUILayout.EndScrollView);
        }
        public static GUICloser ScrollView(ref Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options) {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background, options);
            return new GUICloser(GUILayout.EndScrollView);
        }
        #endregion

        #region Area
        public static GUICloser Area(Rect screenRect) {
            GUILayout.BeginArea(screenRect);
            return new GUICloser(GUILayout.EndArea);
        }
        public static GUICloser Area(Rect screenRect, string text) {
            GUILayout.BeginArea(screenRect, text);
            return new GUICloser(GUILayout.EndArea);
        }
        public static GUICloser Area(Rect screenRect, Texture image) {
            GUILayout.BeginArea(screenRect, image);
            return new GUICloser(GUILayout.EndArea);
        }
        public static GUICloser Area(Rect screenRect, GUIContent content) {
            GUILayout.BeginArea(screenRect, content);
            return new GUICloser(GUILayout.EndArea);
        }
        public static GUICloser Area(Rect screenRect, string text, GUIStyle style) {
            GUILayout.BeginArea(screenRect, text, style);
            return new GUICloser(GUILayout.EndArea);
        }
        public static GUICloser Area(Rect screenRect, Texture image, GUIStyle style) {
            GUILayout.BeginArea(screenRect, image, style);
            return new GUICloser(GUILayout.EndArea);
        }
        public static GUICloser Area(Rect screenRect, GUIContent content, GUIStyle style) {
            GUILayout.BeginArea(screenRect, content, style);
            return new GUICloser(GUILayout.EndArea);
        }
        #endregion
    }

    /// <summary>
    /// Used to call a function, specifically to close a GUI layout.
    /// </summary>
    struct GUICloser : IDisposable {
        internal delegate void CloserFn();

        private CloserFn closerFn;

        internal GUICloser(CloserFn closerFn) {
            this.closerFn = closerFn;
        }

        void IDisposable.Dispose() {
            closerFn();
        }
    }

    /// <summary>
    /// Container for static styles.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class Styles : MonoBehaviourExtended {
        internal override void Awake() {
            SkinsLibrary.OnSkinChanged += updateStyles;
        }

        private const int winButtonSize = 25;
        public static GUIStyle windowButton;
        public static GUIStyle tableButton;
        public static GUIStyle tableLabel;
        public static GUIStyle invalidTextField;
        public static GUIStyle invalidValueLabel;

        private static void updateStyles() {
            var activeTxt = MakeConstantTexture(new Color(1f, 1f, 1f, 0.3f));
            var litTxt = MakeConstantTexture(new Color(1f, 1f, 1f, 0.2f));
            var normalTxt = MakeConstantTexture(new Color(1f, 1f, 1f, 0.1f));
            var clearTxt = MakeConstantTexture(Color.clear);

            GUISkin skin = SkinsLibrary.CurrentSkin;

            windowButton = new GUIStyle(skin.button);
            windowButton.fixedHeight = winButtonSize;
            windowButton.fixedWidth = winButtonSize;
            windowButton.alignment = TextAnchor.LowerCenter;
            windowButton.border = new RectOffset(1, 1, 1, 1);
            windowButton.margin = new RectOffset(2, 2, 8, 2);
            windowButton.padding = new RectOffset(2, 2, 2, 2);
            windowButton.active.background = activeTxt;
            windowButton.focused.background = litTxt;
            windowButton.hover.background = litTxt;
            windowButton.normal.background = normalTxt;

            tableButton = new GUIStyle(skin.button);
            tableButton.margin = new RectOffset(0, 0, 0, 0);
            tableButton.clipping = TextClipping.Clip;
            tableButton.alignment = TextAnchor.UpperLeft;
            tableButton.fontSize = 12;

            tableLabel = new GUIStyle(skin.label);
            tableLabel.margin = new RectOffset(0, 0, 0, 0);
            tableLabel.clipping = TextClipping.Clip;
            tableLabel.alignment = TextAnchor.UpperLeft;
            tableLabel.fontSize = 12;

            invalidTextField = new GUIStyle(skin.textField);
            invalidTextField.active.textColor = Color.red;
            invalidTextField.focused.textColor = Color.red;
            invalidTextField.normal.textColor = Color.red;

            invalidValueLabel = new GUIStyle(skin.label);
            invalidValueLabel.normal.textColor = Color.red;
        }

        private static Texture2D MakeConstantTexture(Color fill) {
            const int size = 32;
            Texture2D txt = new Texture2D(size, size);
            for (int row = 0; row < size; row++) {
                for (int col = 0; col < size; col++) {
                    txt.SetPixel(col, row, fill);
                }
            }
            txt.Apply();
            txt.Compress(false);
            return txt;
        }
    }

    /// <summary>
    /// GUI Window resizing widget.
    /// </summary>
    class WindowResizer {
        private static GUIContent gcDrag = new GUIContent("\u25E2");

        private bool isResizing = false;
        private Rect resizeStart = new Rect();

        private Vector2 minSize;
        private Rect position;

        public WindowResizer(Rect windowRect, Vector2 minSize) {
            this.position = windowRect;
            this.minSize = minSize;
        }

        public Rect Position {
            get { return position; }
            set { position = value; }
        }

        public float Width {
            get { return position.width; }
            set { position.width = value; }
        }

        public float Height {
            get { return position.height; }
            set { position.height = value; }
        }

        public float MinWidth {
            get { return minSize.x; }
            set { minSize.x = value; }
        }

        public float MinHeight {
            get { return minSize.y; }
            set { minSize.y = value; }
        }

        // Helpers to return GUILayoutOptions for GUILayout.Window.

        public GUILayoutOption LayoutMinWidth() {
            return GUILayout.MinWidth(minSize.x);
        }

        public GUILayoutOption LayoutMinHeight() {
            return GUILayout.MinHeight(minSize.y);
        }

        // Originally from the following URL and modified:
        // http://answers.unity3d.com/questions/17676/guiwindow-resize-window.html
        public void HandleResize() {
            Vector2 mouse = GUIUtility.ScreenToGUIPoint(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));

            Rect r = GUILayoutUtility.GetRect(gcDrag, Styles.windowButton);
            if (Event.current.type == EventType.mouseDown && r.Contains(mouse)) {
                isResizing = true;
                resizeStart = new Rect(mouse.x, mouse.y, position.width, position.height);
                //Event.current.Use();  // the GUI.Button below will eat the event, and this way it will show its active state
            } else if (Event.current.type == EventType.mouseUp && isResizing) {
                isResizing = false;
            } else if (!Input.GetMouseButton(0)) {
                // if the mouse is over some other window we won't get an event, this just kind of circumvents that by checking the button state directly
                isResizing = false;
            } else if (isResizing) {
                position.width = Mathf.Max(minSize.x, resizeStart.width + (mouse.x - resizeStart.x));
                position.height = Mathf.Max(minSize.y, resizeStart.height + (mouse.y - resizeStart.y));
                position.xMax = Mathf.Min(Screen.width, position.xMax);  // modifying xMax affects width, not x
                position.yMax = Mathf.Min(Screen.height, position.yMax);  // modifying yMax affects height, not y
            }

            GUI.Button(r, gcDrag, Styles.windowButton);
        }
    }

    /// <summary>
    /// Simple wrapper for a KSP stock toolbar button.
    /// </summary>
    class SimpleStockToolbarButton : IDisposable {
        private ApplicationLauncherButton button;

        /// <summary>
        /// Create and register a button on KSP's stock toolbar.
        /// </summary>
        /// 
        /// Dispose() must be called to unregister the button when no longer
        /// wanted.
        /// 
        /// <param name="onTrue">Delegate called when toggled on.</param>
        /// <param name="onFalse">Delegate called when toggled off.</param>
        /// <param name="scenes">Scenes to enable the button in.</param>
        /// <param name="resourceName">Filename of the button image within the
        /// "Resources" directory of the mod.</param>
        public SimpleStockToolbarButton(
            RUIToggleButton.OnTrue onTrue,
            RUIToggleButton.OnFalse onFalse,
            ApplicationLauncher.AppScenes scenes,
            string resourceName) {
            var texture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                Path.Combine("Resources", resourceName))));
            button = ApplicationLauncher.Instance.AddModApplication(
                onTrue, onFalse, // on true/false
                null, null, // on hover
                null, null, // on enable
                scenes,
                texture);
        }

        public void Dispose() {
            ApplicationLauncher.Instance.RemoveModApplication(button);
            button = null;
        }
    }
}
