namespace MonoKle.Logging
{
    using System;

    /// <summary>
    /// Event arguments for when a log was added.
    /// </summary>
    public class LogAddedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="LogAddedEventArgs"/>.
        /// </summary>
        /// <param name="log">The log.</param>
        public LogAddedEventArgs(Log log)
        {
            Log = log;
        }

        /// <summary>
        /// The added log.
        /// </summary>
        public Log Log
        {
            get;
            private set;
        }
    }
}