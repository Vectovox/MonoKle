namespace MonoKle.Assets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Resources;

    using Microsoft.Xna.Framework.Graphics;

    using MonoKle.Graphics;
    using MonoKle.Resources;

    /// <summary>
    /// Loads and maintains texture assets.
    /// </summary>
    public static class TextureManager
    {
        private static Texture2D defaultTexture;
        private static Dictionary<string, Texture2D> textureStorage = new Dictionary<string, Texture2D>();
        private static Texture2D whiteTexture;

        public static Texture2D GetDefaultTexture()
        {
            return defaultTexture;
        }

        public static Texture2D GetTexture(string id)
        {
            id = id.ToLower();
            if (textureStorage.ContainsKey(id))
            {
                return textureStorage[id];
            }
            return defaultTexture;
        }

        public static Texture2D GetWhiteTexture()
        {
            return whiteTexture;
        }

        public static int Load(string path)
        {
            return Load(path, false);
        }

        public static int Load(string path, bool recurse)
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

        public static void SetDefaultTexture(Texture2D value)
        {
            defaultTexture = value;
        }

        public static int Unload(string id)
        {
            return textureStorage.Remove(id) ? 1 : 0;
        }

        public static int UnloadAll()
        {
            int nUnloaded = textureStorage.Keys.Count;
            textureStorage.Clear();
            return nUnloaded;
        }

        internal static void Initialize()
        {
            defaultTexture = GraphicsHelper.BitmapToTexture2D(GraphicsManager.GetGraphicsDevice(), TextureResources.DefaultTexture);
            whiteTexture = GraphicsHelper.BitmapToTexture2D(GraphicsManager.GetGraphicsDevice(), TextureResources.WhiteTexture);
        }

        private static string GetIdentifier(string path)
        {
            int lastPeriod = path.LastIndexOf('.');
            int lastSlash = path.LastIndexOfAny(new char[] { '\\', '/' });
            if (lastPeriod != -1 && lastSlash != -1 && lastPeriod > lastSlash)
            {
                return path.Substring(lastSlash + 1, lastPeriod - lastSlash - 1);
            }
            return null;
        }

        private static bool IsCompatible(string path)
        {
            return path.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase)
                || path.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase)
                || path.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase);
        }

        private static int LoadDirectory(string path, bool recurse)
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

        private static int LoadFile(string path)
        {
            if (File.Exists(path) && IsCompatible(path))
            {
                String id = GetIdentifier(path).ToLower();
                if (id != null && textureStorage.ContainsKey(id) == false)
                {
                    FileStream stream = File.OpenRead(path);
                    Texture2D tex = Texture2D.FromStream(GraphicsManager.GetGraphicsDevice(), stream);
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