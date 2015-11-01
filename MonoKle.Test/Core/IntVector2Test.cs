namespace MonoKle.Core.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xna.Framework;

    using MonoKle.Core;

    [TestClass]
    public class IntVector2Test
    {
        [TestMethod]
        public void TestConstructors()
        {
            int x = 27, y = -39;
            IntVector2 v = new IntVector2(x, y);
            Assert.AreEqual(v.X, x);
            Assert.AreEqual(v.Y, y);

            Vector2 xy = new Vector2(-57.28f, 23f);
            IntVector2 v2 = new IntVector2(xy);
            Assert.AreEqual(v2.X, (int)xy.X);
            Assert.AreEqual(v2.Y, (int)xy.Y);
        }

        [TestMethod]
        public void TestEqualsObject()
        {
            IntVector2 a = new IntVector2(5, 7);
            IntVector2 b = new IntVector2(5, 7);
            IntVector2 c = new IntVector2(4, 7);
            Assert.IsTrue(a.Equals((object)b));
            Assert.IsFalse(a.Equals((object)c));
        }

        [TestMethod]
        public void TestEqualsVector()
        {
            IntVector2 a = new IntVector2(5, 7);
            IntVector2 b = new IntVector2(5, 7);
            IntVector2 c = new IntVector2(4, 7);
            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
        }

        [TestMethod]
        public void TestLength()
        {
            int x = 3, y = -7;
            Assert.AreEqual(Math.Abs(x), new IntVector2(x, 0).Length());
            Assert.AreEqual(Math.Abs(y), new IntVector2(0, y).Length());
            Assert.AreEqual(Math.Sqrt(x * x + y * y), new IntVector2(x, y).Length());
        }

        [TestMethod]
        public void TestLengthSquared()
        {
            IntVector2 v = new IntVector2(23, -19);
            Assert.AreEqual(v.Length(), Math.Sqrt(v.LengthSquared()));
        }

        [TestMethod]
        public void TestOne()
        {
            Assert.AreEqual(IntVector2.One, new IntVector2(1, 1));
        }

        [TestMethod]
        public void TestOperatorDivide()
        {
            Assert.AreEqual(new IntVector2(3, 2), new IntVector2(15, 10) / 5);
        }

        [TestMethod]
        public void TestOperatorEquals()
        {
            Assert.IsTrue(new IntVector2(3, 2) == new IntVector2(3, 2));
        }

        [TestMethod]
        public void TestOperatorMinus()
        {
            Assert.AreEqual(new IntVector2(-1, 10), new IntVector2(1, 3) - new IntVector2(2, -7));
        }

        [TestMethod]
        public void TestOperatorMultiply()
        {
            Assert.AreEqual(new IntVector2(5, 15), new IntVector2(1, 3) * 5);
            Assert.AreEqual(new IntVector2(1, 3) * 5, 5 * new IntVector2(1, 3));
        }

        [TestMethod]
        public void TestOperatorNotEquals()
        {
            Assert.IsTrue(new IntVector2(1, 3) != new IntVector2(1, 4));
        }

        [TestMethod]
        public void TestOperatorPlus()
        {
            Assert.AreEqual(new IntVector2(3, -4), new IntVector2(1, 3) + new IntVector2(2, -7));
        }

        [TestMethod]
        public void TestToVector2()
        {
            int x = 23, y = -19;
            IntVector2 v = new IntVector2(x, y);
            Assert.AreEqual(new Vector2(x, y), v.ToVector2());
        }

        [TestMethod]
        public void TestZero()
        {
            Assert.AreEqual(IntVector2.Zero, new IntVector2(0, 0));
        }

        [TestMethod]
        public void TestTranslate()
        {
            IntVector2 orig = new IntVector2(-2, -3);
            Assert.AreEqual(orig, new IntVector2(2, 3).Translate(-4, -6));
            Assert.AreEqual(orig.Translate(new IntVector2(1, -2)), orig.Translate(1, -2));
        }
    }
}