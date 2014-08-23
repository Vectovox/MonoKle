namespace MonoKle.IO.FileLoading
{
    using System;
    using System.IO;

    /// <summary>
    /// File loaded that calls on a delegate for the actual loading operation.
    /// </summary>
    public class DelegateFileLoader : AbstractFileLoader
    {
        /// <summary>
        /// The delegate used to perform file loading logic.
        /// </summary>
        public FileLoadingDelegate Delegate
        {
            get; set;
        }

        protected override bool OperateOnFile(Stream fileStream, string filePath)
        {
            if(this.Delegate == null)
            {
                throw new InvalidOperationException("No assigned file loading delegate.");
            }
            else
            {
                return Delegate(fileStream, filePath);
            }
        }
    }
}