namespace MonoKle.Script.Compiler
{
    using System.Collections.Generic;

    /// <summary>
    /// Compilable result container.
    /// </summary>
    public class CompilableResult : ICompilableResult
    {
        /// <summary>
        /// Creates a new instance of <see cref="CompilableResult"/>.
        /// </summary>
        /// <param name="name">Name of the evaluated script.</param>
        /// <param name="semanticsError">Semantics error.</param>
        /// <param name="syntaxError">Syntax error.</param>
        /// <param name="errorMessages">Error message.</param>
        public CompilableResult(string name, bool syntaxError, bool semanticsError, ICollection<string> errorMessages)
        {
            this.ScriptName = name;
            this.Success = syntaxError == false && semanticsError == false;
            this.SyntaxError = syntaxError;
            this.SemanticsError = semanticsError;
            this.ErrorMessages = errorMessages;
        }

        /// <summary>
        /// Gets the error messages associated with the evaluation.
        /// </summary>
        /// <returns>Collection of error messages.</returns>
        public ICollection<string> ErrorMessages
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the evalued script.
        /// </summary>
        /// <returns>Name of the evalued script.</returns>
        public string ScriptName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets whether the evaluation resulted in semantics error.
        /// </summary>
        /// <returns>True if evaluation resulted in semantics error, otherwise false.</returns>
        public bool SemanticsError
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets whether the evaluation was successful or not.
        /// </summary>
        /// <returns>True if successful, otherwise false.</returns>
        public bool Success
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets whether the evaluation resulted in syntax error.
        /// </summary>
        /// <returns>True if evaluation resulted in syntax error, otherwise false.</returns>
        public bool SyntaxError
        {
            get;
            private set;
        }
    }
}