namespace MonoKle.Logging {
    using System;
    using System.Text;

    /// <summary>
    /// An immutable class containing a log message and relevant accompanying information.
    /// </summary>
    public sealed class Log {
        /// <summary>
        /// Creates a new instance of <see cref="Log"/>.
        /// </summary>
        /// <param name="message">The message of the log.</param>
        /// <param name="level">The level of the log.</param>
        /// <param name="time">The time of the log.</param>
        public Log(string message, LogLevel level, DateTime time) {
            Message = message;
            Level = level;
            Time = time;
        }

        /// <summary>
        /// The level of the log.
        /// </summary>
        public LogLevel Level {
            get;
            private set;
        }

        /// <summary>
        /// The message of the log.
        /// </summary>
        public string Message {
            get;
            private set;
        }

        /// <summary>
        /// The time the log was made.
        /// </summary>
        public DateTime Time {
            get;
            private set;
        }

        /// <summary>
        /// Returns the string representation of the log.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append('[');
            sb.Append(Level);
            sb.Append("] ");
            sb.Append(Time.ToLongTimeString());
            sb.Append(" :: ");
            sb.Append(Message);
            return sb.ToString();
        }
    }
}