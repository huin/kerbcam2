
namespace kerbcam2 {
    /// <summary>
    /// Represents an instant in time on a Timeline.
    /// </summary>
    struct TimeKey {
        /// <summary>
        /// Human readable name for the TimeKey.
        /// </summary>
        public string name;

        /// <summary>
        /// Seconds since start of timeline playback.
        /// </summary>
        public float seconds;

        /// <summary>
        /// The ID assigned to the time key. It is unique within a Timeline.
        /// </summary>
        public long id;

        public TimeKey(string name, float seconds) {
            this.name = name;
            this.seconds = seconds;
            this.id = 0;
        }

        public string GetTimeFormatted() {
            return string.Format("{0:0.00}", seconds);
        }
    }
}
