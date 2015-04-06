using UnityEngine;
using System.Collections.Generic;

namespace kerbcam2 {
    class StoryEditor {
        private Story story;
        private static float HEADER_HEIGHT = 40;
        private static float TIME_WIDTH = 60;
        private static float FIRSTCOL_WIDTH = 70;
        private static float ADD_WIDTH = 20;

        public StoryEditor(Story story) {
            this.story = story;
        }

        public void DrawUI() {
            // Render the table.
            using (GU.Vertical()) {
                GUILayoutOption headerHeight = GUILayout.Height(HEADER_HEIGHT);
                GUILayoutOption addWidth = GUILayout.Width(ADD_WIDTH);
                GUILayoutOption timeWidth = GUILayout.Width(TIME_WIDTH);
                GUILayoutOption firstColWidth = GUILayout.Width(FIRSTCOL_WIDTH);
                // Column headers from timeline.
                using (GU.Horizontal()) {
                    GUILayout.Label("Operation", Styles.marginlessLabel, headerHeight, firstColWidth);
                    GUILayout.Button("+", Styles.marginlessButton, headerHeight, addWidth);
                    foreach (TimeKey time in story.Timeline) {
                        string timekeyLabel = string.Format("{0}\n{1}",
                                time.GetTimeFormatted(),
                                time.name);
                        GUILayout.Button(timekeyLabel, Styles.marginlessButton, headerHeight, timeWidth);
                        GUILayout.Button("+", Styles.marginlessButton, headerHeight, addWidth);
                    }
                }
                // Operation rows.
                foreach (IOperation op in story.EnumerateOperations()) {
                    using (GU.Horizontal()) {
                        GUILayout.Button(op.Name, Styles.marginlessButton, firstColWidth);
                        GUILayout.Space(ADD_WIDTH);
                        foreach (TimeKey time in story.Timeline) {
                            string description;
                            if (op.GetNameForTimeKey(time.id, out description)) {
                                GUILayout.Button(description, Styles.marginlessButton, timeWidth);
                                GUILayout.Space(ADD_WIDTH);
                            } else {
                                GUILayout.Space(TIME_WIDTH+ADD_WIDTH);
                            }
                        }
                    }
                }
            }
        }
    }
}
