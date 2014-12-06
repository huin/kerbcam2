using System;

namespace kerbcam2 {
    /// <summary>
    /// Common superclass for all KerbCam2 exceptions.
    /// </summary>
    class BaseException : Exception {
    }

    class NotOwnerException : BaseException {
    }

    class TimeConflictException : BaseException {
    }

    class TimelineMismatchException : BaseException {
    }
}
