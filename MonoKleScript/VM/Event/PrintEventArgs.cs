namespace MonoKle.Script.VM.Event
{
    using System;

    /// <summary>
    /// Event arguments for print events.
    /// </summary>
    public class PrintEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="PrintEventArgs"/>.
        /// </summary>
        /// <param name="message">The print message.</param>
        public PrintEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets the print message.
        /// </summary>
        public string Message
        {
            get; private set;
        }
    }
}