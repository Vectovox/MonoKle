namespace MonoKle.Scripting {
    using System;
    using System.IO;
    using IO;

    /// <summary>
    /// Script source from file.
    /// </summary>
    /// <seealso cref="IScriptSource" />
    public class FileScriptSource : IScriptSource {
        private MFileInfo file;
        private string cache = "";
        private DateTime cacheDate = DateTime.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileScriptSource"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public FileScriptSource(MFileInfo file) {
            this.file = new MFileInfo(file.FullPath);
        }

        /// <summary>
        /// Gets the source code.
        /// </summary>
        /// <value>
        /// The source code.
        /// </value>
        public string Code {
            get {
                file.Update();
                if (file.Exists) {
                    if (Date > cacheDate) {
                        try {
                            using (var sr = new StreamReader(file.OpenRead())) {
                                cache = sr.ReadToEnd();
                                cacheDate = Date;
                            }
                        } catch { cache = ""; }
                    }
                } else {
                    cache = "";
                }

                return cache;
            }
        }

        /// <summary>
        /// Gets the source date in UTC.
        /// </summary>
        /// <value>
        /// The source date in UTC.
        /// </value>
        public DateTime Date {
            get {
                file.Update();
                return file.LastWriteTimeUtc;
            }
        }
    }
}