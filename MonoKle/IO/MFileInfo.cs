namespace MonoKle.IO
{
    using System;
    using System.IO;

    /// <summary>
    /// File information.
    /// </summary>
    public class MFileInfo
    {
        private FileInfo file;

        /// <summary>
        /// Initializes a new instance of the <see cref="MFileInfo"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public MFileInfo(string path)
        {
            this.OriginalPath = path;
            this.Update();
        }

        public DateTime CreationTime => file.CreationTime;
        public DateTime CreationTimeUtc => file.CreationTimeUtc;
        public string DirectoryName => file.DirectoryName;
        public bool Exists => file.Exists;
        public string Extension => file.Extension;
        public string FullPath => file.FullName;

        public DateTime LastAccessTime => file.LastAccessTime;
        public DateTime LastAccessTimeUtc => file.LastAccessTimeUtc;
        public DateTime LastWriteTime => file.LastWriteTime;
        public DateTime LastWriteTimeUtc => file.LastWriteTimeUtc;
        public string Name => file.Name;
        public string NameWithoutExtension => Path.GetFileNameWithoutExtension(Name);
        public string OriginalPath { get; private set; }

        public FileStream Open(FileMode mode) => file.Open(mode);

        public FileStream Open(FileMode mode, FileAccess access) => file.Open(mode, access);

        public FileStream Open(FileMode mode, FileAccess access, FileShare share) => file.Open(mode, access, share);

        public FileStream OpenRead() => file.OpenRead();

        public FileStream OpenWrite() => file.OpenWrite();

        public void Update()
        {
            this.file = new FileInfo(this.OriginalPath);
        }
    }
}