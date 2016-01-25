namespace MonoKle.Graphics
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    
    using Microsoft.Xna.Framework.Graphics;

    public static class GraphicsHelper
    {
        public static Texture2D ImageToTexture2D(GraphicsDevice graphicsDevice, Image image)
        {
            // Size = Height * Width * 4 bytes for each colour value
            int size = image.Height * image.Width * 4;

            // Save image to stream
            MemoryStream ms = new MemoryStream(size);
            image.Save(ms, ImageFormat.Png);

            return Texture2D.FromStream(graphicsDevice, ms);
        }
    }
}