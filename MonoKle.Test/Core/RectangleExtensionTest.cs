namespace MonoKle.Core.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xna.Framework;

    [TestClass]
    public class RectangleExtensionTest
    {
        [TestMethod]
        public void TestContainsCoordinate()
        {
            Assert.IsTrue(new Rectangle(50, 51, 50, 50).Contains(new Vector2Int32(50, 51)));
            Assert.IsTrue(new Rectangle(50, 51, 50, 50).Contains(new Vector2Int32(100, 101)));
            Assert.IsTrue(new Rectangle(50, 51, 50, 50).Contains(new Vector2Int32(75, 75)));

            Assert.IsTrue(new Rectangle(-50, -51, 50, 50).Contains(new Vector2Int32(-25, -25)));
            Assert.IsTrue(new Rectangle(-50, -51, 50, 50).Contains(new Vector2Int32(-50, -51)));
            Assert.IsTrue(new Rectangle(-50, -51, 50, 50).Contains(new Vector2Int32(0, -1)));

            Assert.IsTrue(new Rectangle(-5, -5, 10, 10).Contains(new Vector2Int32(0, 0)));
            Assert.IsTrue(new Rectangle(-5, -5, 10, 10).Contains(new Vector2Int32(0, 1)));
            Assert.IsTrue(new Rectangle(-5, -5, 10, 10).Contains(new Vector2Int32(1, 0)));
            Assert.IsTrue(new Rectangle(-5, -5, 10, 10).Contains(new Vector2Int32(1, 1)));

            Assert.IsFalse(new Rectangle(50, 51, 50, 50).Contains(new Vector2Int32(49, 51)));
            Assert.IsFalse(new Rectangle(50, 51, 50, 50).Contains(new Vector2Int32(50, 50)));
            Assert.IsFalse(new Rectangle(50, 51, 50, 50).Contains(new Vector2Int32(100, 102)));
            Assert.IsFalse(new Rectangle(50, 51, 50, 50).Contains(new Vector2Int32(101, 101)));
            Assert.IsFalse(new Rectangle(50, 51, 50, 50).Contains(new Vector2Int32(-75, -75)));

            Assert.IsFalse(new Rectangle(-50, -51, 50, 50).Contains(new Vector2Int32(-50, -52)));
            Assert.IsFalse(new Rectangle(-50, -51, 50, 50).Contains(new Vector2Int32(-51, -51)));
            Assert.IsFalse(new Rectangle(-50, -51, 50, 50).Contains(new Vector2Int32(0, 0)));
            Assert.IsFalse(new Rectangle(-50, -51, 50, 50).Contains(new Vector2Int32(1, -1)));
        }

        [TestMethod]
        public void TestContainsRectangleSingle()
        {
            // TODO: These will fail as long as issue #2436 in MonoGame is not fixed.
            Assert.IsTrue(new Rectangle(50, 51, 50, 50).Contains(new RectangleSingle(50, 51, 50, 50)));
            Assert.IsTrue(new Rectangle(50, 51, 50, 50).Contains(new RectangleSingle(60, 60, 20, 20)));
            Assert.IsTrue(new Rectangle(-10, -10, 20, 20).Contains(new RectangleSingle(-5, -5, 10, 10)));

            Assert.IsFalse(new Rectangle(-10, -10, 20, 20).Contains(new RectangleSingle(-5, -5, 30, 10)));
            Assert.IsFalse(new Rectangle(-10, -10, 20, 20).Contains(new RectangleSingle(-5, -5, 10, 30)));
            Assert.IsFalse(new Rectangle(-10, -10, 20, 20).Contains(new RectangleSingle(-11, -5, 10, 10)));
            Assert.IsFalse(new Rectangle(-10, -10, 20, 20).Contains(new RectangleSingle(-5, -11, 10, 10)));
            Assert.IsFalse(new Rectangle(-10, -10, 20, 20).Contains(new RectangleSingle(50, 50, 10, 10)));
        }

        [TestMethod]
        public void TestCropCoordinates()
        {
            Assert.AreEqual(new Rectangle(5, 5, 2, 2), new Rectangle(0, 0, 10, 10).Crop(new Vector2Int32(5, 5), new Vector2Int32(7, 7)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 10, 10).Crop(new Vector2Int32(0, 0), new Vector2Int32(10, 10)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 10, 10).Crop(new Vector2Int32(-5, 0), new Vector2Int32(10, 10)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 10, 10).Crop(new Vector2Int32(0, -5), new Vector2Int32(10, 10)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 10, 10).Crop(new Vector2Int32(0, 0), new Vector2Int32(15, 10)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 10, 10).Crop(new Vector2Int32(0, 0), new Vector2Int32(10, 15)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 10, 10).Crop(new Vector2Int32(-5, -5), new Vector2Int32(150, 100)));
            Assert.AreEqual(new Rectangle(1, 0, 9, 10), new Rectangle(0, 0, 10, 10).Crop(new Vector2Int32(1, 0), new Vector2Int32(11, 10)));
            Assert.AreEqual(new Rectangle(0, 1, 10, 9), new Rectangle(0, 0, 10, 10).Crop(new Vector2Int32(0, 1), new Vector2Int32(10, 11)));
            Assert.AreEqual(new Rectangle(0, 0, 9, 10), new Rectangle(0, 0, 10, 10).Crop(new Vector2Int32(0, 0), new Vector2Int32(9, 10)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 9), new Rectangle(0, 0, 10, 10).Crop(new Vector2Int32(0, 0), new Vector2Int32(10, 9)));

            Assert.AreEqual(new Rectangle(1, 0, 9, 10), new Rectangle(10, 10, -10, -10).Crop(new Vector2Int32(1, 0), new Vector2Int32(11, 10)));
            Assert.AreEqual(new Rectangle(0, 1, 10, 9), new Rectangle(10, 10, -10, -10).Crop(new Vector2Int32(0, 1), new Vector2Int32(10, 11)));

            Assert.AreEqual(new Rectangle(1, 0, 9, 10), new Rectangle(0, 0, 10, 10).Crop(new Vector2Int32(12, 10), new Vector2Int32(1, 0)));
            Assert.AreEqual(new Rectangle(0, 1, 10, 9), new Rectangle(0, 0, 10, 10).Crop(new Vector2Int32(10, 12), new Vector2Int32(0, 1)));
        }

        [TestMethod]
        public void TestCropRectangle()
        {
            Assert.AreEqual(new Rectangle(5, 5, 2, 2), new Rectangle(0, 0, 10, 10).Crop(new Rectangle(5, 5, 2, 2)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 10, 10).Crop(new Rectangle(0, 0, 10, 10)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 10, 10).Crop(new Rectangle(-5, 0, 15, 10)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 10, 10).Crop(new Rectangle(0, -5, 10, 15)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 10, 10).Crop(new Rectangle(0, 0, 15, 10)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 10, 10).Crop(new Rectangle(0, 0, 10, 15)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 10, 10).Crop(new Rectangle(-5, -5, 155, 105)));
            Assert.AreEqual(new Rectangle(1, 0, 9, 10), new Rectangle(0, 0, 10, 10).Crop(new Rectangle(1, 0, 10, 10)));
            Assert.AreEqual(new Rectangle(0, 1, 10, 9), new Rectangle(0, 0, 10, 10).Crop(new Rectangle(0, 1, 10, 10)));
            Assert.AreEqual(new Rectangle(0, 0, 9, 10), new Rectangle(0, 0, 10, 10).Crop(new Rectangle(0, 0, 9, 10)));
            Assert.AreEqual(new Rectangle(0, 0, 10, 9), new Rectangle(0, 0, 10, 10).Crop(new Rectangle(0, 0, 10, 9)));

            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(10, 10, -10, -10).Crop(new Rectangle(0, 0, 15, 10)));
            Assert.AreEqual(new Rectangle(0, 1, 10, 9), new Rectangle(10, 10, -10, -10).Crop(new Rectangle(0, 1, 10, 10)));

            Assert.AreEqual(new Rectangle(0, 0, 10, 10), new Rectangle(0, 0, 10, 10).Crop(new Rectangle(15, 10, -15, -10)));
            Assert.AreEqual(new Rectangle(0, 1, 10, 9), new Rectangle(0, 0, 10, 10).Crop(new Rectangle(10, 11, -10, -10)));
        }

        [TestMethod]
        public void TestGetBottomLeft()
        {
            Assert.AreEqual(new Vector2Int32(-1, 12), new Rectangle(-1, 3, 5, 9).GetBottomLeft());
        }

        [TestMethod]
        public void TestGetBottomRight()
        {
            Assert.AreEqual(new Vector2Int32(4, 12), new Rectangle(-1, 3, 5, 9).GetBottomRight());
        }

        [TestMethod]
        public void TestGetTopLeft()
        {
            Assert.AreEqual(new Vector2Int32(-1, 3), new Rectangle(-1, 3, 5, 9).GetTopLeft());
        }

        [TestMethod]
        public void TestGetTopRight()
        {
            Assert.AreEqual(new Vector2Int32(4, 3), new Rectangle(-1, 3, 5, 9).GetTopRight());
        }

        [TestMethod]
        public void TestIsNormalized()
        {
            Assert.IsTrue(new Rectangle(-5, -4, 5, 5).IsNormalized());
            Assert.IsTrue(new Rectangle(-5, -4, 5, 0).IsNormalized());
            Assert.IsTrue(new Rectangle(-5, -4, 0, 5).IsNormalized());
            Assert.IsFalse(new Rectangle(-5, -4, -1, 0).IsNormalized());
            Assert.IsFalse(new Rectangle(-5, -4, 0, -7).IsNormalized());
        }

        [TestMethod]
        public void TestNormalize()
        {
            Assert.IsTrue(new Rectangle(5, 5, 5, 5).Normalize().IsNormalized());
            Assert.IsTrue(new Rectangle(-5, -5, 5, 5).Normalize().IsNormalized());
            Assert.IsTrue(new Rectangle(5, 5, -3, 0).Normalize().IsNormalized());
            Assert.IsTrue(new Rectangle(5, 5, 0, -3).Normalize().IsNormalized());
            Assert.IsTrue(new Rectangle(5, 5, -3, 2).Normalize().IsNormalized());
            Assert.IsTrue(new Rectangle(5, 5, 2, -3).Normalize().IsNormalized());
            Assert.IsTrue(new Rectangle(-5, -5, -2, -3).Normalize().IsNormalized());

            Assert.AreEqual(new Rectangle(-7, -8, 2, 3), new Rectangle(-5, -5, -2, -3).Normalize());
            Assert.AreEqual(new Rectangle(5, 5, 5, 5), new Rectangle(5, 5, 5, 5).Normalize());
            Assert.AreEqual(new Rectangle(2, 5, 3, 2), new Rectangle(5, 5, -3, 2).Normalize());
            Assert.AreEqual(new Rectangle(5, 3, 3, 2), new Rectangle(5, 5, 3, -2).Normalize());
            Assert.AreEqual(new Rectangle(2, 3, 3, 2), new Rectangle(5, 5, -3, -2).Normalize());
        }
    }
}