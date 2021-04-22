using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MonoKle.Tests
{
    [TestClass]
    public class MTextureTests
    {
        [TestMethod]
        public void DurationRow_Animated_Correct() =>
            Assert.AreEqual(TimeSpan.FromMilliseconds(500), new MTexture(null, new MRectangleInt(10, 10, 30, 40), 5, 1, 0, 10).DurationRow);

        [TestMethod]
        public void DurationRow_NonAnimated_Correct() =>
            Assert.AreEqual(TimeSpan.MaxValue, new MTexture(null, new MRectangleInt(10, 10, 30, 40), 5, 1, 0, 0).DurationRow);

        [TestMethod]
        public void DurationColumn_Animated_Correct() =>
            Assert.AreEqual(TimeSpan.FromMilliseconds(500), new MTexture(null, new MRectangleInt(10, 10, 30, 40), 1, 5, 0, 10).DurationColumn);

        [TestMethod]
        public void DurationColumn_NonAnimated_Correct() =>
            Assert.AreEqual(TimeSpan.MaxValue, new MTexture(null, new MRectangleInt(10, 10, 30, 40), 1, 5, 0, 0).DurationColumn);

        [DataTestMethod]
        [DataRow(14, 2, 2, 3, DisplayName = "2 Margin")] // [][][X][X][X][][][][][Y][Y][Y][][]
        [DataRow(12, 3, 1, 2, DisplayName = "1 Margin")] // [][X][X][][][Y][Y][][][Z][Z][]
        [DataRow(6, 3, 0, 2, DisplayName = "0 Margin")]  // [X][X][Y][Y][Z][Z]
        public void FrameWidth_Correct(int width, int frameCount, int atlasMargin, int expectedResult)
        {
            var sut = new MTexture(null, new MRectangleInt(0, 0, width, 5), frameCount, 1, atlasMargin);
            Assert.AreEqual(expectedResult, sut.FrameWidth);
        }

        [DataTestMethod]
        [DataRow(14, 2, 2, 3, DisplayName = "2 Margin")] // [][][X][X][X][][][][][Y][Y][Y][][]
        [DataRow(12, 3, 1, 2, DisplayName = "1 Margin")] // [][X][X][][][Y][Y][][][Z][Z][]
        [DataRow(6, 3, 0, 2, DisplayName = "0 Margin")]  // [X][X][Y][Y][Z][Z]
        public void FrameHeight_Correct(int height, int frameCount, int atlasMargin, int expectedResult)
        {
            var sut = new MTexture(null, new MRectangleInt(0, 0, 5, height), 1, frameCount, atlasMargin);
            Assert.AreEqual(expectedResult, sut.FrameHeight);
        }

        [DataTestMethod]
        // Single row
        [DataRow(64, 32, 2, 1, 0, 0, 0, 0, 0, DisplayName = "Single row - No Margin - First frame")]
        [DataRow(64, 32, 2, 1, 0, 1, 0, 32, 0, DisplayName = "Single row - No Margin - Last frame")]
        [DataRow(68, 34, 2, 1, 1, 0, 0, 1, 1, DisplayName = "Single row - Margin - First frame")]
        [DataRow(68, 34, 2, 1, 1, 1, 0, 35, 1, DisplayName = "Single row - Margin - Last frame")]
        // Single column
        [DataRow(64, 32, 1, 2, 0, 0, 0, 0, 0, DisplayName = "Single column - No Margin - First frame")]
        [DataRow(64, 32, 1, 2, 0, 0, 1, 0, 16, DisplayName = "Single column - No Margin - Last frame")]
        [DataRow(68, 36, 1, 2, 1, 0, 0, 1, 1, DisplayName = "Single column - Margin - First frame")]
        [DataRow(68, 36, 1, 2, 1, 0, 1, 1, 19, DisplayName = "Single column - Margin - Last frame")]
        // Matrix - 0 Margin
        [DataRow(64, 32, 2, 2, 0, 0, 0, 0, 0, DisplayName = "Matrix - 0 Margin - Top Left frame")]
        [DataRow(64, 32, 2, 2, 0, 1, 0, 32, 0, DisplayName = "Matrix - 0 Margin - Top Right frame")]
        [DataRow(64, 32, 2, 2, 0, 0, 1, 0, 16, DisplayName = "Matrix - 0 Margin - Bottom Left frame")]
        [DataRow(64, 32, 2, 2, 0, 1, 1, 32, 16, DisplayName = "Matrix - 0 Margin - Bottom Right frame")]
        // Matrix - 1 Margin
        [DataRow(68, 36, 2, 2, 1, 0, 0, 1, 1, DisplayName = "Matrix - 1 Margin - Top Left frame")]
        [DataRow(68, 36, 2, 2, 1, 1, 0, 35, 1, DisplayName = "Matrix - 1 Margin - Top Right frame")]
        [DataRow(68, 36, 2, 2, 1, 0, 1, 1, 19, DisplayName = "Matrix - 1 Margin - Bottom Left frame")]
        [DataRow(68, 36, 2, 2, 1, 1, 1, 35, 19, DisplayName = "Matrix - 1 Margin - Bottom Right frame")]
        // Matrix - 2 Margin
        [DataRow(72, 40, 2, 2, 2, 0, 0, 2, 2, DisplayName = "Matrix - 2 Margin - Top Left frame")]
        [DataRow(72, 40, 2, 2, 2, 1, 0, 38, 2, DisplayName = "Matrix - 2 Margin - Top Right frame")]
        [DataRow(72, 40, 2, 2, 2, 0, 1, 2, 22, DisplayName = "Matrix - 2 Margin - Bottom Left frame")]
        [DataRow(72, 40, 2, 2, 2, 1, 1, 38, 22, DisplayName = "Matrix - 2 Margin - Bottom Right frame")]
        public void GetCell_AtlasRectangleCorrect(int width, int height, int columns, int rows, int margin, int column, int row, int posX, int posY)
        {
            var sut = new MTexture(null, new MRectangleInt(0, 0, width, height), columns, rows, margin);
            var result = sut.GetCell(column, row);
            Assert.AreEqual(new MRectangleInt(posX, posY, result.FrameWidth, result.FrameHeight), result.AtlasRectangle);
            Assert.AreEqual(1, result.FrameRows);
            Assert.AreEqual(1, result.FrameColumns);
            Assert.AreEqual(0, result.FrameMargin);
        }

        [DataTestMethod]
        [DataRow(0, 0, 0, DisplayName = "Top Left - No Margin")]
        [DataRow(1, 0, 0, DisplayName = "Top Right - No Margin")]
        [DataRow(0, 1, 0, DisplayName = "Bottom Left - No Margin")]
        [DataRow(1, 1, 0, DisplayName = "Bottom Right - No Margin")]
        [DataRow(0, 0, 1, DisplayName = "Top Left - 1 Margin")]
        [DataRow(1, 0, 1, DisplayName = "Top Right - 1 Margin")]
        [DataRow(0, 1, 1, DisplayName = "Bottom Left - 1 Margin")]
        [DataRow(1, 1, 1, DisplayName = "Bottom Right - 1 Margin")]
        public void GetCell_AfterLastFrame_WrapsAround(int x, int y, int margin)
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 68, 36), 2, 2, margin);
            Assert.AreEqual(sut.GetCell(x, y).AtlasRectangle, sut.GetCell(x + 2, y).AtlasRectangle);
            Assert.AreEqual(sut.GetCell(x, y).AtlasRectangle, sut.GetCell(x, y + 2).AtlasRectangle);
            Assert.AreEqual(sut.GetCell(x, y).AtlasRectangle, sut.GetCell(x + 2, y + 2).AtlasRectangle);
        }

        [DataTestMethod]
        // Single row
        [DataRow(64, 2, 1, 0, 0, 0, DisplayName = "Single row - No Margin - First frame")]
        [DataRow(64, 2, 1, 0, 1, 32, DisplayName = "Single row - No Margin - Last frame")]
        [DataRow(68, 2, 1, 1, 0, 1, DisplayName = "Single row - Margin - First frame")]
        [DataRow(68, 2, 1, 1, 1, 35, DisplayName = "Single row - Margin - Last frame")]
        // Multiple rows
        [DataRow(64, 2, 2, 0, 0, 0, DisplayName = "Multiple rows - No Margin - First frame")]
        [DataRow(64, 2, 2, 0, 1, 32, DisplayName = "Multiple rows - No Margin - Last frame")]
        [DataRow(68, 2, 2, 1, 0, 1, DisplayName = "Multiple rows - Margin - First frame")]
        [DataRow(68, 2, 2, 1, 1, 35, DisplayName = "Multiple rows - Margin - Last frame")]
        public void GetColumn_AtlasRectangleCorrect(int width, int columns, int rows, int margin, int column, int posX)
        {
            var sut = new MTexture(null, new MRectangleInt(0, 0, width, 32), columns, rows, margin);
            var result = sut.GetColumn(column);
            Assert.AreEqual(new MRectangleInt(posX, 0, result.FrameWidth, 32), result.AtlasRectangle);
            Assert.AreEqual(1, result.FrameColumns);
            Assert.AreEqual(1, result.FrameRows);
            Assert.AreEqual(0, result.FrameMargin);
            Assert.AreEqual(32, result.FrameHeight);
        }

        [DataTestMethod]
        [DataRow(false, DisplayName = "Without margin")]
        [DataRow(true, DisplayName = "With margin")]
        public void GetColumn_AfterLastFrame_WrapsAround(bool margin)
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 6, 1, margin ? 1 : 0);
            Assert.AreEqual(sut.GetColumn(0).AtlasRectangle, sut.GetColumn(6).AtlasRectangle);
            Assert.AreEqual(sut.GetColumn(5).AtlasRectangle, sut.GetColumn(11).AtlasRectangle);
            Assert.AreEqual(sut.GetColumn(6).AtlasRectangle, sut.GetColumn(12).AtlasRectangle);
        }

        [DataTestMethod]
        // Single column
        [DataRow(64, 1, 2, 0, 0, 0, DisplayName = "Single row - No Margin - First frame")]
        [DataRow(64, 1, 2, 0, 1, 32, DisplayName = "Single row - No Margin - Last frame")]
        [DataRow(68, 1, 2, 1, 0, 1, DisplayName = "Single row - Margin - First frame")]
        [DataRow(68, 1, 2, 1, 1, 35, DisplayName = "Single row - Margin - Last frame")]
        // Multiple columns
        [DataRow(64, 2, 2, 0, 0, 0, DisplayName = "Multiple rows - No Margin - First frame")]
        [DataRow(64, 2, 2, 0, 1, 32, DisplayName = "Multiple rows - No Margin - Last frame")]
        [DataRow(68, 2, 2, 1, 0, 1, DisplayName = "Multiple rows - Margin - First frame")]
        [DataRow(68, 2, 2, 1, 1, 35, DisplayName = "Multiple rows - Margin - Last frame")]
        public void GetRow_AtlasRectangleCorrect(int height, int columns, int rows, int margin, int row, int posY)
        {
            var sut = new MTexture(null, new MRectangleInt(0, 0, 32, height), columns, rows, margin);
            var result = sut.GetRow(row);
            Assert.AreEqual(new MRectangleInt(0, posY, 32, result.FrameHeight), result.AtlasRectangle);
            Assert.AreEqual(1, result.FrameColumns);
            Assert.AreEqual(1, result.FrameRows);
            Assert.AreEqual(0, result.FrameMargin);
            Assert.AreEqual(32, result.FrameWidth);
        }

        [DataTestMethod]
        [DataRow(false, DisplayName = "Without margin")]
        [DataRow(true, DisplayName = "With margin")]
        public void GetRow_AfterLastFrame_WrapsAround(bool margin)
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 60), 1, 6, margin ? 1 : 0);
            Assert.AreEqual(sut.GetRow(0).AtlasRectangle, sut.GetRow(6).AtlasRectangle);
            Assert.AreEqual(sut.GetRow(5).AtlasRectangle, sut.GetRow(11).AtlasRectangle);
            Assert.AreEqual(sut.GetRow(6).AtlasRectangle, sut.GetRow(12).AtlasRectangle);
        }

        [TestMethod]
        public void AnimateRow_Frame_EqualToGetCell()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 2, 2, 0);
            Assert.AreEqual(sut[0, 0].AtlasRectangle, sut.AnimateRow(0, 0).AtlasRectangle);
            Assert.AreEqual(sut[1, 0].AtlasRectangle, sut.AnimateRow(0, 1).AtlasRectangle);
            Assert.AreEqual(sut[0, 1].AtlasRectangle, sut.AnimateRow(1, 0).AtlasRectangle);
            Assert.AreEqual(sut[1, 1].AtlasRectangle, sut.AnimateRow(1, 1).AtlasRectangle);
        }

        [TestMethod]
        public void AnimateRow_Elapsed_EqualToFrame()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 2, 2, 0, 2);
            Assert.AreEqual(sut.AnimateRow(0, 0).AtlasRectangle, sut.AnimateRow(0, TimeSpan.Zero).AtlasRectangle);
            Assert.AreEqual(sut.AnimateRow(1, 0).AtlasRectangle, sut.AnimateRow(1, TimeSpan.Zero).AtlasRectangle);
            Assert.AreEqual(sut.AnimateRow(0, 1).AtlasRectangle, sut.AnimateRow(0, TimeSpan.FromMilliseconds(500)).AtlasRectangle);
            Assert.AreEqual(sut.AnimateRow(1, 1).AtlasRectangle, sut.AnimateRow(1, TimeSpan.FromMilliseconds(500)).AtlasRectangle);
        }

        [TestMethod]
        public void AnimateColumn_Frame_EqualToGetCell()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 2, 2, 0);
            Assert.AreEqual(sut[0, 0].AtlasRectangle, sut.AnimateColumn(0, 0).AtlasRectangle);
            Assert.AreEqual(sut[0, 1].AtlasRectangle, sut.AnimateColumn(0, 1).AtlasRectangle);
            Assert.AreEqual(sut[1, 0].AtlasRectangle, sut.AnimateColumn(1, 0).AtlasRectangle);
            Assert.AreEqual(sut[1, 1].AtlasRectangle, sut.AnimateColumn(1, 1).AtlasRectangle);
        }

        [TestMethod]
        public void AnimateColumn_Elapsed_EqualToFrame()
        {
            var sut = new MTexture(null, new MRectangleInt(10, 10, 30, 40), 2, 2, 0, 2);
            Assert.AreEqual(sut.AnimateColumn(0, 0).AtlasRectangle, sut.AnimateColumn(0, TimeSpan.Zero).AtlasRectangle);
            Assert.AreEqual(sut.AnimateColumn(1, 0).AtlasRectangle, sut.AnimateColumn(1, TimeSpan.Zero).AtlasRectangle);
            Assert.AreEqual(sut.AnimateColumn(0, 1).AtlasRectangle, sut.AnimateColumn(0, TimeSpan.FromMilliseconds(500)).AtlasRectangle);
            Assert.AreEqual(sut.AnimateColumn(1, 1).AtlasRectangle, sut.AnimateColumn(1, TimeSpan.FromMilliseconds(500)).AtlasRectangle);
        }

        [DataTestMethod]
        [DataRow(100, 50, 0, 1, 0, 1, DisplayName = "Zero Columns")]
        [DataRow(100, 50, -1, 1, 0, 1, DisplayName = "Negative Columns")]
        [DataRow(100, 50, 1, 0, 0, 1, DisplayName = "Zero Rows")]
        [DataRow(100, 50, 1, -1, 0, 1, DisplayName = "Negative Rows")]
        [DataRow(100, 50, 1, 1, -1, 1, DisplayName = "Negative Margin")]
        [DataRow(100, 50, 1, 1, 0, -1, DisplayName = "Negative Frame rate")]
        [DataRow(100, 50, 3, 1, 0, 1, DisplayName = "Columns not divisible")]
        [DataRow(100, 50, 1, 3, 0, 1, DisplayName = "Rows not divisible")]
        [DataRow(56, 100, 3, 1, 1, 1, DisplayName = "Margin breaking column divisibility")]
        [DataRow(100, 56, 1, 3, 1, 1, DisplayName = "Margin breaking row divisibility")]
        public void ConstructorAssertion_ThrowsException(int width, int height, int columns, int rows, int margin, int frameRate) =>
            Assert.ThrowsException<ArgumentException>(() => new MTexture(null, new MRectangleInt(0, 0, width, height), columns, rows, margin, frameRate));
    }
}
