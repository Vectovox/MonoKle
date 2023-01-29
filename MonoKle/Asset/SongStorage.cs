using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoKle.Asset
{
    /// <summary>
    /// Loads and maintains song assets that are streamed at playback. Supported types are .ogg and .mp3.
    /// </summary>
    // NOTE: Doesn't use BasicAssetStorage since Song can only be created from URI
    public class SongStorage : AbstractAssetStorage
    {
        private readonly Dictionary<string, Song> _songByIdentifier = new();

        public SongStorage(ILogger logger) : base(logger)
        {
        }

        /// <summary>
        /// Gets the available song identifiers.
        /// </summary>
        public IEnumerable<string> Identifiers => _songByIdentifier.Keys;

        /// <summary>
        /// Accesses the song with the given identifier.
        /// </summary>
        /// <param name="identifier">The identifier of the song to access.</param>
        public Song this[string identifier] => _songByIdentifier[identifier];

        /// <summary>
        /// Gets the song with the given identifier.
        /// </summary>
        /// <param name="identifier">The identifier of the song to get.</param>
        /// <param name="asset">When this method returns, this contains the requested song. Will be null if the song does not exist.</param>
        /// <returns>True if the song was successfully assigned; otherwise false.</returns>
        public bool TryGet(string identifier, out Song? asset)
        {
            if (Contains(identifier))
            {
                asset = this[identifier];
                return true;
            }
            asset = default;
            return false;
        }

        public override int Unload()
        {
            var amount = _songByIdentifier.Count;
            _songByIdentifier.Clear();
            return amount;
        }

        public override bool Contains(string identifier) => _songByIdentifier.ContainsKey(identifier);

        protected override bool ExtensionSupported(string extension) =>
            extension.Equals(".ogg", StringComparison.InvariantCultureIgnoreCase) ||
            extension.Equals(".mp3", StringComparison.InvariantCultureIgnoreCase);

        protected override bool Load(string path, string identifier, string[] args)
        {
            if (_songByIdentifier.ContainsKey(identifier))
            {
                _logger.LogError($"Identifier already loaded '{identifier}'. Skipping.");
                return false;
            }

            try
            {
                var song = GetSong(path);
                _songByIdentifier.Add(identifier, song);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error reading song '{e.Message}'. Skipping.");
                return false;
            }

            return true;
        }

        public override bool Unload(string identifier) => _songByIdentifier.Remove(identifier);

        private static Song GetSong(string path)
        {
            var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            var uri = new Uri(fullPath, UriKind.Absolute);
            // NOTE: Name must be set as path for this to work on Android
            //       https://github.com/MonoGame/MonoGame/issues/3935
            //       Song.AssetUri is not set so MonoGame uses the name
            //       as input for asset storage
            return Song.FromUri(path, uri);
        }
    }
}