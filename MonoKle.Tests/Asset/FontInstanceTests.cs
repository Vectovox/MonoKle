﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MonoKle.Asset
{
    [TestClass]
    public class FontInstanceTests
    {
        private readonly FontInstance _font;
        private readonly FontData _fontData;

        private const int Size = 20;
        private const int Size2 = 10;
        private const float Size2Scaling = Size2 / (float)Size;

        private const int LinePadding = 10;
        private const int SizeWithPadding = Size + LinePadding;
        private const int Size2WithPadding = Size2 + LinePadding;

        private const int SpaceWidth = 15;
        private const int AWidth = 14;
        private const int BWidth = 13;
        private const int GWidth = 17;
        private const int JWidth = 17;

        private const int AHeight = 15;
        private const int BHeight = 20;
        private const int GHeight = 21;
        private const int JHeight = 26;

        public FontInstanceTests()
        {
            var fontFile = new FontFile
            {
                Info = new FontInfo
                {
                    
                },
                Common = new FontCommon
                {
                    LineHeight = Size,
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
                        Height = AHeight,
                        YOffset = 9,
                    },
                    new FontChar
                    {
                        ID = 98, // b
                        XAdvance = BWidth,
                        Height = BHeight,
                        YOffset = 4,
                    },
                    new FontChar
                    {
                        ID = 103, // g
                        XAdvance = GWidth,
                        Height = GHeight,
                        YOffset = 9,
                    },
                    new FontChar
                    {
                        ID = 106, // j
                        XAdvance = JWidth,
                        Height = JHeight,
                        YOffset = 4,
                    }
                },
            };
            _fontData = new FontData("ID", fontFile, new List<Microsoft.Xna.Framework.Graphics.Texture2D>());
            _font = new FontInstance(_fontData) { ColorTag = '#' };
        }

        [DataTestMethod]
        [DataRow("", 0, 0, DisplayName = "Empty string")]
        [DataRow("a", AWidth, AHeight, DisplayName = "Single character 'a'")]
        [DataRow("g", GWidth, GHeight, DisplayName = "Single character 'g'")]
        [DataRow("j", JWidth, JHeight, DisplayName = "Single character 'j'")]
        [DataRow("ab", AWidth + BWidth, BHeight, DisplayName = "Two characters of different height")]
        [DataRow("ag", AWidth + GWidth, GHeight, DisplayName = "Two characters of different starting position")]
        public void MeasureString_Compact_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), _font.WithCompactHeight(true).Measure(testString));

        [DataTestMethod]
        [DataRow("", 0, Size, DisplayName = "Empty string")]
        [DataRow(" ", SpaceWidth, Size, DisplayName = "Only a space")]
        [DataRow("\n", 0, Size * 2, DisplayName = "Only a newline")]
        [DataRow("a", AWidth, Size, DisplayName = "Only a character")]
        [DataRow("aa", AWidth * 2, Size, DisplayName = "Two characters of same size")]
        [DataRow("a\na", AWidth, Size * 2, DisplayName = "Two of same characters on different rows")]
        [DataRow("ab", AWidth + BWidth, Size, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", AWidth, Size * 2, DisplayName = "Two characters of different size on different rows")]
        [DataRow("a#betw\neen#b", AWidth + BWidth, Size, DisplayName = "Color tag not measured")]
        public void MeasureString_DefaultSettings_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), _font.Measure(testString));

        [DataTestMethod]
        [DataRow("a", 0, "a", DisplayName = "Single char that does not fit")]
        [DataRow("aaaa", 1000, "aaaa", DisplayName = "Many chars that fit with margin")]
        [DataRow("a a", AWidth, "a\na", DisplayName = "Two space separated chars but only one fits")]
        [DataRow("a a", AWidth + SpaceWidth + AWidth, "a a", DisplayName = "Two space separated chars and both fit")]
        [DataRow("a a", AWidth + SpaceWidth + AWidth - 1, "a\na", DisplayName = "Two space separated chars that barely does not fit")]
        [DataRow("a a aaaaa a a", AWidth, "a\na\naaaaa a a", DisplayName = "Sentence with word that can never fit")]
        [DataRow("aaaa aa a aaa", 5.5f * AWidth + SpaceWidth, "aaaa\naa a\naaa", DisplayName = "Newline correctly resets width calculation")]
        public void WrapString_DefaultSettings_CorrectValue(string testString, float testWidth, string expectedResult) =>
            Assert.AreEqual(expectedResult, _font.Wrap(testString, testWidth).ToString());

        [DataTestMethod]
        [DataRow("", 0, Size2, DisplayName = "Empty string")]
        [DataRow("ab", (AWidth + BWidth) * Size2Scaling, Size2, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", AWidth * Size2Scaling, Size2 * 2, DisplayName = "Two characters of different size on different rows")]
        public void MeasureString_DifferentSize_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), _font.WithSize(Size2).Measure(testString));

        [DataTestMethod]
        [DataRow("", 0, SizeWithPadding, DisplayName = "Empty string")]
        [DataRow("ab", AWidth + BWidth, SizeWithPadding, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", AWidth, SizeWithPadding * 2, DisplayName = "Two characters of different size on different rows")]
        public void MeasureString_DifferentPadding_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), _font.WithLinePadding(LinePadding).Measure(testString));

        [DataTestMethod]
        [DataRow("", 0, Size2WithPadding, DisplayName = "Empty string")]
        [DataRow("ab", (AWidth + BWidth) * Size2Scaling, Size2WithPadding, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", AWidth * Size2Scaling, Size2WithPadding * 2, DisplayName = "Two characters of different size on different rows")]
        public void MeasureString_DifferentSizeAndPadding_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), _font.WithSize(Size2).WithLinePadding(LinePadding).Measure(testString));

        [TestMethod]
        public void FluentSettings_Chained_SettingsRetained()
        {
            var expected = new FontInstance(_fontData)
            {
                LinePadding = 15,
                Size = 19,
                ColorTag = '#',
                CompactHeight = true,
            };
            var testedInstance = _font.WithLinePadding(expected.LinePadding).WithSize(expected.Size).WithCompactHeight(expected.CompactHeight);
            Assert.AreEqual(expected.LinePadding, testedInstance.LinePadding);
            Assert.AreEqual(expected.Size, testedInstance.Size);
            Assert.AreEqual(expected.ColorTag, testedInstance.ColorTag);
            Assert.AreEqual(expected.CompactHeight, testedInstance.CompactHeight);
        }
    }
}
