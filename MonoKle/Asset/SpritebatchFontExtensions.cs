using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoKle.Asset
{
    /// <summary>
    /// Extensions for spritebatch font drawing.
    /// </summary>
    public static class SpritebatchFontExtensions
    {
        /// <summary>
        /// Draws a string with the color white.
        /// </summary>
        /// <param name="spriteBatch">Active spritebatch.</param>
        /// <param name="font">The font to draw with.</param>
        /// <param name="text">String to draw.</param>
        /// <param name="position">The starting position to draw to.</param>
        public static void DrawString(this SpriteBatch spriteBatch, Font font, string text, Vector2 position) =>
            DrawString(spriteBatch, font, text, position, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

        /// <summary>
        /// Draws a string with the given color.
        /// </summary>
        /// <param name="spriteBatch">Active spritebatch.</param>
        /// <param name="text">String to draw.</param>
        /// <param name="font">The font to draw with.</param>
        /// <param name="position">The starting position to draw to.</param>
        /// <param name="color">The color to draw the text with.</param>
        public static void DrawString(this SpriteBatch spriteBatch, Font font, string text, Vector2 position, Color color) =>
            DrawString(spriteBatch, font, text, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

        /// <summary>
        /// Draws a string with the given color, scale, rotation.
        /// </summary>
        /// <param name="spriteBatch">Active spritebatch.</param>
        /// <param name="font">The font to draw with.</param>
        /// <param name="text">String to draw.</param>
        /// <param name="position">The starting position to draw to.</param>
        /// <param name="color">The color to draw the text with.</param>
        /// <param name="rotation">The rotation of the text.</param>
        /// <param name="origin">The origin to rotate around.</param>
        /// <param name="scale">The scale to draw the text with.</param>
        /// <param name="effect">Applied sprite effects.</param>
        /// <param name="depth">Depth.</param>
        public static void DrawString(this SpriteBatch spriteBatch, Font font, string text, Vector2 position, Color color,
            float rotation, Vector2 origin, float scale, SpriteEffects effect, float depth)
        {
            Vector2 drawPosition = position;
            foreach (char character in text)
            {
                if (character == '\n')
                {
                    // Move to next line
                    drawPosition.Y += font.LineHeight * scale;
                    drawPosition.X = position.X;
                }
                else if (font.TryGetChar(character, out FontChar fontCharacter))
                {
                    var sourceRectangle = new Rectangle(fontCharacter.X, fontCharacter.Y, fontCharacter.Width, fontCharacter.Height);
                    var destinationVector = new Vector2(drawPosition.X, drawPosition.Y + (fontCharacter.YOffset * scale));

                    // Apply rotation
                    if (rotation != 0)
                    {
                        double xd = destinationVector.X - origin.X - position.X;
                        double yd = destinationVector.Y - origin.Y - position.Y;

                        double x = origin.X + (xd * Math.Cos(rotation)) - (yd * Math.Sin(rotation));
                        double y = origin.Y + (xd * Math.Sin(rotation)) + (yd * Math.Cos(rotation));

                        // Translate back
                        destinationVector.X = (float)x + position.X;
                        destinationVector.Y = (float)y + position.Y;
                    }

                    spriteBatch.Draw(font.GetPage(fontCharacter.Page), destinationVector, sourceRectangle, color,
                        rotation, Vector2.Zero, scale, effect, depth);
                    drawPosition.X += fontCharacter.XAdvance * scale;
                }
            }
        }
    }
}
