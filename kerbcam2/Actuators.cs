using UnityEngine;

namespace kerbcam2 {
    /// <summary>
    /// Used to acquire the actuators in use for a Story playback.
    /// </summary>
    class Actuators {
        private readonly Transform cameraTranslation;

        internal Actuators(Transform cameraTranslation) {
            this.cameraTranslation = cameraTranslation;
        }

        public Vector3 CameraPosition {
            get { return cameraTranslation.position; }
            set { cameraTranslation.position = value; }
        }
    }
}
