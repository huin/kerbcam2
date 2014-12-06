using System.Collections.Generic;

namespace kerbcam2 {
    /// <summary>
    /// Story is the top-level container for a playback sequence.
    /// </summary>
    class Story {
        private Timeline timeline;
        private List<Track> tracks;

        public Story() {
            this.timeline = new Timeline();
            this.tracks = new List<Track>();
        }
    }
}
