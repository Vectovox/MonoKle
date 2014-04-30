﻿namespace MonoKle.Core
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xna.Framework;
    using System;

    [TestClass]
    public class AreaIntegerTest
    {
        [TestMethod]
        public void TestConstructors()
        {
            // Test that all constructors work the same
            AreaInteger a = new AreaInteger(-5, -5);
            AreaInteger b = new AreaInteger(new Vector2DInteger(0, 0), new Vector2DInteger(-5, -5));
            AreaInteger c = new AreaInteger(0, 0, -5, -5);
            AreaInteger d = new AreaInteger(new Vector2DInteger(-5, -5));

            Assert.AreEqual(a, b);
            Assert.AreEqual(b, c);
            Assert.AreEqual(c, d);

            // Test negative size
            Assert.AreEqual(new AreaInteger(0, 0, 10, 10), new AreaInteger(10, 10, -10, -10));
            Assert.AreEqual(new AreaInteger(-10, -10, 10, 10), new AreaInteger(0, 0, -10, -10));
            Assert.AreEqual(new AreaInteger(-30, -30, 20, 20), new AreaInteger(-10, -10, -20, -20));
        }

        [TestMethod]
        public void TestContainsCoordinateFloat()
        {
            Assert.IsTrue(new AreaInteger(50, 51, 50, 50).Contains(new Vector2(50, 51)));
            Assert.IsTrue(new AreaInteger(50, 51, 50, 50).Contains(new Vector2(100, 101)));
            Assert.IsTrue(new AreaInteger(50, 51, 50, 50).Contains(new Vector2(75, 75)));

            Assert.IsTrue(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2(-25, -25)));
            Assert.IsTrue(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2(-50, -51)));
            Assert.IsTrue(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2(0, -1)));

            Assert.IsTrue(new AreaInteger(-5, -5, 10, 10).Contains(new Vector2(0, 0)));
            Assert.IsTrue(new AreaInteger(-5, -5, 10, 10).Contains(new Vector2(0, 1)));
            Assert.IsTrue(new AreaInteger(-5, -5, 10, 10).Contains(new Vector2(1, 0)));
            Assert.IsTrue(new AreaInteger(-5, -5, 10, 10).Contains(new Vector2(1, 1)));

            Assert.IsFalse(new AreaInteger(50, 51, 50, 50).Contains(new Vector2(49, 51)));
            Assert.IsFalse(new AreaInteger(50, 51, 50, 50).Contains(new Vector2(50, 50)));
            Assert.IsFalse(new AreaInteger(50, 51, 50, 50).Contains(new Vector2(100, 102)));
            Assert.IsFalse(new AreaInteger(50, 51, 50, 50).Contains(new Vector2(101, 101)));
            Assert.IsFalse(new AreaInteger(50, 51, 50, 50).Contains(new Vector2(-75, -75)));

            Assert.IsFalse(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2(-50, -52)));
            Assert.IsFalse(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2(-51, -51)));
            Assert.IsFalse(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2(0, 0)));
            Assert.IsFalse(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2(1, -1)));
        }

        [TestMethod]
        public void TestContainsCoordinateInt()
        {
            Assert.IsTrue(new AreaInteger(50, 51, 50, 50).Contains(new Vector2DInteger(50, 51)));
            Assert.IsTrue(new AreaInteger(50, 51, 50, 50).Contains(new Vector2DInteger(100, 101)));
            Assert.IsTrue(new AreaInteger(50, 51, 50, 50).Contains(new Vector2DInteger(75, 75)));

            Assert.IsTrue(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2DInteger(-25, -25)));
            Assert.IsTrue(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2DInteger(-50, -51)));
            Assert.IsTrue(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2DInteger(0, -1)));

            Assert.IsTrue(new AreaInteger(-5, -5, 10, 10).Contains(new Vector2DInteger(0, 0)));
            Assert.IsTrue(new AreaInteger(-5, -5, 10, 10).Contains(new Vector2DInteger(0, 1)));
            Assert.IsTrue(new AreaInteger(-5, -5, 10, 10).Contains(new Vector2DInteger(1, 0)));
            Assert.IsTrue(new AreaInteger(-5, -5, 10, 10).Contains(new Vector2DInteger(1, 1)));

            Assert.IsFalse(new AreaInteger(50, 51, 50, 50).Contains(new Vector2DInteger(49, 51)));
            Assert.IsFalse(new AreaInteger(50, 51, 50, 50).Contains(new Vector2DInteger(50, 50)));
            Assert.IsFalse(new AreaInteger(50, 51, 50, 50).Contains(new Vector2DInteger(100, 102)));
            Assert.IsFalse(new AreaInteger(50, 51, 50, 50).Contains(new Vector2DInteger(101, 101)));
            Assert.IsFalse(new AreaInteger(50, 51, 50, 50).Contains(new Vector2DInteger(-75, -75)));

            Assert.IsFalse(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2DInteger(-50, -52)));
            Assert.IsFalse(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2DInteger(-51, -51)));
            Assert.IsFalse(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2DInteger(0, 0)));
            Assert.IsFalse(new AreaInteger(-50, -51, 50, 50).Contains(new Vector2DInteger(1, -1)));
        }

        [TestMethod]
        public void TestContainsArea()
        {
            Assert.IsTrue(new AreaInteger(50, 51, 50, 50).Contains(new Area(50, 51, 50, 50)));
            Assert.IsTrue(new AreaInteger(50, 51, 50, 50).Contains(new Area(60, 60, 20, 20)));
            Assert.IsTrue(new AreaInteger(-10, -10, 20, 20).Contains(new Area(-5, -5, 10, 10)));

            Assert.IsFalse(new AreaInteger(-10, -10, 20, 20).Contains(new Area(-5, -5, 30, 10)));
            Assert.IsFalse(new AreaInteger(-10, -10, 20, 20).Contains(new Area(-5, -5, 10, 30)));
            Assert.IsFalse(new AreaInteger(-10, -10, 20, 20).Contains(new Area(-11, -5, 10, 10)));
            Assert.IsFalse(new AreaInteger(-10, -10, 20, 20).Contains(new Area(-5, -11, 10, 10)));
            Assert.IsFalse(new AreaInteger(-10, -10, 20, 20).Contains(new Area(50, 50, 10, 10)));
        }

        [TestMethod]
        public void TestContainsAreaInt()
        {
            Assert.IsTrue(new AreaInteger(50, 51, 50, 50).Contains(new AreaInteger(50, 51, 50, 50)));
            Assert.IsTrue(new AreaInteger(50, 51, 50, 50).Contains(new AreaInteger(60, 60, 20, 20)));
            Assert.IsTrue(new AreaInteger(-10, -10, 20, 20).Contains(new AreaInteger(-5, -5, 10, 10)));

            Assert.IsFalse(new AreaInteger(-10, -10, 20, 20).Contains(new AreaInteger(-5, -5, 30, 10)));
            Assert.IsFalse(new AreaInteger(-10, -10, 20, 20).Contains(new AreaInteger(-5, -5, 10, 30)));
            Assert.IsFalse(new AreaInteger(-10, -10, 20, 20).Contains(new AreaInteger(-11, -5, 10, 10)));
            Assert.IsFalse(new AreaInteger(-10, -10, 20, 20).Contains(new AreaInteger(-5, -11, 10, 10)));
            Assert.IsFalse(new AreaInteger(-10, -10, 20, 20).Contains(new AreaInteger(50, 50, 10, 10)));
        }

        [TestMethod]
        public void TestClampArea()
        {
            Assert.AreEqual(new Area(-1, -1, 2, 2), new AreaInteger(-5, -5, 10, 10).Clamp(new Area(-1, -1, 2, 2)));
            Assert.AreEqual(new Area(-5, -5, 10, 10), new AreaInteger(-5, -5, 10, 10).Clamp(new Area(-5, -5, 10, 10)));
            Assert.AreEqual(new Area(-5, -5, 10, 10), new AreaInteger(-5, -5, 10, 10).Clamp(new Area(-10, -10, 20, 20)));

            Assert.AreEqual(new Area(-1, -1, 6, 2), new AreaInteger(-5, -5, 10, 10).Clamp(new Area(-1, -1, 10, 2)));
            Assert.AreEqual(new Area(-1, -1, 2, 6), new AreaInteger(-5, -5, 10, 10).Clamp(new Area(-1, -1, 2, 10)));
            Assert.AreEqual(new Area(-5, -1, 3, 5), new AreaInteger(-5, -5, 10, 10).Clamp(new Area(-7, -1, 5, 5)));
            Assert.AreEqual(new Area(-1, -5, 5, 3), new AreaInteger(-5, -5, 10, 10).Clamp(new Area(-1, -7, 5, 5)));
        }

        [TestMethod]
        public void TestClampAreaInteger()
        {
            Assert.AreEqual(new AreaInteger(-1, -1, 2, 2), new AreaInteger(-5, -5, 10, 10).Clamp(new AreaInteger(-1, -1, 2, 2)));
            Assert.AreEqual(new AreaInteger(-5, -5, 10, 10), new AreaInteger(-5, -5, 10, 10).Clamp(new AreaInteger(-5, -5, 10, 10)));
            Assert.AreEqual(new AreaInteger(-5, -5, 10, 10), new AreaInteger(-5, -5, 10, 10).Clamp(new AreaInteger(-10, -10, 20, 20)));

            Assert.AreEqual(new AreaInteger(-1, -1, 6, 2), new AreaInteger(-5, -5, 10, 10).Clamp(new AreaInteger(-1, -1, 10, 2)));
            Assert.AreEqual(new AreaInteger(-1, -1, 2, 6), new AreaInteger(-5, -5, 10, 10).Clamp(new AreaInteger(-1, -1, 2, 10)));
            Assert.AreEqual(new AreaInteger(-5, -1, 3, 5), new AreaInteger(-5, -5, 10, 10).Clamp(new AreaInteger(-7, -1, 5, 5)));
            Assert.AreEqual(new AreaInteger(-1, -5, 5, 3), new AreaInteger(-5, -5, 10, 10).Clamp(new AreaInteger(-1, -7, 5, 5)));
        }

        [TestMethod]
        public void TestClampCoordinate()
        {
            Assert.AreEqual(new Vector2(0, 0), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2(0, 0)));
            Assert.AreEqual(new Vector2(-5, -5), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2(-5, -5)));
            Assert.AreEqual(new Vector2(5, 5), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2(5, 5)));

            Assert.AreEqual(new Vector2(0, 5), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2(0, 7)));
            Assert.AreEqual(new Vector2(5, 0), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2(7, 0)));
            Assert.AreEqual(new Vector2(0, -5), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2(0, -7)));
            Assert.AreEqual(new Vector2(-5, 0), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2(-7, 0)));
        }

        [TestMethod]
        public void TestClampCoordinateInteger()
        {
            Assert.AreEqual(new Vector2DInteger(0, 0), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2DInteger(0, 0)));
            Assert.AreEqual(new Vector2DInteger(-5, -5), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2DInteger(-5, -5)));
            Assert.AreEqual(new Vector2DInteger(5, 5), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2DInteger(5, 5)));

            Assert.AreEqual(new Vector2DInteger(0, 5), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2DInteger(0, 7)));
            Assert.AreEqual(new Vector2DInteger(5, 0), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2DInteger(7, 0)));
            Assert.AreEqual(new Vector2DInteger(0, -5), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2DInteger(0, -7)));
            Assert.AreEqual(new Vector2DInteger(-5, 0), new AreaInteger(-5, -5, 10, 10).Clamp(new Vector2DInteger(-7, 0)));
        }

        [TestMethod]
        public void TestEqualsObject()
        {
            Assert.IsTrue(new AreaInteger(0, 1, 2, 3).Equals((object)new AreaInteger(0, 1, 2, 3)));
            Assert.IsFalse(new AreaInteger(0, 1, -2, 3).Equals((object)new AreaInteger(0, 1, 2, 3)));
            Assert.IsFalse(new AreaInteger(0, 1, 2, 3).Equals((object)new AreaInteger(1, 1, 2, 3)));
        }

        [TestMethod]
        public void TestEqualsAreaInteger()
        {
            Assert.IsTrue(new AreaInteger(0, 1, 2, 3).Equals(new AreaInteger(0, 1, 2, 3)));
            Assert.IsFalse(new AreaInteger(0, 1, -2, 3).Equals(new AreaInteger(0, 1, 2, 3)));
            Assert.IsFalse(new AreaInteger(0, 1, 2, 3).Equals(new AreaInteger(1, 1, 2, 3)));
        }

        [TestMethod]
        public void TestGetBottom()
        {
            Assert.AreEqual(6f, new AreaInteger(1, 2, 3, 4).Bottom);
            Assert.AreEqual(2f, new AreaInteger(-1, -2, 3, 4).Bottom);
            Assert.AreEqual(-2f, new AreaInteger(-1, -2, -3, -4).Bottom);
        }

        [TestMethod]
        public void TestGetBottomLeft()
        {
            Assert.AreEqual(new Vector2DInteger(1, 6), new AreaInteger(1, 2, 3, 4).BottomLeft);
            Assert.AreEqual(new Vector2DInteger(-1, 2), new AreaInteger(-1, -2, 3, 4).BottomLeft);
            Assert.AreEqual(new Vector2DInteger(-4, -2), new AreaInteger(-1, -2, -3, -4).BottomLeft);
        }

        [TestMethod]
        public void TestGetBottomRight()
        {
            Assert.AreEqual(new Vector2DInteger(4, 6), new AreaInteger(1, 2, 3, 4).BottomRight);
            Assert.AreEqual(new Vector2DInteger(2, 2), new AreaInteger(-1, -2, 3, 4).BottomRight);
            Assert.AreEqual(new Vector2DInteger(-1, -2), new AreaInteger(-1, -2, -3, -4).BottomRight);
        }

        [TestMethod]
        public void TestGetLeft()
        {
            Assert.AreEqual(1f, new AreaInteger(1, 2, 3, 4).Left);
            Assert.AreEqual(-1f, new AreaInteger(-1, -2, 3, 4).Left);
            Assert.AreEqual(-4f, new AreaInteger(-1, -2, -3, -4).Left);
        }

        [TestMethod]
        public void TestGetRight()
        {
            Assert.AreEqual(4f, new AreaInteger(1, 2, 3, 4).Right);
            Assert.AreEqual(2f, new AreaInteger(-1, -2, 3, 4).Right);
            Assert.AreEqual(-1f, new AreaInteger(-1, -2, -3, -4).Right);
        }

        [TestMethod]
        public void TestGetTop()
        {
            Assert.AreEqual(2f, new AreaInteger(1, 2, 3, 4).Top);
            Assert.AreEqual(-2f, new AreaInteger(-1, -2, 3, 4).Top);
            Assert.AreEqual(-6f, new AreaInteger(-1, -2, -3, -4).Top);
        }

        [TestMethod]
        public void TestGetTopLeft()
        {
            Assert.AreEqual(new Vector2DInteger(1, 2), new AreaInteger(1, 2, 3, 4).TopLeft);
            Assert.AreEqual(new Vector2DInteger(-1, -2), new AreaInteger(-1, -2, 3, 4).TopLeft);
            Assert.AreEqual(new Vector2DInteger(-4, -6), new AreaInteger(-1, -2, -3, -4).TopLeft);
        }

        [TestMethod]
        public void TestGetTopRight()
        {
            Assert.AreEqual(new Vector2DInteger(4, 2), new AreaInteger(1, 2, 3, 4).TopRight);
            Assert.AreEqual(new Vector2DInteger(2, -2), new AreaInteger(-1, -2, 3, 4).TopRight);
            Assert.AreEqual(new Vector2DInteger(-1, -6), new AreaInteger(-1, -2, -3, -4).TopRight);
        }

        [TestMethod]
        public void TestOperatorEquals()
        {
            Assert.IsTrue(new AreaInteger(0, 1, 2, 3) == new AreaInteger(0, 1, 2, 3));
            Assert.IsFalse(new AreaInteger(0, 1, -2, 3) == new AreaInteger(0, 1, 2, 3));
            Assert.IsFalse(new AreaInteger(0, 1, 2, 3) == new AreaInteger(1, 1, 2, 3));
        }

        [TestMethod]
        public void TestOperatorNotEquals()
        {
            Assert.IsFalse(new AreaInteger(0, 1, 2, 3) != new AreaInteger(0, 1, 2, 3));
            Assert.IsTrue(new AreaInteger(0, 1, -2, 3) != new AreaInteger(0, 1, 2, 3));
            Assert.IsTrue(new AreaInteger(0, 1, 2, 3) != new AreaInteger(1, 1, 2, 3));
        }
    }
}