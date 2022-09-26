﻿using Microsoft.Xna.Framework;
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

        public override bool ContainsIdentifier(string identifier) => _assetStorage.ContainsKey(identifier);

        /// <summary>
        /// Gets the asset with the given identifier.
        /// </summary>
        /// <param name="identifier">The identifier of the asset to get.</param>
        /// <param name="asset">When this method returns, this contains the requested asset. Will be <see cref="Default"/> if the asset does not exist.</param>
        /// <returns>True if the asset was successfully assigned; otherwise false.</returns>
        public bool TryGet(string identifier, out TInstance asset)
        {
            if (ContainsIdentifier(identifier))
            {
                asset = this[identifier];
                return true;
            }
            asset = Default;
            return false;
        }

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
                if (Load(stream, identifier, out var result))
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

        protected abstract bool Load(Stream stream, string identifier, out TData? result);

        public override int Unload()
        {
            int amountUnloaded = _assetStorage.Count;
            _assetStorage.Clear();
            return amountUnloaded;
        }
    }
}