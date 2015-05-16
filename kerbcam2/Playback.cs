using System;
using System.Collections.Generic;
using UnityEngine;

namespace kerbcam2 {
    class Playback : IDisposable {
        private readonly float duration;
        private readonly List<IPlaybackState> states;
        // GameObjects for deletion when playback ends.
        private readonly List<GameObject> forDeletion;
        private readonly Actuators actuators;
        private readonly GameObject runnerObj;
        private readonly Runner runner;
        private PlaybackState state;

        public enum PlaybackState {
            READY,
            RUNNING,
            STOPPED,
        }

        private Playback(float duration, List<IPlaybackState> states,
            List<GameObject> forDeletion, Actuators actuators) {
            this.duration = duration;
            this.states = states;
            this.forDeletion = forDeletion;
            this.actuators = actuators;
            this.runnerObj = new GameObject("kerbcam2.Playback");
            this.runner = this.runnerObj.AddComponent<Runner>();
            this.runner.Init(this);
            this.state = PlaybackState.READY;
        }

        /// <summary>
        /// Starts playback.
        /// </summary>
        public void Start() {
            runner.enabled = true;
            state = PlaybackState.RUNNING;
        }

        /// <summary>
        /// Stops playback.
        /// </summary>
        public void Stop() {
            runner.enabled = false;
            state = PlaybackState.STOPPED;
        }

        /// <summary>
        /// Returns true if playback has started and is still running.
        /// </summary>
        public PlaybackState State {
            get { return state; }
        }

        /// <summary>
        /// Creates a playback that will preview changes, but with an object
        /// to represent the camera.
        /// </summary>
        /// <param name="story">The story to play back.</param>
        /// <returns>The playback to use.</returns>
        public static Playback MakePreview(Story story) {
            var forDeletion = new List<GameObject>();

            // Psuedo-camera:
            var camera = new GameObject("kerbcam2.FakeCamera");
            forDeletion.Add(camera);
            Mesh cameraMesh = camera.AddComponent<MeshFilter>().sharedMesh;
            SquareFrustrum frustrum = new SquareFrustrum(cameraMesh);
            camera.AddComponent<MeshRenderer>();
            var actuators = new Actuators(camera.transform);

            var states = new List<IPlaybackState>();
            foreach (IOperation op in story.EnumerateOperations()) {
                states.Add(op.MakePlayback(actuators));
            }
            return new Playback(story.Timeline.Duration, states, forDeletion, actuators);
        }

        public void Dispose() {
            Stop();
            foreach (GameObject o in forDeletion) {
                o.DestroyGameObject();
            }
            runnerObj.DestroyGameObject();
        }

        private class Runner : MonoBehaviour {
            private Playback playback;
            private float time;

            internal void Init(Playback playback) {
                // Don't start out playing.
                this.enabled = false;
                this.playback = playback;
                this.time = 0;
            }

            public void Update() {
                float dt = Time.deltaTime;
                time += dt;
                if (time > playback.duration) {
                    playback.Stop();
                    return;
                }
                foreach (var state in playback.states) {
                    state.UpdateFor(time);
                }
            }
        }
    }
}
