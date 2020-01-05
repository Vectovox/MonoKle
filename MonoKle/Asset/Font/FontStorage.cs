namespace MonoKle.Asset.Font
{
    using Baking;
    using IO;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// Manages drawable fonts.
    /// </summary>
    public class FontStorage : AbstractAssetStorage<Font>
    {
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontStorage"/> class.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device</param>
        public FontStorage(GraphicsDevice graphicsDevice) => this.graphicsDevice = graphicsDevice;

        protected override bool CheckFile(MFileInfo file) => file.Extension.Equals(".mfnt", StringComparison.InvariantCultureIgnoreCase);

        protected override Font DoLoadStream(Stream stream)
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
                return null;
            }

            var texList = baked.ImageList.Select(byteArray =>
            {
                using (var textureStream = new MemoryStream(byteArray, false))
                {
                    return Texture2D.FromStream(graphicsDevice, textureStream);
                }
            }).ToList();

            return new Font(baked.FontFile, texList);
        }
    }
}
