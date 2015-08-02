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

        private Dictionary<string, Texture2D> textureByTextureName = new Dictionary<string, Texture2D>();
        private GraphicsDevice graphicsDevice;

        public TextureManager(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.DefaultTexture = GraphicsHelper.BitmapToTexture2D(graphicsDevice, TextureResources.DefaultTexture);
            this.WhiteTexture = GraphicsHelper.BitmapToTexture2D(graphicsDevice, TextureResources.WhiteTexture);
        }

        /// <summary>
        /// Returns a dictionary containing all loaded textures and their id keys.
        /// </summary>
        /// <returns>Dictionary with id-texture pairs.</returns>
        public Dictionary<string, Texture2D> GetTextures()
        {
            return new Dictionary<string, Texture2D>(this.textureByTextureName);
        }

        /// <summary>
        /// Returns a dictionary containing all loaded textures in the given group, as well as their id keys.
        /// </summary>
        /// <param name="group">The group to get textures from.</param>
        /// <returns>Dictionary with id-texture pairs.</returns>
        public Dictionary<string, Texture2D> GetTextures(string group)
        {
            Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
            if(this.groupByGroupName.ContainsKey(group))
            {
                foreach(string s in this.groupByGroupName[group])
                {
                    textures.Add(s, this.textureByTextureName[s]);
                }
            }
            return textures;
        }

        /// <summary>
        /// Returns a dictionary containing all texture groups and their corresponding id keys.
        /// </summary>
        /// <returns>Dictionary.</returns>
        public Dictionary<string, HashSet<string>> GetTextureGroups()
        {
            return new Dictionary<string, HashSet<string>>(this.groupByGroupName);
        }

        /// <summary>
        /// Retrieves the texture with the given id at constant time. If no texture exists with the given id, <see cref="DefaultTexture"/> is returned.
        /// </summary>
        /// <param name="id">The id of the texture to retrieve.</param>
        /// <returns>Texture2D</returns>
        public Texture2D GetTexture(string id)
        {
            id = id.ToLower();
            if (this.textureByTextureName.ContainsKey(id))
            {
                return this.textureByTextureName[id];
            }
            return this.DefaultTexture;
        }

        public int Load(string path)
        {
            return this.Load(path, false);
        }

        public int Load(string path, bool recurse)
        {
            return this.Load(path, recurse, DEFAULT_GROUP_NAME);
        }

        public int Load(string path, bool recurse, string group)
        {
            if(Path.IsPathRooted(path))
            {
                throw new ArgumentException("Rooted paths are not supported.");
            }

            int nLoaded = 0;
            if(Directory.Exists(path))
            {
                nLoaded = this.LoadDirectory(path, recurse, group);
            }
            else
            {
                nLoaded = this.LoadFile(path, group);
            }
            Logger.Global.Log("Texture loading (" + path + ") complete. " + nLoaded + " texture(s) loaded into group " + group + ".", LogLevel.Info);
            return nLoaded;
        }

        public int LoadAlias(string path, string alias)
        {
            return this.LoadAlias(path, alias, TextureManager.DEFAULT_GROUP_NAME);
        }

        public int LoadAlias(string path, string alias, string group)
        {
            return this.LoadFile(path, group, alias);
        }

        public int LoadAlias(Dictionary<string, string> dictionary)
        {
            return this.LoadAlias(dictionary, TextureManager.DEFAULT_GROUP_NAME);
        }

        public int LoadAlias(Dictionary<string, string> dictionary, string group)
        {
            int counter = 0;
            foreach(string s in dictionary.Keys)
            {
                counter += this.LoadAlias(s, dictionary[s], group);
            }
            return counter;
        }

        public int UnloadTexture(string id)
        {
            if(this.textureByTextureName.ContainsKey(id))
            {
                string groupName = this.groupNameByTextureName[id];
                this.groupByGroupName[groupName].Remove(id);
                // Remove group if empty
                if(this.groupByGroupName[groupName].Count == 0)
                {
                    this.groupByGroupName.Remove(groupName);
                }
                this.textureByTextureName.Remove(id);
                this.groupNameByTextureName.Remove(id);
                return 1;
            }
            return 0;
        }

        public int UnloadGroup(string group)
        {
            int nUnloaded = 0;
            if(this.groupByGroupName.ContainsKey(group))
            {
                HashSet<string> groupContent = new HashSet<string>(this.groupByGroupName[group]);
                foreach(string s in groupContent)
                {
                    nUnloaded += this.UnloadTexture(s);
                }
            }
            return nUnloaded;
        }

        public int UnloadTextures()
        {
            int nUnloaded = this.textureByTextureName.Keys.Count;
            this.textureByTextureName.Clear();
            this.groupByGroupName.Clear();
            this.groupNameByTextureName.Clear();
            return nUnloaded;
        }

        private bool IsCompatible(string path)
        {
            return path.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase)
                || path.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase)
                || path.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase);
        }

        private int LoadDirectory(string path, bool recurse, string group)
        {
            int nLoaded = 0;
            if (Directory.Exists(path))
            {
                // Load files
                foreach (string s in Directory.GetFiles(path))
                {
                    nLoaded += LoadFile(s, group);
                }

                if (recurse)
                {
                    // Recursively load directories
                    foreach (string s in Directory.GetDirectories(path))
                    {
                        nLoaded += this.LoadDirectory(s, recurse, group);
                    }
                }
            }
            return nLoaded;
        }

        private int LoadFile(string path, string group)
        {
            string id = path;
            return this.LoadFile(path, group, id);
        }

        private int LoadFile(string path, string group, string id)
        {
            string finalID = id.ToLower();
            if (File.Exists(path) && this.IsCompatible(path))
            {
                if (finalID != null && this.textureByTextureName.ContainsKey(finalID) == false)
                {
                    Texture2D tex = null;
                    using (FileStream stream = File.OpenRead(path))
                    {
                        tex = Texture2D.FromStream(this.graphicsDevice, stream);
                    }

                    if (tex != null)
                    {
                        this.textureByTextureName.Add(finalID, tex);
                        this.AddToGroup(group, finalID);
                        return 1;
                    }
                }
            }
            return 0;
        }

        private void AddToGroup(string group, string textureId)
        {
            if(this.textureByTextureName.ContainsKey(textureId))
            {
                if(this.groupByGroupName.ContainsKey(group) == false)
                {
                    this.groupByGroupName.Add(group, new HashSet<string>());
                }

                var groupSet = this.groupByGroupName[group];

                if(groupSet.Contains(textureId) == false)
                {
                    groupSet.Add(textureId);
                    this.groupNameByTextureName.Add(textureId, group);
                }
            }
        }

        private Dictionary<string, HashSet<string>> groupByGroupName = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, string> groupNameByTextureName = new Dictionary<string, string>();
        private const string DEFAULT_GROUP_NAME = "default";
    }
}