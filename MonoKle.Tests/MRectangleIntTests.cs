using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;

namespace MonoKle.Tests
{
    [TestClass]
    public class MRectangleIntTests
    {
        private const int RANDOM_TEST_AMOUNT = 25;

        private readonly Random random = new Random();

        [TestMethod]
        public void Translate_CorrectTranslation()
        {
            for (int i = 0; i < RANDOM_TEST_AMOUNT; i++)
            {
                int x = random.Next(-100, 100);
                int y = random.Next(-100, 100);
                int width = random.Next(0, 100);
                int height = random.Next(0, 100);

                int dx = random.Next(-100, 100);
                int dy = random.Next(-100, 100);

                var sut = new MRectangleInt(x, y, width, height);
                var expected = new MRectangleInt(x + dx, y + dy, width, height);

                // Assert translation methods                
                Assert.AreEqual(expected, sut.Translate(new MPoint2(dx, dy)));
                Assert.AreEqual(expected, sut.TranslateX(dx).TranslateY(dy));
                Assert.AreEqual(expected, sut.Translate(dx, dy));
            }
        }

        [TestMethod]
        public void Dimensions_Correct()
        {
            for (int i = 0; i < RANDOM_TEST_AMOUNT; i++)
            {
                int x = random.Next(-100, 100);
                int y = random.Next(-100, 100);
                int x2 = random.Next(-100, 100);
                int y2 = random.Next(-100, 100);

                var sut = new MRectangleInt(x, y, x2 - x, y2 - y);
                var expected = new MPoint2(Math.Abs(x2 - x), Math.Abs(y2 - y));

                Assert.AreEqual(expected.X, sut.Width);
                Assert.AreEqual(expected.Y, sut.Height);
                Assert.AreEqual(expected, sut.Dimensions);
            }
        }

        [TestMethod]
        public void Constructors_ValuesAssigned()
        {
            // Test that all constructors work the same
            var a = new MRectangleInt(-5, -5);
            var b = new MRectangleInt(new MPoint2(0, 0), new MPoint2(-5, -5));
            var c = new MRectangleInt(0, 0, -5, -5);
            var d = new MRectangleInt(new MPoint2(-5, -5));

            Assert.AreEqual(a, b);
            Assert.AreEqual(b, c);
            Assert.AreEqual(c, d);

            // Test negative size
            Assert.AreEqual(new MRectangleInt(0, 0, 10, 10), new MRectangleInt(10, 10, -10, -10));
            Assert.AreEqual(new MRectangleInt(-10, -10, 10, 10), new MRectangleInt(0, 0, -10, -10));
            Assert.AreEqual(new MRectangleInt(-30, -30, 20, 20), new MRectangleInt(-10, -10, -20, -20));
        }

        [TestMethod]
        public void Contains_CoordinateFloat()
        {
            Assert.IsTrue(new MRectangleInt(50, 51, 50, 50).Contains(new Vector2(50, 51)));
            Assert.IsTrue(new MRectangleInt(50, 51, 50, 50).Contains(new Vector2(100, 101)));
            Assert.IsTrue(new MRectangleInt(50, 51, 50, 50).Contains(new Vector2(75, 75)));

            Assert.IsTrue(new MRectangleInt(-50, -51, 50, 50).Contains(new Vector2(-25, -25)));
            Assert.IsTrue(new MRectangleInt(-50, -51, 50, 50).Contains(new Vector2(-50, -51)));
            Assert.IsTrue(new MRectangleInt(-50, -51, 50, 50).Contains(new Vector2(0, -1)));

            Assert.IsTrue(new MRectangleInt(-5, -5, 10, 10).Contains(new Vector2(0, 0)));
            Assert.IsTrue(new MRectangleInt(-5, -5, 10, 10).Contains(new Vector2(0, 1)));
            Assert.IsTrue(new MRectangleInt(-5, -5, 10, 10).Contains(new Vector2(1, 0)));
            Assert.IsTrue(new MRectangleInt(-5, -5, 10, 10).Contains(new Vector2(1, 1)));

            Assert.IsFalse(new MRectangleInt(50, 51, 50, 50).Contains(new Vector2(49, 51)));
            Assert.IsFalse(new MRectangleInt(50, 51, 50, 50).Contains(new Vector2(50, 50)));
            Assert.IsFalse(new MRectangleInt(50, 51, 50, 50).Contains(new Vector2(100, 102)));
            Assert.IsFalse(new MRectangleInt(50, 51, 50, 50).Contains(new Vector2(101, 101)));
            Assert.IsFalse(new MRectangleInt(50, 51, 50, 50).Contains(new Vector2(-75, -75)));

            Assert.IsFalse(new MRectangleInt(-50, -51, 50, 50).Contains(new Vector2(-50, -52)));
            Assert.IsFalse(new MRectangleInt(-50, -51, 50, 50).Contains(new Vector2(-51, -51)));
            Assert.IsFalse(new MRectangleInt(-50, -51, 50, 50).Contains(new Vector2(0, 0)));
            Assert.IsFalse(new MRectangleInt(-50, -51, 50, 50).Contains(new Vector2(1, -1)));
        }

        [TestMethod]
        public void Contains_CoordinateInt()
        {
            Assert.IsTrue(new MRectangleInt(50, 51, 50, 50).Contains(new MPoint2(50, 51)));
            Assert.IsTrue(new MRectangleInt(50, 51, 50, 50).Contains(new MPoint2(100, 101)));
            Assert.IsTrue(new MRectangleInt(50, 51, 50, 50).Contains(new MPoint2(75, 75)));

            Assert.IsTrue(new MRectangleInt(-50, -51, 50, 50).Contains(new MPoint2(-25, -25)));
            Assert.IsTrue(new MRectangleInt(-50, -51, 50, 50).Contains(new MPoint2(-50, -51)));
            Assert.IsTrue(new MRectangleInt(-50, -51, 50, 50).Contains(new MPoint2(0, -1)));

            Assert.IsTrue(new MRectangleInt(-5, -5, 10, 10).Contains(new MPoint2(0, 0)));
            Assert.IsTrue(new MRectangleInt(-5, -5, 10, 10).Contains(new MPoint2(0, 1)));
            Assert.IsTrue(new MRectangleInt(-5, -5, 10, 10).Contains(new MPoint2(1, 0)));
            Assert.IsTrue(new MRectangleInt(-5, -5, 10, 10).Contains(new MPoint2(1, 1)));

            Assert.IsFalse(new MRectangleInt(50, 51, 50, 50).Contains(new MPoint2(49, 51)));
            Assert.IsFalse(new MRectangleInt(50, 51, 50, 50).Contains(new MPoint2(50, 50)));
            Assert.IsFalse(new MRectangleInt(50, 51, 50, 50).Contains(new MPoint2(100, 102)));
            Assert.IsFalse(new MRectangleInt(50, 51, 50, 50).Contains(new MPoint2(101, 101)));
            Assert.IsFalse(new MRectangleInt(50, 51, 50, 50).Contains(new MPoint2(-75, -75)));

            Assert.IsFalse(new MRectangleInt(-50, -51, 50, 50).Contains(new MPoint2(-50, -52)));
            Assert.IsFalse(new MRectangleInt(-50, -51, 50, 50).Contains(new MPoint2(-51, -51)));
            Assert.IsFalse(new MRectangleInt(-50, -51, 50, 50).Contains(new MPoint2(0, 0)));
            Assert.IsFalse(new MRectangleInt(-50, -51, 50, 50).Contains(new MPoint2(1, -1)));
        }

        [TestMethod]
        public void Contains_Area()
        {
            Assert.IsTrue(new MRectangleInt(50, 51, 50, 50).Contains(new MRectangle(50, 51, 50, 50)));
            Assert.IsTrue(new MRectangleInt(50, 51, 50, 50).Contains(new MRectangle(60, 60, 20, 20)));
            Assert.IsTrue(new MRectangleInt(-10, -10, 20, 20).Contains(new MRectangle(-5, -5, 10, 10)));

            Assert.IsFalse(new MRectangleInt(-10, -10, 20, 20).Contains(new MRectangle(-5, -5, 30, 10)));
            Assert.IsFalse(new MRectangleInt(-10, -10, 20, 20).Contains(new MRectangle(-5, -5, 10, 30)));
            Assert.IsFalse(new MRectangleInt(-10, -10, 20, 20).Contains(new MRectangle(-11, -5, 10, 10)));
            Assert.IsFalse(new MRectangleInt(-10, -10, 20, 20).Contains(new MRectangle(-5, -11, 10, 10)));
            Assert.IsFalse(new MRectangleInt(-10, -10, 20, 20).Contains(new MRectangle(50, 50, 10, 10)));
        }

        [TestMethod]
        public void Contains_AreaInt()
        {
            Assert.IsTrue(new MRectangleInt(50, 51, 50, 50).Contains(new MRectangleInt(50, 51, 50, 50)));
            Assert.IsTrue(new MRectangleInt(50, 51, 50, 50).Contains(new MRectangleInt(60, 60, 20, 20)));
            Assert.IsTrue(new MRectangleInt(-10, -10, 20, 20).Contains(new MRectangleInt(-5, -5, 10, 10)));

            Assert.IsFalse(new MRectangleInt(-10, -10, 20, 20).Contains(new MRectangleInt(-5, -5, 30, 10)));
            Assert.IsFalse(new MRectangleInt(-10, -10, 20, 20).Contains(new MRectangleInt(-5, -5, 10, 30)));
            Assert.IsFalse(new MRectangleInt(-10, -10, 20, 20).Contains(new MRectangleInt(-11, -5, 10, 10)));
            Assert.IsFalse(new MRectangleInt(-10, -10, 20, 20).Contains(new MRectangleInt(-5, -11, 10, 10)));
            Assert.IsFalse(new MRectangleInt(-10, -10, 20, 20).Contains(new MRectangleInt(50, 50, 10, 10)));
        }

        [TestMethod]
        public void Clamp_Area()
        {
            Assert.AreEqual(new MRectangle(-1, -1, 2, 2), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangle(-1, -1, 2, 2)));
            Assert.AreEqual(new MRectangle(-5, -5, 10, 10), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangle(-5, -5, 10, 10)));
            Assert.AreEqual(new MRectangle(-5, -5, 10, 10), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangle(-10, -10, 20, 20)));

            Assert.AreEqual(new MRectangle(-1, -1, 6, 2), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangle(-1, -1, 10, 2)));
            Assert.AreEqual(new MRectangle(-1, -1, 2, 6), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangle(-1, -1, 2, 10)));
            Assert.AreEqual(new MRectangle(-5, -1, 3, 5), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangle(-7, -1, 5, 5)));
            Assert.AreEqual(new MRectangle(-1, -5, 5, 3), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangle(-1, -7, 5, 5)));
        }

        [TestMethod]
        public void Clamp_AreaInteger()
        {
            Assert.AreEqual(new MRectangleInt(-1, -1, 2, 2), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangleInt(-1, -1, 2, 2)));
            Assert.AreEqual(new MRectangleInt(-5, -5, 10, 10), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangleInt(-5, -5, 10, 10)));
            Assert.AreEqual(new MRectangleInt(-5, -5, 10, 10), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangleInt(-10, -10, 20, 20)));

            Assert.AreEqual(new MRectangleInt(-1, -1, 6, 2), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangleInt(-1, -1, 10, 2)));
            Assert.AreEqual(new MRectangleInt(-1, -1, 2, 6), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangleInt(-1, -1, 2, 10)));
            Assert.AreEqual(new MRectangleInt(-5, -1, 3, 5), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangleInt(-7, -1, 5, 5)));
            Assert.AreEqual(new MRectangleInt(-1, -5, 5, 3), new MRectangleInt(-5, -5, 10, 10).Clamp(new MRectangleInt(-1, -7, 5, 5)));
        }

        [TestMethod]
        public void Clamp_Coordinate()
        {
            Assert.AreEqual(new MVector2(0, 0), new MRectangleInt(-5, -5, 10, 10).Clamp(new MVector2(0, 0)));
            Assert.AreEqual(new MVector2(-5, -5), new MRectangleInt(-5, -5, 10, 10).Clamp(new MVector2(-5, -5)));
            Assert.AreEqual(new MVector2(5, 5), new MRectangleInt(-5, -5, 10, 10).Clamp(new MVector2(5, 5)));

            Assert.AreEqual(new MVector2(0, 5), new MRectangleInt(-5, -5, 10, 10).Clamp(new MVector2(0, 7)));
            Assert.AreEqual(new MVector2(5, 0), new MRectangleInt(-5, -5, 10, 10).Clamp(new MVector2(7, 0)));
            Assert.AreEqual(new MVector2(0, -5), new MRectangleInt(-5, -5, 10, 10).Clamp(new MVector2(0, -7)));
            Assert.AreEqual(new MVector2(-5, 0), new MRectangleInt(-5, -5, 10, 10).Clamp(new MVector2(-7, 0)));
        }

        [TestMethod]
        public void Clamp_CoordinateInteger()
        {
            Assert.AreEqual(new MPoint2(0, 0), new MRectangleInt(-5, -5, 10, 10).Clamp(new MPoint2(0, 0)));
            Assert.AreEqual(new MPoint2(-5, -5), new MRectangleInt(-5, -5, 10, 10).Clamp(new MPoint2(-5, -5)));
            Assert.AreEqual(new MPoint2(5, 5), new MRectangleInt(-5, -5, 10, 10).Clamp(new MPoint2(5, 5)));

            Assert.AreEqual(new MPoint2(0, 5), new MRectangleInt(-5, -5, 10, 10).Clamp(new MPoint2(0, 7)));
            Assert.AreEqual(new MPoint2(5, 0), new MRectangleInt(-5, -5, 10, 10).Clamp(new MPoint2(7, 0)));
            Assert.AreEqual(new MPoint2(0, -5), new MRectangleInt(-5, -5, 10, 10).Clamp(new MPoint2(0, -7)));
            Assert.AreEqual(new MPoint2(-5, 0), new MRectangleInt(-5, -5, 10, 10).Clamp(new MPoint2(-7, 0)));
        }

        [TestMethod]
        public void Equals_Object()
        {
            Assert.IsTrue(new MRectangleInt(0, 1, 2, 3).Equals((object)new MRectangleInt(0, 1, 2, 3)));
            Assert.IsFalse(new MRectangleInt(0, 1, -2, 3).Equals((object)new MRectangleInt(0, 1, 2, 3)));
            Assert.IsFalse(new MRectangleInt(0, 1, 2, 3).Equals((object)new MRectangleInt(1, 1, 2, 3)));
        }

        [TestMethod]
        public void Equals_AreaInteger()
        {
            Assert.IsTrue(new MRectangleInt(0, 1, 2, 3).Equals(new MRectangleInt(0, 1, 2, 3)));
            Assert.IsFalse(new MRectangleInt(0, 1, -2, 3).Equals(new MRectangleInt(0, 1, 2, 3)));
            Assert.IsFalse(new MRectangleInt(0, 1, 2, 3).Equals(new MRectangleInt(1, 1, 2, 3)));
        }

        [TestMethod]
        public void Bottom()
        {
            Assert.AreEqual(6f, new MRectangleInt(1, 2, 3, 4).Bottom);
            Assert.AreEqual(2f, new MRectangleInt(-1, -2, 3, 4).Bottom);
            Assert.AreEqual(-2f, new MRectangleInt(-1, -2, -3, -4).Bottom);
        }

        [TestMethod]
        public void BottomLeft()
        {
            Assert.AreEqual(new MPoint2(1, 6), new MRectangleInt(1, 2, 3, 4).BottomLeft);
            Assert.AreEqual(new MPoint2(-1, 2), new MRectangleInt(-1, -2, 3, 4).BottomLeft);
            Assert.AreEqual(new MPoint2(-4, -2), new MRectangleInt(-1, -2, -3, -4).BottomLeft);
        }

        [TestMethod]
        public void BottomRight()
        {
            Assert.AreEqual(new MPoint2(4, 6), new MRectangleInt(1, 2, 3, 4).BottomRight);
            Assert.AreEqual(new MPoint2(2, 2), new MRectangleInt(-1, -2, 3, 4).BottomRight);
            Assert.AreEqual(new MPoint2(-1, -2), new MRectangleInt(-1, -2, -3, -4).BottomRight);
        }

        [TestMethod]
        public void Left()
        {
            Assert.AreEqual(1f, new MRectangleInt(1, 2, 3, 4).Left);
            Assert.AreEqual(-1f, new MRectangleInt(-1, -2, 3, 4).Left);
            Assert.AreEqual(-4f, new MRectangleInt(-1, -2, -3, -4).Left);
        }

        [TestMethod]
        public void Right()
        {
            Assert.AreEqual(4f, new MRectangleInt(1, 2, 3, 4).Right);
            Assert.AreEqual(2f, new MRectangleInt(-1, -2, 3, 4).Right);
            Assert.AreEqual(-1f, new MRectangleInt(-1, -2, -3, -4).Right);
        }

        [TestMethod]
        public void Top()
        {
            Assert.AreEqual(2f, new MRectangleInt(1, 2, 3, 4).Top);
            Assert.AreEqual(-2f, new MRectangleInt(-1, -2, 3, 4).Top);
            Assert.AreEqual(-6f, new MRectangleInt(-1, -2, -3, -4).Top);
        }

        [TestMethod]
        public void TopLeft()
        {
            Assert.AreEqual(new MPoint2(1, 2), new MRectangleInt(1, 2, 3, 4).TopLeft);
            Assert.AreEqual(new MPoint2(-1, -2), new MRectangleInt(-1, -2, 3, 4).TopLeft);
            Assert.AreEqual(new MPoint2(-4, -6), new MRectangleInt(-1, -2, -3, -4).TopLeft);
        }

        [TestMethod]
        public void TopRight()
        {
            Assert.AreEqual(new MPoint2(4, 2), new MRectangleInt(1, 2, 3, 4).TopRight);
            Assert.AreEqual(new MPoint2(2, -2), new MRectangleInt(-1, -2, 3, 4).TopRight);
            Assert.AreEqual(new MPoint2(-1, -6), new MRectangleInt(-1, -2, -3, -4).TopRight);
        }

        [TestMethod]
        public void OperatorEquals()
        {
            Assert.IsTrue(new MRectangleInt(0, 1, 2, 3) == new MRectangleInt(0, 1, 2, 3));
            Assert.IsFalse(new MRectangleInt(0, 1, -2, 3) == new MRectangleInt(0, 1, 2, 3));
            Assert.IsFalse(new MRectangleInt(0, 1, 2, 3) == new MRectangleInt(1, 1, 2, 3));
        }

        [TestMethod]
        public void OperatorNotEquals()
        {
            Assert.IsFalse(new MRectangleInt(0, 1, 2, 3) != new MRectangleInt(0, 1, 2, 3));
            Assert.IsTrue(new MRectangleInt(0, 1, -2, 3) != new MRectangleInt(0, 1, 2, 3));
            Assert.IsTrue(new MRectangleInt(0, 1, 2, 3) != new MRectangleInt(1, 1, 2, 3));
        }

        [TestMethod]
        public void ImplicitConversion_ToRectangle()
        {
            var original = new Rectangle(-10, -20, 35, 37);
            MRectangleInt conv = original;
            Rectangle back = conv;
            Assert.AreEqual(original, back);
        }

        [TestMethod]
        public void ImplicitConversion_FromRectangle()
        {
            var original = new MRectangleInt(-10, -20, 35, 37);
            Rectangle conv = original;
            MRectangleInt back = conv;
            Assert.AreEqual(original, back);
        }
    }
}