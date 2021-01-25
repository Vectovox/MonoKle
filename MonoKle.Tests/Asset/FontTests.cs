using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MonoKle.Asset
{
    [TestClass]
    public class FontTests
    {
        private readonly Font _font;

        public FontTests()
        {
            var fontFile = new FontFile
            {
                Common = new FontCommon
                {
                    LineHeight = 17,
                },
                Chars = new List<FontChar>
                {
                    new FontChar
                    {
                        ID = 32, // space ' '
                        XAdvance = 14,
                    },
                    new FontChar
                    {
                        ID = 97, // a
                        XAdvance = 15,
                    },
                    new FontChar
                    {
                        ID = 98, // b
                        XAdvance = 13,
                    }
                },
            };
            _font = new Font(fontFile, new List<Microsoft.Xna.Framework.Graphics.Texture2D>());
        }

        [DataTestMethod]
        [DataRow("", 0, 17, DisplayName = "Empty string")]
        [DataRow(" ", 14, 17, DisplayName = "Only a space")]
        [DataRow("\n", 0, 17 * 2, DisplayName = "Only a newline")]
        [DataRow("a", 15, 17, DisplayName = "Only a character")]
        [DataRow("aa", 15 * 2, 17, DisplayName = "Two characters of same size")]
        [DataRow("a\na", 15, 17 * 2, DisplayName = "Two of same characters on different rows")]
        [DataRow("ab", 15 + 13, 17, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", 15, 17 * 2, DisplayName = "Two characters of different size on different rows")]
        public void MeasureString_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), _font.MeasureString(testString));

        [DataTestMethod]
        [DataRow("a", 0, "a", DisplayName = "Single char that does not fit")]
        [DataRow("aaaa", 1000, "aaaa", DisplayName = "Many chars that fit with margin")]
        [DataRow("a a", 15, "a\na", DisplayName = "Two space separated chars but only one fits")]
        [DataRow("a a", 15 + 14 + 15, "a a", DisplayName = "Two space separated chars and both fit")]
        [DataRow("a a", 15 + 14 + 15 - 1, "a\na", DisplayName = "Two space separated chars that barely does not fit")]
        [DataRow("a a aaaaa a a", 15, "a\na\naaaaa a a", DisplayName = "Sentence with word that can never fit")]
        public void WrapString_CorrectValue(string testString, float testWidth, string expectedResult) =>
            Assert.AreEqual(expectedResult, _font.WrapString(testString, testWidth));
    }
}
