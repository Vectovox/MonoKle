namespace MonoKle.Core
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xna.Framework;
    using System;

    [TestClass]
    public class IntAreaTest
    {
        private const int RANDOM_TEST_AMOUNT = 100;

        private readonly Random random = new Random();

        [TestMethod]
        public void TestTranslate()
        {
            for(int i = 0; i < IntAreaTest.RANDOM_TEST_AMOUNT; i++)
            {
                int x = random.Next(-100, 100);
                int y = random.Next(-100, 100);
                int w = random.Next(0, 100);
                int h = random.Next(0, 100);

                int tx = random.Next(-100, 100);
                int ty = random.Next(-100, 100);

                IntArea a = new IntArea(x, y, w, h);
                Assert.AreEqual(new IntArea(x + tx, y + ty, w, h), a.Translate(new IntVector2(tx, ty)));

                // Assert X Y methods
                Assert.AreEqual(a.Translate(new IntVector2(tx, 0)), a.TranslateX(tx));
                Assert.AreEqual(a.Translate(new IntVector2(0, ty)), a.TranslateY(ty));
            }
        }

        [TestMethod]
        public void TestWidthHeight()
        {
            for(int i = 0; i < IntAreaTest.RANDOM_TEST_AMOUNT; i++)
            {
                int x = random.Next(-100, 100);
                int y = random.Next(-100, 100);
                int x2 = random.Next(-100, 100);
                int y2 = random.Next(-100, 100);

                IntArea a = new IntArea(x, y, x2 - x, y2 - y);
                Assert.AreEqual(Math.Abs(y2 - y), a.Height);
                Assert.AreEqual(Math.Abs(x2 - x), a.Width);
            }
        }

        [TestMethod]
        public void TestConstructors()
        {
            // Test that all constructors work the same
            IntArea a = new IntArea(-5, -5);
            IntArea b = new IntArea(new IntVector2(0, 0), new IntVector2(-5, -5));
            IntArea c = new IntArea(0, 0, -5, -5);
            IntArea d = new IntArea(new IntVector2(-5, -5));

            Assert.AreEqual(a, b);
            Assert.AreEqual(b, c);
            Assert.AreEqual(c, d);

            // Test negative size
            Assert.AreEqual(new IntArea(0, 0, 10, 10), new IntArea(10, 10, -10, -10));
            Assert.AreEqual(new IntArea(-10, -10, 10, 10), new IntArea(0, 0, -10, -10));
            Assert.AreEqual(new IntArea(-30, -30, 20, 20), new IntArea(-10, -10, -20, -20));
        }

        [TestMethod]
        public void TestContainsCoordinateFloat()
        {
            Assert.IsTrue(new IntArea(50, 51, 50, 50).Contains(new Vector2(50, 51)));
            Assert.IsTrue(new IntArea(50, 51, 50, 50).Contains(new Vector2(100, 101)));
            Assert.IsTrue(new IntArea(50, 51, 50, 50).Contains(new Vector2(75, 75)));

            Assert.IsTrue(new IntArea(-50, -51, 50, 50).Contains(new Vector2(-25, -25)));
            Assert.IsTrue(new IntArea(-50, -51, 50, 50).Contains(new Vector2(-50, -51)));
            Assert.IsTrue(new IntArea(-50, -51, 50, 50).Contains(new Vector2(0, -1)));

            Assert.IsTrue(new IntArea(-5, -5, 10, 10).Contains(new Vector2(0, 0)));
            Assert.IsTrue(new IntArea(-5, -5, 10, 10).Contains(new Vector2(0, 1)));
            Assert.IsTrue(new IntArea(-5, -5, 10, 10).Contains(new Vector2(1, 0)));
            Assert.IsTrue(new IntArea(-5, -5, 10, 10).Contains(new Vector2(1, 1)));

            Assert.IsFalse(new IntArea(50, 51, 50, 50).Contains(new Vector2(49, 51)));
            Assert.IsFalse(new IntArea(50, 51, 50, 50).Contains(new Vector2(50, 50)));
            Assert.IsFalse(new IntArea(50, 51, 50, 50).Contains(new Vector2(100, 102)));
            Assert.IsFalse(new IntArea(50, 51, 50, 50).Contains(new Vector2(101, 101)));
            Assert.IsFalse(new IntArea(50, 51, 50, 50).Contains(new Vector2(-75, -75)));

            Assert.IsFalse(new IntArea(-50, -51, 50, 50).Contains(new Vector2(-50, -52)));
            Assert.IsFalse(new IntArea(-50, -51, 50, 50).Contains(new Vector2(-51, -51)));
            Assert.IsFalse(new IntArea(-50, -51, 50, 50).Contains(new Vector2(0, 0)));
            Assert.IsFalse(new IntArea(-50, -51, 50, 50).Contains(new Vector2(1, -1)));
        }

        [TestMethod]
        public void TestContainsCoordinateInt()
        {
            Assert.IsTrue(new IntArea(50, 51, 50, 50).Contains(new IntVector2(50, 51)));
            Assert.IsTrue(new IntArea(50, 51, 50, 50).Contains(new IntVector2(100, 101)));
            Assert.IsTrue(new IntArea(50, 51, 50, 50).Contains(new IntVector2(75, 75)));

            Assert.IsTrue(new IntArea(-50, -51, 50, 50).Contains(new IntVector2(-25, -25)));
            Assert.IsTrue(new IntArea(-50, -51, 50, 50).Contains(new IntVector2(-50, -51)));
            Assert.IsTrue(new IntArea(-50, -51, 50, 50).Contains(new IntVector2(0, -1)));

            Assert.IsTrue(new IntArea(-5, -5, 10, 10).Contains(new IntVector2(0, 0)));
            Assert.IsTrue(new IntArea(-5, -5, 10, 10).Contains(new IntVector2(0, 1)));
            Assert.IsTrue(new IntArea(-5, -5, 10, 10).Contains(new IntVector2(1, 0)));
            Assert.IsTrue(new IntArea(-5, -5, 10, 10).Contains(new IntVector2(1, 1)));

            Assert.IsFalse(new IntArea(50, 51, 50, 50).Contains(new IntVector2(49, 51)));
            Assert.IsFalse(new IntArea(50, 51, 50, 50).Contains(new IntVector2(50, 50)));
            Assert.IsFalse(new IntArea(50, 51, 50, 50).Contains(new IntVector2(100, 102)));
            Assert.IsFalse(new IntArea(50, 51, 50, 50).Contains(new IntVector2(101, 101)));
            Assert.IsFalse(new IntArea(50, 51, 50, 50).Contains(new IntVector2(-75, -75)));

            Assert.IsFalse(new IntArea(-50, -51, 50, 50).Contains(new IntVector2(-50, -52)));
            Assert.IsFalse(new IntArea(-50, -51, 50, 50).Contains(new IntVector2(-51, -51)));
            Assert.IsFalse(new IntArea(-50, -51, 50, 50).Contains(new IntVector2(0, 0)));
            Assert.IsFalse(new IntArea(-50, -51, 50, 50).Contains(new IntVector2(1, -1)));
        }

        [TestMethod]
        public void TestContainsArea()
        {
            Assert.IsTrue(new IntArea(50, 51, 50, 50).Contains(new Area(50, 51, 50, 50)));
            Assert.IsTrue(new IntArea(50, 51, 50, 50).Contains(new Area(60, 60, 20, 20)));
            Assert.IsTrue(new IntArea(-10, -10, 20, 20).Contains(new Area(-5, -5, 10, 10)));

            Assert.IsFalse(new IntArea(-10, -10, 20, 20).Contains(new Area(-5, -5, 30, 10)));
            Assert.IsFalse(new IntArea(-10, -10, 20, 20).Contains(new Area(-5, -5, 10, 30)));
            Assert.IsFalse(new IntArea(-10, -10, 20, 20).Contains(new Area(-11, -5, 10, 10)));
            Assert.IsFalse(new IntArea(-10, -10, 20, 20).Contains(new Area(-5, -11, 10, 10)));
            Assert.IsFalse(new IntArea(-10, -10, 20, 20).Contains(new Area(50, 50, 10, 10)));
        }

        [TestMethod]
        public void TestContainsAreaInt()
        {
            Assert.IsTrue(new IntArea(50, 51, 50, 50).Contains(new IntArea(50, 51, 50, 50)));
            Assert.IsTrue(new IntArea(50, 51, 50, 50).Contains(new IntArea(60, 60, 20, 20)));
            Assert.IsTrue(new IntArea(-10, -10, 20, 20).Contains(new IntArea(-5, -5, 10, 10)));

            Assert.IsFalse(new IntArea(-10, -10, 20, 20).Contains(new IntArea(-5, -5, 30, 10)));
            Assert.IsFalse(new IntArea(-10, -10, 20, 20).Contains(new IntArea(-5, -5, 10, 30)));
            Assert.IsFalse(new IntArea(-10, -10, 20, 20).Contains(new IntArea(-11, -5, 10, 10)));
            Assert.IsFalse(new IntArea(-10, -10, 20, 20).Contains(new IntArea(-5, -11, 10, 10)));
            Assert.IsFalse(new IntArea(-10, -10, 20, 20).Contains(new IntArea(50, 50, 10, 10)));
        }

        [TestMethod]
        public void TestClampArea()
        {
            Assert.AreEqual(new Area(-1, -1, 2, 2), new IntArea(-5, -5, 10, 10).Clamp(new Area(-1, -1, 2, 2)));
            Assert.AreEqual(new Area(-5, -5, 10, 10), new IntArea(-5, -5, 10, 10).Clamp(new Area(-5, -5, 10, 10)));
            Assert.AreEqual(new Area(-5, -5, 10, 10), new IntArea(-5, -5, 10, 10).Clamp(new Area(-10, -10, 20, 20)));

            Assert.AreEqual(new Area(-1, -1, 6, 2), new IntArea(-5, -5, 10, 10).Clamp(new Area(-1, -1, 10, 2)));
            Assert.AreEqual(new Area(-1, -1, 2, 6), new IntArea(-5, -5, 10, 10).Clamp(new Area(-1, -1, 2, 10)));
            Assert.AreEqual(new Area(-5, -1, 3, 5), new IntArea(-5, -5, 10, 10).Clamp(new Area(-7, -1, 5, 5)));
            Assert.AreEqual(new Area(-1, -5, 5, 3), new IntArea(-5, -5, 10, 10).Clamp(new Area(-1, -7, 5, 5)));
        }

        [TestMethod]
        public void TestClampAreaInteger()
        {
            Assert.AreEqual(new IntArea(-1, -1, 2, 2), new IntArea(-5, -5, 10, 10).Clamp(new IntArea(-1, -1, 2, 2)));
            Assert.AreEqual(new IntArea(-5, -5, 10, 10), new IntArea(-5, -5, 10, 10).Clamp(new IntArea(-5, -5, 10, 10)));
            Assert.AreEqual(new IntArea(-5, -5, 10, 10), new IntArea(-5, -5, 10, 10).Clamp(new IntArea(-10, -10, 20, 20)));

            Assert.AreEqual(new IntArea(-1, -1, 6, 2), new IntArea(-5, -5, 10, 10).Clamp(new IntArea(-1, -1, 10, 2)));
            Assert.AreEqual(new IntArea(-1, -1, 2, 6), new IntArea(-5, -5, 10, 10).Clamp(new IntArea(-1, -1, 2, 10)));
            Assert.AreEqual(new IntArea(-5, -1, 3, 5), new IntArea(-5, -5, 10, 10).Clamp(new IntArea(-7, -1, 5, 5)));
            Assert.AreEqual(new IntArea(-1, -5, 5, 3), new IntArea(-5, -5, 10, 10).Clamp(new IntArea(-1, -7, 5, 5)));
        }

        [TestMethod]
        public void TestClampCoordinate()
        {
            Assert.AreEqual(new Vector2(0, 0), new IntArea(-5, -5, 10, 10).Clamp(new Vector2(0, 0)));
            Assert.AreEqual(new Vector2(-5, -5), new IntArea(-5, -5, 10, 10).Clamp(new Vector2(-5, -5)));
            Assert.AreEqual(new Vector2(5, 5), new IntArea(-5, -5, 10, 10).Clamp(new Vector2(5, 5)));

            Assert.AreEqual(new Vector2(0, 5), new IntArea(-5, -5, 10, 10).Clamp(new Vector2(0, 7)));
            Assert.AreEqual(new Vector2(5, 0), new IntArea(-5, -5, 10, 10).Clamp(new Vector2(7, 0)));
            Assert.AreEqual(new Vector2(0, -5), new IntArea(-5, -5, 10, 10).Clamp(new Vector2(0, -7)));
            Assert.AreEqual(new Vector2(-5, 0), new IntArea(-5, -5, 10, 10).Clamp(new Vector2(-7, 0)));
        }

        [TestMethod]
        public void TestClampCoordinateInteger()
        {
            Assert.AreEqual(new IntVector2(0, 0), new IntArea(-5, -5, 10, 10).Clamp(new IntVector2(0, 0)));
            Assert.AreEqual(new IntVector2(-5, -5), new IntArea(-5, -5, 10, 10).Clamp(new IntVector2(-5, -5)));
            Assert.AreEqual(new IntVector2(5, 5), new IntArea(-5, -5, 10, 10).Clamp(new IntVector2(5, 5)));

            Assert.AreEqual(new IntVector2(0, 5), new IntArea(-5, -5, 10, 10).Clamp(new IntVector2(0, 7)));
            Assert.AreEqual(new IntVector2(5, 0), new IntArea(-5, -5, 10, 10).Clamp(new IntVector2(7, 0)));
            Assert.AreEqual(new IntVector2(0, -5), new IntArea(-5, -5, 10, 10).Clamp(new IntVector2(0, -7)));
            Assert.AreEqual(new IntVector2(-5, 0), new IntArea(-5, -5, 10, 10).Clamp(new IntVector2(-7, 0)));
        }

        [TestMethod]
        public void TestEqualsObject()
        {
            Assert.IsTrue(new IntArea(0, 1, 2, 3).Equals((object)new IntArea(0, 1, 2, 3)));
            Assert.IsFalse(new IntArea(0, 1, -2, 3).Equals((object)new IntArea(0, 1, 2, 3)));
            Assert.IsFalse(new IntArea(0, 1, 2, 3).Equals((object)new IntArea(1, 1, 2, 3)));
        }

        [TestMethod]
        public void TestEqualsAreaInteger()
        {
            Assert.IsTrue(new IntArea(0, 1, 2, 3).Equals(new IntArea(0, 1, 2, 3)));
            Assert.IsFalse(new IntArea(0, 1, -2, 3).Equals(new IntArea(0, 1, 2, 3)));
            Assert.IsFalse(new IntArea(0, 1, 2, 3).Equals(new IntArea(1, 1, 2, 3)));
        }

        [TestMethod]
        public void TestGetBottom()
        {
            Assert.AreEqual(6f, new IntArea(1, 2, 3, 4).Bottom);
            Assert.AreEqual(2f, new IntArea(-1, -2, 3, 4).Bottom);
            Assert.AreEqual(-2f, new IntArea(-1, -2, -3, -4).Bottom);
        }

        [TestMethod]
        public void TestGetBottomLeft()
        {
            Assert.AreEqual(new IntVector2(1, 6), new IntArea(1, 2, 3, 4).BottomLeft);
            Assert.AreEqual(new IntVector2(-1, 2), new IntArea(-1, -2, 3, 4).BottomLeft);
            Assert.AreEqual(new IntVector2(-4, -2), new IntArea(-1, -2, -3, -4).BottomLeft);
        }

        [TestMethod]
        public void TestGetBottomRight()
        {
            Assert.AreEqual(new IntVector2(4, 6), new IntArea(1, 2, 3, 4).BottomRight);
            Assert.AreEqual(new IntVector2(2, 2), new IntArea(-1, -2, 3, 4).BottomRight);
            Assert.AreEqual(new IntVector2(-1, -2), new IntArea(-1, -2, -3, -4).BottomRight);
        }

        [TestMethod]
        public void TestGetLeft()
        {
            Assert.AreEqual(1f, new IntArea(1, 2, 3, 4).Left);
            Assert.AreEqual(-1f, new IntArea(-1, -2, 3, 4).Left);
            Assert.AreEqual(-4f, new IntArea(-1, -2, -3, -4).Left);
        }

        [TestMethod]
        public void TestGetRight()
        {
            Assert.AreEqual(4f, new IntArea(1, 2, 3, 4).Right);
            Assert.AreEqual(2f, new IntArea(-1, -2, 3, 4).Right);
            Assert.AreEqual(-1f, new IntArea(-1, -2, -3, -4).Right);
        }

        [TestMethod]
        public void TestGetTop()
        {
            Assert.AreEqual(2f, new IntArea(1, 2, 3, 4).Top);
            Assert.AreEqual(-2f, new IntArea(-1, -2, 3, 4).Top);
            Assert.AreEqual(-6f, new IntArea(-1, -2, -3, -4).Top);
        }

        [TestMethod]
        public void TestGetTopLeft()
        {
            Assert.AreEqual(new IntVector2(1, 2), new IntArea(1, 2, 3, 4).TopLeft);
            Assert.AreEqual(new IntVector2(-1, -2), new IntArea(-1, -2, 3, 4).TopLeft);
            Assert.AreEqual(new IntVector2(-4, -6), new IntArea(-1, -2, -3, -4).TopLeft);
        }

        [TestMethod]
        public void TestGetTopRight()
        {
            Assert.AreEqual(new IntVector2(4, 2), new IntArea(1, 2, 3, 4).TopRight);
            Assert.AreEqual(new IntVector2(2, -2), new IntArea(-1, -2, 3, 4).TopRight);
            Assert.AreEqual(new IntVector2(-1, -6), new IntArea(-1, -2, -3, -4).TopRight);
        }

        [TestMethod]
        public void TestOperatorEquals()
        {
            Assert.IsTrue(new IntArea(0, 1, 2, 3) == new IntArea(0, 1, 2, 3));
            Assert.IsFalse(new IntArea(0, 1, -2, 3) == new IntArea(0, 1, 2, 3));
            Assert.IsFalse(new IntArea(0, 1, 2, 3) == new IntArea(1, 1, 2, 3));
        }

        [TestMethod]
        public void TestOperatorNotEquals()
        {
            Assert.IsFalse(new IntArea(0, 1, 2, 3) != new IntArea(0, 1, 2, 3));
            Assert.IsTrue(new IntArea(0, 1, -2, 3) != new IntArea(0, 1, 2, 3));
            Assert.IsTrue(new IntArea(0, 1, 2, 3) != new IntArea(1, 1, 2, 3));
        }
    }
}