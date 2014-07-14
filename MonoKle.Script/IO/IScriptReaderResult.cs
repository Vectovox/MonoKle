namespace MonoKle.Script.IO
{
    using MonoKle.Script.Common.Script;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for the result of reading scripts.
    /// </summary>
    public interface IScriptReaderResult
    {
        /// <summary>
        /// Gets the collection of error messages associated with the script reading operation.
        /// </summary>
        ICollection<string> ErrorMessages
        {
            get;
        }

        /// <summary>
        /// Gets the collection of read script sources.
        /// </summary>
        ICollection<ScriptSource> Sources
        {
            get;
        }
    }
}