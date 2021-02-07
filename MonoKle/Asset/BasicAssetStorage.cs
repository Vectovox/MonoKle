using Microsoft.Xna.Framework;
using MonoKle.Logging;
using System.Collections.Generic;
using System.IO;

namespace MonoKle.Asset
{
    public abstract class BasicAssetStorage<T> : BasicAssetStorage<T, T>
    {
        protected override T GetInstance(T data) => data;
    }

    public abstract class BasicAssetStorage<TData, TInstance> : AbstractAssetStorage
    {
        private readonly Dictionary<string, TData> _assetStorage = new Dictionary<string, TData>();

        /// <summary>
        /// Gets or sets the default asset.
        /// </summary>
        public TInstance Default { get; set; }

        /// <summary>
        /// Accesses the asset with the given identifier. If the asset does not exist
        /// the <see cref="Default"/> asset will be returned.
        /// </summary>
        /// <param name="identifier">The identifier of the asset to get.</param>
        public TInstance this[string identifier]
        {
            get
            {
                if (_assetStorage.TryGetValue(identifier, out TData value))
                {
                    return GetInstance(value);
                }
                Logger.Global.Log($"Tried to access non-existing identifier '{identifier}'.", LogLevel.Warning);
                return Default;
            }
        }

        protected abstract TInstance GetInstance(TData data);

        /// <summary>
        /// Gets the available identifiers.
        /// </summary>
        public IEnumerable<string> Identifiers => _assetStorage.Keys;

        public bool Load(Stream stream, string identifier)
        {
            // Do not allow duplicate identifiers
            if (_assetStorage.ContainsKey(identifier))
            {
                Logger.Global.Log($"Identifier already loaded '{identifier}'. Skipping.", LogLevel.Error);
                return false;
            }

            if (Load(stream, out var result))
            {
                _assetStorage.Add(identifier, result);
                return true;
            }

            Logger.Global.Log($"Could not load asset '{identifier}'. Skipping.", LogLevel.Error);
            return false;
        }

        protected override bool Load(string path, string identifier, string[] args)
        {
            return Load(TitleContainer.OpenStream(path), identifier);
        }

        protected abstract bool Load(Stream stream, out TData result);

        /// <summary>
        /// Unloads all assets, returning the amount of asset identifiers unloaded.
        /// </summary>
        public int Unload()
        {
            int amountUnloaded = _assetStorage.Count;
            _assetStorage.Clear();
            return amountUnloaded;
        }
    }
}