namespace MonoKle.Asset
{
    using MonoKle.IO;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Abstract class for loading, storing, and retrieving MonoKle assets.
    /// </summary>
    /// <typeparam name="T">Type of asset to store.</typeparam>
    public abstract class AbstractAssetStorage<T> : AbstractFileReader
    {
        private Dictionary<string, T> assetStorage = new Dictionary<string, T>();
        private string currentGroup = null;
        private Dictionary<string, HashSet<string>> groupDictionary = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, string> groupFromAsset = new Dictionary<string, string>();

        /// <summary>
        /// Gets the amount of assets.
        /// </summary>
        /// <value>
        /// The asset count.
        /// </value>
        public int AssetCount { get { return this.assetStorage.Count; } }

        /// <summary>
        /// Gets or sets the default value, returned whenever trying to access a non-existing asset.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public T DefaultValue { get; set; }

        /// <summary>
        /// Gets the amount of groups.
        /// </summary>
        /// <value>
        /// The group count.
        /// </value>
        public int GroupCount { get { return this.groupDictionary.Count; } }

        /// <summary>
        /// Gets the asset with the associated id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public T GetAsset(string id)
        {
            id = id.ToLower();
            if (this.assetStorage.ContainsKey(id))
            {
                return this.assetStorage[id];
            }
            else
            {
                Logging.Logger.Global.Log("Asset not found: " + id, Logging.LogLevel.Warning);
            }
            return this.DefaultValue;
        }

        /// <summary>
        /// Gets the group of the specified asset.
        /// </summary>
        /// <param name="asset">The asset to get the group for.</param>
        /// <returns></returns>
        public string GetAssetGroup(string asset)
        {
            if (this.groupFromAsset.ContainsKey(asset))
            {
                return this.groupFromAsset[asset];
            }
            return null;
        }

        /// <summary>
        /// Gets all asset identifiers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAssetIdentifiers()
        {
            return new List<string>(this.assetStorage.Keys);
        }

        /// <summary>
        /// Gets all asset identifiers in the specified group.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAssetIdentifiers(string group)
        {
            if (this.groupDictionary.ContainsKey(group))
            {
                return new List<string>(this.groupDictionary[group]);
            }
            return new List<string>();
        }

        /// <summary>
        /// Gets all groups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetGroups()
        {
            return new List<string>(this.groupDictionary.Keys);
        }

        /// <summary>
        /// Loads the file with the provided path, giving it the specified id and optional group.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="id">The identifier to use for the file.</param>
        /// <param name="group">The optinal group to use. May be null.</param>
        /// <returns></returns>
        public FileLoadingResult LoadFileID(string path, string id, string group = null)
        {
            this.currentGroup = group;
            FileLoadingResult result = base.LoadFile(path);
            this.currentGroup = null;
            if (result.Successes > 0)
            {
                T value = this.assetStorage[path];
                this.assetStorage.Remove(path);
                this.assetStorage.Add(id, value);

                if (group != null)
                {
                    this.groupDictionary[group].Remove(path);
                    this.groupDictionary[group].Add(id);
                    this.groupFromAsset.Remove(path);
                    this.groupFromAsset.Add(id, group);
                }
            }
            return result;
        }

        /// <summary>
        /// Loads the files in the given path, adding them to the provided group.
        /// </summary>
        /// <param name="path">The path to load files from.</param>
        /// <param name="recurse">Specifiec whether to recursively find files.</param>
        /// <param name="group">The group to add the files to.</param>
        /// <param name="pattern">Pattern to use when finding files.</param>
        /// <returns></returns>
        public FileLoadingResult LoadFilesGroup(string path, bool recurse, string group, string pattern = "*.*")
        {
            this.currentGroup = group;
            FileLoadingResult result = base.LoadFiles(path, recurse, pattern);
            this.currentGroup = null;
            return result;
        }

        /// <summary>
        /// Loads data from stream.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="id">The id to assign the asset.</param>
        /// <returns>True if successful; otherwise false.</returns>
        public bool LoadStream(Stream stream, string id)
        {
            id = id.ToLower();
            if (this.assetStorage.ContainsKey(id) == false)
            {
                T value = this.DoLoadStream(stream);
                if (value != null)
                {
                    this.assetStorage.Add(id, value);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Loads asset from stream, placing it in the specified asset group.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="id">The id to assign the asset.</param>
        /// <param name="group">Group to place the asset in.</param>
        /// <returns>True if successful; otherwise false.</returns>
        public bool LoadStream(Stream stream, string id, string group)
        {
            bool result = this.LoadStream(stream, id);
            if (result)
            {
                if (this.groupDictionary.ContainsKey(group) == false)
                {
                    this.groupDictionary.Add(group, new HashSet<string>());
                }
                this.groupDictionary[group].Add(id);
                this.groupFromAsset.Add(id, group);
            }
            return result;
        }

        /// <summary>
        /// Unloads all assets.
        /// </summary>
        /// <returns>Integer representing the amount of unloaded entries.</returns>
        public int UnloadAll()
        {
            int nUnloaded = this.assetStorage.Keys.Count;
            this.assetStorage.Clear();
            this.groupDictionary.Clear();
            this.groupFromAsset.Clear();
            Logging.Logger.Global.Log("Unloaded all assets", Logging.LogLevel.Info);
            return nUnloaded;
        }

        /// <summary>
        /// Unloads the asset with the given identifier.
        /// </summary>
        /// <param name="id">The identifier of the entry to unload.</param>
        /// <returns>Integer representing the amount of unloaded entries.</returns>
        public int UnloadAsset(string id)
        {
            if (this.assetStorage.Remove(id))
            {
                if (this.groupFromAsset.ContainsKey(id))
                {
                    this.groupDictionary[this.groupFromAsset[id]].Remove(id);
                    if (this.groupDictionary[this.groupFromAsset[id]].Count == 0)
                    {
                        this.groupDictionary.Remove(this.groupFromAsset[id]);
                    }
                    this.groupFromAsset.Remove(id);
                }
                Logging.Logger.Global.Log("Unloaded asset:" + id, Logging.LogLevel.Debug);
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Unloads all assets within the provided group.
        /// </summary>
        /// <param name="group">The group to remove assets belonging to.</param>
        /// <returns>Amount of assets removed.</returns>
        public int UnloadGroup(string group)
        {
            int n = 0;
            if (this.groupDictionary.ContainsKey(group))
            {
                foreach (string s in this.groupDictionary[group])
                {
                    n += this.UnloadAsset(s);
                }
            }
            return n;
        }

        protected abstract T DoLoadStream(Stream stream);

        protected override bool ReadFile(Stream fileStream, MFileInfo file)
        {
            if (this.currentGroup == null)
            {
                return this.LoadStream(fileStream, file.OriginalPath);
            }
            else
            {
                return this.LoadStream(fileStream, file.OriginalPath, this.currentGroup);
            }
        }
    }
}