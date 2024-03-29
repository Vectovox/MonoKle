﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoKle.Tests
{
    [TestClass]
    public class MRectangleIntTests
    {
        private const int RANDOM_TEST_AMOUNT = 25;

        private readonly Random _random = new Random();

        [TestMethod]
        public void Redimension_CorrectlySet()
        {
            int x = 5;
            int y = 7;
            int width = 11;
            int height = 31;
            int newWidth = 50;
            int newHeight = 70;
            MRectangleInt sut = new MRectangleInt(x, y, width, height);
            MRectangleInt expected = new MRectangleInt(x, y, newWidth, newHeight);

            Assert.AreEqual(expected, sut.Redimension(newWidth, newHeight));
            Assert.AreEqual(expected, sut.Redimension(new MPoint2(newWidth, newHeight)));
            Assert.AreEqual(expected, sut.RedimensionWidth(newWidth).RedimensionHeight(newHeight));
        }

        [TestMethod]
        public void Resize_CorrectResizing()
        {
            for (int i = 0; i < RANDOM_TEST_AMOUNT; i++)
            {
                int x = _random.Next(-100, 100);
                int y = _random.Next(-100, 100);
                int width = _random.Next(0, 100);
                int height = _random.Next(0, 100);

                int dWidth = _random.Next(-100, 100);
                int dHeight = _random.Next(-100, 100);

                var sut = new MRectangleInt(x, y, width, height);
                var expected = new MRectangleInt(x, y, width + dWidth, height + dHeight);

                // Assert translation methods                
                Assert.AreEqual(expected, sut.Resize(new MPoint2(dWidth, dHeight)));
                Assert.AreEqual(expected, sut.ResizeWidth(dWidth).ResizeHeight(dHeight));
                Assert.AreEqual(expected, sut.Resize(dWidth, dHeight));
            }
        }

        [TestMethod]
        public void Translate_CorrectTranslation()
        {
            for (int i = 0; i < RANDOM_TEST_AMOUNT; i++)
            {
                int x = _random.Next(-100, 100);
                int y = _random.Next(-100, 100);
                int width = _random.Next(0, 100);
                int height = _random.Next(0, 100);

                int dx = _random.Next(-100, 100);
                int dy = _random.Next(-100, 100);

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
                int x = _random.Next(-100, 100);
                int y = _random.Next(-100, 100);
                int x2 = _random.Next(-100, 100);
                int y2 = _random.Next(-100, 100);

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
        public void ImplicitConversion_ToRectangle_ValueKept()
        {
            var original = new Rectangle(-10, -20, 35, 37);
            MRectangleInt conv = original;
            Rectangle back = conv;
            Assert.AreEqual(original, back);
        }

        [TestMethod]
        public void ImplicitConversion_FromRectangle_ValueKept()
        {
            var original = new MRectangleInt(-10, -20, 35, 37);
            Rectangle conv = original;
            MRectangleInt back = conv;
            Assert.AreEqual(original, back);
        }

        [TestMethod]
        public void ImplicitConversion_ToRectangle_Equal()
        {
            var sut = new MRectangleInt(-50, -23, 170, 99);
            AssertRectangleEqual(sut, sut);
        }

        [TestMethod]
        public void Constructor_EqualToRectangle()
        {
            var sut = new MRectangleInt(-50, -23, 170, 99);
            var rect = new Rectangle(-50, -23, 170, 99);
            AssertRectangleEqual(rect, sut);
        }

        private void AssertRectangleEqual(Rectangle rect, MRectangleInt sut)
        {
            Assert.AreEqual(rect.X, sut.Left);
            Assert.AreEqual(rect.Y, sut.Top);
            Assert.AreEqual(rect.Width, sut.Width);
            Assert.AreEqual(rect.Height, sut.Height);
            Assert.AreEqual(rect.Right, sut.Right);
            Assert.AreEqual(rect.Bottom, sut.Bottom);
        }

        [DataTestMethod]
        [DataRow(9, 9, 5, 5, 10, 10, 4, 4, DisplayName = "Top left corner")]
        [DataRow(24, 9, 5, 5, 24, 10, 1, 4, DisplayName = "Top right corner")]
        [DataRow(9, 24, 5, 5, 10, 24, 4, 1, DisplayName = "Bottom left corner")]
        [DataRow(24, 24, 5, 5, 24, 24, 1, 1, DisplayName = "Bottom right corner")]
        [DataRow(9, 9, 20, 20, 10, 10, 15, 15, DisplayName = "Enveloped")]
        [DataRow(9, 11, 20, 5, 10, 11, 15, 5, DisplayName = "Horizontal cross")]
        public void Intersect_CorrectValueCommutative(int x, int y, int w, int h, int ex, int ey, int ew, int eh)
        {
            var testRectangle = new MRectangleInt(10, 10, 15, 15);
            var intersectRectangle = new MRectangleInt(x, y, w, h);
            var expectedRectangle = new MRectangleInt(ex, ey, ew, eh);
            // Assert we get the expected rectangle commutatively
            Assert.AreEqual(expectedRectangle, testRectangle.Intersect(intersectRectangle));
            Assert.AreEqual(expectedRectangle, intersectRectangle.Intersect(testRectangle));
        }

        [TestMethod]
        public void Scale_Growing_OriginZero_Correct()
        {
            var testRectangle = new MRectangleInt(-5, -5, 10, 10);
            var result = testRectangle.Scale(2f);
            var expected = new MRectangleInt(-10, -10, 20, 20);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Scale_Shrinking_OriginZero_Correct()
        {
            var testRectangle = new MRectangleInt(-5, -5, 10, 10);
            var result = testRectangle.Scale(0.5f);
            var expected = new MRectangleInt(-3, -3, 5, 5);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Scale_Growing_Correct()
        {
            var testRectangle = new MRectangleInt(0, 0, 10, 10);
            var result = testRectangle.Scale(2f);
            var expected = new MRectangleInt(-5, -5, 20, 20);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Scale_Shrinking_Correct()
        {
            var testRectangle = new MRectangleInt(0, 0, 10, 10);
            var result = testRectangle.Scale(0.5f);
            var expected = new MRectangleInt(2, 2, 5, 5);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Scale_Uniform()
        {
            var testRectangle = new MRectangleInt(0, 0, 10, 15);
            var result = testRectangle.Scale(2f);
            var expected = new MRectangleInt(-5, -7, 20, 30);
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(0, 0, 100, 100, 0, 0, 100, 100, DisplayName = "Self intersect")]
        [DataRow(0, 0, 100, 100, 10, 10, 50, 50, DisplayName = "A contains B")]
        [DataRow(10, 10, 50, 50, 0, 0, 100, 100, DisplayName = "B contains A")]
        [DataRow(-50, -50, 75, 75, 0, 0, 100, 100, DisplayName = "A intersects top-left B")]
        [DataRow(25, -25, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects top B")]
        [DataRow(75, -25, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects top-right B")]
        [DataRow(75, 25, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects right B")]
        [DataRow(75, 75, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects bottom-right B")]
        [DataRow(25, 75, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects bottom B")]
        [DataRow(-25, 75, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects bottom-left B")]
        [DataRow(-25, 25, 50, 50, 0, 0, 100, 100, DisplayName = "A intersects left B")]
        [DataRow(-25, -25, 50, 200, 0, 0, 100, 100, DisplayName = "A covers left B")]
        [DataRow(-25, -25, 200, 50, 0, 0, 100, 100, DisplayName = "A covers top B")]
        [DataRow(-25, -25, 25, 25, 0, 0, 100, 100, DisplayName = "A touches B")]
        public void Intersects_TrueAndCommutative(int ax, int ay, int aw, int ah, int bx, int by, int bw, int bh)
        {
            var rectangleA = new MRectangleInt(ax, ay, aw, ah);
            var rectangleB = new MRectangleInt(bx, by, bw, bh);
            Assert.IsTrue(rectangleA.Intersects(rectangleB), "A does not intersect B");
            Assert.IsTrue(rectangleB.Intersects(rectangleA), "B does not intersect A");
        }

        [TestMethod]
        public void Intersects_FalseAndCommutative()
        {
            var rectangleA = new MRectangleInt(0, 0, 25, 25);
            var rectangleB = new MRectangleInt(-25, -25, 15, 15);
            Assert.IsFalse(rectangleA.Intersects(rectangleB), "A does intersect B");
            Assert.IsFalse(rectangleB.Intersects(rectangleA), "B does intersect A");
        }
    }
}