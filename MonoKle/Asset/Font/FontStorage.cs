namespace MonoKle.Asset.Font
{
    using Microsoft.Xna.Framework.Graphics;
    using Baking;
    using MonoKle.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Drawing.Imaging;
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
        public FontStorage(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        protected override bool CheckFile(string filePath)
        {
            return filePath.EndsWith(".mfnt", StringComparison.CurrentCultureIgnoreCase);
        }

        protected override Font DoLoadStream(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(BakedFont));
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

            List<Texture2D> texList = new List<Texture2D>();
            ImageSerializer isr = new ImageSerializer(ImageFormat.Png);
            foreach (byte[] i in baked.ImageList)
            {
                Image image = isr.BytesToImage(i);
                texList.Add(GraphicsHelper.ImageToTexture2D(graphicsDevice, image));
            }

            return new Font(baked.FontFile, texList);
        }
    }
}