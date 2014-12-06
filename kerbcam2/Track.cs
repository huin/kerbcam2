using System.Collections.Generic;

namespace kerbcam2 {
    class Track {
        /// <summary>
        /// Human readable name for the track.
        /// </summary>
        private string name;

        private Timeline timeline;

        /// <summary>
        /// The operations that are part of the track.
        /// </summary>
        private List<IOperation> operations;

        // TODO: operations may need to be sorted.

        // TODO: enforce the non-overlaps when an operation's time range
        // changes.

        /// <summary>
        /// Create a new track.
        /// </summary>
        /// <param name="timeline">The timeline that operations reference.</param>
        public Track(Timeline timeline, string name) {
            this.name = name;
            this.timeline = timeline;
            this.operations = new List<IOperation>();
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Adds an operation to a track's ownership.
        /// </summary>
        /// <param name="op">The operation to add.</param>
        /// <exception cref="TimeConflict">|op| time range conflicts with
        /// another operation in the track.</exception>
        /// <exception cref="TimelineMismatchException">The timeline for |op|
        /// is not that of the track.</exception>
        public void AddOperation(IOperation op) {
            if (!ReferenceEquals(op.GetTimeline(), timeline)) {
                throw new TimelineMismatchException();
            }
            foreach (IOperation existingOp in operations) {
                if (IsOverlap(op, existingOp)) {
                    throw new TimeConflictException();
                }
            }
            operations.Add(op);
        }

        private static bool IsOverlap(IOperation a, IOperation b) {
            float a0 = a.GetStartTime().seconds;
            float a1 = a.GetEndTime().seconds;
            float b0 = b.GetStartTime().seconds;
            float b1 = b.GetEndTime().seconds;
            if (a.IsInstantaneous()) {
                if (b.IsInstantaneous()) {
                    return a0 == b0;
                } else {
                    return a0 >= b0 && a0 < b1;
                }
            } else if (b.IsInstantaneous()) {
                return b0 >= a0 && b0 < a1;
            } else {
                return (a0 >= b0 && a0 < b1)
                    || (a1 > b0 && a1 < b1)
                    || (b0 >= a0 && b0 < a1)
                    || (b1 > a0 && b1 < a1);
            }
        }

        /// <summary>
        /// Atomically move the operation to another track, if possible.
        /// </summary>
        /// <param name="op">The operation to move.</param>
        /// <param name="newOwner">The track to move the operation into.</param>
        /// <exception cref="NotOwnerException">|op| is not a member of the
        /// track</exception>
        /// <exception cref="TimeConflictException">|op| time range conflicts
        /// with another operation on |newOwner|.</exception>
        public void MoveOperationOwnership(IOperation op, Track newOwner) {
            if (!ReferenceEquals(newOwner.timeline, timeline)) {
                throw new TimelineMismatchException();
            }
            int index = operations.IndexOf(op);
            if (index == -1) {
                throw new NotOwnerException();
            }
            newOwner.AddOperation(op);
            operations.RemoveAt(index);
        }

        // TODO: Mutate world methods.
    }
}
