using Microsoft.Xna.Framework;
using MonoKle.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoKle.Asset
{
    public abstract class BasicAssetStorage<T> : BasicAssetStorage<T, T> where T : class
    {
        public BasicAssetStorage(Logger logger) : base(logger) { }
        protected override T GetInstance(T data) => data;
    }

    public abstract class BasicAssetStorage<TData, TInstance> : AbstractAssetStorage where TData : class
    {
        public BasicAssetStorage(Logger logger) : base(logger) { }

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
                _logger.Log($"Tried to access non-existing identifier '{identifier}'.", LogLevel.Warning);
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
                _logger.Log($"Identifier already loaded '{identifier}'.", LogLevel.Error);
                return false;
            }

            try
            {
                if (Load(stream, out var result))
                {
                    _assetStorage.Add(identifier, result!);
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.Log($"Unhandled exception reading asset '{identifier}': {e.Message}", LogLevel.Error);
            }

            _logger.Log($"Could not read asset file for '{identifier}'.", LogLevel.Error);
            return false;
        }

        protected override bool Load(string path, string identifier, string[] args)
        {
            try
            {
                using var stream = TitleContainer.OpenStream(path);
                return Load(stream, identifier);
            }
            catch (FileNotFoundException)
            {
                _logger.Log($"File not found in path '{path}'.", LogLevel.Error);
            }

            return false;
        }

        protected abstract bool Load(Stream stream, out TData? result);

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