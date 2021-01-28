using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MonoKle.Asset
{
    [TestClass]
    public class FontTests
    {
        private readonly Font _font;
        private const int _lineHeight = 17;
        private const int _spaceWidth = 15;
        private const int _aWidth = 14;
        private const int _bWidth = 13;

        public FontTests()
        {
            var fontFile = new FontFile
            {
                Common = new FontCommon
                {
                    LineHeight = _lineHeight,
                },
                Chars = new List<FontChar>
                {
                    new FontChar
                    {
                        ID = 32, // space ' '
                        XAdvance = _spaceWidth,
                    },
                    new FontChar
                    {
                        ID = 97, // a
                        XAdvance = _aWidth,
                    },
                    new FontChar
                    {
                        ID = 98, // b
                        XAdvance = _bWidth,
                    }
                },
            };
            _font = new Font(fontFile, new List<Microsoft.Xna.Framework.Graphics.Texture2D>());
        }

        [DataTestMethod]
        [DataRow("", 0, _lineHeight, DisplayName = "Empty string")]
        [DataRow(" ", _spaceWidth, _lineHeight, DisplayName = "Only a space")]
        [DataRow("\n", 0, _lineHeight * 2, DisplayName = "Only a newline")]
        [DataRow("a", _aWidth, _lineHeight, DisplayName = "Only a character")]
        [DataRow("aa", _aWidth * 2, _lineHeight, DisplayName = "Two characters of same size")]
        [DataRow("a\na", _aWidth, _lineHeight * 2, DisplayName = "Two of same characters on different rows")]
        [DataRow("ab", _aWidth + _bWidth, _lineHeight, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", _aWidth, _lineHeight * 2, DisplayName = "Two characters of different size on different rows")]
        public void MeasureString_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), _font.MeasureString(testString));

        [DataTestMethod]
        [DataRow("a", 0, "a", DisplayName = "Single char that does not fit")]
        [DataRow("aaaa", 1000, "aaaa", DisplayName = "Many chars that fit with margin")]
        [DataRow("a a", _aWidth, "a\na", DisplayName = "Two space separated chars but only one fits")]
        [DataRow("a a", _aWidth + _spaceWidth + _aWidth, "a a", DisplayName = "Two space separated chars and both fit")]
        [DataRow("a a", _aWidth + _spaceWidth + _aWidth - 1, "a\na", DisplayName = "Two space separated chars that barely does not fit")]
        [DataRow("a a aaaaa a a", _aWidth, "a\na\naaaaa a a", DisplayName = "Sentence with word that can never fit")]
        [DataRow("aaaa aa a aaa", 5.5f * _aWidth + _spaceWidth, "aaaa\naa a\naaa", DisplayName = "Newline correctly resets width calculation")]
        public void WrapString_CorrectValue(string testString, float testWidth, string expectedResult) =>
            Assert.AreEqual(expectedResult, _font.WrapString(testString, testWidth));
    }
}
