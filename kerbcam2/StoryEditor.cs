using UnityEngine;

namespace kerbcam2 {
    class StoryEditor {
        private Story story;

        public StoryEditor(Story story) {
            this.story = story;
        }

        public void DrawUI() {
            using (GU.Horizontal()) {
                foreach (TimeKey timekey in story.Timeline) {
                    string timekeyLabel = string.Format("{0}\n{1}",
                            timekey.GetTimeFormatted(),
                            timekey.name);
                    GUILayout.Button(timekeyLabel,
                        GUILayout.Height(40), GUILayout.Width(60));
                }
            }
        }
    }
}
