using System.Collections.Generic;

namespace kerbcam2 {
    /// <summary>
    /// Handles modification of the world (or camera) during a given time range.
    /// Strictly speaking, the times are inclusive of the start time, and
    /// exclusive of the end time, except where they are equal, which implies
    /// an instantaneous effect.
    /// </summary>
    interface IOperation {
        /// <summary>
        /// Return description for the time key ID, if it exists.
        /// </summary>
        /// <param name="id">The timekey ID.</param>
        /// <param name="description">Returns the description.</param>
        /// <returns>true if the operation has the time key, false otherwise.</returns>
        bool GetTimeKeyDescription(long id, out string description);

        /// <summary>
        /// Create an object for playing back the operation.
        /// </summary>
        /// <param name="actuators">The actuators to use during
        /// playback.</param>
        /// <returns>An object that applies the operation to the
        /// world.</returns>
        IPlaybackState MakePlayback(Actuators actuators);
    }

    /// <summary>
    /// Applies the effects for playback.
    /// </summary>
    interface IPlaybackState {
        /// <summary>
        /// Update the world for the new time.
        /// </summary>
        /// <param name="time">Seconds since timeline time zero. Values passed
        /// to this must monotonically increase between subsequent
        /// calls. time must be >= zero.</param>
        /// <exception cref="TimeValueException">A bad value for time was
        /// given.</exception>
        void UpdateFor(float time);
    }
}
