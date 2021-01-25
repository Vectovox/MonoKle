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
        public MTexture White { get; }

        /// <summary>
        /// Gets a default error texture.
        /// </summary>
        public MTexture Error { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureStorageNew"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        public TextureStorage(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            Error = new MTexture(new Texture2D(graphicsDevice, 1, 1).Fill(Color.Purple));
            White = new MTexture(new Texture2D(graphicsDevice, 1, 1).Fill(Color.White));
        }

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
                        ? new MTexture(_textureByPath[data.Path], data.FrameCount, data.FrameRate)
                        : new MTexture(_textureByPath[data.Path], data.AtlasRectangle.Value, data.FrameCount, data.FrameRate);
                }
                return Error;
            }
        }

        /// <summary>
        /// Gets the available texture identifiers.
        /// </summary>
        public IEnumerable<string> Identifiers => _textureDataByIdentifier.Keys;

        /// <summary>
        /// Loads a texture using the provided <see cref="TextureData"/> and identifier.
        /// </summary>
        /// <param name="identifier">The unique identifier for the texture.</param>
        /// <param name="data">The texture data.</param>
        public bool Load(string identifier, TextureData data)
        {
            // Do not allow duplicate identifiers
            if (_textureDataByIdentifier.ContainsKey(identifier))
            {
                Logger.Global.Log($"Identifier already loaded '{identifier}'. Skipping.", LogLevel.Error);
                return false;
            }

            // Load texture if it wasn't already loaded
            if (!_textureByPath.ContainsKey(data.Path))
            {
                try
                {
                    using var stream = TitleContainer.OpenStream(data.Path);
                    var texture = Texture2D.FromStream(_graphicsDevice, stream);
                    _textureByPath.Add(data.Path, texture);
                }
                catch (Exception e)
                {
                    Logger.Global.Log($"Error reading texture '{e.Message}'. Skipping.", LogLevel.Error);
                    return false;
                }
            }

            _textureDataByIdentifier.Add(identifier, data);
            return true;
        }

        protected override bool FileSupported(string extension) =>
            extension.Equals(".png", StringComparison.InvariantCultureIgnoreCase)
            || extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase)
            || extension.Equals(".gif", StringComparison.InvariantCultureIgnoreCase);

        protected override bool Load(string path, string identifier, params string[] args)
        {
            // Parse atlas rectangle
            MRectangleInt? atlasRectangle = null;
            if (args.Length > 0 && args[0] != "-")
            {
                var rectangleParts = args[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
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

            // Parse animation data
            int frameCount = 1;
            int frameRate = 1;
            if (args.Length > 2)
            {
                if (!int.TryParse(args[1], out frameCount) ||
                    !int.TryParse(args[2], out frameRate))
                {
                    Logger.Global.Log($"Error reading animation data for identifier '{identifier}'. Skipping.", LogLevel.Error);
                    return false;
                }
            }

            return Load(identifier, new TextureData
            {
                Path = path,
                AtlasRectangle = atlasRectangle,
                FrameCount = frameCount,
                FrameRate = frameRate,
            });
        }

        /// <summary>
        /// Unloads all textures, returning the amount of texture identifiers unloaded.
        /// </summary>
        public int Unload()
        {
            int amountUnloaded = _textureDataByIdentifier.Count;
            _textureByPath.Clear();
            _textureDataByIdentifier.Clear();
            return amountUnloaded;
        }

        public class TextureData
        {
            public string Path = string.Empty;
            public MRectangleInt? AtlasRectangle = null;
            public int FrameCount = 1;
            public int FrameRate = 1;
        }
    }
}
