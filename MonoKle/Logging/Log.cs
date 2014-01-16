using System.Text;
namespace MonoKle.Logging
{
    /// <summary>
    /// A class containing a log message and relevant accompanying information.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Creates a new instance of <see cref="Log"/>.
        /// </summary>
        /// <param name="message">The message of the log.</param>
        /// <param name="level">The level of the log.</param>
        public Log(string message, LogLevel level)
        {
            this.Message = message;
            this.level = level;
        }

        /// <summary>
        /// The level of the log.
        /// </summary>
        public LogLevel level
        {
            get; private set;
        }

        /// <summary>
        /// The message of the log.
        /// </summary>
        public string Message
        {
            get; private set;
        }

        /// <summary>
        /// Returns the string representation of the log.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(this.level);
            sb.Append("]: ");
            sb.Append(this.Message);
            return sb.ToString();
        }
    }
}