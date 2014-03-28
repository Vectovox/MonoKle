namespace MonoKleScript.IO.Error
{
    using System;

    /// <summary>
    /// Event arguments for script reading error.
    /// </summary>
    public class ScriptReadingErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="ScriptReadingErrorEventArgs"/>.
        /// </summary>
        /// <param name="message">The script reading error message.</param>
        public ScriptReadingErrorEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets the script reading error message.
        /// </summary>
        public string Message
        {
            get; private set;
        }
    }
}