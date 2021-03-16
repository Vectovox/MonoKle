using Microsoft.Xna.Framework;
using MonoKle.Logging;
using System;
using System.IO;

namespace MonoKle.Asset
{
    /// <summary>
    /// Abstract class for loading, storing, and retrieving MonoKle assets.
    /// </summary>
    public abstract class AbstractAssetStorage
    {
        protected readonly Logger _logger;

        public AbstractAssetStorage(Logger logger)
        {
            _logger = logger;
        }

        protected abstract bool FileSupported(string extension);
        protected abstract bool Load(string path, string identifier, string[] args);

        /// <summary>
        /// Loads the asset at the specified path, using the given identifier.
        /// </summary>
        /// <param name="path">The path to load the asset at.</param>
        /// <param name="identifier">The identifier to use for the asset.</param>
        public bool Load(string path, string identifier)
        {
            if (FileSupported(new FileInfo(path).Extension))
            {
                return Load(path, identifier, Array.Empty<string>());
            }

            _logger.Log($"File not supported '{path}'.", LogLevel.Error);
            return false;
        }

        /// <summary>
        /// Load assets from the manifest in the provided path.
        /// </summary>
        /// <remarks>Case sensitive on some platforms.</remarks>
        public int LoadFromManifest(string manifestPath)
        {
            // Open manifest
            StreamReader reader;
            try
            {
                reader = new StreamReader(TitleContainer.OpenStream(manifestPath));
            }
            catch (IOException e)
            {
                throw new IOException($"Manifest file could not be read at '{manifestPath}'", e);
            }

            // Read manifest and load the files
            int counter = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine().Trim();

                // Skip commented lines
                if (line.Length == 0 || line.StartsWith("#"))
                {
                    continue;
                }

                // Read identifier and path
                var lineParts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (lineParts.Length < 2)
                {
                    _logger.Log($"Manifest line '{line}' does not contain path and identifier", LogLevel.Error);
                    continue;
                }
                var identifier = lineParts[0];
                var path = lineParts[1];

                if (FileSupported(new FileInfo(path).Extension))
                {
                    // Load file and send remaining parts as arguments
                    if (Load(path, identifier, lineParts.Length > 2 ? lineParts[2..] : Array.Empty<string>()))
                    {
                        counter++;
                    }
                    else
                    {
                        _logger.Log($"Could not load asset '{line}' from manifest", LogLevel.Error);
                    }
                }
            }

            return counter;
        }
    }
}