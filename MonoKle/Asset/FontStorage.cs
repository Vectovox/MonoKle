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
        public FontStorage(GraphicsDevice graphicsDevice) => _graphicsDevice = graphicsDevice;

        protected override bool FileSupported(string extension) => extension.Equals(".mfnt", StringComparison.InvariantCultureIgnoreCase);

        protected override FontInstance GetInstance(FontData data) => new FontInstance(data);

        protected override bool Load(Stream stream, out FontData result)
        {
            var serializer = new XmlSerializer(typeof(BakedFont));
            object o = serializer.Deserialize(stream);
            BakedFont baked;

            try
            {
                baked = (BakedFont)o;
            }
            catch
            {
                result = null;
                return false;
            }

            var texList = baked.ImageList.Select(byteArray =>
            {
                using (var textureStream = new MemoryStream(byteArray, false))
                {
                    return Texture2D.FromStream(_graphicsDevice, textureStream);
                }
            }).ToList();

            result = new FontData(baked.FontFile, texList);
            return true;
        }
    }
}
