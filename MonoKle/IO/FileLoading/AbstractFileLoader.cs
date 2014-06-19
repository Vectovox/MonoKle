namespace MonoKle.IO.FileLoading
{
    using System.IO;

    /// <summary>
    /// Abstract implementation of <see cref="IFileLoader"/>, providing a template pattern method for file loading operations.
    /// </summary>
    public abstract class AbstractFileLoader
    {
        /// <summary>
        /// Loads the file(s) in the given path.
        /// </summary>
        /// <param name="path">The path in which to load files.</param>
        public FileLoadingResult LoadFiles(string path)
        {
            return this.LoadFiles(path, false);
        }

        /// <summary>
        /// Loads the file(s) in the given path; recursively if specified.
        /// </summary>
        /// <param name="path">The path in which to load files.</param>
        /// <param name="recurse">Parameter specifying if to do a recursive search.</param>
        public FileLoadingResult LoadFiles(string path, bool recurse)
        {
            return this.LoadFiles(path, recurse, "*.*");
        }

        /// <summary>
        /// Loads the file(s) in the given path with the provided pattern; recursively if specified.
        /// </summary>
        /// <param name="path">The path in which to load files.</param>
        /// <param name="recurse">Parameter specifying if to do a recursive search.</param>
        /// <param name="pattern">The pattern that files have to fulfill in order to be loaded.</param>
        public FileLoadingResult LoadFiles(string path, bool recurse, string pattern)
        {
            string[] files = Directory.GetFiles(path, pattern, recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            int successes = 0;
            
            foreach(string f in files)
            {
                using(FileStream stream = File.OpenRead(f))
                {
                    if(this.OperateOnFile(stream))
                    {
                        successes++;
                    }
                }
            }

            return new FileLoadingResult(successes, files.Length - successes);
        }

        /// <summary>
        /// Template pattern method that is called on each file that is loaded. The stream is disposed after the call.
        /// </summary>
        /// <param name="fileStream">A filestream to an opened file.</param>
        protected abstract bool OperateOnFile(Stream fileStream);
    }
}