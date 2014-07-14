namespace MonoKle.Script.IO
{
    using System.Collections.Generic;
    using System.IO;

    using MonoKle.Script.Common.Script;

    /// <summary>
    /// Interface for input reader of scripts.
    /// </summary>
    public interface IScriptReader
    {
        /// <summary>
        /// Reads all script sources found searching in the given path.
        /// </summary>
        /// <param name="path">Path to search in. May point out a directory or a file.</param>
        /// <param name="recurse">If true, search will include sub-directories.</param>
        /// <returns>Script reading result.</returns>
        IScriptReaderResult ReadScriptSources(string path, bool recurse);

        /// <summary>
        /// Reads all script sources found in the given stream.
        /// </summary>
        /// <param name="stream">The stream to get scripts from.</param>
        /// <returns>Script reading result.</returns>
        IScriptReaderResult ReadScriptSources(Stream stream);

        /// <summary>
        /// Reads all script sources found in the given string.
        /// </summary>
        /// <param name="scriptString">The string to read scripts from.</param>
        /// <returns>Script reading result.</returns>
        IScriptReaderResult ReadScriptSources(string scriptString);
    }
}