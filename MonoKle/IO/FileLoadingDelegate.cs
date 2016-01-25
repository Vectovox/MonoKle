namespace MonoKle.IO
{
    using System.IO;

    /// <summary>
    /// Delegate for loading a file.
    /// </summary>
    /// <param name="stream">The stream to the loaded file.</param>
    /// <param name="path">The path of the loaded file.</param>
    /// <returns>True if loading was successful.</returns>
    public delegate bool FileLoadingDelegate(Stream stream, string path);
}