using System.Collections.Generic;
using System.IO;

namespace MonoKle.IO
{
    /// <summary>
    /// Abstract implementation of <see cref="IFileLoader"/>, providing a template pattern method for file loading operations.
    /// </summary>
    public abstract class AbstractFileFinder : IFileLoader
    {
        /// <summary>
        /// Loads the file with the given path.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        public FileLoadingResult LoadFile(string path)
        {
            var foundFiles = new List<string>();
            int failures = 0;

            var file = new MFileInfo(path);
            if (file.Exists)
            {
                if (CheckFile(file))
                {
                    if (OperateOnFile(file))
                    {
                        foundFiles.Add(path);
                    }
                    else
                    {
                        failures++;
                    }
                }
            }

            return new FileLoadingResult(foundFiles, failures);
        }

        /// <summary>
        /// Loads the files in the given path.
        /// </summary>
        /// <param name="path">The path in which to load files.</param>
        public FileLoadingResult LoadFiles(string path) => LoadFiles(path, false);

        /// <summary>
        /// Loads the files in the given path; recursively if specified.
        /// </summary>
        /// <param name="path">The path in which to load files.</param>
        /// <param name="recurse">Parameter specifying if to do a recursive search.</param>
        public FileLoadingResult LoadFiles(string path, bool recurse) => LoadFiles(path, recurse, "*.*");

        /// <summary>
        /// Loads the files in the given path with the provided pattern; recursively if specified.
        /// </summary>
        /// <param name="path">The path in which to load files.</param>
        /// <param name="recurse">Parameter specifying if to do a recursive search.</param>
        /// <param name="pattern">The pattern that files have to fulfill in order to be loaded.</param>
        public FileLoadingResult LoadFiles(string path, bool recurse, string pattern)
        {
            var foundFiles = new List<string>();
            int failures = 0;

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, pattern, recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                foreach (string f in files)
                {
                    var file = new MFileInfo(f);
                    if (file.Exists)
                    {
                        if (CheckFile(file))
                        {
                            if (OperateOnFile(file))
                            {
                                foundFiles.Add(path);
                            }
                            else
                            {
                                failures++;
                            }
                        }
                    }
                }
            }

            return new FileLoadingResult(foundFiles, failures);
        }

        /// <summary>
        /// Checks if the file is valid.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        protected virtual bool CheckFile(MFileInfo file) => true;

        /// <summary>
        /// Template pattern method that is called on each file that is found.
        /// </summary>
        /// <param name="file">The file.</param>
        protected abstract bool OperateOnFile(MFileInfo file);
    }
}