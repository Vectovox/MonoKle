namespace MonoKle.Scripting
{
    /// <summary>
    /// Class representing an error when compiling a script.
    /// </summary>
    public class ScriptCompilationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptCompilationError"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="line">The line.</param>
        /// <param name="isWarning">if set to <c>true</c> [is warning].</param>
        public ScriptCompilationError(string message, int line, bool isWarning)
        {
            this.Message = message;
            this.Line = line;
            this.IsWarning = isWarning;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is warning.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is warning; otherwise, <c>false</c>.
        /// </value>
        public bool IsWarning { get; set; }

        /// <summary>
        /// Gets or sets the line number that the error occured at.
        /// </summary>
        /// <value>
        /// The line.
        /// </value>
        public int Line { get; set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"Line {Line} - {Message}";
        }
    }
}