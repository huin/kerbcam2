using System;
using UnityEngine;

namespace kerbcam2 {
    /// <summary>
    /// Represents an instant in time on a Timeline.
    /// </summary>
    class TimeKey : IComparable {
        private readonly Timeline timeline;
        private string name;
        private float seconds;

        public TimeKey(Timeline timeline, string name, float seconds) {
            this.timeline = timeline;
            this.name = name;
            this.seconds = seconds;
        }
        
        public int CompareTo(object obj) {
            if (obj == null) {
                return 1;
            }
            TimeKey key = obj as TimeKey;
            if (key != null) {
                return this.seconds.CompareTo(key.seconds);
            }
            throw new ArgumentException(obj.ToString());
        }

        /// <summary>
        /// Human readable name for the TimeKey.
        /// </summary>
        public string Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Seconds since start of timeline playback.
        /// </summary>
        public float Seconds {
            get { return seconds; }
            set {
                this.seconds = value;
                timeline.ResortKeyOrder();
            }
        }

        internal void RawSetSeconds(float seconds) {
            this.seconds = seconds;
        }

        public string GetTimeFormatted() {
            return string.Format("{0:0.00}\"", seconds);
        }

        public IItemEditor MakeEditor() {
            return new TimeKeyEditor(this);
        }

        private class TimeKeyEditor : IItemEditor {
            private readonly TimeKey key;
            private readonly CheckedField<float> secondsField;

            public TimeKeyEditor(TimeKey key) {
                this.key = key;
                this.secondsField = new CheckedField<float>(key.seconds, ValueParser.floatParser);
            }

            public IItemEditor DrawUI() {
                using (GU.Vertical()) {
                    using (GU.Horizontal()) {
                        GUILayout.Label("Name: ");
                        key.name = GUILayout.TextField(key.name);
                    }
                    using (GU.Horizontal()) {
                        GUILayout.Label("Seconds: ");
                        if (secondsField.DrawUI(ref key.seconds)) {
                            key.timeline.ResortKeyOrder();
                        }
                    }
                }
                return this;
            }

            public bool IsEditing(object o) {
                return object.ReferenceEquals(key, o);
            }
        }
    }
}
