﻿using System.Collections.Generic;
using System;
using UnityEngine;

namespace kerbcam2 {
    class BezierTranslator : IOperation {
        private string name;
        private readonly Timeline timeline;
        private readonly Dictionary<long, Key> keys = new Dictionary<long, Key>();

        public BezierTranslator(Timeline timeline, string name) {
            this.timeline = timeline;
            this.name = name;
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public bool TryGetKey(long tid, out IOperationKey key) {
            Key skey;
            bool found = keys.TryGetValue(tid, out skey);
            key = skey;
            return found;
        }

        public IOperationKey AddKey(long tid) {
            if (keys.ContainsKey(tid)) {
                throw new TimeConflictException();
            }
            return keys[tid] = new Key();
        }

        public IPlaybackState MakePlayback(Actuators actuators) {
            throw new NotImplementedException();
        }

        public IItemEditor MakeEditor() {
            return new Editor(this);
        }

        private class Editor : IItemEditor {
            private readonly BezierTranslator op;

            public Editor(BezierTranslator bezierTranslator) {
                this.op = bezierTranslator;
            }
            public IItemEditor DrawUI() {
                using (GU.Horizontal()) {
                    GUILayout.Label("Name: ");
                    op.Name = GUILayout.TextField(op.Name);
                }
                return this;
            }
        }

        private class Key : IOperationKey {
            // TODO: The object that the node is relative to. For now we just use the origin.
            internal string name = "";
            internal Vector3d position;
            internal Vector3d moveIn, moveOut;

            public string Name {
                get { return name; }
                set { name = value; }
            }

            public IItemEditor MakeEditor() {
                return new KeyEditor(this);
            }
        }

        private class KeyEditor : IItemEditor {
            private readonly Key key;
            private readonly Vector3dField posField;
            private readonly Vector3dField moveInField, moveOutField;

            public KeyEditor(Key key) {
                this.key = key;
                posField = new Vector3dField(key.position);
                moveInField = new Vector3dField(key.moveIn);
                moveOutField = new Vector3dField(key.moveOut);
            }
            public IItemEditor DrawUI() {
                using (GU.Vertical()) {
                    key.Name = GU.LabelledTextField("Name: ", key.Name);

                    using (GU.Horizontal()) {
                        GUILayout.Label("Position: ");
                        posField.DrawUI(ref key.position);
                    }
                    using (GU.Horizontal()) {
                        GUILayout.Label("In: ");
                        moveInField.DrawUI(ref key.moveIn);
                    }
                    using (GU.Horizontal()) {
                        GUILayout.Label("Out: ");
                        moveOutField.DrawUI(ref key.moveOut);
                    }
                }
                return this;
            }
        }
    }
}
