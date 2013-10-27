namespace MonoKle.Assets.Font
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Font for drawing strings.
    /// </summary>
    public class Font
    {
        private FontFile data;
        private Dictionary<char, FontChar> fontCharByChar = new Dictionary<char, FontChar>();
        private Texture2D image;

        /// <summary>
        /// Creates a new instance of <see cref="Font"/>
        /// </summary>
        /// <param name="data">The data representation.</param>
        /// <param name="image">The image representation.</param>
        public Font(FontFile data, Texture2D image)
        {
            this.data = data;
            this.image = image;
            foreach(var fc in data.Chars)
            {
                char c = (char)fc.ID;
                fontCharByChar.Add(c, fc);
            }
        }

        /// <summary>
        /// Draws a string with the given color.
        /// </summary>
        /// <param name="spriteBatch">Active spritebatch.</param>
        /// <param name="text">String to draw.</param>
        /// <param name="position">The starting position to draw to.</param>
        /// <param name="color">The color to draw the text with.</param>
        public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            foreach (char c in text)
            {
                FontChar fc;
                if (this.fontCharByChar.TryGetValue(c, out fc))
                {
                    var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                    var dPos = new Vector2(position.X + fc.XOffset, position.Y + fc.YOffset);

                    spriteBatch.Draw(this.image, dPos, sourceRectangle, color);
                    position.X += fc.XAdvance;
                }
            }
        }

        /// <summary>
        /// Draws a string with the color white.
        /// </summary>
        /// <param name="spriteBatch">Active spritebatch.</param>
        /// <param name="text">String to draw.</param>
        /// <param name="position">The starting position to draw to.</param>
        public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position)
        {
            DrawString(spriteBatch, text, position, Color.White);
        }
    }
}