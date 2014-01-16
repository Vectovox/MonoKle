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
            foreach (var fc in data.Chars)
            {
                char c = (char)fc.ID;
                this.fontCharByChar.Add(c, fc);
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
            DrawString(spriteBatch, text, position, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
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
            DrawString(spriteBatch, text, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Draws a string with the given color, scale, rotation.
        /// </summary>
        /// <param name="spriteBatch">Active spritebatch.</param>
        /// <param name="text">String to draw.</param>
        /// <param name="position">The starting position to draw to.</param>
        /// <param name="color">The color to draw the text with.</param>
        /// <param name="rotation">The rotation of the text.</param>
        /// <param name="origin">The origin to rotate around.</param>
        /// <param name="scale">The scale to draw the text with.</param>
        /// <param name="effect">Applied sprite effects.</param>
        /// <param name="depth">Depth.</param>
        public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float depth)
        {
            Vector2 drawPos = position; // Cursor
            foreach (char c in text)
            {
                // Set cursor to next line
                if (c == '\n')
                {
                    drawPos.Y += this.data.Common.LineHeight * scale;
                    drawPos.X = position.X;
                }
                else
                {
                    FontChar fc;
                    if (this.fontCharByChar.TryGetValue(c, out fc))
                    {
                        var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                        var destinationVector = new Vector2(drawPos.X, drawPos.Y + (fc.YOffset * scale));

                        double xd = destinationVector.X - origin.X - position.X;
                        double yd = destinationVector.Y - origin.Y - position.Y;

                        double x = origin.X + (xd * Math.Cos(rotation)) - (yd * Math.Sin(rotation));
                        double y = origin.Y + (xd * Math.Sin(rotation)) + (yd * Math.Cos(rotation));

                        // Translate back
                        destinationVector.X = (float)x + position.X;
                        destinationVector.Y = (float)y + position.Y;

                        spriteBatch.Draw(this.image, destinationVector, sourceRectangle, color, rotation, Vector2.Zero, scale, effect, depth);
                        drawPos.X += fc.XAdvance * scale;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the size of the given string.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <returns>A Vector2 representing the size.</returns>
        public Vector2 MeasureString(string text)
        {
            return MeasureString(text, 1f);
        }

        /// <summary>
        /// Returns the size of the given string with the specified scale.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="scale">The scale.</param>
        /// <returns>A vector2 representing the size.</returns>
        public Vector2 MeasureString(string text, float scale)
        {
            Vector2 rowSize = Vector2.Zero;
            Vector2 totalSize = Vector2.Zero;
            
            foreach (char c in text)
            {
                if (c == '\n')
                {
                    if(totalSize.X < rowSize.X)
                    {
                        totalSize.X = rowSize.X;
                    }
                    rowSize.X = 0;
                    totalSize.Y += data.Info.Size;
                }
                else
                {
                    FontChar fc;
                    if (this.fontCharByChar.TryGetValue(c, out fc))
                    {
                        if (fc.Height > rowSize.Y)
                        {
                            rowSize.Y = fc.Height;
                        }
                        rowSize.X += fc.XAdvance;
                    }
                }
            }

            if (totalSize.X < rowSize.X)
            {
                totalSize.X = rowSize.X;
            }
            totalSize.Y += data.Info.Size;
            return totalSize * scale;
        }
    }
}