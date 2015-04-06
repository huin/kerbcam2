using UnityEngine;
using System.Collections.Generic;

namespace kerbcam2 {
    class StoryEditor {
        private Story story;

        public StoryEditor(Story story) {
            this.story = story;
        }

        public void DrawUI() {
            // Render the table.
            using (GU.Vertical()) {
                // Column headers.
                using (GU.Horizontal()) {
                    GUILayout.Label("Operation", Styles.marginlessLabel,
                            GUILayout.Height(40), GUILayout.Width(70));
                    foreach (TimeKey time in story.Timeline) {
                        string timekeyLabel = string.Format("{0}\n{1}",
                                time.GetTimeFormatted(),
                                time.name);
                        GUILayout.Button(timekeyLabel, Styles.marginlessButton,
                            GUILayout.Height(40), GUILayout.Width(60));
                    }
                }
                // Example rows.
                foreach (IOperation op in story.EnumerateOperations()) {
                    using (GU.Horizontal()) {
                        GUILayout.Button(op.Name, Styles.marginlessButton, GUILayout.Width(70));
                        foreach (TimeKey time in story.Timeline) {
                            string description;
                            if (op.GetNameForTimeKey(time.id, out description)) {
                                GUILayout.Button(description, Styles.marginlessButton, GUILayout.Width(60));
                            } else {
                                GUILayout.Space(60);
                            }
                        }
                    }
                }
            }
        }
    }
}
