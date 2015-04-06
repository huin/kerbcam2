using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kerbcam2 {
    /// <summary>
    /// Implements an operation for the purposes of GUI experimentation.
    /// </summary>
    class DummyOperation : IOperation {
        public Dictionary<long, string> items;
        private Timeline timeline;

        public DummyOperation(Timeline timeline) {
            items = new Dictionary<long, string>();
            this.timeline = timeline;
        }

        public bool GetTimeKeyDescription(long id, out string description) {
            return items.TryGetValue(id, out description);
        }

        public IPlaybackState MakePlayback(Actuators actuators) {
            throw new NotImplementedException();
        }
    }
}
