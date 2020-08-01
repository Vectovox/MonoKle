using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;

namespace MonoKle.Tests
{
    [TestClass]
    public class MRectangleTests
    {
        private const int RANDOM_TEST_AMOUNT = 100;

        private readonly Random random = new Random();

        [TestMethod]
        public void TestClampArea()
        {
            Assert.AreEqual(new MRectangle(-1, -1, 2, 2), new MRectangle(-5, -5, 10, 10).Clamp(new MRectangle(-1, -1, 2, 2)));
            Assert.AreEqual(new MRectangle(-5, -5, 10, 10), new MRectangle(-5, -5, 10, 10).Clamp(new MRectangle(-5, -5, 10, 10)));
            Assert.AreEqual(new MRectangle(-5, -5, 10, 10), new MRectangle(-5, -5, 10, 10).Clamp(new MRectangle(-10, -10, 20, 20)));

            Assert.AreEqual(new MRectangle(-1, -1, 6, 2), new MRectangle(-5, -5, 10, 10).Clamp(new MRectangle(-1, -1, 10, 2)));
            Assert.AreEqual(new MRectangle(-1, -1, 2, 6), new MRectangle(-5, -5, 10, 10).Clamp(new MRectangle(-1, -1, 2, 10)));
            Assert.AreEqual(new MRectangle(-5, -1, 3, 5), new MRectangle(-5, -5, 10, 10).Clamp(new MRectangle(-7, -1, 5, 5)));
            Assert.AreEqual(new MRectangle(-1, -5, 5, 3), new MRectangle(-5, -5, 10, 10).Clamp(new MRectangle(-1, -7, 5, 5)));
        }

        [TestMethod]
        public void TestClampCoordinate()
        {
            Assert.AreEqual(new MVector2(0, 0), new MRectangle(-5, -5, 10, 10).Clamp(new MVector2(0, 0)));
            Assert.AreEqual(new MVector2(-5, -5), new MRectangle(-5, -5, 10, 10).Clamp(new MVector2(-5, -5)));
            Assert.AreEqual(new MVector2(5, 5), new MRectangle(-5, -5, 10, 10).Clamp(new MVector2(5, 5)));

            Assert.AreEqual(new MVector2(0, 5), new MRectangle(-5, -5, 10, 10).Clamp(new MVector2(0, 7)));
            Assert.AreEqual(new MVector2(5, 0), new MRectangle(-5, -5, 10, 10).Clamp(new MVector2(7, 0)));
            Assert.AreEqual(new MVector2(0, -5), new MRectangle(-5, -5, 10, 10).Clamp(new MVector2(0, -7)));
            Assert.AreEqual(new MVector2(-5, 0), new MRectangle(-5, -5, 10, 10).Clamp(new MVector2(-7, 0)));
        }

        [TestMethod]
        public void TestConstructors()
        {
            // Test that all constructors work the same
            var a = new MRectangle(new MRectangleInt(-5, -5, 5, 5));
            var b = new MRectangle(-5, -5);
            var c = new MRectangle(new Vector2(0, 0), new Vector2(-5, -5));
            var d = new MRectangle(new MPoint2(0, 0), new MPoint2(-5, -5));
            var e = new MRectangle(0, 0, -5f, -5f);
            var f = new MRectangle(new Vector2(-5, -5));
            var g = new MRectangle(new MCircle(new MVector2(-2.5f, -2.5f), 2.5f));

            Assert.AreEqual(a, b);
            Assert.AreEqual(b, c);
            Assert.AreEqual(c, d);
            Assert.AreEqual(d, e);
            Assert.AreEqual(e, f);
            Assert.AreEqual(f, g);

            // Test negative size
            Assert.AreEqual(new MRectangle(0, 0, 10, 10), new MRectangle(10, 10, -10, -10));
            Assert.AreEqual(new MRectangle(-10, -10, 10, 10), new MRectangle(0, 0, -10, -10));
            Assert.AreEqual(new MRectangle(-30, -30, 20, 20), new MRectangle(-10, -10, -20, -20));
        }

        [TestMethod]
        public void TestContainsArea()
        {
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Contains(new MRectangle(50, 51, 50, 50)));
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Contains(new MRectangle(60, 60, 20, 20)));
            Assert.IsTrue(new MRectangle(-10, -10, 20, 20).Contains(new MRectangle(-5, -5, 10, 10)));

            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Contains(new MRectangle(-5, -5, 30, 10)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Contains(new MRectangle(-5, -5, 10, 30)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Contains(new MRectangle(-11, -5, 10, 10)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Contains(new MRectangle(-5, -11, 10, 10)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Contains(new MRectangle(50, 50, 10, 10)));
        }

        [TestMethod]
        public void TestContainsAreaInt()
        {
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Contains(new MRectangleInt(50, 51, 50, 50)));
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Contains(new MRectangleInt(60, 60, 20, 20)));
            Assert.IsTrue(new MRectangle(-10, -10, 20, 20).Contains(new MRectangleInt(-5, -5, 10, 10)));

            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Contains(new MRectangleInt(-5, -5, 30, 10)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Contains(new MRectangleInt(-5, -5, 10, 30)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Contains(new MRectangleInt(-11, -5, 10, 10)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Contains(new MRectangleInt(-5, -11, 10, 10)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Contains(new MRectangleInt(50, 50, 10, 10)));
        }

        [TestMethod]
        public void TestContainsCoordinateFloat()
        {
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Contains(new Vector2(50, 51)));
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Contains(new Vector2(100, 101)));
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Contains(new Vector2(75, 75)));

            Assert.IsTrue(new MRectangle(-50, -51, 50, 50).Contains(new Vector2(-25, -25)));
            Assert.IsTrue(new MRectangle(-50, -51, 50, 50).Contains(new Vector2(-50, -51)));
            Assert.IsTrue(new MRectangle(-50, -51, 50, 50).Contains(new Vector2(0, -1)));

            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Contains(new Vector2(0, 0)));
            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Contains(new Vector2(0, 1)));
            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Contains(new Vector2(1, 0)));
            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Contains(new Vector2(1, 1)));

            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Contains(new Vector2(49, 51)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Contains(new Vector2(50, 50)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Contains(new Vector2(100, 102)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Contains(new Vector2(101, 101)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Contains(new Vector2(-75, -75)));

            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Contains(new Vector2(-50, -52)));
            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Contains(new Vector2(-51, -51)));
            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Contains(new Vector2(0, 0)));
            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Contains(new Vector2(1, -1)));
        }

        [TestMethod]
        public void TestContainsCoordinateInt()
        {
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Contains(new MPoint2(50, 51)));
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Contains(new MPoint2(100, 101)));
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Contains(new MPoint2(75, 75)));

            Assert.IsTrue(new MRectangle(-50, -51, 50, 50).Contains(new MPoint2(-25, -25)));
            Assert.IsTrue(new MRectangle(-50, -51, 50, 50).Contains(new MPoint2(-50, -51)));
            Assert.IsTrue(new MRectangle(-50, -51, 50, 50).Contains(new MPoint2(0, -1)));

            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Contains(new MPoint2(0, 0)));
            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Contains(new MPoint2(0, 1)));
            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Contains(new MPoint2(1, 0)));
            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Contains(new MPoint2(1, 1)));

            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Contains(new MPoint2(49, 51)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Contains(new MPoint2(50, 50)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Contains(new MPoint2(100, 102)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Contains(new MPoint2(101, 101)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Contains(new MPoint2(-75, -75)));

            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Contains(new MPoint2(-50, -52)));
            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Contains(new MPoint2(-51, -51)));
            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Contains(new MPoint2(0, 0)));
            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Contains(new MPoint2(1, -1)));
        }

        [TestMethod]
        public void TestEnvelopsArea()
        {
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Envelops(new MRectangle(60, 60, 20, 20)));
            Assert.IsTrue(new MRectangle(-10, -10, 20, 20).Envelops(new MRectangle(-5, -5, 10, 10)));

            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MRectangle(50, 60, 20, 20)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MRectangle(60, 60, 40, 20)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MRectangle(60, 51, 20, 20)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MRectangle(60, 60, 20, 41)));

            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Envelops(new MRectangle(-5, -5, 30, 10)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Envelops(new MRectangle(-5, -5, 10, 30)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Envelops(new MRectangle(-11, -5, 10, 10)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Envelops(new MRectangle(-5, -11, 10, 10)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Envelops(new MRectangle(50, 50, 10, 10)));
        }

        [TestMethod]
        public void TestEnvelopsAreaInt()
        {
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Envelops(new MRectangleInt(60, 60, 20, 20)));
            Assert.IsTrue(new MRectangle(-10, -10, 20, 20).Envelops(new MRectangleInt(-5, -5, 10, 10)));

            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MRectangle(50, 60, 20, 20)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MRectangle(60, 60, 40, 20)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MRectangle(60, 51, 20, 20)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MRectangle(60, 60, 20, 41)));

            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Envelops(new MRectangleInt(-5, -5, 30, 10)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Envelops(new MRectangleInt(-5, -5, 10, 30)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Envelops(new MRectangleInt(-11, -5, 10, 10)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Envelops(new MRectangleInt(-5, -11, 10, 10)));
            Assert.IsFalse(new MRectangle(-10, -10, 20, 20).Envelops(new MRectangleInt(50, 50, 10, 10)));
        }

        [TestMethod]
        public void TestEnvelopsCoordinateFloat()
        {
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new Vector2(50, 75)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new Vector2(100, 75)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new Vector2(75, 51)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new Vector2(75, 101)));
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Envelops(new Vector2(75, 75)));

            Assert.IsTrue(new MRectangle(-50, -51, 50, 50).Envelops(new Vector2(-25, -25)));

            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Envelops(new Vector2(0, 0)));
            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Envelops(new Vector2(0, 1)));
            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Envelops(new Vector2(1, 0)));
            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Envelops(new Vector2(1, 1)));

            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new Vector2(49, 51)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new Vector2(50, 50)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new Vector2(100, 102)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new Vector2(101, 101)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new Vector2(-75, -75)));

            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Envelops(new Vector2(-50, -52)));
            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Envelops(new Vector2(-51, -51)));
            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Envelops(new Vector2(0, 0)));
            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Envelops(new Vector2(1, -1)));
        }

        [TestMethod]
        public void TestEnvelopsCoordinateInt()
        {
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MPoint2(50, 75)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MPoint2(100, 75)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MPoint2(75, 51)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MPoint2(75, 101)));
            Assert.IsTrue(new MRectangle(50, 51, 50, 50).Envelops(new MPoint2(75, 75)));

            Assert.IsTrue(new MRectangle(-50, -51, 50, 50).Envelops(new MPoint2(-25, -25)));

            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Envelops(new MPoint2(0, 0)));
            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Envelops(new MPoint2(0, 1)));
            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Envelops(new MPoint2(1, 0)));
            Assert.IsTrue(new MRectangle(-5, -5, 10, 10).Envelops(new MPoint2(1, 1)));

            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MPoint2(49, 51)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MPoint2(50, 50)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MPoint2(100, 102)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MPoint2(101, 101)));
            Assert.IsFalse(new MRectangle(50, 51, 50, 50).Envelops(new MPoint2(-75, -75)));

            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Envelops(new MPoint2(-50, -52)));
            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Envelops(new MPoint2(-51, -51)));
            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Envelops(new MPoint2(0, 0)));
            Assert.IsFalse(new MRectangle(-50, -51, 50, 50).Envelops(new MPoint2(1, -1)));
        }

        [TestMethod]
        public void TestEqualsArea()
        {
            Assert.IsTrue(new MRectangle(0, 1, 2, 3).Equals(new MRectangle(0, 1, 2, 3)));
            Assert.IsFalse(new MRectangle(0, 1, -2, 3).Equals(new MRectangle(0, 1, 2, 3)));
            Assert.IsFalse(new MRectangle(0, 1, 2, 3).Equals(new MRectangle(1, 1, 2, 3)));
        }

        [TestMethod]
        public void TestEqualsObject()
        {
            Assert.IsTrue(new MRectangle(0, 1, 2, 3).Equals((object)new MRectangle(0, 1, 2, 3)));
            Assert.IsFalse(new MRectangle(0, 1, -2, 3).Equals((object)new MRectangle(0, 1, 2, 3)));
            Assert.IsFalse(new MRectangle(0, 1, 2, 3).Equals((object)new MRectangle(1, 1, 2, 3)));
        }

        [TestMethod]
        public void TestGetBottom()
        {
            Assert.AreEqual(6f, new MRectangle(1, 2, 3, 4).Bottom);
            Assert.AreEqual(2f, new MRectangle(-1, -2, 3, 4).Bottom);
            Assert.AreEqual(-2f, new MRectangle(-1, -2, -3, -4).Bottom);
        }

        [TestMethod]
        public void TestGetBottomLeft()
        {
            Assert.AreEqual(new MVector2(1, 6), new MRectangle(1, 2, 3, 4).BottomLeft);
            Assert.AreEqual(new MVector2(-1, 2), new MRectangle(-1, -2, 3, 4).BottomLeft);
            Assert.AreEqual(new MVector2(-4, -2), new MRectangle(-1, -2, -3, -4).BottomLeft);
        }

        [TestMethod]
        public void TestGetBottomRight()
        {
            Assert.AreEqual(new MVector2(4, 6), new MRectangle(1, 2, 3, 4).BottomRight);
            Assert.AreEqual(new MVector2(2, 2), new MRectangle(-1, -2, 3, 4).BottomRight);
            Assert.AreEqual(new MVector2(-1, -2), new MRectangle(-1, -2, -3, -4).BottomRight);
        }

        [TestMethod]
        public void TestGetLeft()
        {
            Assert.AreEqual(1f, new MRectangle(1, 2, 3, 4).Left);
            Assert.AreEqual(-1f, new MRectangle(-1, -2, 3, 4).Left);
            Assert.AreEqual(-4f, new MRectangle(-1, -2, -3, -4).Left);
        }

        [TestMethod]
        public void TestGetRight()
        {
            Assert.AreEqual(4f, new MRectangle(1, 2, 3, 4).Right);
            Assert.AreEqual(2f, new MRectangle(-1, -2, 3, 4).Right);
            Assert.AreEqual(-1f, new MRectangle(-1, -2, -3, -4).Right);
        }

        [TestMethod]
        public void TestGetTop()
        {
            Assert.AreEqual(2f, new MRectangle(1, 2, 3, 4).Top);
            Assert.AreEqual(-2f, new MRectangle(-1, -2, 3, 4).Top);
            Assert.AreEqual(-6f, new MRectangle(-1, -2, -3, -4).Top);
        }

        [TestMethod]
        public void TestGetTopLeft()
        {
            Assert.AreEqual(new MVector2(1, 2), new MRectangle(1, 2, 3, 4).TopLeft);
            Assert.AreEqual(new MVector2(-1, -2), new MRectangle(-1, -2, 3, 4).TopLeft);
            Assert.AreEqual(new MVector2(-4, -6), new MRectangle(-1, -2, -3, -4).TopLeft);
        }

        [TestMethod]
        public void TestGetTopRight()
        {
            Assert.AreEqual(new MVector2(4, 2), new MRectangle(1, 2, 3, 4).TopRight);
            Assert.AreEqual(new MVector2(2, -2), new MRectangle(-1, -2, 3, 4).TopRight);
            Assert.AreEqual(new MVector2(-1, -6), new MRectangle(-1, -2, -3, -4).TopRight);
        }

        [TestMethod]
        public void TestOperatorEquals()
        {
            Assert.IsTrue(new MRectangle(0, 1, 2, 3) == new MRectangle(0, 1, 2, 3));
            Assert.IsFalse(new MRectangle(0, 1, -2, 3) == new MRectangle(0, 1, 2, 3));
            Assert.IsFalse(new MRectangle(0, 1, 2, 3) == new MRectangle(1, 1, 2, 3));
        }

        [TestMethod]
        public void TestOperatorNotEquals()
        {
            Assert.IsFalse(new MRectangle(0, 1, 2, 3) != new MRectangle(0, 1, 2, 3));
            Assert.IsTrue(new MRectangle(0, 1, -2, 3) != new MRectangle(0, 1, 2, 3));
            Assert.IsTrue(new MRectangle(0, 1, 2, 3) != new MRectangle(1, 1, 2, 3));
        }

        [TestMethod]
        public void TestTranslate()
        {
            for (int i = 0; i < MRectangleTests.RANDOM_TEST_AMOUNT; i++)
            {
                float x = random.Next(-100, 100);
                float y = random.Next(-100, 100);
                float w = random.Next(0, 100);
                float h = random.Next(0, 100);

                float tx = random.Next(-100, 100);
                float ty = random.Next(-100, 100);

                var a = new MRectangle(x, y, w, h);
                Assert.AreEqual(new MRectangle(x + tx, y + ty, w, h), a.Translate(new Vector2(tx, ty)));

                // Assert X Y methods
                Assert.AreEqual(a.Translate(new Vector2(tx, 0)), a.TranslateX(tx));
                Assert.AreEqual(a.Translate(new Vector2(0, ty)), a.TranslateY(ty));
            }
        }

        [TestMethod]
        public void TestWidthHeight()
        {
            for (int i = 0; i < MRectangleTests.RANDOM_TEST_AMOUNT; i++)
            {
                float x = random.Next(-100, 100);
                float y = random.Next(-100, 100);
                float x2 = random.Next(-100, 100);
                float y2 = random.Next(-100, 100);

                var a = new MRectangle(x, y, x2 - x, y2 - y);
                Assert.AreEqual(Math.Abs(y2 - y), a.Height);
                Assert.AreEqual(Math.Abs(x2 - x), a.Width);
            }
        }

        [TestMethod]
        public void AspectRatio()
        {
            var testRectangle = new MRectangle(16, 9);
            var testRectangle2 = new MRectangle(4, 3);
            Assert.AreEqual(testRectangle.Width / testRectangle.Height, testRectangle.AspectRatio);
            Assert.AreEqual(testRectangle2.Width / testRectangle2.Height, testRectangle2.AspectRatio);
        }

        [TestMethod]
        public void ScaleToFit_LargerThanBounding_LetterboxedTopBottom()
        {
            var testRectangle = new MRectangle(16, 9);
            var boundingRectangle = new MRectangle(4, 3);
            var result = testRectangle.ScaleToFit(boundingRectangle);
            Assert.AreEqual(testRectangle.AspectRatio, result.AspectRatio, "The aspect ratio must be preserved");
            Assert.AreEqual(new MRectangle(4, 2.25f), result);
        }

        [TestMethod]
        public void ScaleToFit_LargerThanBounding_LetterboxedLeftRight()
        {
            var testRectangle = new MRectangle(24, 18);
            var boundingRectangle = new MRectangle(16, 9);
            var result = testRectangle.ScaleToFit(boundingRectangle);
            Assert.AreEqual(testRectangle.AspectRatio, result.AspectRatio, "The aspect ratio must be preserved");
            Assert.AreEqual(new MRectangle(12, 9), result);
        }

        [TestMethod]
        public void ScaleToFit_LargerThanBounding_NoLetterbox()
        {
            var testRectangle = new MRectangle(32, 18);
            var boundingRectangle = new MRectangle(16, 9);
            var result = testRectangle.ScaleToFit(boundingRectangle);
            Assert.AreEqual(testRectangle.AspectRatio, result.AspectRatio, "The aspect ratio must be preserved");
            Assert.AreEqual(boundingRectangle, result);
        }

        [TestMethod]
        public void ScaleToFit_SmallerThanBounding_LetterboxedTopBottom()
        {
            var testRectangle = new MRectangle(16, 9);
            var boundingRectangle = new MRectangle(40, 30);
            var result = testRectangle.ScaleToFit(boundingRectangle);
            Assert.AreEqual(testRectangle.AspectRatio, result.AspectRatio, "The aspect ratio must be preserved");
            Assert.AreEqual(new MRectangle(40, 22.5f), result);
        }

        [TestMethod]
        public void ScaleToFit_SmallerThanBounding_LetterboxedLeftRight()
        {
            var testRectangle = new MRectangle(24, 18);
            var boundingRectangle = new MRectangle(160, 90);
            var result = testRectangle.ScaleToFit(boundingRectangle);
            Assert.AreEqual(testRectangle.AspectRatio, result.AspectRatio, "The aspect ratio must be preserved");
            Assert.AreEqual(new MRectangle(120, 90), result);
        }

        [TestMethod]
        public void ScaleToFit_SmallerThanBounding_NoLetterbox()
        {
            var testRectangle = new MRectangle(16, 9);
            var boundingRectangle = new MRectangle(32, 18);
            var result = testRectangle.ScaleToFit(boundingRectangle);
            Assert.AreEqual(testRectangle.AspectRatio, result.AspectRatio, "The aspect ratio must be preserved");
            Assert.AreEqual(boundingRectangle, result);
        }

        [TestMethod]
        public void ScaleToFit_PositionInvariant()
        {
            var testRectangle = new MRectangle(20, 30, 24, 18);
            var boundingRectangle = new MRectangle(160, 90);
            var result = testRectangle.ScaleToFit(boundingRectangle);
            Assert.AreEqual(testRectangle.AspectRatio, result.AspectRatio, "The aspect ratio must be preserved");
            Assert.AreEqual(testRectangle.TopLeft, result.TopLeft);
        }

        [DataRow(1f, 1f)]
        [DataRow(2f, 2f)]
        [DataRow(3f, 3f)]
        [TestMethod]
        public void ScaleToFill_SameRatio_ReturnsBounding(float width, float height)
        {
            var testRectangle = new MRectangle(width, height);
            var boundingRectangle = new MRectangle(2, 2);
            var result = testRectangle.ScaleToFill(boundingRectangle);
            Assert.AreEqual(testRectangle.AspectRatio, result.AspectRatio, "The aspect ratio must be preserved");
            Assert.AreEqual(boundingRectangle, result);
        }

        [TestMethod]
        [DataRow(3f, 3f)]
        [DataRow(2f, 4f)]
        public void ScaleToFill_AlreadyFills_ReturnsOriginal(float width, float height)
        {
            var testRectangle = new MRectangle(width, height);
            var boundingRectangle = new MRectangle(2, 3);
            var result = testRectangle.ScaleToFill(boundingRectangle);
            Assert.AreEqual(testRectangle.AspectRatio, result.AspectRatio, "The aspect ratio must be preserved");
            Assert.AreEqual(testRectangle, result);
        }

        [TestMethod]
        [DataRow(14f, 12f, 11.66666f, 10f, DisplayName = "Larger horizontal rectangle fits height")]
        [DataRow(10f, 12f, 8.333333f, 10f, DisplayName = "Larger vertical rectangle fits height")]
        [DataRow(5f, 20f, 4f, 16f, DisplayName = "Larger vertical rectangle fits width")]
        [DataRow(3f, 2f, 15f, 10f, DisplayName = "Smaller horizontal rectangle fits height")]
        [DataRow(3.9f, 9f, 4.333333f, 10f, DisplayName = "Smaller vertical rectangle fits height")]
        [DataRow(3f, 9.9f, 4f, 13.2f, DisplayName = "Smaller vertical rectangle fits width")]
        [DataRow(3f, 11f, 4f, 14.66666f, DisplayName = "Vertical rectangle with smaller width and larger height fits width")]
        [DataRow(5f, 9f, 5.55555f, 10f, DisplayName = "Vertical rectangle with smaller height and larger width fits height")]
        public void ScaleToFill_VerticalBounding(float width, float height, float expectedWidth, float expectedHeight)
        {
            var testRectangle = new MRectangle(width, height);
            var boundingRectangle = new MRectangle(4, 10);
            var result = testRectangle.ScaleToFill(boundingRectangle);
            Assert.AreEqual(testRectangle.AspectRatio, result.AspectRatio, 0.00001f, "The aspect ratio must be preserved");
            Assert.AreEqual(expectedWidth, result.Width, 0.00001f);
            Assert.AreEqual(expectedHeight, result.Height, 0.00001f);
        }

        [TestMethod]
        [DataRow(12f, 14f, 10, 11.66666f, DisplayName = "Larger vertical rectangle fits width")]
        [DataRow(12f, 10f, 10f, 8.333333f, DisplayName = "Larger horizontal rectangle fits width")]
        [DataRow(20f, 5f, 16f, 4f, DisplayName = "Larger horizontal rectangle fits height")]
        [DataRow(2f, 3f, 10f, 15f, DisplayName = "Smaller vertical rectangle fits width")]
        [DataRow(9f, 3.9f, 10f, 4.333333f, DisplayName = "Smaller horizontal rectangle fits width")]
        [DataRow(9.9f, 3f, 13.2f, 4f, DisplayName = "Smaller horizontal rectangle fits height")]
        [DataRow(11f, 3f, 14.66666f, 4f, DisplayName = "Horizontal rectangle with smaller height and larger width fits height")]
        [DataRow(9f, 5f, 10f, 5.55555f, DisplayName = "Horizontal rectangle with smaller width and larger height fits width")]
        public void ScaleToFill_HorizontalBounding(float width, float height, float expectedWidth, float expectedHeight)
        {
            var testRectangle = new MRectangle(width, height);
            var boundingRectangle = new MRectangle(10, 4);
            var result = testRectangle.ScaleToFill(boundingRectangle);
            Assert.AreEqual(testRectangle.AspectRatio, result.AspectRatio, 0.00001f, "The aspect ratio must be preserved");
            Assert.AreEqual(expectedWidth, result.Width, 0.00001f);
            Assert.AreEqual(expectedHeight, result.Height, 0.00001f);
        }

        [TestMethod]
        public void ScaleToFill_PositionInvariant()
        {
            var testRectangle = new MRectangle(10, 20, 9f, 5f);
            var boundingRectangle = new MRectangle(10, 4);
            var result = testRectangle.ScaleToFill(boundingRectangle);
            Assert.AreEqual(testRectangle.AspectRatio, result.AspectRatio, 0.00001f, "The aspect ratio must be preserved");
            Assert.AreEqual(testRectangle.TopLeft, result.TopLeft);
        }

        [TestMethod]
        public void PositionCenter_Point()
        {
            var testRectangle = new MRectangle(16, 9);
            var point = new MVector2(32, 18);
            var result = testRectangle.PositionCenter(point);
            Assert.AreEqual(new MRectangle(32 - 8, 18 - 4.5f, testRectangle.Width, testRectangle.Height), result);
        }

        [TestMethod]
        public void PositionCenter_Rectangle()
        {
            var testRectangle = new MRectangle(16, 9);
            var boundingRectangle = new MRectangle(10, 10, 32, 18);
            var result = testRectangle.PositionCenter(boundingRectangle);
            Assert.AreEqual(new MRectangle(26 - 8, 19 - 4.5f, testRectangle.Width, testRectangle.Height), result);
        }

        [TestMethod]
        public void Dimensions()
        {
            var testRectangle = new MRectangle(12, 13, 14, 15);
            Assert.AreEqual(new MVector2(14, 15), testRectangle.Dimensions);
        }
    }
}
