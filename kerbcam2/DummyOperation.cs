using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kerbcam2 {
    /// <summary>
    /// Implements an operation for the purposes of GUI experimentation.
    /// </summary>
    class DummyOperation : IOperation {
        public List<TimeKey> items;

        public DummyOperation() {
            items = new List<TimeKey>();
        }

        public TimeKey GetBeginTime() {
            throw new NotImplementedException();
        }

        public TimeKey GetEndTime() {
            throw new NotImplementedException();
        }

        public Timeline GetTimeline() {
            throw new NotImplementedException();
        }

        public EnumerableWrapper<TimeKey> EnumerateTimes() {
            return new EnumerableWrapper<TimeKey>(items);
        }

        public IPlaybackState MakePlayback(Actuators actuators) {
            throw new NotImplementedException();
        }
    }
}
