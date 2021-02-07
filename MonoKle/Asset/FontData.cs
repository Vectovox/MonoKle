using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Asset
{
    /// <summary>
    /// Class that stores font data information.
    /// </summary>
    public class FontData
    {
        private readonly Dictionary<char, FontChar> _fontCharByChar;
        private readonly List<Texture2D> _pageList;

        /// <summary>
        /// Gets the pixel height of the font text.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Creates a new instance of <see cref="FontData"/>.
        /// </summary>
        /// <param name="data">The data representation.</param>
        /// <param name="pageList">The image representations.</param>
        public FontData(FontFile data, List<Texture2D> pageList)
        {
            Size = data.Common.LineHeight;
            _fontCharByChar = data.Chars.ToDictionary(ch => (char)ch.ID);
            _pageList = pageList;
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
    }
}
