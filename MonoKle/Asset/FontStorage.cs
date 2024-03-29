using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MonoKle.Asset
{
    /// <summary>
    /// Stores drawable fonts.
    /// </summary>
    public class FontStorage : BasicAssetStorage<FontData, FontInstance>
    {
        private readonly GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontStorage"/> class.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device</param>
        /// <param name="logger">The logger to use.</param>
        public FontStorage(GraphicsDevice graphicsDevice, ILogger<FontStorage> logger)
            : base(logger) => _graphicsDevice = graphicsDevice;

        protected override bool ExtensionSupported(string extension) =>
            extension.Equals(".mfnt", StringComparison.InvariantCultureIgnoreCase);

        protected override FontInstance GetInstance(FontData data) => new(data);

        protected override bool Load(Stream stream, string identifier, out FontData? result)
        {
            var serializer = new XmlSerializer(typeof(BakedFont));
            
            BakedFont baked;
            try
            {
                baked = (BakedFont)serializer.Deserialize(stream);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error reading font: {e.Message}");
                result = null;
                return false;
            }

            var texList = baked.ImageList.Select(byteArray =>
            {
                using var textureStream = new MemoryStream(byteArray, false);
                return Texture2D.FromStream(_graphicsDevice, textureStream);
            }).ToList();

            result = new FontData(identifier, baked.FontFile, texList);
            return true;
        }
    }
}
