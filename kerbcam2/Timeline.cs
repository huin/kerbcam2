using System.Collections.Generic;
using UnityEngine;

namespace kerbcam2 {
    class Timeline {
        private long nextId;
        private Dictionary<long, TimeKey> keys;
        private List<long> keyOrder;

        public Timeline() {
            nextId = 1;
            keys = new Dictionary<long, TimeKey>();
            keyOrder = new List<long>();
        }

        private void ResortKeyOrder() {
            // Right now we just lazily reorder the whole list even if only one
            // item changed. If this turns out to be too slow, we'll optimize.
            keyOrder.Sort(delegate(long id1, long id2) {
                TimeKey key1 = keys[id1];
                TimeKey key2 = keys[id2];
                return key1.seconds.CompareTo(key2.seconds);
            });
        }

        /// <summary>
        /// Access TimeKey by ordered index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TimeKey this[int index] {
            get { return keys[keyOrder[index]]; }
        }

        /// <summary>
        /// Returns number of TimeKeys.
        /// </summary>
        public int Count {
            get { return keys.Count; }
        }

        /// <summary>
        /// Iterates over the TimeKeys in order.
        /// </summary>
        /// <returns></returns>
        public TimeKeyEnumerator GetEnumerator() {
            return new TimeKeyEnumerator(this);
        }

        public struct TimeKeyEnumerator : IEnumerator<TimeKey> {
            Timeline timeline;
            IEnumerator<long> orderEnum;
            internal TimeKeyEnumerator(Timeline timeline) {
                this.timeline = timeline;
                orderEnum = timeline.keyOrder.GetEnumerator();
            }
            object System.Collections.IEnumerator.Current {
                get { return Current; }
            }
            public TimeKey Current {
                get { return timeline.keys[orderEnum.Current]; }
            }
            public void Dispose() { orderEnum.Dispose(); }
            public bool MoveNext() { return orderEnum.MoveNext(); }
            public void Reset() { orderEnum.Reset(); }
        }

        /// <summary>
        /// Adds a copy of a TimeKey to the timeline.
        /// </summary>
        /// <param name="key">The value of the key to add. Its ID is ignored.</param>
        /// <returns>The added key.</returns>
        public TimeKey NewTimeKey(TimeKey key) {
            key.id = nextId;
            nextId++;
            keys[key.id] = key;
            keyOrder.Add(key.id);
            ResortKeyOrder();
            return key;
        }

        /// <summary>
        /// Removes a TimeKey from the timeline.
        /// </summary>
        /// <param name="id">The ID of the key to remove.</param>
        /// <returns>true if key found and removed, false otherwise.</returns>
        public bool RemoveTimeKey(long id) {
            bool found = keys.Remove(id);
            if (found) {
                keyOrder.Remove(id);
            }
            return found;
        }

        /// <summary>
        /// Updates the values of a TimeKey. Its ID remains the same.
        /// </summary>
        /// <param name="key">The key to update. Its ID uniquely identifies
        /// the key to update within the timeline.</param>
        /// <exception cref="KeyNotFoundException">No TimeKey with the given
        /// ID exists.</exception>
        /// <exception cref="TimeConflictException">Another TimeKey has the
        /// same time.</exception>
        public void UpdateTimeKey(TimeKey key) {
            // Cause exception to be raised if doesn't exist.
            TimeKey current = keys[key.id];
            if (current.seconds == key.seconds) {
                // No-op shortcut.
                return;
            }
            foreach (TimeKey other in keys.Values) {
                if (other.id != key.id && other.seconds == key.seconds) {
                    throw new TimeConflictException();
                }
            }
            keys[key.id] = key;
            ResortKeyOrder();
        }

        public void ShiftAll(float offset) {
            foreach (long tid in keyOrder) {
                TimeKey t = keys[tid];
                t.seconds += offset;
                keys[tid] = t;
            }
        }

        /// <summary>
        /// Returns an editor for the identified TimeKey.
        /// </summary>
        /// <param name="tid">ID of the TimeKey.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">No TimeKey with the given ID exists.</exception>
        public IItemEditor GetEditorForTimeKey(long tid) {
            return new TimeKeyEditor(this, keys[tid]);
        }

        /// <summary>
        /// Returns a copy of the TimeKey with the given ID.
        /// </summary>
        /// <param name="id">The ID of the TimeKey to return.</param>
        /// <returns>Copy of the TimeKey.</returns>
        /// <exception cref="KeyNotFoundException">No TimeKey with the given ID exists.</exception>
        public TimeKey GetTimeKey(long id) {
            return keys[id];
        }

        private class TimeKeyEditor : IItemEditor {
            private Timeline timeline;
            private TimeKey key;
            private CheckedField<float> secondsField;

            public TimeKeyEditor(Timeline timeline, TimeKey key) {
                this.timeline = timeline;
                this.key = key;
                secondsField = new CheckedField<float>(key.seconds, ValueParser.floatParser);
            }

            public IItemEditor DrawUI() {
                timeline.GetTimeKey(key.id);
                using (GU.Vertical()) {
                    using (GU.Horizontal()) {
                        GUILayout.Label("Name: ");
                        key.name = GUILayout.TextField(key.name);
                    }
                    using (GU.Horizontal()) {
                        GUILayout.Label("Seconds: ");
                        secondsField.DrawUI(ref key.seconds);
                    }
                }
                timeline.UpdateTimeKey(key);
                return this;
            }
        }
    }
}
