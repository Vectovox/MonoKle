using MonoKle.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoKle.Logging
{
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
            LoggingLevel = LogLevel.Info;
            Size = byte.MaxValue;
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
            get => loggingLevel;
            set => loggingLevel = value;
        }

        /// <summary>
        /// The maximum amount of logs to be stored.
        /// </summary>
        [CVar("log_size")]
        public short Size
        {
            get => size;
            set { size = value; TrimLogs(); }
        }

        /// <summary>
        /// Clears all current logs from memory.
        /// </summary>
        public void Clear() => logs.Clear();

        /// <summary>
        /// Dumps the current logs to a provided stream, losing the currently stored logs. The provided stream is closed.
        /// </summary>
        /// <param name="stream">The stream to dump to.</param>
        public void DumpLog(Stream stream)
        {
            WriteLog(stream);
            Clear();
        }

        /// <summary>
        /// Returns a list containing the currently stored logs.
        /// </summary>
        /// <returns><see cref="IEnumerable{Log}"/> containing current logs.</returns>
        public IEnumerable<Log> GetLogs() => new LinkedList<Log>(logs);

        /// <summary>
        /// Logs a given message with a logging level of <see cref="LogLevel.Info"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(string message) => Log(message, LogLevel.Info);

        /// <summary>
        /// Logs a given message with the provided logging level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">The severity level of the log.</param>
        public void Log(string message, LogLevel level)
        {
            if (level <= loggingLevel)
            {
                var newLog = new Log(message, level, DateTime.Now);
                logs.AddLast(newLog);
                if (logs.Count > size)
                {
                    logs.RemoveFirst();
                }
                OnLogAdded(newLog);
            }
        }

        /// <summary>
        /// Writes the currently stored logs to a provided stream. The provided stream is closed.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void WriteLog(Stream stream)
        {
            using var writer = new StreamWriter(stream);
            foreach (Log l in GetLogs())
            {
                writer.WriteLine(l.ToString());
            }
        }

        /// <summary>
        /// Raises the LogAddedEvent.
        /// </summary>
        protected void OnLogAdded(Log newLog)
        {
            LogAddedEventHandler handler = LogAddedEvent;
            if (handler != null)
            {
                handler(this, new LogAddedEventArgs(newLog));
            }
        }

        private void TrimLogs()
        {
            while (logs.Count > size)
            {
                logs.RemoveFirst();
            }
        }
    }
}
