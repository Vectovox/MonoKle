namespace MonoKleScript.Compiler.Event
{
    using System;

    /// <summary>
    /// Event arguments for compilation error.
    /// </summary>
    public class CompilationErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="CompilationErrorEventArgs"/>.
        /// </summary>
        /// <param name="message">The compilation error message.</param>
        public CompilationErrorEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets the compilation error message.
        /// </summary>
        public string Message
        {
            get; private set;
        }
    }
}