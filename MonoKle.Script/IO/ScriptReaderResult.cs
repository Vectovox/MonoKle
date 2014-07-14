namespace MonoKle.Script.IO
{
    using MonoKle.Script.Common.Script;
    using System.Collections.Generic;

    /// <summary>
    /// Result for reading a script.
    /// </summary>
    public class ScriptReaderResult : IScriptReaderResult
    {
        /// <summary>
        /// Gets the collection of error messages associated with the script reading operation.
        /// </summary>
        public ICollection<string> ErrorMessages
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the collection of read script sources.
        /// </summary>
        public ICollection<ScriptSource> Sources
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of <see cref="ScriptReaderResult"/>.
        /// </summary>
        /// <param name="sources">The read sources.</param>
        /// <param name="errorMessages">The error messages.</param>
        public ScriptReaderResult(ICollection<ScriptSource> sources, ICollection<string> errorMessages)
        {
            this.Sources = sources;
            this.ErrorMessages = errorMessages;
        }
    }
}
