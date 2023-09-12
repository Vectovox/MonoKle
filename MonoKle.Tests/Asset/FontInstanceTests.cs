using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Asset
{
    [TestClass]
    public class FontInstanceTests
    {
        private static readonly FontInstance _defaultFont = GetDefaultFont();
        private static readonly FontInstance _japaneseFont = GetJapaneseFont();

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
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), _defaultFont.WithCompactHeight(true).Measure(testString));

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
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), _defaultFont.Measure(testString));

        [DataTestMethod]
        [DataRow("", 0, DefaultSize2, DisplayName = "Empty string")]
        [DataRow("ab", (DefaultAWidth + DefaultBWidth) * DefaultSize2Scaling, DefaultSize2, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", DefaultAWidth * DefaultSize2Scaling, DefaultSize2 * 2, DisplayName = "Two characters of different size on different rows")]
        public void MeasureString_DifferentSize_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), _defaultFont.WithSize(DefaultSize2).Measure(testString));

        [DataTestMethod]
        [DataRow("", 0, DefaultSizeWithPadding, DisplayName = "Empty string")]
        [DataRow("ab", DefaultAWidth + DefaultBWidth, DefaultSizeWithPadding, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", DefaultAWidth, DefaultSizeWithPadding * 2, DisplayName = "Two characters of different size on different rows")]
        public void MeasureString_DifferentPadding_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), _defaultFont.WithLinePadding(DefaultLinePadding).Measure(testString));

        [DataTestMethod]
        [DataRow("", 0, DefaultSize2WithPadding, DisplayName = "Empty string")]
        [DataRow("ab", (DefaultAWidth + DefaultBWidth) * DefaultSize2Scaling, DefaultSize2WithPadding, DisplayName = "Two characters of different size")]
        [DataRow("a\nb", DefaultAWidth * DefaultSize2Scaling, DefaultSize2WithPadding * 2, DisplayName = "Two characters of different size on different rows")]
        public void MeasureString_DifferentSizeAndPadding_CorrectValue(string testString, float expectedWidth, float expectedHeight) =>
            Assert.AreEqual(new MVector2(expectedWidth, expectedHeight), _defaultFont.WithSize(DefaultSize2).WithLinePadding(DefaultLinePadding).Measure(testString));

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
            Assert.AreEqual(expectedResult, _defaultFont.Wrap(testString, testWidth).ToString());

        [DataTestMethod]
        [DataRow("カメラ速度", 3, "カメラ\n速度", DisplayName = "Single break")]
        [DataRow("カメラ速度", 2, "カメ\nラ速\n度", DisplayName = "Double break")]
        [DataRow("カメラ速度", 1, "カ\nメ\nラ\n速\n度", DisplayName = "Mega break")]
        [DataRow("カメaaラ速度", 3, "カメ\naaラ\n速度", DisplayName = "Not breaking latin")]
        public void WrapString_Japanese_CharacterWrappedCorrectly(string testString, float testWidth, string expectedResult) =>
            Assert.AreEqual(expectedResult, _japaneseFont.Wrap(testString, testWidth).ToString());

        [DataTestMethod]
        [DynamicData(nameof(UnallowedJapaneseLineStart))]
        public void WrapString_Japanese_UnallowedLineStart_NotWrapped(int charValue)
        {
            char c = (char)charValue;
            var toTest = $"カメラ{c}速度";
            var expected = $"カメ\nラ{c}速\n度";
            Assert.AreEqual(expected, _japaneseFont.Wrap(toTest, 3).ToString());
        }

        [DataTestMethod]
        [DynamicData(nameof(UnallowedJapaneseLineEnd))]
        public void WrapString_Japanese_UnallowedLineEnd_NotWrapped(int charValue)
        {
            char c = (char)charValue;
            var toTest = $"カメ{c}ラ速度";
            var expected = $"カメ\n{c}ラ速\n度";
            Assert.AreEqual(expected, _japaneseFont.Wrap(toTest, 3).ToString());
        }

        [TestMethod]
        public void WrapString_MixedWrapMethods_CorrectWrapping()
        {
            var toTest = "すすすすすすすす す";
            var expected = "すすす\nすすす\nすす\nす";
            var wrapped = _japaneseFont.Wrap(toTest, 3);
            Assert.AreEqual(expected, wrapped.ToString());
        }

        [TestMethod]
        public void FluentSettings_Chained_SettingsRetained()
        {
            var expected = new FontInstance(_defaultFont.FontData)
            {
                LinePadding = 15,
                Size = 19,
                ColorTag = '#',
                CompactHeight = true,
            };
            var testedInstance = _defaultFont.WithLinePadding(expected.LinePadding).WithSize(expected.Size).WithCompactHeight(expected.CompactHeight);
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
                // Use expected character ranges, including some latin and stuff that will be unallowed
                // in certain situations
                // See: https://en.wikipedia.org/wiki/Line_breaking_rules_in_East_Asian_languages
                Chars = InclusiveRange(0x3000, 0x303f)
                    .Concat(InclusiveRange(0x3040, 0x309f))
                    .Concat(InclusiveRange(0x30a0, 0x31ff))
                    .Concat(InclusiveRange(0x4e00, 0x9faf))
                    .Concat(InclusiveRange(0xff00, 0xffef))
                    .Concat(InclusiveRange(0x0020, 0x00bb))
                    .Concat(InclusiveRange(0x2010, 0x2049))
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

        private static IEnumerable<object[]> UnallowedJapaneseLineStart
        {
            get
            {
                return new[]
                {
                    new object[] { 0x0021 },
                    // Quotations skipped for now.
                    //new object[] { 0x0022 },
                    //new object[] { 0x0027 },
                    new object[] { 0x0029 },
                    new object[] { 0x002c },
                    new object[] { 0x002e },
                    new object[] { 0x003a },
                    new object[] { 0x003b },
                    new object[] { 0x005d },
                    new object[] { 0x00bb },
                    new object[] { 0x2010 },
                    new object[] { 0x2013 },
                    new object[] { 0x203c },
                    new object[] { 0x2047 },
                    new object[] { 0x2048 },
                    new object[] { 0x2049 },
                    new object[] { 0x3001 },
                    new object[] { 0x3002 },
                    new object[] { 0x3005 },
                    new object[] { 0x3009 },
                    new object[] { 0x300b },
                    new object[] { 0x300d },
                    new object[] { 0x300f },
                    new object[] { 0x3011 },
                    new object[] { 0x3015 },
                    new object[] { 0x3017 },
                    new object[] { 0x3019 },
                    new object[] { 0x301c },
                    new object[] { 0x301f },
                    new object[] { 0x303b },
                    new object[] { 0x3041 },
                    new object[] { 0x3043 },
                    new object[] { 0x3045 },
                    new object[] { 0x3047 },
                    new object[] { 0x3049 },
                    new object[] { 0x3063 },
                    new object[] { 0x3083 },
                    new object[] { 0x3085 },
                    new object[] { 0x3087 },
                    new object[] { 0x308e },
                    new object[] { 0x3095 },
                    new object[] { 0x3096 },
                    new object[] { 0x30a0 },
                    new object[] { 0x30a1 },
                    new object[] { 0x30a3 },
                    new object[] { 0x30a5 },
                    new object[] { 0x30a7 },
                    new object[] { 0x30a9 },
                    new object[] { 0x30c3 },
                    new object[] { 0x30e3 },
                    new object[] { 0x30e5 },
                    new object[] { 0x30e7 },
                    new object[] { 0x30ee },
                    new object[] { 0x30f5 },
                    new object[] { 0x30f6 },
                    new object[] { 0x30fb },
                    new object[] { 0x30fc },
                    new object[] { 0x30fd },
                    new object[] { 0x30fe },
                    new object[] { 0x31f0 },
                    new object[] { 0x31f1 },
                    new object[] { 0x31f2 },
                    new object[] { 0x31f3 },
                    new object[] { 0x31f4 },
                    new object[] { 0x31f5 },
                    new object[] { 0x31f6 },
                    new object[] { 0x31f7 },
                    new object[] { 0x31f8 },
                    new object[] { 0x31f9 },
                    new object[] { 0x31fa },
                    new object[] { 0x31fb },
                    new object[] { 0x31fc },
                    new object[] { 0x31fd },
                    new object[] { 0x31fe },
                    new object[] { 0x31ff },
                    new object[] { 0xff1f },
                    new object[] { 0xff5d },
                    new object[] { 0xff60 },
                };
            }
        }

        private static IEnumerable<object[]> UnallowedJapaneseLineEnd
        {
            get
            {
                return new[]
                {
                    // Quotations skipped for now.
                    //new object[] { 0x22 },
                    //new object[] { 0x27 },
                    new object[] { 0x28 },
                    new object[] { 0x5b },
                    new object[] { 0xab },
                    new object[] { 0x3008 },
                    new object[] { 0x300a },
                    new object[] { 0x300c },
                    new object[] { 0x300e },
                    new object[] { 0x3010 },
                    new object[] { 0x3014 },
                    new object[] { 0x3016 },
                    new object[] { 0x3018 },
                    new object[] { 0x301d },
                    new object[] { 0xff5b },
                    new object[] { 0xff5f },
                };
            }
        }
    }
}
