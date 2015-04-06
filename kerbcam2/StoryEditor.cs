using System;
using UnityEngine;

namespace kerbcam2 {
    class StoryEditor {
        private Story story;
        private Vector2 timeTableScrollPos;
        private IItemEditor itemEditor = null;
        private static float HEADER_HEIGHT = 40;
        private static float TIME_WIDTH = 60;
        private static float FIRSTCOL_WIDTH = 70;
        private static float ADD_WIDTH = 20;

        public StoryEditor(Story story) {
            this.story = story;
        }

        public void DrawUI() {
            using (GU.Vertical()) {
                DrawTimeTable();
                if (itemEditor != null) {
                    if (itemEditor.DrawUI()) {
                        itemEditor = null;
                    }
                }
            }
        }

        private void DrawTimeTable() {
            using (GU.ScrollView(ref timeTableScrollPos)) {
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
                            TimeKey key = timeline[i];
                            string timekeyLabel = string.Format("{0}\n{1}",
                                    key.GetTimeFormatted(), key.name);
                            if (GUILayout.Button(timekeyLabel, Styles.marginlessButton, headerHeight, timeWidth)) {
                                itemEditor = timeline.GetEditorForTimeKey(key.id);
                            }
                            if (GUILayout.Button("+", Styles.marginlessButton, headerHeight, addWidth)) {
                                TimeKey newTime;
                                try {
                                    TimeKey nextTime = timeline[i + 1];
                                    newTime = new TimeKey("", (key.seconds + nextTime.seconds) / 2);
                                } catch (ArgumentOutOfRangeException) {
                                    newTime = new TimeKey("", key.seconds + 1);
                                }
                                story.Timeline.NewTimeKey(newTime);
                            }
                        }
                    }
                    // Operation rows.
                    foreach (IOperation op in story.EnumerateOperations()) {
                        using (GU.Horizontal()) {
                            if (GUILayout.Button(op.Name, Styles.marginlessButton, firstColWidth)) {
                                itemEditor = op.MakeEditor();
                            }
                            GUILayout.Space(ADD_WIDTH);
                            for (int i = 0; i < timeline.Count; i++) {
                                TimeKey time = timeline[i];
                                IOperationKey opKey;
                                if (op.TryGetKey(time.id, out opKey)) {
                                    if (GUILayout.Button(opKey.Name, Styles.marginlessButton, timeWidth)) {
                                        itemEditor = opKey.MakeEditor();
                                    }
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
