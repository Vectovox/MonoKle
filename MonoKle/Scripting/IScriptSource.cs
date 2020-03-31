using System;

namespace MonoKle.Scripting
{
    /// <summary>
    /// Interface for a script source.
    /// </summary>
    public interface IScriptSource
    {
        /// <summary>
        /// Gets the source code.
        /// </summary>
        /// <value>
        /// The source code.
        /// </value>
        string Code { get; }

        /// <summary>
        /// Gets the source date in UTC.
        /// </summary>
        /// <value>
        /// The source date in UTC.
        /// </value>
        DateTime Date { get; }
    }
}
