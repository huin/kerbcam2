using UnityEngine;

namespace kerbcam2 {
    class StoryEditor {
        private Story story;

        public StoryEditor(Story story) {
            this.story = story;
        }

        public void DrawUI() {
            using (GU.Vertical()) {
                // Column headers.
                using (GU.Horizontal()) {
                    foreach (TimeKey timekey in story.Timeline) {
                        string timekeyLabel = string.Format("{0}\n{1}",
                                timekey.GetTimeFormatted(),
                                timekey.name);
                        GUILayout.Button(timekeyLabel,
                            GUILayout.Height(40), GUILayout.Width(60));

                        // When (Event.current.type == EventType.Repaint), capture from
                        // GUILayoutUtility.GetLastRect to know column size/positions
                    }
                }
                // Example rows.
                foreach (IOperation op in story.EnumerateOperations()) {
                    using (GU.Horizontal()) {
                        foreach (TimeKey time in op.EnumerateTimes()) {
                            // TODO: display times in the table
                        }
                    }
                }
            }
        }
    }
}
