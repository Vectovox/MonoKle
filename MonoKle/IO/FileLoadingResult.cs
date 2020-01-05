using System.Collections.Generic;

namespace MonoKle.IO
{
    /// <summary>
    /// Immutable struct for the results of loading files.
    /// </summary>
    public struct FileLoadingResult
    {
        /// <summary>
        /// Creates a new <see cref="FileLoadingResult"/>.
        /// </summary>
        /// <param name="loadedFiles">The successfully loaded file paths.</param>
        /// <param name="failures">The amount of files that failed to load.</param>
        public FileLoadingResult(IList<string> loadedFiles, int failures)
            : this(loadedFiles, failures, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="FileLoadingResult"/>.
        /// </summary>
        /// <param name="loadedFiles">The successfully loaded file paths.</param>
        /// <param name="failures">The amount of files that failed to load.</param>
        /// <param name="extra">Extra data to associate with the file loading.</param>
        public FileLoadingResult(IList<string> loadedFiles, int failures, object extra) : this()
        {
            LoadedFiles = loadedFiles;
            Failures = failures;
            Extra = extra;
        }

        /// <summary>
        /// Gets the extra data associated with loading files. Can be null.
        /// </summary>
        public object Extra
        {
            get; private set;
        }

        /// <summary>
        /// Gets the paths of the loaded files.
        /// </summary>
        /// <value>
        /// The loaded file paths.
        /// </value>
        public IList<string> LoadedFiles { get; private set; }

        /// <summary>
        /// Gets the amount of files that failed to load.
        /// </summary>
        public int Failures
        {
            get; private set;
        }

        /// <summary>
        /// Gets the amount of successfully loaded files.
        /// </summary>
        public int Successes => LoadedFiles.Count;

        /// <summary>
        /// Gets the total amount of files opened during the loading operation.
        /// </summary>
        public int TotalFiles => Successes + Failures;
    }
}