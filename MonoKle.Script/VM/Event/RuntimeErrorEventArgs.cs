namespace MonoKle.Script.VM.Event
{
    using System;

    /// <summary>
    /// Event arguments for runtimne error.
    /// </summary>
    public class RuntimeErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="RuntimeErrorEventArgs"/>.
        /// </summary>
        /// <param name="message">The runtime error message.</param>
        public RuntimeErrorEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets the runtime error message.
        /// </summary>
        public string Message
        {
            get; private set;
        }
    }
}