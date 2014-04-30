namespace MonoKle.Core.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xna.Framework;

    using MonoKle.Core;

    [TestClass]
    public class Vector2DIntegerTest
    {
        [TestMethod]
        public void TestConstructors()
        {
            int x = 27, y = -39;
            Vector2DInteger v = new Vector2DInteger(x, y);
            Assert.AreEqual(v.X, x);
            Assert.AreEqual(v.Y, y);

            Vector2 xy = new Vector2(-57.28f, 23f);
            Vector2DInteger v2 = new Vector2DInteger(xy);
            Assert.AreEqual(v2.X, (int)xy.X);
            Assert.AreEqual(v2.Y, (int)xy.Y);
        }

        [TestMethod]
        public void TestEqualsObject()
        {
            Vector2DInteger a = new Vector2DInteger(5, 7);
            Vector2DInteger b = new Vector2DInteger(5, 7);
            Vector2DInteger c = new Vector2DInteger(4, 7);
            Assert.IsTrue(a.Equals((object)b));
            Assert.IsFalse(a.Equals((object)c));
        }

        [TestMethod]
        public void TestEqualsVector()
        {
            Vector2DInteger a = new Vector2DInteger(5, 7);
            Vector2DInteger b = new Vector2DInteger(5, 7);
            Vector2DInteger c = new Vector2DInteger(4, 7);
            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
        }

        [TestMethod]
        public void TestLength()
        {
            int x = 3, y = -7;
            Assert.AreEqual(Math.Abs(x), new Vector2DInteger(x, 0).Length());
            Assert.AreEqual(Math.Abs(y), new Vector2DInteger(0, y).Length());
            Assert.AreEqual(Math.Sqrt(x * x + y * y), new Vector2DInteger(x, y).Length());
        }

        [TestMethod]
        public void TestLengthSquared()
        {
            Vector2DInteger v = new Vector2DInteger(23, -19);
            Assert.AreEqual(v.Length(), Math.Sqrt(v.LengthSquared()));
        }

        [TestMethod]
        public void TestOne()
        {
            Assert.AreEqual(Vector2DInteger.One, new Vector2DInteger(1, 1));
        }

        [TestMethod]
        public void TestOperatorDivide()
        {
            Assert.AreEqual(new Vector2DInteger(3, 2), new Vector2DInteger(15, 10) / 5);
        }

        [TestMethod]
        public void TestOperatorEquals()
        {
            Assert.IsTrue(new Vector2DInteger(3, 2) == new Vector2DInteger(3, 2));
        }

        [TestMethod]
        public void TestOperatorMinus()
        {
            Assert.AreEqual(new Vector2DInteger(-1, 10), new Vector2DInteger(1, 3) - new Vector2DInteger(2, -7));
        }

        [TestMethod]
        public void TestOperatorMultiply()
        {
            Assert.AreEqual(new Vector2DInteger(5, 15), new Vector2DInteger(1, 3) * 5);
            Assert.AreEqual(new Vector2DInteger(1, 3) * 5, 5 * new Vector2DInteger(1, 3));
        }

        [TestMethod]
        public void TestOperatorNotEquals()
        {
            Assert.IsTrue(new Vector2DInteger(1, 3) != new Vector2DInteger(1, 4));
        }

        [TestMethod]
        public void TestOperatorPlus()
        {
            Assert.AreEqual(new Vector2DInteger(3, -4), new Vector2DInteger(1, 3) + new Vector2DInteger(2, -7));
        }

        [TestMethod]
        public void TestToVector2()
        {
            int x = 23, y = -19;
            Vector2DInteger v = new Vector2DInteger(x, y);
            Assert.AreEqual(new Vector2(x, y), v.ToVector2());
        }

        [TestMethod]
        public void TestZero()
        {
            Assert.AreEqual(Vector2DInteger.Zero, new Vector2DInteger(0, 0));
        }
    }
}