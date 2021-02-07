using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MonoKle.Asset
{
    [TestClass]
    public class FontTests
    {
        private readonly FontInstance _font;
        private const int LineHeight = 17;
        private const int SpaceWidth = 15;
        private const int AWidth = 14;
        private const int BWidth = 13;

        public FontTests()
        {
            var fontFile = new FontFile
            {
                Common = new FontCommon
                {
                    LineHeight = LineHeight,
                },
                Chars = new List<FontChar>
                {
                    new FontChar
                    {
                        ID = 32, // space ' '
                        XAdvance = SpaceWidth,
                    },
                    new FontChar
                    {
                        ID = 97, // a
                        XAdvance = AWidth,
                    },
                    new FontChar
                    {
                        ID = 98, // b
                        XAdvance = BWidth,
                    }
                },
            };
            _font = new FontInstance(new FontData(fontFile, new List<Microsoft.Xna.Framework.Graphics.Texture2D>()));
        }

        [DataTestMethod]
        [DataRow("", 0, LineHeight, DisplayName = "Empty string")]
        [DataRow(" ", SpaceWidth, LineHeight, DisplayName = "Only a space")]
        [DataRow("\n", 0, LineHeight * 2, DisplayName = "Only a newline")]
        [DataRow("a", AWidth, LineHeight, DisplayName = "Only a character")]
        [DataRow("aa", AWidth * 2, LineHeight, DisplayName = "Two characters of same size")]
        [DataRow("a\na", AWidth, LineHeight * 2, DisplayName = "Two of same characters on different rows")]
        [DataRow("ab", AWidth + BWidth, LineHeight, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", AWidth, LineHeight * 2, DisplayName = "Two characters of different size on different rows")]
        public void MeasureString_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), _font.Measure(testString));

        [DataTestMethod]
        [DataRow("a", 0, "a", DisplayName = "Single char that does not fit")]
        [DataRow("aaaa", 1000, "aaaa", DisplayName = "Many chars that fit with margin")]
        [DataRow("a a", AWidth, "a\na", DisplayName = "Two space separated chars but only one fits")]
        [DataRow("a a", AWidth + SpaceWidth + AWidth, "a a", DisplayName = "Two space separated chars and both fit")]
        [DataRow("a a", AWidth + SpaceWidth + AWidth - 1, "a\na", DisplayName = "Two space separated chars that barely does not fit")]
        [DataRow("a a aaaaa a a", AWidth, "a\na\naaaaa a a", DisplayName = "Sentence with word that can never fit")]
        [DataRow("aaaa aa a aaa", 5.5f * AWidth + SpaceWidth, "aaaa\naa a\naaa", DisplayName = "Newline correctly resets width calculation")]
        public void WrapString_CorrectValue(string testString, float testWidth, string expectedResult) =>
            Assert.AreEqual(expectedResult, _font.Wrap(testString, testWidth));
    }
}
