namespace MonoKle.Assets.Font
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Microsoft.Xna.Framework.Graphics;

    using MonoKle.Graphics;
    using MonoKle.Resources;

    /// <summary>
    /// Manages drawable fonts.
    /// </summary>
    public class FontManager
    {
        private Dictionary<string, Font> fontStorage = new Dictionary<string, Font>();
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontManager"/> class.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device</param>
        public FontManager(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

            // Set up default font
            Stream dataStream = new MemoryStream(FontResources.DefaultFont);
            FontFile fontData = FontLoader.Load(dataStream);
            Texture2D fontTexture = GraphicsHelper.BitmapToTexture2D(graphicsDevice, TextureResources.DefaultFont);
            DefaultFont = new Font(fontData, fontTexture);
        }

        /// <summary>
        /// Gets or sets the default font that will be used when retrieving a non-existing font.
        /// </summary>
        public Font DefaultFont
        {
            get; set;
        }

        /// <summary>
        /// Returns the font with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the font to return.</param>
        /// <returns>A <see cref="Font"/></returns>
        public Font GetFont(string id)
        {
            id = id.ToLower();
            if (this.fontStorage.ContainsKey(id))
            {
                return this.fontStorage[id];
            }
            return DefaultFont;
        }

        /// <summary>
        /// Loads the given font or the given directory.
        /// </summary>
        /// <param name="path">Path of font or directory.</param>
        /// <returns>Integer representing the amount of loaded fonts.</returns>
        public int Load(string path)
        {
            return Load(path, false);
        }

        /// <summary>
        /// Loads the given font or the given directory.
        /// </summary>
        /// <param name="path">Path of font or directory.</param>
        /// <param name="recurse">If true, indicates that directory search will be recursive.</param>
        /// <returns>Integer representing the amount of loaded fonts.</returns>
        public int Load(string path, bool recurse)
        {
            if (Directory.Exists(path))
            {
                return LoadDirectory(path, recurse);
            }
            else
            {
                return LoadFile(path);
            }
        }

        /// <summary>
        /// Unloads the font with the given identifier.
        /// </summary>
        /// <param name="id">The identifier of the font to unload.</param>
        /// <returns>Integer representing the amount of unloaded fonts.</returns>
        public int Unload(string id)
        {
            return this.fontStorage.Remove(id) ? 1 : 0;
        }

        /// <summary>
        /// Unloads all fonts.
        /// </summary>
        /// <returns>Integer representing the amount of unloaded fonts.</returns>
        public int UnloadAll()
        {
            int nUnloaded = this.fontStorage.Keys.Count;
            this.fontStorage.Clear();
            return nUnloaded;
        }

        private string GetFolder(string path)
        {
            int lastPeriod = path.LastIndexOf('.');
            int lastSlash = path.LastIndexOfAny(new char[] { '\\', '/' });
            if (lastPeriod != -1 && lastSlash != -1 && lastPeriod > lastSlash)
            {
                return path.Substring(lastSlash + 1, lastPeriod - lastSlash - 1);
            }
            return null;
        }

        private string GetIdentifier(string path)
        {
            int lastPeriod = path.LastIndexOf('.');
            int lastSlash = path.LastIndexOfAny(new char[] { '\\', '/' });
            if (lastPeriod != -1 && lastSlash != -1 && lastPeriod > lastSlash)
            {
                return path.Substring(lastSlash + 1, lastPeriod - lastSlash - 1);
            }
            return null;
        }

        private bool IsCompatible(string path)
        {
            return path.EndsWith(".fnt", StringComparison.CurrentCultureIgnoreCase);
        }

        private int LoadDirectory(string path, bool recurse)
        {
            int nLoaded = 0;
            if (Directory.Exists(path))
            {
                // Load files
                foreach (string s in Directory.GetFiles(path))
                {
                    nLoaded += LoadFile(s);
                }

                if (recurse)
                {
                    // Recursively load directories
                    foreach (string s in Directory.GetDirectories(path))
                    {
                        nLoaded += LoadDirectory(s, recurse);
                    }
                }
            }
            return nLoaded;
        }

        private int LoadFile(string path)
        {
            if (File.Exists(path) && IsCompatible(path))
            {
                string id = GetIdentifier(path).ToLower();
                if (id != null && this.fontStorage.ContainsKey(id) == false)
                {
                    DirectoryInfo parent = Directory.GetParent(path);
                    string imagePath = parent.FullName + "\\" + id + ".png";
                    if (File.Exists(imagePath))
                    {
                        FileStream dataStream = File.OpenRead(path);
                        FontFile fontFile = FontLoader.Load(dataStream);
                        dataStream.Close();

                        FileStream imageStream = File.OpenRead(imagePath);
                        Texture2D tex = Texture2D.FromStream(this.graphicsDevice, imageStream);
                        imageStream.Close();

                        if (tex != null)
                        {
                            this.fontStorage.Add(id, new Font(fontFile, tex));
                            return 1;
                        }
                    }
                }
            }
            return 0;
        }
    }
}