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
        private Dictionary<string, T> storage = new Dictionary<string, T>();

        /// <summary>
        /// Gets or sets the default value, returned whenever trying to access a non-existing entry.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public T DefaultValue { get; set; }

        /// <summary>
        /// Gets the data with the associated id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public T GetData(string id)
        {
            id = id.ToLower();
            if (this.storage.ContainsKey(id))
            {
                return this.storage[id];
            }
            return DefaultValue;
        }

        /// <summary>
        /// Loads data from stream.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <returns>True if successful; otherwise false.</returns>
        public bool LoadStream(Stream stream, string id)
        {
            if (this.storage.ContainsKey(id) == false)
            {
                T value = this.DoLoadStream(stream, id);
                if (value != null)
                {
                    this.storage.Add(id, value);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Unloads the entry with the given identifier.
        /// </summary>
        /// <param name="id">The identifier of the entry to unload.</param>
        /// <returns>Integer representing the amount of unloaded entries.</returns>
        public int Unload(string id)
        {
            return this.storage.Remove(id) ? 1 : 0;
        }

        /// <summary>
        /// Unloads all entries.
        /// </summary>
        /// <returns>Integer representing the amount of unloaded entries.</returns>
        public int UnloadAll()
        {
            int nUnloaded = this.storage.Keys.Count;
            this.storage.Clear();
            return nUnloaded;
        }

        protected abstract T DoLoadStream(Stream stream, string id);

        protected override bool OperateOnFile(Stream fileStream, string filePath)
        {
            return this.LoadStream(fileStream, this.IdFromPath(filePath));
        }

        private string IdFromPath(string path)
        {
            return path;
        }
    }
}