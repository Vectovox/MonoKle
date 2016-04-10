namespace MonoKle.IO
{
    using System.IO;

    /// <summary>
    /// Abstract implementation of <see cref="IFileLoader"/>, providing a template pattern method for file loading operations.
    /// </summary>
    public abstract class AbstractFileReader : AbstractFileFinder
    {
        /// <summary>
        /// Template pattern method that is called on each file that is found.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        protected override bool OperateOnFile(MFileInfo file)
        {
            if(file.Exists)
            {
                try
                {
                    using (Stream stream = file.OpenRead())
                    {
                        this.ReadFile(stream, file);
                        return true;
                    }
                }
                catch (IOException e)
                {

                }
            }
            return false;
        }

        /// <summary>
        /// Template pattern method that is called on each file that is loaded. The stream is disposed after the call.
        /// </summary>
        /// <param name="fileStream">A filestream to a read file.</param>
        /// <param name="file">The read file.</param>
        protected abstract bool ReadFile(Stream fileStream, MFileInfo file);
    }
}