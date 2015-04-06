using System;
using UnityEngine;

namespace kerbcam2 {
    class StoryEditor {
        private Story story;
        private Vector2 tableScrollPos;
        private static float HEADER_HEIGHT = 40;
        private static float TIME_WIDTH = 60;
        private static float FIRSTCOL_WIDTH = 70;
        private static float ADD_WIDTH = 20;

        public StoryEditor(Story story) {
            this.story = story;
        }

        public void DrawUI() {
            using (GU.ScrollView(ref tableScrollPos)) {
                // Render the table.
                using (GU.Vertical()) {
                    GUILayoutOption headerHeight = GUILayout.Height(HEADER_HEIGHT);
                    GUILayoutOption addWidth = GUILayout.Width(ADD_WIDTH);
                    GUILayoutOption timeWidth = GUILayout.Width(TIME_WIDTH);
                    GUILayoutOption firstColWidth = GUILayout.Width(FIRSTCOL_WIDTH);
                    Timeline timeline = story.Timeline;
                    // Column headers from timeline.
                    using (GU.Horizontal()) {
                        GUILayout.Label("Operation", Styles.marginlessLabel, headerHeight, firstColWidth);
                        if (GUILayout.Button("+", Styles.marginlessButton, headerHeight, addWidth)) {
                            story.Timeline.ShiftAll(1);
                            story.Timeline.NewTimeKey(new TimeKey("", 0));
                        }
                        for (int i = 0; i < timeline.Count; i++) {
                            TimeKey time = timeline[i];
                            string timekeyLabel = string.Format("{0}\n{1}",
                                    time.GetTimeFormatted(), time.name);
                            GUILayout.Button(timekeyLabel, Styles.marginlessButton, headerHeight, timeWidth);
                            if (GUILayout.Button("+", Styles.marginlessButton, headerHeight, addWidth)) {
                                TimeKey newTime;
                                try {
                                    TimeKey nextTime = timeline[i + 1];
                                    newTime = new TimeKey("", (time.seconds + nextTime.seconds) / 2);
                                } catch (ArgumentOutOfRangeException) {
                                    newTime = new TimeKey("", time.seconds + 1);
                                }
                                story.Timeline.NewTimeKey(newTime);
                            }
                        }
                    }
                    // Operation rows.
                    foreach (IOperation op in story.EnumerateOperations()) {
                        using (GU.Horizontal()) {
                            GUILayout.Button(op.Name, Styles.marginlessButton, firstColWidth);
                            GUILayout.Space(ADD_WIDTH);
                            for (int i = 0; i < timeline.Count; i++) {
                                TimeKey time = timeline[i];
                                string description;
                                if (op.GetNameForTimeKey(time.id, out description)) {
                                    GUILayout.Button(description, Styles.marginlessButton, timeWidth);
                                    GUILayout.Space(ADD_WIDTH);
                                } else {
                                    GUILayout.Space(TIME_WIDTH + ADD_WIDTH);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
