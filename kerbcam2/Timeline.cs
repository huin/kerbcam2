using System;
using System.Collections.Generic;

namespace kerbcam2 {
    class Timeline {
        private List<TimeKey> keys = new List<TimeKey>();

        /// <summary>
        /// Returns number of TimeKeys.
        /// </summary>
        public int Count {
            get { return keys.Count; }
        }

        public TimeKey this[int index] {
            get { return keys[index]; }
        }

        /// <summary>
        /// Iterates over the TimeKeys in order.
        /// </summary>
        /// <returns></returns>
        public TimeKeyEnumerator GetEnumerator() {
            return new TimeKeyEnumerator(this);
        }

        public struct TimeKeyEnumerator : IEnumerator<TimeKey> {
            private readonly IEnumerator<TimeKey> e;
            internal TimeKeyEnumerator(Timeline timeline) {
                e = timeline.keys.GetEnumerator();
            }
            object System.Collections.IEnumerator.Current {
                get { return e.Current; }
            }
            public TimeKey Current {
                get { return e.Current; }
            }
            public void Dispose() { new NotImplementedException(); }
            public bool MoveNext() { return e.MoveNext(); }
            public void Reset() { e.Reset(); }
        }

        /// <summary>
        /// Adds a copy of a TimeKey to the timeline.
        /// </summary>
        /// <param name="key">The key to add.</param>
        public TimeKey NewTimeKey(string name, float seconds) {
            TimeKey key = new TimeKey(this, name, seconds);
            keys.Add(key);
            ResortKeyOrder();
            return key;
        }

        /// <summary>
        /// Removes a TimeKey from the timeline.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns>true if key found and removed, false otherwise.</returns>
        public bool RemoveTimeKey(TimeKey key) {
            return keys.Remove(key);
        }

        public void ShiftAll(float offset) {
            foreach (TimeKey key in keys) {
                key.RawSetSeconds(key.Seconds + offset);
            }
        }

        internal void ResortKeyOrder() {
            // Right now we just lazily reorder the whole list even if only one
            // item changed. If this turns out to be too slow, we'll optimize.
            keys.Sort();
        }
    }
}
