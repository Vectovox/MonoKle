namespace MonoKle.Script.Compiler
{
    using MonoKle.Script.Common.Script;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for compilation results.
    /// </summary>
    public interface ICompilationResult
    {
        /// <summary>
        /// Gets the name of the compiled script.
        /// </summary>
        /// <returns>Name of the compiled script.</returns>
        string ScriptName { get; }

        /// <summary>
        /// Gets whether the compilation was successful or not.
        /// </summary>
        /// <returns>True if successful, otherwise false.</returns>
        bool Success { get; }

        /// <summary>
        /// Gets whether the compilation resulted in semantics error.
        /// </summary>
        /// <returns>True if compilation resulted in semantics error, otherwise false.</returns>
        bool SemanticsError { get; }

        /// <summary>
        /// Gets whether the compilation resulted in syntax error.
        /// </summary>
        /// <returns>True if compilation resulted in syntax error, otherwise false.</returns>
        bool SyntaxError { get; }

        /// <summary>
        /// Gets the compiled script.
        /// </summary>
        /// <returns>ByteScript if compilation was successful, otherwise null.</returns>
        ByteScript Script { get; }

        /// <summary>
        /// Gets the error messages associated with the compilation.
        /// </summary>
        /// <returns>Collection of error messages.</returns>
        ICollection<string> ErrorMessages { get; }
    }
}
