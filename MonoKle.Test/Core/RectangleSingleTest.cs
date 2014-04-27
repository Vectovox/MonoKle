namespace MonoKle.Core
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xna.Framework;

    [TestClass]
    public class RectangleSingleTest
    {
        [TestMethod]
        public void TestConstructors()
        {
            RectangleSingle a = new RectangleSingle(new Rectangle(-5, -6, 10, 11));
            RectangleSingle b = new RectangleSingle(new Vector2(-5, -6), new Vector2(5, 5));
            RectangleSingle c = new RectangleSingle(-5, -6, 10, 11);

            Assert.AreEqual(a, b);
            Assert.AreEqual(b, c);

            Assert.AreEqual(new RectangleSingle(0, 0, 15, 10), new RectangleSingle(15, 10));
            Assert.AreEqual(new RectangleSingle(-15, -10, 15, 10), new RectangleSingle(-15, -10));

            Assert.AreEqual(new RectangleSingle(0, 0, 10, 10), new RectangleSingle(10, 10, -10, -10));
            Assert.AreEqual(new RectangleSingle(0, 0, -10, -10), new RectangleSingle(-10, -10, 10, 10));
            Assert.AreEqual(new RectangleSingle(-10, -10, -20, -20), new RectangleSingle(-30, -30, 20, 20));
        }

        [TestMethod]
        public void TestContainsCoordinateFloat()
        {
            Assert.IsTrue(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2(50, 51)));
            Assert.IsTrue(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2(100, 101)));
            Assert.IsTrue(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2(75, 75)));

            Assert.IsTrue(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2(-25, -25)));
            Assert.IsTrue(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2(-50, -51)));
            Assert.IsTrue(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2(0, -1)));

            Assert.IsTrue(new RectangleSingle(-5, -5, 10, 10).Contains(new Vector2(0, 0)));
            Assert.IsTrue(new RectangleSingle(-5, -5, 10, 10).Contains(new Vector2(0, 1)));
            Assert.IsTrue(new RectangleSingle(-5, -5, 10, 10).Contains(new Vector2(1, 0)));
            Assert.IsTrue(new RectangleSingle(-5, -5, 10, 10).Contains(new Vector2(1, 1)));

            Assert.IsFalse(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2(49, 51)));
            Assert.IsFalse(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2(50, 50)));
            Assert.IsFalse(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2(100, 102)));
            Assert.IsFalse(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2(101, 101)));
            Assert.IsFalse(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2(-75, -75)));

            Assert.IsFalse(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2(-50, -52)));
            Assert.IsFalse(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2(-51, -51)));
            Assert.IsFalse(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2(0, 0)));
            Assert.IsFalse(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2(1, -1)));
        }

        [TestMethod]
        public void TestContainsCoordinateInt()
        {
            Assert.IsTrue(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2Int32(50, 51)));
            Assert.IsTrue(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2Int32(100, 101)));
            Assert.IsTrue(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2Int32(75, 75)));

            Assert.IsTrue(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2Int32(-25, -25)));
            Assert.IsTrue(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2Int32(-50, -51)));
            Assert.IsTrue(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2Int32(0, -1)));

            Assert.IsTrue(new RectangleSingle(-5, -5, 10, 10).Contains(new Vector2Int32(0, 0)));
            Assert.IsTrue(new RectangleSingle(-5, -5, 10, 10).Contains(new Vector2Int32(0, 1)));
            Assert.IsTrue(new RectangleSingle(-5, -5, 10, 10).Contains(new Vector2Int32(1, 0)));
            Assert.IsTrue(new RectangleSingle(-5, -5, 10, 10).Contains(new Vector2Int32(1, 1)));

            Assert.IsFalse(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2Int32(49, 51)));
            Assert.IsFalse(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2Int32(50, 50)));
            Assert.IsFalse(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2Int32(100, 102)));
            Assert.IsFalse(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2Int32(101, 101)));
            Assert.IsFalse(new RectangleSingle(50, 51, 50, 50).Contains(new Vector2Int32(-75, -75)));

            Assert.IsFalse(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2Int32(-50, -52)));
            Assert.IsFalse(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2Int32(-51, -51)));
            Assert.IsFalse(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2Int32(0, 0)));
            Assert.IsFalse(new RectangleSingle(-50, -51, 50, 50).Contains(new Vector2Int32(1, -1)));
        }

        [TestMethod]
        public void TestContainsRectangle()
        {
            Assert.IsTrue(new RectangleSingle(50, 51, 50, 50).Contains(new Rectangle(50, 51, 50, 50)));
            Assert.IsTrue(new RectangleSingle(50, 51, 50, 50).Contains(new Rectangle(60, 60, 20, 20)));
            Assert.IsTrue(new RectangleSingle(-10, -10, 20, 20).Contains(new Rectangle(-5, -5, 10, 10)));

            Assert.IsFalse(new RectangleSingle(-10, -10, 20, 20).Contains(new Rectangle(-5, -5, 30, 10)));
            Assert.IsFalse(new RectangleSingle(-10, -10, 20, 20).Contains(new Rectangle(-5, -5, 10, 30)));
            Assert.IsFalse(new RectangleSingle(-10, -10, 20, 20).Contains(new Rectangle(-11, -5, 10, 10)));
            Assert.IsFalse(new RectangleSingle(-10, -10, 20, 20).Contains(new Rectangle(-5, -11, 10, 10)));
            Assert.IsFalse(new RectangleSingle(-10, -10, 20, 20).Contains(new Rectangle(50, 50, 10, 10)));
        }

        [TestMethod]
        public void TestContainsRectangleSingle()
        {
            Assert.IsTrue(new RectangleSingle(50, 51, 50, 50).Contains(new RectangleSingle(50, 51, 50, 50)));
            Assert.IsTrue(new RectangleSingle(50, 51, 50, 50).Contains(new RectangleSingle(60, 60, 20, 20)));
            Assert.IsTrue(new RectangleSingle(-10, -10, 20, 20).Contains(new RectangleSingle(-5, -5, 10, 10)));

            Assert.IsFalse(new RectangleSingle(-10, -10, 20, 20).Contains(new RectangleSingle(-5, -5, 30, 10)));
            Assert.IsFalse(new RectangleSingle(-10, -10, 20, 20).Contains(new RectangleSingle(-5, -5, 10, 30)));
            Assert.IsFalse(new RectangleSingle(-10, -10, 20, 20).Contains(new RectangleSingle(-11, -5, 10, 10)));
            Assert.IsFalse(new RectangleSingle(-10, -10, 20, 20).Contains(new RectangleSingle(-5, -11, 10, 10)));
            Assert.IsFalse(new RectangleSingle(-10, -10, 20, 20).Contains(new RectangleSingle(50, 50, 10, 10)));
        }

        [TestMethod]
        public void TestCrop()
        {
            Assert.AreEqual(new RectangleSingle(5, 5, 2, 2), new RectangleSingle(0, 0, 10, 10).Crop(new RectangleSingle(5, 5, 2, 2)));
            Assert.AreEqual(new RectangleSingle(0, 0, 10, 10), new RectangleSingle(0, 0, 10, 10).Crop(new RectangleSingle(0, 0, 10, 10)));
            Assert.AreEqual(new RectangleSingle(0, 0, 10, 10), new RectangleSingle(0, 0, 10, 10).Crop(new RectangleSingle(-5, 0, 15, 10)));
            Assert.AreEqual(new RectangleSingle(0, 0, 10, 10), new RectangleSingle(0, 0, 10, 10).Crop(new RectangleSingle(0, -5, 10, 15)));
            Assert.AreEqual(new RectangleSingle(0, 0, 10, 10), new RectangleSingle(0, 0, 10, 10).Crop(new RectangleSingle(0, 0, 15, 10)));
            Assert.AreEqual(new RectangleSingle(0, 0, 10, 10), new RectangleSingle(0, 0, 10, 10).Crop(new RectangleSingle(0, 0, 10, 15)));
            Assert.AreEqual(new RectangleSingle(0, 0, 10, 10), new RectangleSingle(0, 0, 10, 10).Crop(new RectangleSingle(-5, -5, 155, 105)));
            Assert.AreEqual(new RectangleSingle(1, 0, 9, 10), new RectangleSingle(0, 0, 10, 10).Crop(new RectangleSingle(1, 0, 10, 10)));
            Assert.AreEqual(new RectangleSingle(0, 1, 10, 9), new RectangleSingle(0, 0, 10, 10).Crop(new RectangleSingle(0, 1, 10, 10)));
            Assert.AreEqual(new RectangleSingle(0, 0, 9, 10), new RectangleSingle(0, 0, 10, 10).Crop(new RectangleSingle(0, 0, 9, 10)));
            Assert.AreEqual(new RectangleSingle(0, 0, 10, 9), new RectangleSingle(0, 0, 10, 10).Crop(new RectangleSingle(0, 0, 10, 9)));
        }

        [TestMethod]
        public void TestEqualsObject()
        {
            Assert.IsTrue(new RectangleSingle(0, 1, 2, 3).Equals((object)new RectangleSingle(0, 1, 2, 3)));
            Assert.IsFalse(new RectangleSingle(0, 1, -2, 3).Equals((object)new RectangleSingle(0, 1, 2, 3)));
            Assert.IsFalse(new RectangleSingle(0, 1, 2, 3).Equals((object)new RectangleSingle(1, 1, 2, 3)));
        }

        [TestMethod]
        public void TestEqualsRectangleSingle()
        {
            Assert.IsTrue(new RectangleSingle(0, 1, 2, 3).Equals(new RectangleSingle(0, 1, 2, 3)));
            Assert.IsFalse(new RectangleSingle(0, 1, -2, 3).Equals(new RectangleSingle(0, 1, 2, 3)));
            Assert.IsFalse(new RectangleSingle(0, 1, 2, 3).Equals(new RectangleSingle(1, 1, 2, 3)));
        }

        [TestMethod]
        public void TestGetBottom()
        {
            Assert.AreEqual(6f, new RectangleSingle(1, 2, 3, 4).GetBottom());
            Assert.AreEqual(2f, new RectangleSingle(-1, -2, 3, 4).GetBottom());
            Assert.AreEqual(-2f, new RectangleSingle(-1, -2, -3, -4).GetBottom());
        }

        [TestMethod]
        public void TestGetBottomLeft()
        {
            Assert.AreEqual(new Vector2(1, 6), new RectangleSingle(1, 2, 3, 4).GetBottomLeft());
            Assert.AreEqual(new Vector2(-1, 2), new RectangleSingle(-1, -2, 3, 4).GetBottomLeft());
            Assert.AreEqual(new Vector2(-4, -2), new RectangleSingle(-1, -2, -3, -4).GetBottomLeft());
        }

        [TestMethod]
        public void TestGetBottomRight()
        {
            Assert.AreEqual(new Vector2(4, 6), new RectangleSingle(1, 2, 3, 4).GetBottomRight());
            Assert.AreEqual(new Vector2(2, 2), new RectangleSingle(-1, -2, 3, 4).GetBottomRight());
            Assert.AreEqual(new Vector2(-1, -2), new RectangleSingle(-1, -2, -3, -4).GetBottomRight());
        }

        [TestMethod]
        public void TestGetLeft()
        {
            Assert.AreEqual(1f, new RectangleSingle(1, 2, 3, 4).GetLeft());
            Assert.AreEqual(-1f, new RectangleSingle(-1, -2, 3, 4).GetLeft());
            Assert.AreEqual(-4f, new RectangleSingle(-1, -2, -3, -4).GetLeft());
        }

        [TestMethod]
        public void TestGetRight()
        {
            Assert.AreEqual(4f, new RectangleSingle(1, 2, 3, 4).GetRight());
            Assert.AreEqual(2f, new RectangleSingle(-1, -2, 3, 4).GetRight());
            Assert.AreEqual(-1f, new RectangleSingle(-1, -2, -3, -4).GetRight());
        }

        [TestMethod]
        public void TestGetTop()
        {
            Assert.AreEqual(2f, new RectangleSingle(1, 2, 3, 4).GetTop());
            Assert.AreEqual(-2f, new RectangleSingle(-1, -2, 3, 4).GetTop());
            Assert.AreEqual(-6f, new RectangleSingle(-1, -2, -3, -4).GetTop());
        }

        [TestMethod]
        public void TestGetTopLeft()
        {
            Assert.AreEqual(new Vector2(1, 2), new RectangleSingle(1, 2, 3, 4).GetTopLeft());
            Assert.AreEqual(new Vector2(-1, -2), new RectangleSingle(-1, -2, 3, 4).GetTopLeft());
            Assert.AreEqual(new Vector2(-4, -6), new RectangleSingle(-1, -2, -3, -4).GetTopLeft());
        }

        [TestMethod]
        public void TestGetTopRight()
        {
            Assert.AreEqual(new Vector2(4, 2), new RectangleSingle(1, 2, 3, 4).GetTopRight());
            Assert.AreEqual(new Vector2(2, -2), new RectangleSingle(-1, -2, 3, 4).GetTopRight());
            Assert.AreEqual(new Vector2(-1, -6), new RectangleSingle(-1, -2, -3, -4).GetTopRight());
        }

        [TestMethod]
        public void TestOperatorEquals()
        {
            Assert.IsTrue(new RectangleSingle(0, 1, 2, 3) == new RectangleSingle(0, 1, 2, 3));
            Assert.IsFalse(new RectangleSingle(0, 1, -2, 3) == new RectangleSingle(0, 1, 2, 3));
            Assert.IsFalse(new RectangleSingle(0, 1, 2, 3) == new RectangleSingle(1, 1, 2, 3));
        }

        [TestMethod]
        public void TestOperatorNotEquals()
        {
            Assert.IsFalse(new RectangleSingle(0, 1, 2, 3) != new RectangleSingle(0, 1, 2, 3));
            Assert.IsTrue(new RectangleSingle(0, 1, -2, 3) != new RectangleSingle(0, 1, 2, 3));
            Assert.IsTrue(new RectangleSingle(0, 1, 2, 3) != new RectangleSingle(1, 1, 2, 3));
        }

        [TestMethod]
        public void TestToRectangle()
        {
            Assert.AreEqual(new Rectangle(0, 1, 2, 3), new RectangleSingle(0, 1, 2, 3).ToRectangle());
            Assert.AreEqual(new Rectangle(0, 1, 2, 3), new RectangleSingle(2, 4, -2, -3).ToRectangle());
        }
    }
}