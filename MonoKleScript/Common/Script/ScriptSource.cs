namespace MonoKleScript.Common.Script
{
    /// <summary>
    /// Container class representing the soure of a script.
    /// </summary>
    public class ScriptSource
    {
        /// <summary>
        /// Header of the script.
        /// </summary>
        /// <returns>Header of script.</returns>
        public ScriptHeader Header { get; private set; }

        /// <summary>
        /// The textual source of the script.
        /// </summary>
        /// <returns>String source.</returns>
        public string Text { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="ScriptSource"/>.
        /// </summary>
        /// <param name="text">Text representation of the script.</param>
        /// <param name="header">Header of the script.</param>
        public ScriptSource(string text, ScriptHeader header)
        {
            this.Text = text;
            this.Header = header;
        }
    }
}
