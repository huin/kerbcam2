using System.Collections.Generic;

namespace kerbcam2 {
    /// <summary>
    /// Story is the top-level container for a playback sequence.
    /// </summary>
    class Story {
        private Timeline timeline;
        private List<IOperation> operations;

        public Story() {
            this.timeline = new Timeline();
            this.operations = new List<IOperation>();
        }

        public Timeline Timeline {
            get { return timeline; }
        }

        public IEnumerator<IOperation> EnumerateOperations() {
            return operations.GetEnumerator();
        }
    }
}
