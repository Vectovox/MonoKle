namespace MonoKle.Graphics
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    
    using Microsoft.Xna.Framework.Graphics;

    public static class GraphicsHelper
    {
        public static Texture2D BitmapToTexture2D(GraphicsDevice graphicsDevice, Bitmap bitmap)
        {
            // Size = Height * Width * 4 bytes for each colour value
            int size = bitmap.Height * bitmap.Width * 4;

            // Save image to stream
            MemoryStream ms = new MemoryStream(size);
            bitmap.Save(ms, ImageFormat.Png);

            return Texture2D.FromStream(graphicsDevice, ms);
        }
    }
}