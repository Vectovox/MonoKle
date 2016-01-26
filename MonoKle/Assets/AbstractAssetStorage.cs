namespace MonoKle.Assets
{
    using MonoKle.IO;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Abstract class for loading, storing, and retrieving MonoKle assets.
    /// </summary>
    /// <typeparam name="T">Type of asset to store.</typeparam>
    public abstract class AbstractAssetStorage<T> : AbstractFileLoader
    {
        private string currentGroup = null;
        private Dictionary<string, HashSet<string>> groupDictionary = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, string> groupFromEntry = new Dictionary<string, string>();
        private Dictionary<string, T> storage = new Dictionary<string, T>();

        /// <summary>
        /// Gets or sets the default value, returned whenever trying to access a non-existing asset.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public T DefaultValue { get; set; }

        /// <summary>
        /// Gets the asset with the associated id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public T GetAsset(string id)
        {
            id = id.ToLower();
            if (this.storage.ContainsKey(id))
            {
                return this.storage[id];
            }
            return this.DefaultValue;
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
                T value = this.storage[path];
                this.storage.Remove(path);
                this.storage.Add(id, value);

                if (group != null)
                {
                    this.groupDictionary[group].Remove(path);
                    this.groupDictionary[group].Add(id);
                    this.groupFromEntry.Remove(path);
                    this.groupFromEntry.Add(id, group);
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
            if (this.storage.ContainsKey(id) == false)
            {
                T value = this.DoLoadStream(stream);
                if (value != null)
                {
                    this.storage.Add(id, value);
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
                this.groupFromEntry.Add(id, group);
            }
            return result;
        }

        /// <summary>
        /// Unloads all assets.
        /// </summary>
        /// <returns>Integer representing the amount of unloaded entries.</returns>
        public int UnloadAll()
        {
            int nUnloaded = this.storage.Keys.Count;
            this.storage.Clear();
            this.groupDictionary.Clear();
            this.groupFromEntry.Clear();
            return nUnloaded;
        }

        /// <summary>
        /// Unloads the asset with the given identifier.
        /// </summary>
        /// <param name="id">The identifier of the entry to unload.</param>
        /// <returns>Integer representing the amount of unloaded entries.</returns>
        public int UnloadAsset(string id)
        {
            if (this.storage.Remove(id))
            {
                if (this.groupFromEntry.ContainsKey(id))
                {
                    this.groupDictionary[this.groupFromEntry[id]].Remove(id);
                    if (this.groupDictionary[this.groupFromEntry[id]].Count == 0)
                    {
                        this.groupDictionary.Remove(this.groupFromEntry[id]);
                    }
                    this.groupFromEntry.Remove(id);
                }
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

        protected override bool OperateOnFile(Stream fileStream, string filePath)
        {
            if (this.currentGroup == null)
            {
                return this.LoadStream(fileStream, filePath);
            }
            else
            {
                return this.LoadStream(fileStream, filePath, this.currentGroup);
            }
        }
    }
}