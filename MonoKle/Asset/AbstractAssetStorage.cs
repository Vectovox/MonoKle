using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace MonoKle.Asset
{
    /// <summary>
    /// Abstract class for loading, storing, and retrieving MonoKle assets.
    /// </summary>
    public abstract class AbstractAssetStorage
    {
        protected readonly ILogger _logger;

        public AbstractAssetStorage(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Loads the asset at the specified path, using the given identifier.
        /// </summary>
        /// <param name="path">The path to load the asset at.</param>
        /// <param name="identifier">The identifier to use for the asset.</param>
        public bool Load(string path, string identifier)
        {
            if (ExtensionSupported(new FileInfo(path).Extension))
            {
                return Load(path, identifier, Array.Empty<string>());
            }

            _logger.LogError($"File not supported '{path}'.");
            return false;
        }

        /// <summary>
        /// Loads assets specified in the provided manifest.
        /// </summary>
        /// <param name="manifestPath">Path to the manifest file.</param>
        /// <remarks>Case sensitive on some platforms.</remarks>
        /// <returns>Amount of loaded assets.</returns>
        public int LoadFromManifest(string manifestPath) =>
            OperateOnManifest(manifestPath, (path, identifier, lineParts) => Load(path, identifier, lineParts));

        /// <summary>
        /// Unloads assets specified in the provided manifest.
        /// </summary>
        /// <param name="manifestPath">Path to the manifest file.</param>
        /// <returns>Amount of unloaded assets.</returns>
        public int UnloadFromManifest(string manifestPath) =>
            OperateOnManifest(manifestPath, (path, identifier, lineParts) => Unload(identifier));

        /// <summary>
        /// Returns whether the given identifier is present in the <see cref="AbstractAssetStorage"/>.
        /// </summary>
        /// <param name="identifier">The identifier to check for.</param>
        /// <returns>True if present; otherwise false.</returns>
        public abstract bool Contains(string identifier);
        /// <summary>
        /// Unloads all assets, returning the amount of asset identifiers unloaded.
        /// </summary>
        public abstract int Unload();
        /// <summary>
        /// Unloads the asset with the given identifier.
        /// </summary>
        /// <param name="identifier">Identifier of the asset to unload.</param>
        /// <returns>True if unloaded; otherwise false.</returns>
        public abstract bool Unload(string identifier);

        protected abstract bool ExtensionSupported(string extension);
        protected abstract bool Load(string path, string identifier, string[] args);

        private int OperateOnManifest(string manifestPath, Func<string, string, string[], bool> operation)
        {
            // Open manifest
            Stream stream;
            try
            {
                stream = TitleContainer.OpenStream(manifestPath);
            }
            catch (IOException e)
            {
                throw new IOException($"Manifest file could not be read at '{manifestPath}'", e);
            }
            using var reader = new StreamReader(stream);

            // Read manifest and load the files
            var counter = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine()!.Trim();

                // Skip commented lines
                if (line.Length == 0 || line.StartsWith("#"))
                {
                    continue;
                }

                // Read identifier and path
                var lineParts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (lineParts.Length < 2)
                {
                    _logger.LogError($"Manifest line '{line}' does not contain path and identifier");
                    continue;
                }
                var identifier = lineParts[0];
                var path = lineParts[1];

                if (ExtensionSupported(new FileInfo(path).Extension))
                {
                    if (operation(path, identifier, lineParts.Length > 2 ? lineParts[2..] : Array.Empty<string>()))
                    {
                        counter++;
                    }
                    else
                    {
                        _logger.LogError($"Could not operate on asset '{line}' from manifest");
                    }
                }
            }

            return counter;
        }
    }
}