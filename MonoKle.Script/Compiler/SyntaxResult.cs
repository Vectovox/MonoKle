namespace MonoKle.Script.Compiler
{
    using System.Collections.Generic;

    /// <summary>
    /// Syntax checking result container.
    /// </summary>
    public class SyntaxResult : ISyntaxResult
    {
        /// <summary>
        /// Creates a new instane of <see cref="SyntaxResult"/>.
        /// </summary>
        /// <param name="scriptName">Name of the script checked.</param>
        /// <param name="errorMessages">Error messages of the check.</param>
        public SyntaxResult(string scriptName, ICollection<string> errorMessages)
        {
            this.ScriptName = scriptName;
            this.SyntaxError = errorMessages.Count != 0;
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