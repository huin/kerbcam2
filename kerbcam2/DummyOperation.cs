using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kerbcam2 {
    /// <summary>
    /// Implements an operation for the purposes of GUI experimentation.
    /// </summary>
    class DummyOperation : IOperation {
        private Dictionary<long, string> items;
        private string name;
        private Timeline timeline;

        public DummyOperation(Timeline timeline, string name) {
            items = new Dictionary<long, string>();
            this.timeline = timeline;
            this.name = name;
        }

        public string Name {
            get { return name; }
        }

        public Dictionary<long, string> Items {
            get { return items; }
        }

        public bool GetNameForTimeKey(long id, out string name) {
            return items.TryGetValue(id, out name);
        }

        public IPlaybackState MakePlayback(Actuators actuators) {
            throw new NotImplementedException();
        }
    }
}
