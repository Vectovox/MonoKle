using Microsoft.Xna.Framework.Media;
using MonoKle.Logging;
using System;
using System.Collections.Generic;

namespace MonoKle.Asset
{
    /// <summary>
    /// Loads and maintains song assets that are streamed at playback. Supported types are .ogg and .mp3.
    /// </summary>
    public class SongStorage : AbstractAssetStorage
    {
        private readonly Dictionary<string, Song> _songByIdentifier = new Dictionary<string, Song>();

        public SongStorage(Logger logger) : base(logger)
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

        protected override bool FileSupported(string extension) =>
            extension.Equals(".ogg", StringComparison.InvariantCultureIgnoreCase) ||
            extension.Equals(".mp3", StringComparison.InvariantCultureIgnoreCase);

        protected override bool Load(string path, string identifier, string[] args)
        {
            if (_songByIdentifier.ContainsKey(identifier))
            {
                _logger.Log($"Identifier already loaded '{identifier}'. Skipping.", LogLevel.Error);
                return false;
            }

            try
            {
                var uri = new Uri(path, UriKind.Relative);
                var song = Song.FromUri(identifier, uri);
                _songByIdentifier.Add(identifier, song);
            }
            catch (Exception e)
            {
                _logger.Log($"Error reading song '{e.Message}'. Skipping.", LogLevel.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Unloads all songs, returning the amount of songs unloaded.
        /// </summary>
        public int Unload()
        {
            var amount = _songByIdentifier.Count;
            _songByIdentifier.Clear();
            return amount;
        }
    }
}