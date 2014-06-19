namespace MonoKle.IO.FileLoading
{
    /// <summary>
    /// Interface defining a file loader.
    /// </summary>
    public interface IFileLoader
    {
        /// <summary>
        /// Loads the file(s) in the given path.
        /// </summary>
        /// <param name="path">The path in which to load files.</param>
        /// <returns>Result of the loading.</returns>
        FileLoadingResult LoadFiles(string path);

        /// <summary>
        /// Loads the file(s) in the given path; recursively if specified.
        /// </summary>
        /// <param name="path">The path in which to load files.</param>
        /// <param name="recurse">Parameter specifying if to do a recursive search.</param>
        /// <returns>Result of the loading.</returns>
        FileLoadingResult LoadFiles(string path, bool recurse);

        /// <summary>
        /// Loads the file(s) in the given path with the provided pattern; recursively if specified.
        /// </summary>
        /// <param name="path">The path in which to load files.</param>
        /// <param name="recurse">Parameter specifying if to do a recursive search.</param>
        /// <param name="pattern">The pattern that files have to fulfill in order to be loaded.</param>
        /// <returns>Result of the loading.</returns>
        FileLoadingResult LoadFiles(string path, bool recurse, string pattern);
    }
}
