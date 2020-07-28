using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Asset
{
    /// <summary>
    /// Font for drawing strings.
    /// </summary>
    public class Font
    {
        private readonly FontFile data;
        private readonly Dictionary<char, FontChar> fontCharByChar;
        private readonly List<Texture2D> pageList;

        /// <summary>
        /// Gets the line height of the font.
        /// </summary>
        public int LineHeight => data.Common.LineHeight;

        /// <summary>
        /// Creates a new instance of <see cref="Font"/>
        /// </summary>
        /// <param name="data">The data representation.</param>
        /// <param name="pageList">The image representations.</param>
        public Font(FontFile data, List<Texture2D> pageList)
        {
            this.data = data;
            this.pageList = pageList;
            fontCharByChar = data.Chars.ToDictionary(ch => (char)ch.ID);
        }

        /// <summary>
        /// Gets the font character associated with the provided character.
        /// </summary>
        /// <param name="character">The character to get the font character for.</param>
        /// <param name="value">When this method returns, contains the associated value of the provided character</param>
        /// <returns>True if there was an associated value with the provided character</returns>
        // TODO: Do not expose internal data. New type please!
        public bool TryGetChar(char character, out FontChar value) => fontCharByChar.TryGetValue(character, out value);

        /// <summary>
        /// Gets the <see cref="Texture2D"/> for the given page.
        /// </summary>
        /// <param name="page">The page to get.</param>
        /// <returns>A texture for the given page.</returns>
        public Texture2D GetPage(int page) => pageList[page];

        /// <summary>
        /// Returns the size of the given string.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <returns>A Vector2 representing the size.</returns>
        public Vector2 MeasureString(string text) => MeasureString(text, 1f);

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
                    if (totalSize.X < rowSize.X)
                    {
                        totalSize.X = rowSize.X;
                    }
                    rowSize.X = 0;
                    totalSize.Y += data.Info.Size;
                }
                else
                {
                    if (fontCharByChar.TryGetValue(c, out FontChar fc))
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
            if (width > maximumWidth)
            {
                int startPtr = 0;
                for (int i = 1; i < text.Length - startPtr; i++)
                {
                    if (text[startPtr + i].Equals('\n'))
                    {
                        startPtr += i;
                        i = 0;
                    }
                    else
                    {
                        float length = MeasureString(text.Substring(startPtr, i), scale).X;
                        if (length > maximumWidth)
                        {
                            int index = text.LastIndexOfAny(new char[] { ' ', '.', ',' }, startPtr + i - 1, i);
                            if (index != -1)
                            {
                                text = text.Remove(index, 1).Insert(index, "\n");
                            }
                            startPtr = index + 1;
                            i = 0;
                        }
                    }
                }
            }

            return text;
        }
    }
}
