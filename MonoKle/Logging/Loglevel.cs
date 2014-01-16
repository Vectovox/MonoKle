namespace MonoKle.Logging
{
    /// <summary>
    /// Levels of logging that indicates severity.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Lowest level of logging, indicating that an error that will affect the operation of the software.
        /// </summary>
        Error = 1,
        /// <summary>
        /// Low level of logging, indicating that the operation of the software may be compromised due to an unexpected event.
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Medium level of logging, used for software lifecycle events (start, stop, etc.) and may help in analysing issues.
        /// </summary>
        Info = 3,
        /// <summary>
        /// High level of logging, used for debugging and isolating issues in non-trivial methods.
        /// </summary>
        Debug = 4,
        /// <summary>
        /// Highest level of logging, used to give very detailed debugging information under trivial circumstances (such as each loop iteration).
        /// </summary>
        Trace = 5
    }
}