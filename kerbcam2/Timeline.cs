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

        public float Duration {
            get {
                if (keys.Count > 0) {
                    return keys[keys.Count - 1].Seconds;
                } else {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Iterates over the TimeKeys in order.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TimeKey> GetEnumerator() {
            return keys.GetEnumerator();
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
