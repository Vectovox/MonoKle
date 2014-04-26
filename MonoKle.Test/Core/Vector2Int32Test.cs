namespace MonoKle.Core.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xna.Framework;

    using MonoKle.Core;

    [TestClass]
    public class Vector2Int32Test
    {
        [TestMethod]
        public void TestConstructors()
        {
            int x = 27, y = -39;
            Vector2Int32 v = new Vector2Int32(x, y);
            Assert.AreEqual(v.X, x);
            Assert.AreEqual(v.Y, y);

            Vector2 xy = new Vector2(-57.28f, 23f);
            Vector2Int32 v2 = new Vector2Int32(xy);
            Assert.AreEqual(v2.X, (int)xy.X);
            Assert.AreEqual(v2.Y, (int)xy.Y);
        }

        [TestMethod]
        public void TestEquals()
        {
            Vector2Int32 a = new Vector2Int32(5, 7);
            Vector2Int32 b = new Vector2Int32(5, 7);
            Vector2Int32 c = new Vector2Int32(4, 7);
            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
        }

        [TestMethod]
        public void TestIsWithin()
        {
            // Int vector
            Assert.IsTrue(new Vector2Int32(3, 2).IsWithin(new Vector2Int32(5, 7)));
            Assert.IsFalse(new Vector2Int32(3, 2).IsWithin(new Vector2Int32(1, 1)));
            Assert.IsTrue(new Vector2Int32(-3, -2).IsWithin(new Vector2Int32(-5, -7)));
            Assert.IsFalse(new Vector2Int32(-3, -2).IsWithin(new Vector2Int32(-1, -1)));
            Assert.IsFalse(new Vector2Int32(3, 2).IsWithin(new Vector2Int32(-5, -7)));
            Assert.IsTrue(new Vector2Int32(5, 7).IsWithin(new Vector2Int32(5, 7)));

            // Float vector
            Assert.IsTrue(new Vector2Int32(3, 2).IsWithin(new Vector2(5, 7)));
            Assert.IsFalse(new Vector2Int32(3, 2).IsWithin(new Vector2(1, 1)));
            Assert.IsTrue(new Vector2Int32(-3, -2).IsWithin(new Vector2(-5, -7)));
            Assert.IsFalse(new Vector2Int32(-3, -2).IsWithin(new Vector2(-1, -1)));
            Assert.IsFalse(new Vector2Int32(3, 2).IsWithin(new Vector2(-5, -7)));
            Assert.IsTrue(new Vector2Int32(5, 7).IsWithin(new Vector2(5, 7)));

            // Int vectors
            Assert.IsTrue(new Vector2Int32(8, 8).IsWithin(new Vector2Int32(5, 7), new Vector2Int32(10, 10)));
            Assert.IsTrue(new Vector2Int32(-1, -1).IsWithin(new Vector2Int32(-5, -7), new Vector2Int32(10, 10)));
            Assert.IsTrue(new Vector2Int32(10, 10).IsWithin(new Vector2Int32(5, 7), new Vector2Int32(10, 10)));
            Assert.IsTrue(new Vector2Int32(-5, -7).IsWithin(new Vector2Int32(-5, -7), new Vector2Int32(10, 10)));
            Assert.IsTrue(new Vector2Int32(-5, 10).IsWithin(new Vector2Int32(-5, -7), new Vector2Int32(10, 10)));
            Assert.IsFalse(new Vector2Int32(-5, -8).IsWithin(new Vector2Int32(-5, -7), new Vector2Int32(10, 10)));
            Assert.IsFalse(new Vector2Int32(-5, 11).IsWithin(new Vector2Int32(-5, -7), new Vector2Int32(10, 10)));
            Assert.IsTrue(new Vector2Int32(8, 8).IsWithin(new Vector2Int32(10, 10), new Vector2Int32(5, 7)));
            Assert.IsTrue(new Vector2Int32(-8, -8).IsWithin(new Vector2Int32(-5, -7), new Vector2Int32(-10, -10)));

            // Float vectors
            Assert.IsTrue(new Vector2Int32(8, 8).IsWithin(new Vector2(5, 7), new Vector2(10, 10)));
            Assert.IsTrue(new Vector2Int32(-1, -1).IsWithin(new Vector2(-5, -7), new Vector2(10, 10)));
            Assert.IsTrue(new Vector2Int32(10, 10).IsWithin(new Vector2(5, 7), new Vector2(10, 10)));
            Assert.IsTrue(new Vector2Int32(-5, -7).IsWithin(new Vector2(-5, -7), new Vector2(10, 10)));
            Assert.IsTrue(new Vector2Int32(-5, 10).IsWithin(new Vector2(-5, -7), new Vector2(10, 10)));
            Assert.IsFalse(new Vector2Int32(-5, -8).IsWithin(new Vector2(-5, -7), new Vector2(10, 10)));
            Assert.IsFalse(new Vector2Int32(-5, 11).IsWithin(new Vector2(-5, -7), new Vector2(10, 10)));
            Assert.IsTrue(new Vector2Int32(8, 8).IsWithin(new Vector2(10, 10), new Vector2(5, 7)));
            Assert.IsTrue(new Vector2Int32(-8, -8).IsWithin(new Vector2(-5, -7), new Vector2(-10, -10)));

            // Int rectangle
            Assert.IsTrue(new Vector2Int32(8, 8).IsWithin(new Rectangle(5, 7, 5, 3)));
            Assert.IsTrue(new Vector2Int32(-1, -1).IsWithin(new Rectangle(-5, -7, 15, 17)));
            Assert.IsTrue(new Vector2Int32(10, 10).IsWithin(new Rectangle(5, 7, 5, 3)));
            Assert.IsTrue(new Vector2Int32(-5, -7).IsWithin(new Rectangle(-5, -7, 15, 17)));
            Assert.IsTrue(new Vector2Int32(-5, 10).IsWithin(new Rectangle(-5, -7, 15, 17)));
            Assert.IsFalse(new Vector2Int32(-5, -8).IsWithin(new Rectangle(-5, -7, 15, 17)));
            Assert.IsFalse(new Vector2Int32(-5, 11).IsWithin(new Rectangle(-5, -7, 15, 17)));
            Assert.IsTrue(new Vector2Int32(8, 8).IsWithin(new Rectangle(10, 10, -5, -3)));
            Assert.IsTrue(new Vector2Int32(-12, -11).IsWithin(new Rectangle(-10, -10, -5, -3)));

            // Float rectangle
            Assert.IsTrue(new Vector2Int32(8, 8).IsWithin(new RectangleSingle(5, 7, 5, 3)));
            Assert.IsTrue(new Vector2Int32(-1, -1).IsWithin(new RectangleSingle(-5, -7, 15, 17)));
            Assert.IsTrue(new Vector2Int32(10, 10).IsWithin(new RectangleSingle(5, 7, 5, 3)));
            Assert.IsTrue(new Vector2Int32(-5, -7).IsWithin(new RectangleSingle(-5, -7, 15, 17)));
            Assert.IsTrue(new Vector2Int32(-5, 10).IsWithin(new RectangleSingle(-5, -7, 15, 17)));
            Assert.IsFalse(new Vector2Int32(-5, -8).IsWithin(new RectangleSingle(-5, -7, 15, 17)));
            Assert.IsFalse(new Vector2Int32(-5, 11).IsWithin(new RectangleSingle(-5, -7, 15, 17)));
            Assert.IsTrue(new Vector2Int32(8, 8).IsWithin(new RectangleSingle(10, 10, -5, -3)));
            Assert.IsTrue(new Vector2Int32(-12, -11).IsWithin(new RectangleSingle(-10, -10, -5, -3)));
        }

        [TestMethod]
        public void TestLength()
        {
            int x = 3, y = -7;
            Assert.AreEqual(Math.Abs(x), new Vector2Int32(x, 0).Length());
            Assert.AreEqual(Math.Abs(y), new Vector2Int32(0, y).Length());
            Assert.AreEqual(Math.Sqrt(x * x + y * y), new Vector2Int32(x, y).Length());
        }

        [TestMethod]
        public void TestLengthSquared()
        {
            Vector2Int32 v = new Vector2Int32(23, -19);
            Assert.AreEqual(v.Length(), Math.Sqrt(v.LengthSquared()));
        }

        [TestMethod]
        public void TestOne()
        {
            Assert.AreEqual(Vector2Int32.One, new Vector2Int32(1, 1));
        }

        [TestMethod]
        public void TestOperatorDivide()
        {
            Assert.AreEqual(new Vector2Int32(3, 2), new Vector2Int32(15, 10) / 5);
        }

        [TestMethod]
        public void TestOperatorEquals()
        {
            Assert.IsTrue(new Vector2Int32(3, 2) == new Vector2Int32(3, 2));
        }

        [TestMethod]
        public void TestOperatorMinus()
        {
            Assert.AreEqual(new Vector2Int32(-1, 10), new Vector2Int32(1, 3) - new Vector2Int32(2, -7));
        }

        [TestMethod]
        public void TestOperatorMultiply()
        {
            Assert.AreEqual(new Vector2Int32(5, 15), new Vector2Int32(1, 3) * 5);
            Assert.AreEqual(new Vector2Int32(1, 3) * 5, 5 * new Vector2Int32(1, 3));
        }

        [TestMethod]
        public void TestOperatorNotEquals()
        {
            Assert.IsTrue(new Vector2Int32(1, 3) != new Vector2Int32(1, 4));
        }

        [TestMethod]
        public void TestOperatorPlus()
        {
            Assert.AreEqual(new Vector2Int32(3, -4), new Vector2Int32(1, 3) + new Vector2Int32(2, -7));
        }

        [TestMethod]
        public void TestToVector2()
        {
            int x = 23, y = -19;
            Vector2Int32 v = new Vector2Int32(x, y);
            Assert.AreEqual(new Vector2(x, y), v.ToVector2());
        }

        [TestMethod]
        public void TestZero()
        {
            Assert.AreEqual(Vector2Int32.Zero, new Vector2Int32(0, 0));
        }
    }
}