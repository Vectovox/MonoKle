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
                    if (rowWidth > 0)
                    {
                        rowWidth += _fontData.Outline;
                    }
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
            if (rowWidth > 0)
            {
                rowWidth += _fontData.Outline;
            }
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
                var insertPadding = 0;  // Non-replacing newlines need to apply padding on subsequent lines

                // Iterate all characters and update the new string as we go
                for (var currentIndex = 0; currentIndex < text.Length; currentIndex++)
                {
                    // Measure the current line to the pointer index
                    var currentText = text[lineStartIndex..(currentIndex + 1)];
                    var lineWidth = Measure(currentText).X;

                    if (lineWidth > maximumWidth)
                    {
                        // Find break point
                        var breakData = GetBreak(text, lineStartIndex, currentIndex - 1);
                        if (breakData.Index == -1)
                        {
                            // No good place to break, so end it here already
                            break;
                        }

                        // Apply break point
                        if (breakData.Replace)
                        {
                            wrappedText.Remove(breakData.Index, 1);
                            wrappedText.Insert(breakData.Index, '\n');
                            lineStartIndex = breakData.Index + 1;
                        }
                        else
                        {
                            wrappedText.Insert(breakData.Index + insertPadding + 1, '\n');
                            lineStartIndex = breakData.Index + 1;
                            insertPadding++;
                        }
                    }
                }

                return wrappedText.ToString();
            }

            return text;
        }

        /// <summary>
        /// Helper to find a suitable text break point.
        /// </summary>
        private static (int Index, bool Replace) GetBreak(ReadOnlySpan<char> text, int inclusiveStart, int inclusiveEnd)
        {
            for (int i = inclusiveEnd; i >= inclusiveStart; i--)
            {
                var currentChar = text[i];
                var nextChar = text[i + 1];

                // Replace upcoming space
                // whitespace, ideographic space
                if (nextChar == ' ' || nextChar == 0x3000)
                {
                    return (i + 1, true);
                }

                // Check for specific languages
                if (IsJapanese(currentChar) && AllowedJapaneseBreak(currentChar, nextChar))
                {
                    // Japanese can break anywhere
                    return (i, false);
                }
            }

            // If first character was a space, we can break that one (it's not checked above)
            if (text[inclusiveStart] == ' ')
            {
                return (inclusiveStart, true);
            }

            // Found no break index
            return (-1, false);

            static bool IsJapanese(char c) =>
                (c >= 0x300 && c <= 0x303f)
                || (c >= 0x3040 && c <= 0x309f)
                || (c >= 0x30a0 && c <= 0x30ff)
                || (c >= 0x4e00 && c <= 0x9faf)
                || (c >= 0xff00 && c <= 0xffef);

            static bool AllowedJapaneseBreak(char lineEnd, char lineStart)
            {
                // NOTE: https://en.wikipedia.org/wiki/Line_breaking_rules_in_East_Asian_languages
                // NOTE: Quotations 0x0022 + 0x0027 are not checked for now since start and end overlap
                //       and it's probably (HEH) not a big thing.
                // Perf: Try to evaluate early by checking ranges

                // Check start
                if (lineStart == 0x2010 || lineStart == 0x2013 || lineStart == 0x203c
                    || lineStart == 0xff1f || lineStart == 0xff5d || lineStart == 0xff60)
                {
                    return false;
                }
                if (lineStart >= 0x2047 && lineStart <= 0x2049)
                {
                    return false;
                }
                if (lineStart >= 0x31f0 && lineStart <= 0x31ff)
                {
                    return false;
                }
                if (lineStart >= 0x30fb && lineStart <= 0x30fe)
                {
                    return false;
                }
                if (lineStart <= 0x00bb)
                {
                    if (lineStart == 0x0021 || lineStart == 0x0029
                        || lineStart == 0x002c || lineStart == 0x002e || lineStart == 0x003a || lineStart == 0x003b
                        || lineStart == 0x005d || lineStart == 0x00bb)
                    {
                        return false;
                    }
                }
                if (lineStart >= 0x3001 && lineStart <= 0x30f6)
                {
                    if (lineStart == 0x3001 || lineStart == 0x3002 || lineStart == 0x3005
                        || lineStart == 0x3009 || lineStart == 0x300b || lineStart == 0x300d || lineStart == 0x300f
                        || lineStart == 0x3011 || lineStart == 0x3015 || lineStart == 0x3017 || lineStart == 0x3019
                        || lineStart == 0x301c || lineStart == 0x301f || lineStart == 0x303b || lineStart == 0x3041
                        || lineStart == 0x3043 || lineStart == 0x3045 || lineStart == 0x3047 || lineStart == 0x3049
                        || lineStart == 0x3063 || lineStart == 0x3083 || lineStart == 0x3085 || lineStart == 0x3087
                        || lineStart == 0x308e || lineStart == 0x3095 || lineStart == 0x3096 || lineStart == 0x30a0
                        || lineStart == 0x30a1 || lineStart == 0x30a3 || lineStart == 0x30a5 || lineStart == 0x30a7
                        || lineStart == 0x30a9 || lineStart == 0x30c3 || lineStart == 0x30e3 || lineStart == 0x30e5
                        || lineStart == 0x30e7 || lineStart == 0x30ee || lineStart == 0x30f5 || lineStart == 0x30f6)
                    {
                        return false;
                    }
                }

                // Check end
                if (lineEnd >= 0x3008 && lineEnd <= 0x301d)
                {
                    if (lineEnd == 0x3008 || lineEnd == 0x300a || lineEnd == 0x300c
                    || lineEnd == 0x300e || lineEnd == 0x3008 || lineEnd == 0x3010 || lineEnd == 0x3014
                    || lineEnd == 0x3016 || lineEnd == 0x3018 || lineEnd == 0x301d)
                    {
                        return false;
                    }
                }
                if (lineEnd == 0x0022 || lineEnd == 0x0027 || lineEnd == 0x0028 || lineEnd == 0x005b
                    || lineEnd == 0x00ab || lineEnd == 0xff5b || lineEnd == 0xff5f)
                {
                    return false;
                }

                // Good to go
                return true;
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
