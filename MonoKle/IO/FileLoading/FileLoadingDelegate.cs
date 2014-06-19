namespace MonoKle.IO.FileLoading
{
    using System.IO;

    /// <summary>
    /// Delegate for loading a file.
    /// </summary>
    /// <param name="stream">The stream to the file to load.</param>
    /// <returns>True if loading was successful.</returns>
    public delegate bool FileLoadingDelegate(Stream stream);
}