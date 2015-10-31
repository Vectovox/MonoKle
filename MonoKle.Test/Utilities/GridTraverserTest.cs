using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonoKle.Core;
using Microsoft.Xna.Framework;

namespace MonoKle.Utilities
{
    [TestClass]
    public class GridTraverserTest
    {
        private float cellSize = 32;

        [TestMethod]
        public void EqualMethods()
        {
            GridTraverser gt = new GridTraverser(cellSize);

            var all = gt.TraverseAll(new Vector2(5, 10), new Vector2(55, 120));
            int i = 0;
            foreach(Vector2DInteger v in gt.TraverseIteratively(new Vector2(5, 10), new Vector2(55, 120)))
            {
                Assert.AreEqual(all[i], v);
                i++;
            }
        }

        [TestMethod]
        public void CorrectHorizontal()
        {
            GridTraverser gt = new GridTraverser(cellSize);

            CollectionAssert.AreEqual(new Vector2DInteger[]{
                new Vector2DInteger(0,1),
                new Vector2DInteger(1,1),
                new Vector2DInteger(2,1)
            }, gt.TraverseAll(new Vector2(14, 49), new Vector2(82, 49)).ToArray());
        }

        [TestMethod]
        public void CorrectVertical()
        {
            GridTraverser gt = new GridTraverser(cellSize);

            CollectionAssert.AreEqual(new Vector2DInteger[]{
                new Vector2DInteger(1,0),
                new Vector2DInteger(1,1),
                new Vector2DInteger(1,2)
            }, gt.TraverseAll(new Vector2(47, 14), new Vector2(47, 81)).ToArray());
        }

        [TestMethod]
        public void CorrectDiagonalAbove()
        {
            GridTraverser gt = new GridTraverser(cellSize);

            CollectionAssert.AreEqual(new Vector2DInteger[]{
                new Vector2DInteger(1,0),
                new Vector2DInteger(1,1),
                new Vector2DInteger(2,1)
            }, gt.TraverseAll(new Vector2(46, 26), new Vector2(75, 55)).ToArray());
        }

        [TestMethod]
        public void CorrectDiagonalBelow()
        {
            GridTraverser gt = new GridTraverser(cellSize);

            CollectionAssert.AreEqual(new Vector2DInteger[]{
                new Vector2DInteger(0,1),
                new Vector2DInteger(0,2),
                new Vector2DInteger(1,2)
            }, gt.TraverseAll(new Vector2(11, 58), new Vector2(42, 89)).ToArray());
        }

        [TestMethod]
        public void CorrectDiagonalMiddle()
        {
            GridTraverser gt = new GridTraverser(cellSize);

            CollectionAssert.AreEqual(new Vector2DInteger[]{
                new Vector2DInteger(0,0),
                new Vector2DInteger(0,1),
                new Vector2DInteger(1,1),
                new Vector2DInteger(1,2),
                new Vector2DInteger(2,2)
            }, gt.TraverseAll(new Vector2(9, 21), new Vector2(76, 91)).ToArray());
        }

        [TestMethod]
        public void CorrectHorizontalReverse()
        {
            GridTraverser gt = new GridTraverser(cellSize);

            var first = gt.TraverseAll(new Vector2(14, 49), new Vector2(82, 49));
            var second = gt.TraverseAll(new Vector2(82, 49), new Vector2(14, 49));
            second.Reverse();

            CollectionAssert.AreEqual(first, second);
        }

        [TestMethod]
        public void CorrectVerticalReverse()
        {
            GridTraverser gt = new GridTraverser(cellSize);

            var first = gt.TraverseAll(new Vector2(47, 14), new Vector2(47, 81));
            var second = gt.TraverseAll(new Vector2(47, 81), new Vector2(47, 14));
            second.Reverse();

            CollectionAssert.AreEqual(first, second);
        }

        [TestMethod]
        public void CorrectDiagonalAboveReverse()
        {
            GridTraverser gt = new GridTraverser(cellSize);

            var first = gt.TraverseAll(new Vector2(46, 26), new Vector2(75, 55));
            var second = gt.TraverseAll(new Vector2(75, 55), new Vector2(46, 26));
            second.Reverse();

            CollectionAssert.AreEqual(first, second);
        }

        [TestMethod]
        public void CorrectDiagonalBelowReverse()
        {
            GridTraverser gt = new GridTraverser(cellSize);

            var first = gt.TraverseAll(new Vector2(11, 58), new Vector2(42, 89));
            var second = gt.TraverseAll(new Vector2(42, 89), new Vector2(11, 58));
            second.Reverse();

            CollectionAssert.AreEqual(first, second);
        }

        [TestMethod]
        public void CorrectDiagonalMiddleReverse()
        {
            GridTraverser gt = new GridTraverser(cellSize);

            var first = gt.TraverseAll(new Vector2(9, 21), new Vector2(76, 91));
            var second = gt.TraverseAll(new Vector2(76, 91), new Vector2(9, 21));
            second.Reverse();

            CollectionAssert.AreEqual(first, second);
        }
    }
}
