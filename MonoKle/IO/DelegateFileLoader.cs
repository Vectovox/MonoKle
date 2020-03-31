using System;
using System.IO;

namespace MonoKle.IO
{
    /// <summary>
    /// File loaded that calls on a delegate for the actual loading operation.
    /// </summary>
    public class DelegateFileLoader : AbstractFileReader
    {
        /// <summary>
        /// The delegate used to perform file loading logic.
        /// </summary>
        public FileLoadingDelegate Delegate
        {
            get; set;
        }

        protected override bool ReadFile(Stream fileStream, MFileInfo file)
        {
            if (Delegate == null)
            {
                throw new InvalidOperationException("No assigned file loading delegate.");
            }
            else
            {
                return Delegate(fileStream, file);
            }
        }
    }
}