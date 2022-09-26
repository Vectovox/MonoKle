using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Graphics;
using MonoKle.Logging;
using System;
using System.Collections.Generic;

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
        private readonly Dictionary<string, MTexture> _textureCache = new Dictionary<string, MTexture>();

        /// <summary>
        /// Gets a square white texture.
        /// </summary>
        public MTexture White { get; }

        /// <summary>
        /// Gets a default error texture.
        /// </summary>
        public MTexture Error { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureStorage"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="logger">The logger to use.</param>
        public TextureStorage(GraphicsDevice graphicsDevice, Logger logger) : base(logger)
        {
            _graphicsDevice = graphicsDevice;
            Error = new MTexture(new Texture2D(graphicsDevice, 1, 1).Fill(Color.Purple), "error", "~Error");
            White = new MTexture(new Texture2D(graphicsDevice, 1, 1).Fill(Color.White), "white", "~White");
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
                // Check if texture has been cached
                if (_textureCache.TryGetValue(identifier, out var cachedTexture))
                {
                    return cachedTexture;
                }

                // Cache and return new texture
                if (_textureDataByIdentifier.TryGetValue(identifier, out var data))
                {
                    try
                    {
                        var newTexture = data.AtlasRectangle == null
                            ? new MTexture(_textureByPath[data.Path], identifier, data.Path,
                                data.FrameColumns, data.FrameRows, data.FrameMargin, data.FrameRate)
                            : new MTexture(_textureByPath[data.Path], identifier, data.Path,
                                data.AtlasRectangle.Value, data.FrameColumns, data.FrameRows, data.FrameMargin, data.FrameRate);
                        _textureCache.Add(identifier, newTexture);
                        return newTexture;
                    }
                    catch (Exception e)
                    {
                        _logger.Log($"Error instantiating texture '{identifier}': {e.Message}", LogLevel.Error);
                    }
                }
                else
                {
                    _logger.Log($"Accessed non-existing texture: '{identifier}'.", LogLevel.Warning);
                }
                return Error;
            }
        }

        public override bool Contains(string identifier) => _textureDataByIdentifier.ContainsKey(identifier);

        /// <summary>
        /// Gets the texture with the given identifier.
        /// </summary>
        /// <param name="identifier">The identifier of the texture to get.</param>
        /// <param name="asset">When this method returns, this contains the requested texture. Will be <see cref="Error"/> if the texture does not exist.</param>
        /// <returns>True if the texture was successfully assigned; otherwise false.</returns>
        public bool TryGet(string identifier, out MTexture asset)
        {
            if (Contains(identifier))
            {
                asset = this[identifier];
                return true;
            }
            asset = Error;
            return false;
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
        /// <returns>True if loading was successful; otherwise false.</returns>
        public bool Load(string identifier, TextureData data)
        {
            // Do not allow duplicate identifiers
            if (_textureDataByIdentifier.ContainsKey(identifier))
            {
                _logger.Log($"Identifier already loaded '{identifier}'. Skipping.", LogLevel.Error);
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
                    _logger.Log($"Error reading texture '{e.Message}'. Skipping.", LogLevel.Error);
                    return false;
                }
            }

            _textureDataByIdentifier.Add(identifier, data);
            return true;
        }

        /// <summary>
        /// Loads the provided in-memory texture using the provided <see cref="TextureData"/> and identifier.
        /// </summary>
        /// <remarks>
        /// The <see cref="TextureData.Path"/> property needs to be populated as it serves as the in-memory path representation
        /// for atlas purposes, e.g. a sub-texture being located in the provided texture.
        /// </remarks>
        /// <param name="identifier">The unique identifier for the texture.</param>
        /// <param name="texture">The texture to load.</param>
        /// <param name="data">The texture data. Path property must be populated.</param>
        /// <returns>True if loading was successful; otherwise false.</returns>
        public bool Load(string identifier, Texture2D texture, TextureData data)
        {
            if (_textureDataByIdentifier.ContainsKey(identifier))
            {
                _logger.Log($"Identifier already loaded '{identifier}'. Skipping.", LogLevel.Error);
                return false;
            }

            if (_textureByPath.ContainsKey(data.Path))
            {
                _logger.Log($"Path '{data.Path}' already loaded ({identifier}). Skipping.", LogLevel.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(data.Path))
            {
                _logger.Log($"Path must be proided for '{identifier}'. Skipping.", LogLevel.Error);
                return false;
            }

            _textureByPath.Add(data.Path, texture);
            _textureDataByIdentifier.Add(identifier, data);
            return true;
        }

        protected override bool FileSupported(string extension) =>
            extension.Equals(".png", StringComparison.InvariantCultureIgnoreCase)
            || extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase)
            || extension.Equals(".gif", StringComparison.InvariantCultureIgnoreCase);

        protected override bool Load(string path, string identifier, string[] args)
        {
            // Format is: ... atlasRectangle (- to use full texture) | frameColumns,frameRows | frameMargin | frameRate
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
                    _logger.Log($"Error reading atlas rectangle for identifier '{identifier}'. Skipping.", LogLevel.Error);
                    return false;
                }
            }

            // Parse frames and columns
            int frameColumns = 1;
            int frameRows = 1;
            if (args.Length > 1)
            {
                var sizeParts = args[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (sizeParts.Length != 2 ||
                    !int.TryParse(sizeParts[0], out frameColumns) ||
                    !int.TryParse(sizeParts[1], out frameRows))
                {
                    _logger.Log($"Error reading columns and rows for identifier '{identifier}'. Skipping.", LogLevel.Error);
                    return false;
                }
            }

            // Parse frame margin
            int frameMargin = 0;
            if (args.Length > 2 && !int.TryParse(args[2], out frameMargin))
            {
                _logger.Log($"Error reading frame margin for identifier '{identifier}'. Skipping.", LogLevel.Error);
                return false;
            }

            // Parse animation frame rate
            int frameRate = 1;
            if (args.Length > 3 && !int.TryParse(args[3], out frameRate))
            {
                _logger.Log($"Error reading framerate for identifier '{identifier}'. Skipping.", LogLevel.Error);
                return false;
            }

            return Load(identifier, new TextureData
            {
                Path = path,
                AtlasRectangle = atlasRectangle,
                FrameColumns = frameColumns,
                FrameRows = frameRows,
                FrameMargin = frameMargin,
                FrameRate = frameRate,
            });
        }

        public override int Unload()
        {
            int amountUnloaded = _textureDataByIdentifier.Count;
            _textureByPath.Clear();
            _textureDataByIdentifier.Clear();
            _textureCache.Clear();
            return amountUnloaded;
        }

        public class TextureData
        {
            public string Path = string.Empty;
            public MRectangleInt? AtlasRectangle = null;
            public int FrameColumns = 1;
            public int FrameRows = 1;
            public int FrameRate = 0;
            public int FrameMargin = 0;
        }
    }
}
