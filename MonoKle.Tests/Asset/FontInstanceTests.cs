using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Asset
{
    [TestClass]
    public class FontInstanceTests
    {
        private const int DefaultSize = 20;
        private const int DefaultSize2 = 10;
        private const float DefaultSize2Scaling = DefaultSize2 / (float)DefaultSize;

        private const int DefaultLinePadding = 10;
        private const int DefaultSizeWithPadding = DefaultSize + DefaultLinePadding;
        private const int DefaultSize2WithPadding = DefaultSize2 + DefaultLinePadding;

        private const int DefaultSpaceWidth = 15;
        private const int DefaultAWidth = 14;
        private const int DefaultBWidth = 13;
        private const int DefaultGWidth = 17;
        private const int DeafultJWidth = 17;

        private const int DefaultAHeight = 15;
        private const int DefaultBHeight = 20;
        private const int DefaultGHeight = 21;
        private const int DefaultJHeight = 26;

        private const int JapWidth = 1;
        private const int JapHeight = 1;

        [DataTestMethod]
        [DataRow("", 0, 0, DisplayName = "Empty string")]
        [DataRow("a", DefaultAWidth, DefaultAHeight, DisplayName = "Single character 'a'")]
        [DataRow("g", DefaultGWidth, DefaultGHeight, DisplayName = "Single character 'g'")]
        [DataRow("j", DeafultJWidth, DefaultJHeight, DisplayName = "Single character 'j'")]
        [DataRow("ab", DefaultAWidth + DefaultBWidth, DefaultBHeight, DisplayName = "Two characters of different height")]
        [DataRow("ag", DefaultAWidth + DefaultGWidth, DefaultGHeight, DisplayName = "Two characters of different starting position")]
        public void MeasureString_Compact_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), GetDefaultFont().WithCompactHeight(true).Measure(testString));

        [DataTestMethod]
        [DataRow("", 0, DefaultSize, DisplayName = "Empty string")]
        [DataRow(" ", DefaultSpaceWidth, DefaultSize, DisplayName = "Only a space")]
        [DataRow("\n", 0, DefaultSize * 2, DisplayName = "Only a newline")]
        [DataRow("a", DefaultAWidth, DefaultSize, DisplayName = "Only a character")]
        [DataRow("aa", DefaultAWidth * 2, DefaultSize, DisplayName = "Two characters of same size")]
        [DataRow("a\na", DefaultAWidth, DefaultSize * 2, DisplayName = "Two of same characters on different rows")]
        [DataRow("ab", DefaultAWidth + DefaultBWidth, DefaultSize, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", DefaultAWidth, DefaultSize * 2, DisplayName = "Two characters of different size on different rows")]
        [DataRow("a#betw\neen#b", DefaultAWidth + DefaultBWidth, DefaultSize, DisplayName = "Color tag not measured")]
        public void MeasureString_DefaultSettings_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), GetDefaultFont().Measure(testString));

        [DataTestMethod]
        [DataRow("", 0, DefaultSize2, DisplayName = "Empty string")]
        [DataRow("ab", (DefaultAWidth + DefaultBWidth) * DefaultSize2Scaling, DefaultSize2, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", DefaultAWidth * DefaultSize2Scaling, DefaultSize2 * 2, DisplayName = "Two characters of different size on different rows")]
        public void MeasureString_DifferentSize_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), GetDefaultFont().WithSize(DefaultSize2).Measure(testString));

        [DataTestMethod]
        [DataRow("", 0, DefaultSizeWithPadding, DisplayName = "Empty string")]
        [DataRow("ab", DefaultAWidth + DefaultBWidth, DefaultSizeWithPadding, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", DefaultAWidth, DefaultSizeWithPadding * 2, DisplayName = "Two characters of different size on different rows")]
        public void MeasureString_DifferentPadding_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), GetDefaultFont().WithLinePadding(DefaultLinePadding).Measure(testString));

        [DataTestMethod]
        [DataRow("", 0, DefaultSize2WithPadding, DisplayName = "Empty string")]
        [DataRow("ab", (DefaultAWidth + DefaultBWidth) * DefaultSize2Scaling, DefaultSize2WithPadding, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", DefaultAWidth * DefaultSize2Scaling, DefaultSize2WithPadding * 2, DisplayName = "Two characters of different size on different rows")]
        public void MeasureString_DifferentSizeAndPadding_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), GetDefaultFont().WithSize(DefaultSize2).WithLinePadding(DefaultLinePadding).Measure(testString));

        [DataTestMethod]
        [DataRow("a", 0, "a", DisplayName = "Single char that does not fit")]
        [DataRow("aaaa", 1000, "aaaa", DisplayName = "Many chars that fit with margin")]
        [DataRow("a a", DefaultAWidth, "a\na", DisplayName = "Two space separated chars but only one fits")]
        [DataRow("a a", DefaultAWidth + DefaultSpaceWidth, "a\na", DisplayName = "Final char does not fit")]
        [DataRow("a a", DefaultAWidth + DefaultSpaceWidth + DefaultAWidth, "a a", DisplayName = "Two space separated chars and both fit")]
        [DataRow("a a", DefaultAWidth + DefaultSpaceWidth + DefaultAWidth - 1, "a\na", DisplayName = "Two space separated chars that barely does not fit")]
        [DataRow("a a aaaaa a a", DefaultAWidth, "a\na\naaaaa a a", DisplayName = "Sentence with word that can never fit")]
        [DataRow("aaaa aa a aaa", 5.5f * DefaultAWidth + DefaultSpaceWidth, "aaaa\naa a\naaa", DisplayName = "Newline correctly resets width calculation")]
        [DataRow(" a", DefaultSpaceWidth, "\na", DisplayName = "Single line starts with space")]
        [DataRow(" a a", DefaultSpaceWidth, "\na\na", DisplayName = "Multi-line starts with space")]
        public void WrapString_DefaultSettings_CorrectValue(string testString, float testWidth, string expectedResult) =>
            Assert.AreEqual(expectedResult, GetDefaultFont().Wrap(testString, testWidth).ToString());

        [DataTestMethod]
        [DataRow("カメラ速度", 3, "カメラ\n速度", DisplayName = "Single break")]
        [DataRow("カメラ速度", 2, "カメ\nラ速\n度", DisplayName = "Double break")]
        [DataRow("カメラ速度", 1, "カ\nメ\nラ\n速\n度", DisplayName = "Mega break")]
        [DataRow("カメaaラ速度", 3, "カメ\naaラ\n速度", DisplayName = "Not breaking latin")]
        public void WrapString_Japanese_CharacterWrappedCorrectly(string testString, float testWidth, string expectedResult) =>
            Assert.AreEqual(expectedResult, GetJapaneseFont().Wrap(testString, testWidth).ToString());

        [TestMethod]
        public void FluentSettings_Chained_SettingsRetained()
        {
            var expected = new FontInstance(GetDefaultFont().FontData)
            {
                LinePadding = 15,
                Size = 19,
                ColorTag = '#',
                CompactHeight = true,
            };
            var testedInstance = GetDefaultFont().WithLinePadding(expected.LinePadding).WithSize(expected.Size).WithCompactHeight(expected.CompactHeight);
            Assert.AreEqual(expected.LinePadding, testedInstance.LinePadding);
            Assert.AreEqual(expected.Size, testedInstance.Size);
            Assert.AreEqual(expected.ColorTag, testedInstance.ColorTag);
            Assert.AreEqual(expected.CompactHeight, testedInstance.CompactHeight);
        }

        private static FontInstance GetDefaultFont()
        {
            var fontFile = new FontFile
            {
                Info = new FontInfo
                {

                },
                Common = new FontCommon
                {
                    LineHeight = DefaultSize,
                },
                Chars = new List<FontChar>
                {
                    new FontChar
                    {
                        ID = 32, // space ' '
                        XAdvance = DefaultSpaceWidth,
                    },
                    new FontChar
                    {
                        ID = 97, // a
                        XAdvance = DefaultAWidth,
                        Height = DefaultAHeight,
                        YOffset = 9,
                    },
                    new FontChar
                    {
                        ID = 98, // b
                        XAdvance = DefaultBWidth,
                        Height = DefaultBHeight,
                        YOffset = 4,
                    },
                    new FontChar
                    {
                        ID = 103, // g
                        XAdvance = DefaultGWidth,
                        Height = DefaultGHeight,
                        YOffset = 9,
                    },
                    new FontChar
                    {
                        ID = 106, // j
                        XAdvance = DeafultJWidth,
                        Height = DefaultJHeight,
                        YOffset = 4,
                    }
                },
            };
            var data = new FontData("ID", fontFile, new List<Microsoft.Xna.Framework.Graphics.Texture2D>());
            return new FontInstance(data) { ColorTag = '#' };
        }

        private static FontInstance GetJapaneseFont()
        {
            var fontFile = new FontFile
            {
                Info = new FontInfo
                {

                },
                Common = new FontCommon
                {
                    LineHeight = JapHeight,
                },
                Chars = InclusiveRange(0x3000, 0x303f)
                    .Concat(InclusiveRange(0x3040, 0x309f))
                    .Concat(InclusiveRange(0x30a0, 0x30ff))
                    .Concat(InclusiveRange(0x4e00, 0x9faf))
                    .Concat(InclusiveRange(0xff00, 0xffef))
                    .Concat(InclusiveRange(97, 97)) // Add some latin too
                    .Select(id => new FontChar
                    {
                        ID = id,
                        XAdvance = JapWidth,
                        Height = JapHeight,
                        YOffset = 0,
                    }).ToList(),
            };
            var data = new FontData("ID", fontFile, new List<Microsoft.Xna.Framework.Graphics.Texture2D>());
            return new FontInstance(data) { ColorTag = '#' };
        }

        private static IEnumerable<int> InclusiveRange(int start, int end)
        {
            for (var i = start; i <= end; i++)
            {
                yield return i;
            }
        }
    }
}
