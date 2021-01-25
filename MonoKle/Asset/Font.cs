using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Asset
{
    /// <summary>
    /// Font for drawing strings.
    /// </summary>
    public class Font
    {
        private readonly FontFile _data;
        private readonly Dictionary<char, FontChar> _fontCharByChar;
        private readonly List<Texture2D> _pageList;

        /// <summary>
        /// Gets the line height of the font.
        /// </summary>
        public int LineHeight => _data.Common.LineHeight;

        /// <summary>
        /// Creates a new instance of <see cref="Font"/>
        /// </summary>
        /// <param name="data">The data representation.</param>
        /// <param name="pageList">The image representations.</param>
        public Font(FontFile data, List<Texture2D> pageList)
        {
            _data = data;
            _pageList = pageList;
            _fontCharByChar = data.Chars.ToDictionary(ch => (char)ch.ID);
        }

        /// <summary>
        /// Gets the font character associated with the provided character.
        /// </summary>
        /// <param name="character">The character to get the font character for.</param>
        /// <param name="value">When this method returns, contains the associated value of the provided character</param>
        /// <returns>True if there was an associated value with the provided character</returns>
        // TODO: Do not expose internal data. New type please!
        public bool TryGetChar(char character, out FontChar value) => _fontCharByChar.TryGetValue(character, out value);

        /// <summary>
        /// Gets the <see cref="Texture2D"/> for the given page.
        /// </summary>
        /// <param name="page">The page to get.</param>
        /// <returns>A texture for the given page.</returns>
        public Texture2D GetPage(int page) => _pageList[page];

        /// <summary>
        /// Returns the size of the given string.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <returns>A Vector2 representing the size.</returns>
        public MVector2 MeasureString(string text) => MeasureString(text, 1f);

        /// <summary>
        /// Returns the size of the given string with the specified scale.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="scale">The scale.</param>
        /// <returns>A vector2 representing the size.</returns>
        public MVector2 MeasureString(string text, float scale)
        {
            float rowSize = 0f;
            Vector2 totalSize = new Vector2(0f, LineHeight);

            foreach (char character in text)
            {
                // Update totals on linebreak
                if (character == '\n')
                {
                    // X-component
                    totalSize.X = Math.Max(totalSize.X, rowSize);
                    rowSize = 0;

                    // Y-component
                    totalSize.Y += LineHeight;
                }
                else if (_fontCharByChar.TryGetValue(character, out FontChar fontCharacter))
                {
                    // Measure character and set the line size
                    rowSize += fontCharacter.XAdvance;
                }
            }

            // Final update to total size
            totalSize.X = Math.Max(totalSize.X, rowSize);
            return totalSize * scale;
        }

        /// <summary>
        /// Wraps a text to fit in a given width, placing linebreaks where applicable. Assuming a scale of 1.0f.
        /// </summary>
        /// <param name="text">The text to wrap</param>
        /// <param name="maximumWidth">The maximum width allowed for the wrapped text</param>
        /// <returns>A string containing the wrapped text</returns>
        public string WrapString(string text, float maximumWidth) => WrapString(text, maximumWidth, 1f);

        /// <summary>
        /// Wraps a text to fit in a given width, placing linebreaks where applicable.
        /// </summary>
        /// <param name="text">The text to wrap</param>
        /// <param name="maximumWidth">The maximum width allowed for the wrapped text</param>
        /// <param name="scale">Scale of font to adjust for.</param>
        /// <returns>A string containing the wrapped text</returns>
        public string WrapString(string text, float maximumWidth, float scale)
        {
            float width = MeasureString(text, scale).X;
            string newText = text;
            if (width > maximumWidth)
            {
                for (int i = 1; i < text.Length; i++)
                {
                    float newWidth = MeasureString(newText.Substring(0, i), scale).X;
                    if (newWidth > maximumWidth)
                    {
                        int lastPlaceToCut = newText.LastIndexOfAny(new char[] { ' ', '.', ',' }, i, i);
                        if (lastPlaceToCut == -1)
                        {
                            // No good place to cut the text so end it here already
                            break;
                        }
                        newText = newText.Remove(lastPlaceToCut, 1).Insert(lastPlaceToCut, "\n");
                    }
                }
            }

            return newText;
        }
    }
}
