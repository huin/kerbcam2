
namespace kerbcam2 {
    /// <summary>
    /// Handles modification of the world (or camera) during a given time range.
    /// Strictly speaking, the times are inclusive of the start time, and
    /// exclusive of the end time, except where they are equal, which implies
    /// an instantaneous effect.
    /// </summary>
    interface IOperation {
        /// <summary>
        /// Returns the earliest TimeKey for the operation.
        /// </summary>
        /// <returns>Earliest TimeKey for the operation.</returns>
        TimeKey GetStartTime();

        /// <summary>
        /// Returns the latest TimeKey for the operation. This is equal to the
        /// start time when the operation is instantaneous.
        /// </summary>
        /// <returns>Earliest TimeKey for the operation.</returns>
        TimeKey GetEndTime();

        /// <summary>
        /// The timeline that the operation uses for its TimeKeys.
        /// </summary>
        /// <returns>The related Timeline.</returns>
        Timeline GetTimeline();

        // TODO: Method(s) relating to having an effect on the world.
        // The methods within "Mutate world" cause the operation to act upon
        // the world.
        #region Mutate world
        /// <summary>
        /// Tells the operation to precompute anything it needs to prior to
        /// playback.
        /// </summary>
        void PrepareForUpdates();

        void UpdateFor(Actuators actuators, float time);
        // Things that interpolate across multiple time keys will need to track
        // which relevant time keys they are between.
        #endregion
    }
}
