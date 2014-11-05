namespace MonoKle.Logging
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Class for handling logs and triggering log events.
    /// </summary>
    public class Logger
    {
        private static Logger globalLogger;

        private LogLevel loggingLevel;
        private LinkedList<Log> logs = new LinkedList<Log>();
        private short size;

        /// <summary>
        /// Creates instance of <see cref="Logger"/>.
        /// </summary>
        public Logger()
        {
            this.LoggingLevel = LogLevel.Info;
            this.Size = byte.MaxValue;
        }

        /// <summary>
        /// Event that triggers when a log is added.
        /// </summary>
        public event LogAddedEventHandler LogAddedEvent;

        /// <summary>
        /// Returns the instance of the global logger.
        /// </summary>
        /// <returns></returns>
        public static Logger Global
        {
            get
            {
                Logger.globalLogger = Logger.globalLogger ?? new Logger();
                return Logger.globalLogger;
            }
        }

        /// <summary>
        /// The current logging level accepted.
        /// </summary>
        public LogLevel LoggingLevel
        {
            get { return this.loggingLevel; }
            set { this.loggingLevel = value; }
        }

        /// <summary>
        /// The maximum amount of logs to be stored.
        /// </summary>
        public short Size
        {
            get { return this.size; }
            set { this.size = value; this.TrimLogs(); }
        }

        /// <summary>
        /// Logs a given message with a logging level of <see cref="LogLevel.Info"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(string message)
        {
            this.Log(message, LogLevel.Info);
        }

        /// <summary>
        /// Logs a given message with the provided logging level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">The level of the log.</param>
        public void Log(string message, LogLevel level)
        {
            if(level <= this.loggingLevel)
            {
                Log newLog = new Log(message, level);
                this.logs.AddLast(newLog);
                if(this.logs.Count > this.size)
                {
                    this.logs.RemoveFirst();
                }
                this.OnLogAdded(new LogAddedEventArgs(newLog));
            }
        }

        /// <summary>
        /// Clears all current logs from memory.
        /// </summary>
        public void Clear()
        {
            logs.Clear();
        }

        /// <summary>
        /// Dumps the current logs to a provided stream, losing the currently stored logs. The provided stream is closed.
        /// </summary>
        /// <param name="stream">The stream to dump to.</param>
        public void DumpLog(Stream stream)
        {
            this.WriteLog(stream);
            this.Clear();
        }

        /// <summary>
        /// Returns a list containing the currently stored logs.
        /// </summary>
        /// <returns><see cref="IEnumerable"/> containing current logs.</returns>
        public IEnumerable<Log> GetLogs()
        {
            return new LinkedList<Log>(this.logs);
        }

        /// <summary>
        /// Writes the currently stored logs to a provided stream. The provided stream is closed.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void WriteLog(Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            foreach(Log l in this.GetLogs())
            {
                writer.WriteLine(l.ToString());
            }
            writer.Close();
        }

        /// <summary>
        /// Raises the LogAddedEvent.
        /// </summary>
        protected void OnLogAdded(LogAddedEventArgs e)
        {
            LogAddedEventHandler handler = this.LogAddedEvent;
            if(handler != null)
            {
                handler(this, e);
            }
        }

        private void TrimLogs()
        {
            while(this.logs.Count > this.size)
            {
                this.logs.RemoveFirst();
            }
        }
    }
}