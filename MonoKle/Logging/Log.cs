namespace MonoKle.Logging
{
    using System;
    using System.Text;

    /// <summary>
    /// An immutable class containing a log message and relevant accompanying information.
    /// </summary>
    public sealed class Log
    {
        /// <summary>
        /// Creates a new instance of <see cref="Log"/>.
        /// </summary>
        /// <param name="message">The message of the log.</param>
        /// <param name="level">The level of the log.</param>
        /// <param name="time">The time of the log.</param>
        public Log(string message, LogLevel level, DateTime time)
        {
            this.Message = message;
            this.Level = level;
            this.Time = time;
        }

        /// <summary>
        /// The level of the log.
        /// </summary>
        public LogLevel Level
        {
            get;
            private set;
        }

        /// <summary>
        /// The message of the log.
        /// </summary>
        public string Message
        {
            get;
            private set;
        }

        /// <summary>
        /// The time the log was made.
        /// </summary>
        public DateTime Time
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the string representation of the log.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            sb.Append(this.Level);
            sb.Append("] ");
            sb.Append(this.Time.ToLongTimeString());
            sb.Append(" :: ");
            sb.Append(this.Message);
            return sb.ToString();
        }
    }
}