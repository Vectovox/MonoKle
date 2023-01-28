using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

namespace MonoKle.Asset
{
    /// <summary>
    /// Font instance for drawing strings with settings.
    /// </summary>
    public class FontInstance
    {
        private static readonly StringBuilder _stringBuilderCache = new();
        private static readonly Func<char, Color, Color> _defaultColorFunc = new(DefaultColorMethod);
        private static readonly char[] _wrapCharacters = new char[] { ' ' };

        private readonly FontData _fontData;
        
        /// <summary>
        /// Initializes a new instance of <see cref="FontInstance"/> class with the
        /// default <see cref="Size"/> assigned from the font data.
        /// </summary>
        /// <param name="fontData"></param>
        public FontInstance(FontData fontData)
        {
            _fontData = fontData;
            Size = fontData.Size;
        }

        /// <summary>
        /// Gets the underlying font data.
        /// </summary>
        public FontData FontData => _fontData;

        /// <summary>
        /// Gets or sets the size, in pixels.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the line height padding, in pixels.
        /// </summary>
        public int LinePadding { get; set; }

        /// <summary>
        /// Gets or sets the character denoting opening and closing of a non-rendered color tag section.
        /// </summary>
        public char ColorTag { get; set; } = '\\';

        /// <summary>
        /// Gets or sets whether color tags should be processed.
        /// </summary>
        public bool ColorTagEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets whether compact mode should be used. If true, line height is discarded and text is
        /// measured and drawn as close to the drawing position as possible.
        /// </summary>
        /// <remarks>
        /// If true, text can easily be centered visually correct on the Y axis but may "jump around"
        /// when text changes. Does not work for multi-line strings.
        /// </remarks>
        public bool CompactHeight { get; set; }

        private float ScaleFactor => Size / (float)_fontData.Size;

        /// <summary>
        /// Returns a new instance of the same font with the given size.
        /// </summary>
        /// <param name="size">The font size to use.</param>
        /// <returns>New instance of <see cref="FontInstance"/> with the provided size.</returns>
        public FontInstance WithSize(int size) => new(_fontData)
            { Size = size, LinePadding = LinePadding, ColorTag = ColorTag, CompactHeight = CompactHeight, ColorTagEnabled = ColorTagEnabled };

        /// <summary>
        /// Returns a new instance of the same font with the given line height padding.
        /// </summary>
        /// <param name="padding">The line height padding to use.</param>
        /// <returns>New instance of <see cref="FontInstance"/> with the provided line height padding.</returns>
        public FontInstance WithLinePadding(int padding) => new(_fontData)
            { Size = Size, LinePadding = padding, ColorTag = ColorTag, CompactHeight = CompactHeight, ColorTagEnabled = ColorTagEnabled };

        /// <summary>
        /// Returns a new instance of the same font with the given compact height mode.
        /// </summary>
        /// <param name="compactHeight">The compact height mode to use.</param>
        /// <returns>New instance of <see cref="FontInstance"/> with the provided compact height mode.</returns>
        public FontInstance WithCompactHeight(bool compactHeight) => new(_fontData)
            { Size = Size, LinePadding = LinePadding, ColorTag = ColorTag, CompactHeight = compactHeight, ColorTagEnabled = ColorTagEnabled };

        /// <summary>
        /// Returns a new instance of the same font with the given color tag.
        /// </summary>
        /// <returns>New instance of <see cref="FontInstance"/> with the provided color tag.</returns>
        public FontInstance WithColorTag(char tag) => new(_fontData)
            { Size = Size, LinePadding = LinePadding, ColorTag = tag, CompactHeight = CompactHeight, ColorTagEnabled = ColorTagEnabled };

        /// <summary>
        /// Returns the size of the given text.
        /// </summary>
        /// <remarks>
        /// The result will encompass an area that will fit the given string completely.
        /// The letter sizes are used to determine width and line height is used for height.
        /// </remarks>
        /// <param name="text">The text to measure.</param>
        /// <returns>An <see cref="MVector2"/> representing the size.</returns>
        public MVector2 Measure(ReadOnlySpan<char> text)
        {
            // Precompute for efficiency
            var scaleFactor = ScaleFactor;
            var singleOutline = _fontData.Outline * scaleFactor;
            var doubleOutline = singleOutline * 2 ;
            var lineHeightIncrease = Size + LinePadding + singleOutline;
            
            // Set initial values
            var rowWidth = 0f;
            var rowHeightMin = Size;
            var rowHeightMax = 0f;
            var totalSize = new Vector2(0f, CompactHeight ? 0f : Size + LinePadding + doubleOutline);
            var tagOpen = false;

            // Iterate all characters to measure string
            foreach (char character in text)
            {
                if (character == ColorTag && ColorTagEnabled)
                {
                    // Open/close color tag
                    tagOpen = !tagOpen;
                }
                else if (tagOpen)
                {
                    // Do not record size if in a color tag
                }
                else if (character == '\n')
                {
                    // Update totals on linebreak
                    // X-component
                    totalSize.X = Math.Max(totalSize.X, rowWidth);
                    rowWidth = 0f;

                    // Y-component
                    totalSize.Y += lineHeightIncrease;
                }
                else if (_fontData.TryGetChar(character, out FontChar fontCharacter))
                {
                    // Measure character and set the line size
                    rowWidth += fontCharacter.XAdvance + _fontData.Outline;
                    if (CompactHeight)
                    {
                        rowHeightMin = Math.Min(rowHeightMin, fontCharacter.YOffset + _fontData.Outline);
                        rowHeightMax = Math.Max(rowHeightMax, fontCharacter.Height + _fontData.Outline + fontCharacter.YOffset);
                    }
                }
            }

            // Final update to total size
            totalSize.X = Math.Max(totalSize.X, rowWidth) * scaleFactor;
            if (CompactHeight)
            {
                totalSize.Y = (totalSize.Y + Math.Max(0f, rowHeightMax - rowHeightMin)) * scaleFactor;
            }
            return totalSize;
        }

        /// <summary>
        /// Wraps the provided string to fit the given width, placing linebreaks where applicable.
        /// </summary>
        /// <param name="text">The text to wrap.</param>
        /// <param name="maximumWidth">The maximum width the text can take up.</param>
        /// <returns>A string containing the wrapped text.</returns>
        public ReadOnlySpan<char> Wrap(ReadOnlySpan<char> text, float maximumWidth)
        {
            var originalWidth = Measure(text).X;
            
            // Check if NOOP
            if (originalWidth > maximumWidth)
            {
                // Prepare string manipulation
                _stringBuilderCache.Clear();
                var wrappedText = _stringBuilderCache.Append(text);
                var lineStartIndex = 0;

                // Iterate all characters and update the new string as we go
                for (int i = 1; i <= text.Length; i++)
                {
                    // Measure the current line to the pointer index
                    var lineWidth = Measure(text[lineStartIndex..i]).X;

                    if (lineWidth > maximumWidth)
                    {
                        // Too wide so put a newline in the last previous space
                        int lastPlaceToCut = LastIndexOfAny(text, _wrapCharacters, i - 1, i - lineStartIndex);
                        if (lastPlaceToCut == -1)
                        {
                            // No good place to cut the text so end it here already
                            break;
                        }
                        wrappedText.Remove(lastPlaceToCut, 1).Insert(lastPlaceToCut, '\n');

                        // Update indices
                        lineStartIndex = lastPlaceToCut + 1;
                    }
                }

                return wrappedText.ToString();
            }

            return text;

            // Helper for span that emulates the string version
            static int LastIndexOfAny(ReadOnlySpan<char> text, char[] characters, int startIndex, int count)
            {
                var end = startIndex - count;
                for (int i = startIndex; i > end; i--)
                {
                    for (int j = 0; j < characters.Length; j++)
                    {
                        if (characters[j] == text[i])
                        {
                            return i;
                        }
                    }
                }
                return -1;
            }
        }

        /// <summary>
        /// Draws the given string with an active spritebatch.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text in.</param>
        /// <param name="color">The color with which to draw the text.</param>
        public void Draw(SpriteBatch spriteBatch, ReadOnlySpan<char> text, Vector2 position, Color color) =>
            Draw(spriteBatch, text, position, color, 0f, MVector2.Zero, 0, SpriteEffects.None);

        /// <summary>
        /// Draws the given string with an active spritebatch.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text in.</param>
        /// <param name="color">The color with which to draw the text.</param>
        /// <param name="colorSelector">Color selector function called on color tags.</param>
        public void Draw(SpriteBatch spriteBatch, ReadOnlySpan<char> text, Vector2 position, Color color, Func<char, Color, Color> colorSelector) =>
            Draw(spriteBatch, text, position, color, 0f, MVector2.Zero, 0, SpriteEffects.None, colorSelector);

        /// <summary>
        /// Draws the given string with an active spritebatch, applying rotation.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text in.</param>
        /// <param name="color">The color with which to draw the text.</param>
        /// <param name="rotation">The rotation of the text.</param>
        /// <param name="origin">The origin of the text rotation.</param>
        public void Draw(SpriteBatch spriteBatch, ReadOnlySpan<char> text, Vector2 position, Color color,
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
        public void Draw(SpriteBatch spriteBatch, ReadOnlySpan<char> text, Vector2 position, Color color,
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
        public void Draw(SpriteBatch spriteBatch, ReadOnlySpan<char> text, Vector2 position, Color color,
            float rotation, Vector2 origin, float layerDepth, SpriteEffects effect) =>
            Draw(spriteBatch, text, position, color, rotation, origin, layerDepth, effect, _defaultColorFunc);

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
        /// <param name="colorSelector">Color selector function called on color tags.</param>
        public void Draw(SpriteBatch spriteBatch, ReadOnlySpan<char> text, Vector2 position, Color color,
            float rotation, Vector2 origin, float layerDepth, SpriteEffects effect, Func<char, Color, Color> colorSelector)
        {
            if (CompactHeight)
            {
                position.Y -= MinimumYOffset(text);
            }

            // Precompute for efficiency
            var scaleFactor = ScaleFactor;
            var singleOutline = _fontData.Outline * scaleFactor;
            var originalPosition = new Vector2(position.X + singleOutline, position.Y + singleOutline);

            // Set initial values
            var drawPosition = originalPosition;
            var tagOpen = false;
            var currentColor = color;

            // Iterate and draw characters
            foreach (char character in text)
            {
                if (character == ColorTag && ColorTagEnabled)
                {
                    // Open/close color tag
                    tagOpen = !tagOpen;
                }
                else if (tagOpen)
                {
                    // Allow color switch
                    currentColor = colorSelector(character, color);
                }
                else if (character == '\n')
                {
                    // Move to next line
                    drawPosition.Y += Size + LinePadding + singleOutline;
                    drawPosition.X = originalPosition.X;
                }
                else if (_fontData.TryGetChar(character, out FontChar fontCharacter))
                {
                    var destinationVector = new Vector2(drawPosition.X + fontCharacter.XOffset * scaleFactor, drawPosition.Y + fontCharacter.YOffset * scaleFactor);

                    // Apply rotation
                    if (rotation != 0)
                    {
                        var xd = destinationVector.X - origin.X - position.X;
                        var yd = destinationVector.Y - origin.Y - position.Y;

                        var x = origin.X + (xd * Math.Cos(rotation)) - (yd * Math.Sin(rotation));
                        var y = origin.Y + (xd * Math.Sin(rotation)) + (yd * Math.Cos(rotation));

                        // Translate back
                        destinationVector.X = (float)x + position.X;
                        destinationVector.Y = (float)y + position.Y;
                    }

                    var sourceRectangle = new Rectangle(fontCharacter.X, fontCharacter.Y, fontCharacter.Width, fontCharacter.Height);

                    spriteBatch.Draw(_fontData.GetPage(fontCharacter.Page), destinationVector, sourceRectangle,
                        currentColor, rotation, Vector2.Zero, scaleFactor, effect, layerDepth);
                    drawPosition.X += fontCharacter.XAdvance * scaleFactor + singleOutline;
                }
            }
        }

        private static Color DefaultColorMethod(char token, Color original) => original;

        private int MinimumYOffset(ReadOnlySpan<char> text)
        {
            var smallestOffset = int.MaxValue;
            foreach (var character in text)
            {
                if (_fontData.TryGetChar(character, out FontChar fontCharacter))
                {
                    smallestOffset = Math.Min(fontCharacter.YOffset + _fontData.Outline, smallestOffset);
                }
            }
            return smallestOffset;
        }
    }
}
