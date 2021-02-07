using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoKle.Asset
{
    /// <summary>
    /// Font instance for drawing strings with settings.
    /// </summary>
    public class FontInstance
    {
        private readonly FontData _fontData;

        /// <summary>
        /// Initializes a new instance of <see cref="FontInstance"/> class with the default <see cref="Size"/>
        /// and <see cref="LineHeight"/> assigned from the font data.
        /// </summary>
        /// <param name="fontData"></param>
        public FontInstance(FontData fontData)
        {
            _fontData = fontData;
            Size = fontData.Size;
        }

        /// <summary>
        /// Returns a new instance of the same font with the given size.
        /// </summary>
        /// <param name="size">The font size to use.</param>
        /// <returns>New instance of <see cref="FontInstance"/> with the provided size.</returns>
        public FontInstance WithSize(int size) => new FontInstance(_fontData) { Size = size, LinePadding = LinePadding };

        /// <summary>
        /// Returns a new instance of the same font with the given line height padding.
        /// </summary>
        /// <param name="padding">The line height padding to use.</param>
        /// <returns>New instance of <see cref="FontInstance"/> with the provided line height padding.</returns>
        public FontInstance WithLinePadding(int padding) => new FontInstance(_fontData) { LinePadding = padding, Size = Size };

        /// <summary>
        /// Gets or sets the size, in pixels.
        /// </summary>
        public int Size { get; set; }
        private float ScaleFactor => Size / (float)_fontData.Size;

        /// <summary>
        /// Gets or sets the line height padding, in pixels.
        /// </summary>
        public int LinePadding { get; set; }

        /// <summary>
        /// Returns the size of the given text.
        /// </summary>
        /// <remarks>
        /// The result will encompass an area that will fit the given string completely.
        /// The letter sizes are used to determine width and line height is used for height.
        /// </remarks>
        /// <param name="text">The text to measure.</param>
        /// <returns>An <see cref="MVector2"/> representing the size.</returns>
        public MVector2 Measure(string text)
        {
            float rowSize = 0f;
            Vector2 totalSize = new Vector2(0f, Size + LinePadding);

            foreach (char character in text)
            {
                // Update totals on linebreak
                if (character == '\n')
                {
                    // X-component
                    totalSize.X = Math.Max(totalSize.X, rowSize);
                    rowSize = 0;

                    // Y-component
                    totalSize.Y += Size + LinePadding;
                }
                else if (_fontData.TryGetChar(character, out FontChar fontCharacter))
                {
                    // Measure character and set the line size
                    rowSize += fontCharacter.XAdvance;
                }
            }

            // Final update to total size
            totalSize.X = Math.Max(totalSize.X, rowSize) * ScaleFactor;
            return totalSize;
        }

        /// <summary>
        /// Wraps the provided string to fit the given width, placing linebreaks where applicable.
        /// </summary>
        /// <param name="text">The text to wrap.</param>
        /// <param name="maximumWidth">The maximum width the text can take up.</param>
        /// <returns>A string containing the wrapped text.</returns>
        public string Wrap(string text, float maximumWidth)
        {
            float width = Measure(text).X;
            string newText = text;
            if (width > maximumWidth)
            {
                int lineStartIndex = 0;
                for (int i = 1; i <= text.Length; i++)
                {
                    // Measure the current line to the pointer index
                    var lineSubString = newText[lineStartIndex..i];
                    float lineWidth = Measure(lineSubString).X;
                    if (lineWidth > maximumWidth)
                    {
                        // Too wide so put a newline in the last previous space
                        int lastPlaceToCut = newText.LastIndexOfAny(new char[] { ' ' }, i - 1, i - lineStartIndex);
                        if (lastPlaceToCut == -1)
                        {
                            // No good place to cut the text so end it here already
                            break;
                        }
                        newText = newText.Remove(lastPlaceToCut, 1).Insert(lastPlaceToCut, "\n");

                        // Update indices
                        lineStartIndex = lastPlaceToCut;
                    }
                }
            }

            return newText;
        }

        /// <summary>
        /// Draws the given string with an active spritebatch, applying rotation.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text in.</param>
        /// <param name="color">The color with which to draw the text.</param>
        public void Draw(SpriteBatch spriteBatch, string text, Vector2 position, Color color) =>
            Draw(spriteBatch, text, position, color, 0f, MVector2.Zero, 0, SpriteEffects.None);

        /// <summary>
        /// Draws the given string with an active spritebatch, applying rotation.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text in.</param>
        /// <param name="color">The color with which to draw the text.</param>
        /// <param name="rotation">The rotation of the text.</param>
        /// <param name="origin">The origin of the text rotation.</param>
        public void Draw(SpriteBatch spriteBatch, string text, Vector2 position, Color color,
            float rotation, Vector2 origin) =>
                Draw(spriteBatch, text, position, color, rotation, origin, 0, SpriteEffects.None);

        /// <summary>
        /// Draws the given string with an active spritebatch.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text in.</param>
        /// <param name="color">The color with which to draw the text.</param>
        /// <param name="rotation">The rotation of the text.</param>
        /// <param name="origin">The origin of the text rotation.</param>
        /// <param name="layerDepth">The layer depth.</param>
        public void Draw(SpriteBatch spriteBatch, string text, Vector2 position, Color color,
            float rotation, Vector2 origin, int layerDepth) =>
                Draw(spriteBatch, text, position, color, rotation, origin, layerDepth, SpriteEffects.None);

        /// <summary>
        /// Draws the given string with an active spritebatch.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text in.</param>
        /// <param name="color">The color with which to draw the text.</param>
        /// <param name="rotation">The rotation of the text.</param>
        /// <param name="origin">The origin of the text rotation.</param>
        /// <param name="layerDepth">The layer depth.</param>
        /// <param name="effect">The sprite effects to apply.</param>
        public void Draw(SpriteBatch spriteBatch, string text, Vector2 position, Color color,
            float rotation, Vector2 origin, float layerDepth, SpriteEffects effect)
        {
            float scaleFactor = ScaleFactor;    // Precompute for efficiency
            Vector2 drawPosition = position;
            foreach (char character in text)
            {
                if (character == '\n')
                {
                    // Move to next line
                    drawPosition.Y += Size + LinePadding;
                    drawPosition.X = position.X;
                }
                else if (_fontData.TryGetChar(character, out FontChar fontCharacter))
                {
                    var sourceRectangle = new Rectangle(fontCharacter.X, fontCharacter.Y, fontCharacter.Width, fontCharacter.Height);
                    var destinationVector = new Vector2(drawPosition.X, drawPosition.Y + (fontCharacter.YOffset * scaleFactor));

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

                    spriteBatch.Draw(_fontData.GetPage(fontCharacter.Page), destinationVector, sourceRectangle, color,
                        rotation, Vector2.Zero, scaleFactor, effect, layerDepth);
                    drawPosition.X += fontCharacter.XAdvance * scaleFactor;
                }
            }
        }
    }
}
