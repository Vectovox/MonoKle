namespace MonoKle.Assets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Resources;

    using Microsoft.Xna.Framework.Graphics;

    using MonoKle.Graphics;
    using MonoKle.Resources;
    using MonoKle.Logging;

    /// <summary>
    /// Loads and maintains texture assets.
    /// </summary>
    public class TextureManager
    {
        public Texture2D DefaultTexture { get; set; }
        public Texture2D WhiteTexture { get; private set; }

        private Dictionary<string, Texture2D> textureStorage = new Dictionary<string, Texture2D>();
        private GraphicsDevice graphicsDevice;

        public TextureManager(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            DefaultTexture = GraphicsHelper.BitmapToTexture2D(graphicsDevice, TextureResources.DefaultTexture);
            WhiteTexture = GraphicsHelper.BitmapToTexture2D(graphicsDevice, TextureResources.WhiteTexture);
        }

        public Texture2D GetTexture(string id)
        {
            id = id.ToLower();
            if (textureStorage.ContainsKey(id))
            {
                return textureStorage[id];
            }
            return DefaultTexture;
        }

        public int Load(string path)
        {
            return Load(path, false);
        }

        public int Load(string path, bool recurse)
        {
            int nLoaded = 0;
            if (Directory.Exists(path))
            {
                nLoaded = LoadDirectory(path, recurse);
            }
            else
            {
                nLoaded = LoadFile(path);
            }
            Logger.GetGlobalInstance().AddLog("Texture loading (" + path + ") complete. " + nLoaded + " texture(s) loaded.", LogLevel.Info);
            return nLoaded;
        }

        public int Unload(string id)
        {
            return textureStorage.Remove(id) ? 1 : 0;
        }

        public int UnloadAll()
        {
            int nUnloaded = textureStorage.Keys.Count;
            textureStorage.Clear();
            return nUnloaded;
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
            return path.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase)
                || path.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase)
                || path.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase);
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
                String id = GetIdentifier(path).ToLower();
                if (id != null && textureStorage.ContainsKey(id) == false)
                {
                    FileStream stream = File.OpenRead(path);
                    Texture2D tex = Texture2D.FromStream(graphicsDevice, stream);
                    stream.Close();

                    if (tex != null)
                    {
                        textureStorage.Add(id, tex);
                        return 1;
                    }
                }
            }
            return 0;
        }
    }
}