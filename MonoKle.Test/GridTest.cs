namespace MonoKle {
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xna.Framework;

    [TestClass]
    public class GridTest {
        private const float cellSize = 32;
        private MGrid grid;

        [TestInitialize()]
        public void Initialize() => this.grid = new MGrid(32);

        [TestMethod]
        public void CellFromPoint_CorrectPositive() {
            var point = new MVector2(5, 35);
            var expected = new MPoint2(0, 1);
            Assert.AreEqual(expected, grid.CellFromPoint(point));
        }

        [TestMethod]
        public void CellFromPoint_CorrectNegative() {
            var point = new MVector2(-5, -35);
            var expected = new MPoint2(-1, -2);
            Assert.AreEqual(expected, grid.CellFromPoint(point));
        }

        [TestMethod]
        public void CellRectangle_CorrectPositive() {
            var point = new MPoint2(0, 1);
            var expected = new MRectangle(0, cellSize, cellSize, cellSize);
            Assert.AreEqual(expected, grid.CellRectangle(point));
        }

        [TestMethod]
        public void CellRectangle_CorrectNegative() {
            var point = new MPoint2(-1, -2);
            var expected = new MRectangle(-cellSize, -2 * cellSize, cellSize, cellSize);
            Assert.AreEqual(expected, grid.CellRectangle(point));
        }

        [TestMethod]
        public void CellsFromCircle_Correct() {
            var circle = new MCircle(new MVector2(16f, 16f), 17f);

            var expected = new List<MPoint2>()
            {
                new MPoint2(0,0),
                new MPoint2(0,-1),
                new MPoint2(-1,0),
                new MPoint2(1,0),
                new MPoint2(0,1)
            };
            var result = grid.CellsFromCircle(circle);

            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void CellsFromLine_EqualMethods() {
            var all = grid.CellsFromLine(new Vector2(5, 10), new Vector2(55, 120));
            int i = 0;
            foreach (MPoint2 v in grid.TraverseLine(new Vector2(5, 10), new Vector2(55, 120))) {
                Assert.AreEqual(all[i], v);
                i++;
            }
        }

        [TestMethod]
        public void CellsFromLine_CorrectHorizontal() => CollectionAssert.AreEqual(new MPoint2[]{
                new MPoint2(0,1),
                new MPoint2(1,1),
                new MPoint2(2,1)
            }, grid.CellsFromLine(new Vector2(14, 49), new Vector2(82, 49)).ToArray());

        [TestMethod]
        public void CellsFromLine_CorrectVertical() => CollectionAssert.AreEqual(new MPoint2[]{
                new MPoint2(1,0),
                new MPoint2(1,1),
                new MPoint2(1,2)
            }, grid.CellsFromLine(new Vector2(47, 14), new Vector2(47, 81)).ToArray());

        [TestMethod]
        public void CellsFromLine_CorrectDiagonalAbove() => CollectionAssert.AreEqual(new MPoint2[]{
                new MPoint2(1,0),
                new MPoint2(1,1),
                new MPoint2(2,1)
            }, grid.CellsFromLine(new Vector2(46, 26), new Vector2(75, 55)).ToArray());

        [TestMethod]
        public void CellsFromLine_CorrectDiagonalBelow() => CollectionAssert.AreEqual(new MPoint2[]{
                new MPoint2(0,1),
                new MPoint2(0,2),
                new MPoint2(1,2)
            }, grid.CellsFromLine(new Vector2(11, 58), new Vector2(42, 89)).ToArray());

        [TestMethod]
        public void CellsFromLine_CorrectDiagonalMiddle() => CollectionAssert.AreEqual(new MPoint2[]{
                new MPoint2(0,0),
                new MPoint2(0,1),
                new MPoint2(1,1),
                new MPoint2(1,2),
                new MPoint2(2,2)
            }, grid.CellsFromLine(new Vector2(9, 21), new Vector2(76, 91)).ToArray());

        [TestMethod]
        public void CellsFromLine_CorrectHorizontalReverse() {
            var first = grid.CellsFromLine(new Vector2(14, 49), new Vector2(82, 49));
            var second = grid.CellsFromLine(new Vector2(82, 49), new Vector2(14, 49));
            second.Reverse();

            CollectionAssert.AreEqual(first, second);
        }

        [TestMethod]
        public void CellsFromLine_CorrectVerticalReverse() {
            var first = grid.CellsFromLine(new Vector2(47, 14), new Vector2(47, 81));
            var second = grid.CellsFromLine(new Vector2(47, 81), new Vector2(47, 14));
            second.Reverse();

            CollectionAssert.AreEqual(first, second);
        }

        [TestMethod]
        public void CellsFromLine_CorrectDiagonalAboveReverse() {
            var first = grid.CellsFromLine(new Vector2(46, 26), new Vector2(75, 55));
            var second = grid.CellsFromLine(new Vector2(75, 55), new Vector2(46, 26));
            second.Reverse();

            CollectionAssert.AreEqual(first, second);
        }

        [TestMethod]
        public void CellsFromLine_CorrectDiagonalBelowReverse() {
            var first = grid.CellsFromLine(new Vector2(11, 58), new Vector2(42, 89));
            var second = grid.CellsFromLine(new Vector2(42, 89), new Vector2(11, 58));
            second.Reverse();

            CollectionAssert.AreEqual(first, second);
        }

        [TestMethod]
        public void CellsFromLine_CorrectDiagonalMiddleReverse() {
            var first = grid.CellsFromLine(new Vector2(9, 21), new Vector2(76, 91));
            var second = grid.CellsFromLine(new Vector2(76, 91), new Vector2(9, 21));
            second.Reverse();

            CollectionAssert.AreEqual(first, second);
        }
    }
}