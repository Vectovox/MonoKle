using Microsoft.Xna.Framework;
using MonoKle.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonoKle.Asset
{
    /// <summary>
    /// Abstract class for loading, storing, and retrieving MonoKle assets.
    /// </summary>
    /// <typeparam name="T">Type of asset to store.</typeparam>
    public abstract class AbstractAssetStorage<T>
    {
        private Dictionary<string, T> assetStorage = new Dictionary<string, T>();
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
        public T GetAsset(string id)
        {
            id = id.ToLower();
            if (assetStorage.TryGetValue(id, out T value))
            {
                return value;
            }
            Logger.Global.Log($"Asset not found: {id}", LogLevel.Warning);
            return DefaultValue;
        }

        /// <summary>
        /// Gets the group of the specified asset.
        /// </summary>
        /// <param name="asset">The asset id to get the group for.</param>
        public string GetAssetGroup(string id)
        {
            if (groupFromAsset.TryGetValue(id.ToLower(), out var group))
            {
                return group;
            }
            return null;
        }

        /// <summary>
        /// Gets all asset identifiers.
        /// </summary>
        public IEnumerable<string> GetAssetIdentifiers() => assetStorage.Keys.ToList();

        /// <summary>
        /// Gets all asset identifiers in the specified group.
        /// </summary>
        public IEnumerable<string> GetAssetIdentifiers(string group)
        {
            if (groupDictionary.TryGetValue(group.ToLower(), out var identifiers))
            {
                return identifiers;
            }
            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets all groups.
        /// </summary>
        public IEnumerable<string> GetGroups() => groupDictionary.Keys.ToList();

        /// <summary>
        /// Load the asset manifest file.
        /// </summary>
        public int LoadFromManifest()
        {
            try
            {
                (var manifestStream, var prefix) = GetManifest();
                using StreamReader reader = new StreamReader(manifestStream);

                int counter = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (Load(GetAssetPath(prefix, line), line, null))
                    {
                        counter++;
                    }
                    else
                    {
                        Logger.Global.Log($"Could not load asset '{line}' from manifest", LogLevel.Error);
                    }
                }
                return counter;
            }
            catch (IOException)
            {
                return 0;
            }
        }

        // Like Path.Combine() but with forward slash to be compatible with android (possible *nix too?)
        private string GetAssetPath(string prefix, string assetPath) =>
            prefix.Length == 0
            ? assetPath
            : $"{prefix}/{assetPath}";

        // Loads expected manifest file and returns the stream to it + the expected asset prefix
        private (Stream, string) GetManifest()
        {
            // Android
            try
            {
                return (TitleContainer.OpenStream("assets.manifest"), string.Empty);
            }
            catch (IOException)
            {
            }

            // PC
            return (TitleContainer.OpenStream("Assets/assets.manifest"), "Assets");
        }

        /// <summary>
        /// Loads the asset in the relative asset path, using the given id, adding it to the provided group.
        /// </summary>
        /// <param name="path">The path to load files from.</param>
        /// <param name="id">The identifier to use for the asset.</param>
        /// <param name="group">The group to add the files to.</param>
        public bool Load(string path, string id, string group)
        {
            if (!FileSupported(new FileInfo(path).Extension))
            {
                return false;
            }

            try
            {
                var stream = TitleContainer.OpenStream(path);
                return Load(stream, id, group);
            }
            catch (IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// Loads data from stream.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="id">The id to assign the asset.</param>
        /// <returns>True if successful; otherwise false.</returns>
        public bool Load(Stream stream, string id) => Load(stream, id, null);

        /// <summary>
        /// Loads asset from stream, placing it in the specified asset group.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="id">The id to assign the asset.</param>
        /// <param name="group">Group to place the asset in.</param>
        /// <returns>True if successful; otherwise false.</returns>
        public bool Load(Stream stream, string id, string group)
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
        /// Unloads the asset with the given identifier, returning the amount of assets unloaded.
        /// </summary>
        /// <param name="id">The identifier of the entry to unload.</param>
        public int Unload(string id)
        {
            id = id.ToLower();
            if (assetStorage.Remove(id))
            {
                if (groupFromAsset.TryGetValue(id, out var group))
                {
                    groupDictionary[group].Remove(id);
                    if (groupDictionary[group].Count == 0)
                    {
                        groupDictionary.Remove(group);
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
        public int UnloadGroup(string group)
        {
            group = group.ToLower();
            int n = 0;
            if (groupDictionary.ContainsKey(group))
            {
                foreach (string s in groupDictionary[group].ToList())
                {
                    n += Unload(s);
                }
            }
            return n;
        }

        protected abstract T DoLoadStream(Stream stream);

        protected abstract bool FileSupported(string extension);

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