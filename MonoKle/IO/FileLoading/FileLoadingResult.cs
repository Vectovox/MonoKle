namespace MonoKle.IO.FileLoading
{
    /// <summary>
    /// Immutable struct for the results of loading files.
    /// </summary>
    public struct FileLoadingResult
    {
        /// <summary>
        /// Creates a new <see cref="FileLoadingResult"/>.
        /// </summary>
        /// <param name="successes">The amount of successfully loaded files.</param>
        /// <param name="failures">The amount of files that failed to load.</param>
        public FileLoadingResult(int successes, int failures)
            : this(successes, failures, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="FileLoadingResult"/>.
        /// </summary>
        /// <param name="successes">The amount of successfully loaded files.</param>
        /// <param name="failures">The amount of files that failed to load.</param>
        /// <param name="extra">Extra data to associate with the file loading.</param>
        public FileLoadingResult(int successes, int failures, object extra) : this()
        {
            this.Successes = successes;
            this.Failures = failures;
            this.Extra = extra;
        }

        /// <summary>
        /// Gets the extra data associated with loading files. Can be null.
        /// </summary>
        public object Extra
        {
            get; private set;
        }

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
        public int Successes
        {
            get; private set;
        }

        /// <summary>
        /// Gets the total amount of files opened during the loading operation.
        /// </summary>
        public int TotalFiles
        {
            get { return this.Successes + this.Failures; }
        }
    }
}