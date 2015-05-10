using System;
using System.Collections.Generic;
using UnityEngine;

namespace kerbcam2 {
    /// <summary>
    /// Implements an operation for the purposes of GUI experimentation.
    /// </summary>
    class DummyOperation : IOperation {
        private Dictionary<long, DummyOperationKey> items;
        private string name;
        private Timeline timeline;

        public DummyOperation(Timeline timeline, string name) {
            items = new Dictionary<long, DummyOperationKey>();
            this.timeline = timeline;
            this.name = name;
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public IOperationKey AddKey(long tid) {
            if (items.ContainsKey(tid)) {
                throw new TimeConflictException();
            }
            return items[tid] = new DummyOperationKey("");
        }

        public bool TryGetKey(long id, out IOperationKey key) {
            DummyOperationKey dkey;
            bool found = items.TryGetValue(id, out dkey);
            key = dkey;
            return found;
        }

        public IPlaybackState MakePlayback(Actuators actuators) {
            throw new NotImplementedException();
        }

        public IItemEditor MakeEditor() {
            return new DummyOperatiorEditor(this);
        }

        private class DummyOperatiorEditor : IItemEditor {
            private DummyOperation op;

            public DummyOperatiorEditor(DummyOperation op) {
                this.op = op;
            }

            IItemEditor IItemEditor.DrawUI() {
                using (GU.Horizontal()) {
                    GUILayout.Label("Name: ");
                    op.Name = GUILayout.TextField(op.Name);
                }
                return this;
            }
            public bool IsEditing(object o) {
                return object.ReferenceEquals(op, o);
            }
        }

        private class DummyOperationKey : IOperationKey {
            private string name;

            public DummyOperationKey(string name) {
                this.name = name;
            }

            public string Name {
                get { return name; }
                set { name = value; }
            }
            
            public IItemEditor MakeEditor() {
 	            return new DummyOperatiorKeyEditor(this);
            }
        }

        private class DummyOperatiorKeyEditor : IItemEditor {
            private DummyOperationKey key;

            public DummyOperatiorKeyEditor(DummyOperationKey key) {
                this.key = key;
            }

            IItemEditor IItemEditor.DrawUI() {
                using (GU.Horizontal()) {
                    GUILayout.Label("Name: ");
                    key.Name = GUILayout.TextField(key.Name);
                }
                return this;
            }
            public bool IsEditing(object o) {
                return object.ReferenceEquals(key, o);
            }
        }
    }
}
