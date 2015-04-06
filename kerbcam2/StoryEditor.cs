using UnityEngine;
using System.Collections.Generic;

namespace kerbcam2 {
    class StoryEditor {
        private Story story;
        private Dictionary<long, float> headers;

        public StoryEditor(Story story) {
            this.story = story;
            this.headers = new Dictionary<long, float>();
        }

        public void DrawUI() {
            bool repaint = Event.current.type == EventType.Repaint;
            if (repaint) {
                headers.Clear();
            }
            using (GU.Vertical()) {
                // Column headers.
                using (GU.Horizontal()) {
                    foreach (TimeKey time in story.Timeline) {
                        string timekeyLabel = string.Format("{0}\n{1}",
                                time.GetTimeFormatted(),
                                time.name);
                        GUILayout.Button(timekeyLabel, Styles.marginlessButton,
                            GUILayout.Height(40), GUILayout.Width(60));
                        if (repaint) {
                            headers.Add(time.id, GUILayoutUtility.GetLastRect().width);
                        }
                    }
                }
                // Example rows.
                foreach (IOperation op in story.EnumerateOperations()) {
                    using (GU.Horizontal()) {
                        foreach (TimeKey time in story.Timeline) {
                            float width;
                            headers.TryGetValue(time.id, out width);
                            string description;
                            if (op.GetTimeKeyDescription(time.id, out description)) {
                                GUILayout.Button(description, Styles.marginlessButton, GUILayout.Width(width));
                            } else {
                                GUILayout.Space(width);
                            }
                        }
                    }
                }
            }
        }
    }
}
