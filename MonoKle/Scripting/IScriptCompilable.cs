using System;

namespace MonoKle.Scripting
{
    /// <summary>
    /// Compilable script interface.
    /// </summary>
    /// <seealso cref="IScript" />
    public interface IScriptCompilable : IScript
    {
        /// <summary>
        /// Gets or sets the compilation date in UTC.
        /// </summary>
        /// <value>
        /// The compilation date in UTC.
        /// </value>
        DateTime CompilationDate { get; set; }

        /// <summary>
        /// Gets or sets the internal script implementation.
        /// </summary>
        /// <value>
        /// The internal script.
        /// </value>
        ScriptImplementation InternalScript { get; set; }
    }
}