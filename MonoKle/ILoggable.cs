﻿namespace MonoKle
{
    using MonoKle.Logging;

    /// <summary>
    /// Interface for classes utilizing a logger.
    /// </summary>
    public interface ILoggable
    {
        /// <summary>
        /// Gets or sets the logger instance.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        Logger Logger { get; set; }
    }
}
