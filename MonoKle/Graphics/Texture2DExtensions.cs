using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoKle.Graphics
{
    /// <summary>
    /// Extensions for <see cref="Texture2D"/>.
    /// </summary>
    public static class Texture2DExtensions
    {
        /// <summary>
        /// Sets the data of the texture using the provided paint method.
        /// </summary>
        /// <param name="texture">The texture to set data on</param>
        /// <param name="paint">Paint function for each pixel index</param>
        /// <returns>The texture</returns>
        public static Texture2D Paint(this Texture2D texture, Func<int, Color> paint)
        {
            var data = new Color[texture.Width * texture.Height];

            for (int pixel = 0; pixel < data.Length; pixel++)
            {
                data[pixel] = paint(pixel);
            }

            texture.SetData(data);
            return texture;
        }

        /// <summary>
        /// Sets the texture to the provided color.
        /// </summary>
        /// <param name="texture">The texture to set.</param>
        /// <param name="color">The color to set the texture to.</param>
        /// <returns>The texture.</returns>
        public static Texture2D Fill(this Texture2D texture, Color color) => texture.Paint(x => color);
    }
}