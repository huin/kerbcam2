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
                            story.Timeline.NewTimeKey(new TimeKey("", 0));
                        }
                        for (int i = 0; i < timeline.Count; i++) {
                            TimeKey key = timeline[i];
                            string timekeyLabel = string.Format("{0}\n{1}",
                                    key.GetTimeFormatted(), key.name);
                            if (GUILayout.Button(timekeyLabel, Styles.tableButton, headerHeight, timeWidth)) {
                                itemEditor = timeline.GetEditorForTimeKey(key.id);
                            }
                            if (GUILayout.Button("+", Styles.emptyTableButton, headerHeight, addWidth)) {
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
                            if (GUILayout.Button(op.Name, Styles.tableButton, firstColWidth)) {
                                itemEditor = op.MakeEditor();
                            }
                            GUILayout.Label("", Styles.emptyTableLabel, addWidth);
                            for (int i = 0; i < timeline.Count; i++) {
                                TimeKey time = timeline[i];
                                IOperationKey opKey;
                                if (op.TryGetKey(time.id, out opKey)) {
                                    if (GUILayout.Button(opKey.Name, Styles.tableButton, timeWidth)) {
                                        itemEditor = opKey.MakeEditor();
                                    }
                                } else {
                                    if (GUILayout.Button("+", Styles.emptyTableButton, timeWidth)) {
                                        itemEditor = op.AddKey(time.id).MakeEditor();
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
        }
    }
}
