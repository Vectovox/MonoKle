using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Graphics;
using MonoKle.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoKle.Asset
{
    /// <summary>
    /// Loads and maintains texture assets.
    /// </summary>
    public class TextureStorage : AbstractAssetStorage
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Dictionary<string, Texture2D> _textureByPath = new Dictionary<string, Texture2D>();
        private readonly Dictionary<string, TextureData> _textureDataByIdentifier = new Dictionary<string, TextureData>();

        /// <summary>
        /// Gets a square white texture.
        /// </summary>
        public Texture2D White { get; }

        /// <summary>
        /// Gets a default error texture.
        /// </summary>
        public Texture2D Error { get; }

        /// <summary>
        /// Accesses the texture with the given identifier. If the texture does not exist
        /// the <see cref="Error"/> texture will be returned.
        /// </summary>
        /// <param name="identifier">The identifier of the texture to get.</param>
        public MTexture this[string identifier]
        {
            get
            {
                if (_textureDataByIdentifier.ContainsKey(identifier))
                {
                    var data = _textureDataByIdentifier[identifier];
                    return data.AtlasRectangle == null
                        ? new MTexture(_textureByPath[data.Path])
                        : new MTexture(_textureByPath[data.Path], data.AtlasRectangle.Value);
                }
                return new MTexture(Error);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureStorageNew"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        public TextureStorage(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            Error = new Texture2D(graphicsDevice, 16, 16).Fill(Color.Purple);
            White = new Texture2D(graphicsDevice, 16, 16).Fill(Color.White);
        }

        protected override bool FileSupported(string extension) =>
            extension.Equals(".png", StringComparison.InvariantCultureIgnoreCase)
            || extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase)
            || extension.Equals(".gif", StringComparison.InvariantCultureIgnoreCase);

        protected override bool Load(string path, string identifier, params string[] args)
        {
            // Do not allow duplicate identifiers
            if (_textureDataByIdentifier.ContainsKey(identifier))
            {
                Logger.Global.Log($"Identifier already loaded '{identifier}'. Skipping.", LogLevel.Error);
                return false;
            }

            // Parse atlas rectangle
            MRectangleInt? atlasRectangle = null;
            if (args.Length > 0)
            {
                var rectangleParts = args[0].Split(',');
                if (rectangleParts.Length == 4 &&
                    int.TryParse(rectangleParts[0], out var x) &&
                    int.TryParse(rectangleParts[1], out var y) &&
                    int.TryParse(rectangleParts[2], out var width) &&
                    int.TryParse(rectangleParts[3], out var height))
                {
                    atlasRectangle = new MRectangleInt(x, y, width, height);
                }
                else
                {
                    Logger.Global.Log($"Error reading atlas rectangle for identifier '{identifier}'. Skipping.", LogLevel.Error);
                    return false;
                }
            }

            // Load texture if it wasn't already loaded
            if (!_textureByPath.ContainsKey(path))
            {
                try
                {
                    using var stream = File.OpenRead(path);
                    var texture = Texture2D.FromStream(_graphicsDevice, stream);
                    _textureByPath.Add(path, texture);
                }
                catch (Exception e)
                {
                    Logger.Global.Log($"Error reading texture '{e.Message}'. Skipping.", LogLevel.Error);
                    return false;
                }
            }

            _textureDataByIdentifier.Add(identifier, new TextureData
            {
                Path = path,
                AtlasRectangle = atlasRectangle,
            });

            return true;
        }

        public int Unload()
        {
            int amountUnloaded = _textureDataByIdentifier.Count;
            _textureByPath.Clear();
            _textureDataByIdentifier.Clear();
            return amountUnloaded;
        }

        private class TextureData
        {
            public string Path = string.Empty;
            public MRectangleInt? AtlasRectangle = null;
        }

        public struct MTexture
        {
            public readonly Texture2D Texture;
            public readonly MRectangleInt AtlasRectangle;

            public MTexture(Texture2D texture) : this(texture, new MRectangleInt(0, 0, texture.Width, texture.Height)) { }

            public MTexture(Texture2D texture, MRectangleInt atlasRectangle)
            {
                Texture = texture;
                AtlasRectangle = atlasRectangle;
            }
        }
    }

    public abstract class AbstractAssetStorage
    {
        protected abstract bool FileSupported(string extension);
        protected abstract bool Load(string path, string identifier, params string[] args);

        public bool Load(string path, string identifier)
        {
            if (FileSupported(new FileInfo(path).Extension))
            {
                return Load(path, identifier, Array.Empty<string>());
            }

            Logger.Global.Log($"File not supported '{path}'.", LogLevel.Error);
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

            // Load manifest files
            int counter = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var lineParts = line.Split(' ');

                if (lineParts.Length < 2)
                {
                    Logger.Global.Log($"Manifest line '{line}' does not contain path and identifier", LogLevel.Error);
                    continue;
                }

                var identifier = lineParts[0];
                var path = lineParts[1];

                if (FileSupported(new FileInfo(path).Extension))
                {
                    if (Load(path, identifier, lineParts.Length > 2 ? lineParts[2..] : Array.Empty<string>()))
                    {
                        counter++;
                    }
                    else
                    {
                        Logger.Global.Log($"Could not load asset '{line}' from manifest", LogLevel.Error);
                    }
                }
            }

            return counter;
        }
    }
}