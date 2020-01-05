namespace MonoKle.Asset
{
    using MonoKle.IO;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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
        public int AssetCount => assetStorage.Count;

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
        public int GroupCount => groupDictionary.Count;

        /// <summary>
        /// Gets the asset with the associated id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public T GetAsset(string id)
        {
            id = id.ToLower();
            if (assetStorage.ContainsKey(id))
            {
                return assetStorage[id];
            }
            else
            {
                Logging.Logger.Global.Log("Asset not found: " + id, Logging.LogLevel.Warning);
            }
            return DefaultValue;
        }

        /// <summary>
        /// Gets the group of the specified asset.
        /// </summary>
        /// <param name="asset">The asset to get the group for.</param>
        /// <returns></returns>
        public string GetAssetGroup(string asset)
        {
            asset = asset.ToLower();
            if (groupFromAsset.ContainsKey(asset))
            {
                return groupFromAsset[asset];
            }
            return null;
        }

        /// <summary>
        /// Gets all asset identifiers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAssetIdentifiers() => new List<string>(assetStorage.Keys);

        /// <summary>
        /// Gets all asset identifiers in the specified group.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAssetIdentifiers(string group)
        {
            group = group.ToLower();
            if (groupDictionary.ContainsKey(group))
            {
                return new List<string>(groupDictionary[group]);
            }
            return new List<string>();
        }

        /// <summary>
        /// Gets all groups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetGroups() => new List<string>(groupDictionary.Keys);

        /// <summary>
        /// Loads the file with the provided path, giving it the specified id and optional group.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="id">The identifier to use for the file.</param>
        /// <param name="group">The optinal group to use. May be null.</param>
        /// <returns></returns>
        public FileLoadingResult LoadFileId(string path, string id, string group = null)
        {
            path = path.ToLower();
            id = id.ToLower();

            // If asset already exists, create an alias for it
            if (assetStorage.ContainsKey(path))
            {
                if (AddAsset(assetStorage[path], id, group))
                {
                    return new FileLoadingResult(new List<string> { path }, 0);
                }
                return new FileLoadingResult(new List<string>(), 1);
            }
            else
            {
                // Otherwise add new item and modify it
                currentGroup = group;
                FileLoadingResult result = base.LoadFile(path);
                currentGroup = null;

                if (result.Successes > 0)
                {
                    T value = assetStorage[path];

                    if (UnloadAsset(path) != 0)
                    {
                        AddAsset(value, id, group);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Loads the file in the given path, adding it to the provided group.
        /// </summary>
        /// <param name="path">The path to load files from.</param>
        /// <param name="group">The group to add the files to.</param>
        public FileLoadingResult LoadFileGroup(string path, string group)
        {
            currentGroup = group;
            FileLoadingResult result = base.LoadFile(path);
            currentGroup = null;
            return result;
        }

        /// <summary>
        /// Loads the files in the given path, adding them to the provided group.
        /// </summary>
        /// <param name="path">The path to load files from.</param>
        /// <param name="recurse">Specifiec whether to recursively find files.</param>
        /// <param name="group">The group to add the files to.</param>
        /// <param name="pattern">Pattern to use when finding files.</param>
        public FileLoadingResult LoadFilesGroup(string path, bool recurse, string group, string pattern = "*.*")
        {
            currentGroup = group;
            FileLoadingResult result = base.LoadFiles(path, recurse, pattern);
            currentGroup = null;
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
            if (assetStorage.ContainsKey(id) == false)
            {
                T value = DoLoadStream(stream);
                return AddAsset(value, id);
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
            id = id.ToLower();
            if (assetStorage.ContainsKey(id) == false)
            {
                T value = DoLoadStream(stream);
                return AddAsset(value, id, group);
            }
            return false;
        }

        /// <summary>
        /// Unloads all assets.
        /// </summary>
        /// <returns>Integer representing the amount of unloaded entries.</returns>
        public int UnloadAll()
        {
            int nUnloaded = assetStorage.Keys.Count;
            assetStorage.Clear();
            groupDictionary.Clear();
            groupFromAsset.Clear();
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
            id = id.ToLower();
            if (assetStorage.Remove(id))
            {
                if (groupFromAsset.ContainsKey(id))
                {
                    groupDictionary[groupFromAsset[id]].Remove(id);
                    if (groupDictionary[groupFromAsset[id]].Count == 0)
                    {
                        groupDictionary.Remove(groupFromAsset[id]);
                    }
                    groupFromAsset.Remove(id);
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
            group = group.ToLower();
            int n = 0;
            if (groupDictionary.ContainsKey(group))
            {
                foreach (string s in groupDictionary[group].ToList())
                {
                    n += UnloadAsset(s);
                }
            }
            return n;
        }

        protected abstract T DoLoadStream(Stream stream);

        protected override bool ReadFile(Stream fileStream, MFileInfo file)
        {
            if (currentGroup == null)
            {
                return LoadStream(fileStream, file.OriginalPath);
            }
            else
            {
                return LoadStream(fileStream, file.OriginalPath, currentGroup);
            }
        }

        private bool AddAsset(T value, string id, string group = null)
        {
            if (id != null && !assetStorage.ContainsKey(id) && value != null)
            {
                assetStorage.Add(id, value);
                if (group != null)
                {
                    if (groupDictionary.ContainsKey(group) == false)
                    {
                        groupDictionary.Add(group, new HashSet<string>());
                    }
                    groupDictionary[group].Add(id);
                    groupFromAsset.Add(id, group);
                }
                return true;
            }
            return false;
        }
    }
}