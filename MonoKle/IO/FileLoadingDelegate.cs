using System.IO;

namespace MonoKle.IO
{
    /// <summary>
    /// Delegate for loading a file.
    /// </summary>
    /// <param name="stream">The stream to the loaded file.</param>
    /// <param name="file">The loaded file.</param>
    /// <returns>True if loading was successful.</returns>
    public delegate bool FileLoadingDelegate(Stream stream, MFileInfo file);
}