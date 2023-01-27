using Microsoft.Extensions.Logging;

namespace MonoKle
{
    /// <summary>
    /// Interface for classes utilizing a logger.
    /// </summary>
    public interface ILogged
    {
        /// <summary>
        /// Gets or sets the logger instance.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        ILogger Logger { get; set; }
    }
}
