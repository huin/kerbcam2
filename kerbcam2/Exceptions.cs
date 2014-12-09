using System;

namespace kerbcam2 {
    /// <summary>
    /// Common superclass for all KerbCam2 exceptions.
    /// </summary>
    class BaseException : Exception {
    }

    class TimeConflictException : BaseException {
    }

    class TimeValueException : BaseException {
    }
}
