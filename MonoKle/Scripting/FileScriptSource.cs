namespace MonoKle.Scripting
{
    using IO;
    using System;
    using System.IO;

    /// <summary>
    /// Script source from file.
    /// </summary>
    /// <seealso cref="MonoKle.Scripting.IScriptSource" />
    public class FileScriptSource : IScriptSource
    {
        private MFileInfo file;
        private string cache = "";
        private DateTime cacheDate = DateTime.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileScriptSource"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public FileScriptSource(MFileInfo file)
        {
            this.file = new MFileInfo(file.FullPath);
        }

        /// <summary>
        /// Gets the source code.
        /// </summary>
        /// <value>
        /// The source code.
        /// </value>
        public string Code
        {
            get
            {
                file.Update();
                if (file.Exists)
                {
                    if (this.Date > this.cacheDate)
                    {
                        try
                        {
                            using (StreamReader sr = new StreamReader(file.OpenRead()))
                            {
                                this.cache = sr.ReadToEnd();
                                this.cacheDate = this.Date;
                            }
                        }
                        catch { this.cache = ""; }
                    }
                }
                else
                {
                    this.cache = "";
                }

                return this.cache;
            }
        }

        /// <summary>
        /// Gets the source date in UTC.
        /// </summary>
        /// <value>
        /// The source date in UTC.
        /// </value>
        public DateTime Date
        {
            get
            {
                file.Update();
                return file.LastWriteTimeUtc;
            }
        }
    }
}