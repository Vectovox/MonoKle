namespace MonoKle.Scripting
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface defining methods for a script.
    /// </summary>
    public interface IScript
    {
        /// <summary>
        /// Gets a value indicating whether this instance can execute.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can execute; otherwise, <c>false</c>.
        /// </value>
        bool CanExecute { get; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        List<ScriptCompilationError> Errors { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is outdated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is outdated; otherwise, <c>false</c>.
        /// </value>
        bool IsOutdated { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the type of the return value.
        /// </summary>
        /// <value>
        /// The type of the return value.
        /// </value>
        Type ReturnType { get; }

        /// <summary>
        /// Gets a value indicating whether the script instance returns a value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it returns a value; otherwise, <c>false</c>.
        /// </value>
        bool ReturnsValue { get; }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        IScriptSource Source { get; }

        /// <summary>
        /// Executes with the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        ScriptExecution Execute(params object[] parameters);
    }
}