namespace MonoKle.Assets.Font
{
    using Microsoft.Xna.Framework.Graphics;
    using MonoKle.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

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
            BinaryFormatter bf = new BinaryFormatter();
            object o = bf.Deserialize(stream);
            FontBaker.BakedFont baked;

            try
            {
                baked = (FontBaker.BakedFont)o;
            }
            catch
            {
                return null;
            }

            List<Texture2D> texList = new List<Texture2D>();
            foreach (Image i in baked.ImageList)
            {
                texList.Add(GraphicsHelper.ImageToTexture2D(graphicsDevice, i));
            }

            return new Font(baked.FontFile, texList);
        }
    }
}