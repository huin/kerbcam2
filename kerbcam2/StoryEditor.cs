using System;
using UnityEngine;

namespace kerbcam2 {
    class StoryEditor {
        private readonly Story story;
        private Vector2 timeTableScrollPos;
        private IItemEditor itemEditor = null;
        private const float HEADER_HEIGHT = 40;
        private const float TIME_WIDTH = 60;
        private const float FIRSTCOL_WIDTH = 70;
        private const float ADD_WIDTH = 30;
        private const string EMPTY_CELL_STR = "\u25C6";

        public StoryEditor(Story story) {
            this.story = story;
        }

        public void DrawUI() {
            using (GU.Vertical()) {
                DrawTimeTable();
                if (itemEditor != null) {
                    itemEditor = itemEditor.DrawUI();
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
                        GUILayout.Label("Operation", Styles.tableLabel, headerHeight, firstColWidth);
                        if (GUILayout.Button("+", Styles.emptyTableButton, headerHeight, addWidth)) {
                            story.Timeline.ShiftAll(1);
                            itemEditor = story.Timeline.NewTimeKey("", 0).MakeEditor();
                        }
                        for (int i = 0; i < timeline.Count; i++) {
                            TimeKey key = timeline[i];
                            string timekeyLabel = string.Format("{0}\n{1}",
                                    key.GetTimeFormatted(), key.Name);
                            if (GUILayout.Button(timekeyLabel, CellStyle(key), headerHeight, timeWidth)) {
                                itemEditor = key.MakeEditor();
                            }
                            if (GUILayout.Button("+", Styles.emptyTableButton, headerHeight, addWidth)) {
                                TimeKey newTime;
                                try {
                                    TimeKey nextTime = timeline[i + 1];
                                    newTime = story.Timeline.NewTimeKey("", (key.Seconds + nextTime.Seconds) / 2);
                                } catch (ArgumentOutOfRangeException) {
                                    newTime = story.Timeline.NewTimeKey("", key.Seconds + 1);
                                }
                                itemEditor = newTime.MakeEditor();
                            }
                        }
                    }
                    // Operation rows.
                    foreach (IOperation op in story.EnumerateOperations()) {
                        using (GU.Horizontal()) {
                            if (GUILayout.Button(op.Name, CellStyle(op), firstColWidth)) {
                                itemEditor = op.MakeEditor();
                            }
                            GUILayout.Label("", Styles.emptyTableLabel, addWidth);
                            for (int i = 0; i < timeline.Count; i++) {
                                TimeKey timeKey = timeline[i];
                                IOperationKey opKey;
                                if (op.TryGetKey(timeKey, out opKey)) {
                                    string label = opKey.Name;
                                    if (label == "") {
                                        label = EMPTY_CELL_STR;
                                    }
                                    if (GUILayout.Button(label, CellStyle(opKey), timeWidth)) {
                                        itemEditor = opKey.MakeEditor();
                                    }
                                } else {
                                    if (GUILayout.Button("+", Styles.emptyTableButton, timeWidth)) {
                                        itemEditor = op.AddKey(timeKey).MakeEditor();
                                    }
                                }
                                GUILayout.Label("", Styles.emptyTableLabel, addWidth);
                            }
                        }
                    }
                    // Add operation.
                    if (GUILayout.Button("+", Styles.emptyTableButton, firstColWidth)) {
                        itemEditor = new NewOperationEditor(this.story);
                    }
                }
            }
        }

        private GUIStyle CellStyle(object o) {
            if (itemEditor != null && itemEditor.IsEditing(o)) {
                return Styles.selTableButton;
            } else {
                return Styles.tableButton;
            }
        }

        private class NewOperationEditor : IItemEditor {
            private Story story;

            public NewOperationEditor(Story story) {
                this.story = story;
            }
            public IItemEditor DrawUI() {
                IOperation op = null;
                using (GU.Vertical()) {
                    GUILayout.Label("Choose operation to create.");
                    if (GUILayout.Button("Dummy")) {
                        op = new DummyOperation(story.Timeline, "dummy");
                    }
                    if (GUILayout.Button("Bezier translator")) {
                        op = new BezierTranslator(story.Timeline, "bezier translator");
                    }
                    if (GUILayout.Button("(cancel)", Styles.destructiveButton)) {
                        return null;
                    }
                }
                if (op != null) {
                    story.AddOperation(op);
                    return op.MakeEditor();
                }
                return this;
            }
            public bool IsEditing(object o) {
                return false;
            }
        }
    }
}
