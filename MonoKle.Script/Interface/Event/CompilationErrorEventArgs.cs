namespace MonoKle.Script.Interface.Event
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Event arguments for compilation error.
    /// </summary>
    public class CompilationErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="CompilationErrorEventArgs"/>.
        /// </summary>
        /// <param name="messages">The compilation error messages.</param>
        /// <param name="script">The script that got the compilation error.</param>
        public CompilationErrorEventArgs(ICollection<string> messages, string script)
        {
            this.Messages = messages;
            this.Script = script;
        }

        /// <summary>
        /// Gets the compilation error message.
        /// </summary>
        public ICollection<string> Messages
        {
            get; private set;
        }

        /// <summary>
        /// Gets the script that got the compilation error.
        /// </summary>
        /// <returns>Script</returns>
        public string Script
        {
            get; private set;
        }
    }
}