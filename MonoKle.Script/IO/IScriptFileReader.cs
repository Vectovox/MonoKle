namespace MonoKle.Script.IO
{
    using MonoKle.Script.Common.Script;
    using MonoKle.Script.IO.Event;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for input reader of scripts.
    /// </summary>
    public interface IScriptFileReader
    {
        /// <summary>
        /// Event fired if there is an error reading a script.
        /// </summary>
        event ScriptReadingErrorEventHandler ScriptReadingError;

        /// <summary>
        /// Gets all script sources found searching in the given path.
        /// </summary>
        /// <param name="path">Path to search in. May point out a directory or a file.</param>
        /// <param name="recurse">If true, search will include sub-directories.</param>
        /// <returns>Script sources.</returns>
        ICollection<ScriptSource> GetScriptSources(string path, bool recurse);
    }
}
