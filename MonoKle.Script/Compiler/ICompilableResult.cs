namespace MonoKle.Script.Compiler
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for the evaluation result of script compilability.
    /// </summary>
    public interface ICompilableResult
    {
        /// <summary>
        /// Gets the error messages associated with the evaluation.
        /// </summary>
        /// <returns>Collection of error messages.</returns>
        ICollection<string> ErrorMessages
        {
            get;
        }

        /// <summary>
        /// Gets the name of the evalued script.
        /// </summary>
        /// <returns>Name of the evalued script.</returns>
        string ScriptName
        {
            get;
        }

        /// <summary>
        /// Gets whether the evaluation resulted in semantics error.
        /// </summary>
        /// <returns>True if evaluation resulted in semantics error, otherwise false.</returns>
        bool SemanticsError
        {
            get;
        }

        /// <summary>
        /// Gets whether the evaluation was successful or not.
        /// </summary>
        /// <returns>True if successful, otherwise false.</returns>
        bool Success
        {
            get;
        }

        /// <summary>
        /// Gets whether the evaluation resulted in syntax error.
        /// </summary>
        /// <returns>True if evaluation resulted in syntax error, otherwise false.</returns>
        bool SyntaxError
        {
            get;
        }
    }
}