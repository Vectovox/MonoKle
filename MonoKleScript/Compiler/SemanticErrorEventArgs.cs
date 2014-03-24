namespace MonoKleScript.Compiler
{
    using System;

    /// <summary>
    /// Event arguments for semantics error.
    /// </summary>
    public class SemanticErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="SemanticErrorEventArgs"/>.
        /// </summary>
        /// <param name="message">The semantics error message.</param>
        public SemanticErrorEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets the semantics error message.
        /// </summary>
        public string Message
        {
            get; private set;
        }
    }
}