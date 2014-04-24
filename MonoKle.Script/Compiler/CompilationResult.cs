namespace MonoKle.Script.Compiler
{
    using MonoKle.Script.Common.Script;
    using System.Collections.Generic;

    /// <summary>
    /// Compilation result container.
    /// </summary>
    public class CompilationResult : ICompilationResult
    {
        /// <summary>
        /// Gets the name of the compiled script.
        /// </summary>
        /// <returns>Name of the compiled script.</returns>
        public string ScriptName { get; private set;  }

        /// <summary>
        /// Gets the error messages associated with the compilation.
        /// </summary>
        /// <returns>Collection of error messages.</returns>
        public ICollection<string> ErrorMessages { get; private set; }

        /// <summary>
        /// Gets the compiled script.
        /// </summary>
        /// <returns>ByteScript if compilation was successful, otherwise null.</returns>
        public ByteScript Script { get; private set; }

        /// <summary>
        /// Gets whether the compilation was successful or not.
        /// </summary>
        /// <returns>True if successful, otherwise false.</returns>
        public bool Success { get; private set; }

        /// <summary>
        /// Gets whether the compilation resulted in semantics error.
        /// </summary>
        /// <returns>True if compilation resulted in semantics error, otherwise false.</returns>
        public bool SemanticsError { get; private set; }

        /// <summary>
        /// Gets whether the compilation resulted in syntax error.
        /// </summary>
        /// <returns>True if compilation resulted in syntax error, otherwise false.</returns>
        public bool SyntaxError { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="CompilationResult"/>.
        /// </summary>
        /// <param name="name">Name of the compiled script.</param>
        /// <param name="success">Compilation success.</param>
        /// <param name="semanticsError">Semantics error.</param>
        /// <param name="syntaxError">Syntax error.</param>
        /// <param name="script">Compiled script.</param>
        /// <param name="errorMessages">Error message.</param>
        public CompilationResult(string name, bool success, bool syntaxError, bool semanticsError, ByteScript script, ICollection<string> errorMessages)
        {
            this.ScriptName = name;
            this.Success = success;
            this.SyntaxError = syntaxError;
            this.SemanticsError = semanticsError;
            this.Script = script;
            this.ErrorMessages = errorMessages;
        }
    }
}
