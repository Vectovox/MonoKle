using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace MonoKle.Core.Test
{
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
        public void TestCrop()
        {
            Rectangle rect = new Rectangle(0, 0, 10, 10);
            rect.Crop(new Rectangle(5, 5, 2, 2));
            Assert.AreEqual(new Rectangle(5, 5, 2, 2), rect);

            rect = new Rectangle(0, 0, 10, 10);
            rect.Crop(new Rectangle(0, 0, 10, 10));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), rect);

            rect = new Rectangle(0, 0, 10, 10);
            rect.Crop(new Rectangle(-5, 0, 15, 10));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), rect);

            rect = new Rectangle(0, 0, 10, 10);
            rect.Crop(new Rectangle(0, -5, 10, 15));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), rect);

            rect = new Rectangle(0, 0, 10, 10);
            rect.Crop(new Rectangle(0, 0, 15, 10));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), rect);

            rect = new Rectangle(0, 0, 10, 10);
            rect.Crop(new Rectangle(0, 0, 10, 15));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), rect);

            rect = new Rectangle(0, 0, 10, 10);
            rect.Crop(new Rectangle(-5, -5, 155, 105));
            Assert.AreEqual(new Rectangle(0, 0, 10, 10), rect);

            rect = new Rectangle(0, 0, 10, 10);
            rect.Crop(new Rectangle(1, 0, 10, 10));
            Assert.AreEqual(new Rectangle(1, 0, 9, 10), rect);

            rect = new Rectangle(0, 0, 10, 10);
            rect.Crop(new Rectangle(0, 1, 10, 10));
            Assert.AreEqual(new Rectangle(0, 1, 10, 9), rect);

            rect = new Rectangle(0, 0, 10, 10);
            rect.Crop(new Rectangle(0, 0, 9, 10));
            Assert.AreEqual(new Rectangle(0, 0, 9, 10), rect);

            rect = new Rectangle(0, 0, 10, 10);
            rect.Crop(new Rectangle(0, 0, 10, 9));
            Assert.AreEqual(new Rectangle(0, 0, 10, 9), rect);

        }

        [TestMethod]
        public void TestGetTopRight()
        {
            Assert.AreEqual(new Vector2Int32(4, 3), new Rectangle(-1, 3, 5, 9).GetTopRight());
        }

        [TestMethod]
        public void TestGetTopLeft()
        {
            Assert.AreEqual(new Vector2Int32(-1, 3), new Rectangle(-1, 3, 5, 9).GetTopLeft());
        }

        [TestMethod]
        public void TestGetBottomRight()
        {
            Assert.AreEqual(new Vector2Int32(4, 12), new Rectangle(-1, 3, 5, 9).GetBottomRight());
        }

        [TestMethod]
        public void TestGetBottomLeft()
        {
            Assert.AreEqual(new Vector2Int32(-1, 12), new Rectangle(-1, 3, 5, 9).GetBottomLeft());
        }
    }
}
