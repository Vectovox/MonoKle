namespace MonoKle.Core.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xna.Framework;

    [TestClass]
    public class IntVector3Test
    {
        [TestMethod]
        public void TestConstructors()
        {
            int x = 27, y = -39, z = 12;
            IntVector3 v = new IntVector3(x, y, z);
            Assert.AreEqual(v.X, x);
            Assert.AreEqual(v.Y, y);
            Assert.AreEqual(v.Z, z);

            Vector3 xyz = new Vector3(-57.28f, 23f, 19.87f);
            IntVector3 v2 = new IntVector3(xyz);
            Assert.AreEqual(v2.X, (int)xyz.X);
            Assert.AreEqual(v2.Y, (int)xyz.Y);
            Assert.AreEqual(v2.Z, (int)xyz.Z);

            IntVector2 xy = new IntVector2(-6, 5);
            IntVector3 v3 = new IntVector3(xy, z);
            Assert.AreEqual(v3.X, xy.X);
            Assert.AreEqual(v3.Y, xy.Y);
            Assert.AreEqual(v3.Z, z);
        }

        [TestMethod]
        public void TestEquals()
        {
            IntVector3 a = new IntVector3(5, 7, -1);
            IntVector3 b = new IntVector3(5, 7, -1);
            IntVector3 c = new IntVector3(5, 7, -2);
            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
        }

        [TestMethod]
        public void TestLength()
        {
            int x = 3, y = 7, z = -5;
            Assert.AreEqual(Math.Abs(x), new IntVector3(x, 0, 0).Length());
            Assert.AreEqual(Math.Abs(y), new IntVector3(0, y, 0).Length());
            Assert.AreEqual(Math.Abs(z), new IntVector3(0, 0, z).Length());
            Assert.AreEqual(Math.Sqrt(x * x + y * y + z * z), new IntVector3(x, y, z).Length());
        }

        [TestMethod]
        public void TestLengthSquared()
        {
            IntVector3 v = new IntVector3(23, -19, 7);
            Assert.AreEqual(v.Length(), Math.Sqrt(v.LengthSquared()));
        }

        [TestMethod]
        public void TestOne()
        {
            Assert.AreEqual(IntVector3.One, new IntVector3(1, 1, 1));
        }

        [TestMethod]
        public void TestOperatorDivide()
        {
            Assert.AreEqual(new IntVector3(3, 2, 5), new IntVector3(15, 10, 25) / 5);
        }

        [TestMethod]
        public void TestOperatorEquals()
        {
            Assert.IsTrue(new IntVector3(3, 2, -8) == new IntVector3(3, 2, -8));
        }

        [TestMethod]
        public void TestOperatorMinus()
        {
            Assert.AreEqual(new IntVector3(-1, 10, 4), new IntVector3(1, 3, 0) - new IntVector3(2, -7, -4));
        }

        [TestMethod]
        public void TestOperatorMultiply()
        {
            Assert.AreEqual(new IntVector3(5, 15, 10), new IntVector3(1, 3, 2) * 5);
            Assert.AreEqual(new IntVector3(-5, 3, 7) * 5, 5 * new IntVector3(-5, 3, 7));
        }

        [TestMethod]
        public void TestOperatorNotEquals()
        {
            Assert.IsTrue(new IntVector3(1, 3, -19) != new IntVector3(1, 3, 19));
        }

        [TestMethod]
        public void TestOperatorPlus()
        {
            Assert.AreEqual(new IntVector3(3, -4, 0), new IntVector3(1, 3, 17) + new IntVector3(2, -7, -17));
        }

        [TestMethod]
        public void TestToVector3()
        {
            int x = 23, y = -19, z = -3;
            IntVector3 v = new IntVector3(x, y, z);
            Assert.AreEqual(new Vector3(x, y, z), v.ToVector3());
        }

        [TestMethod]
        public void TestZero()
        {
            Assert.AreEqual(IntVector3.Zero, new IntVector3(0, 0, 0));
        }

        [TestMethod]
        public void TestTranslate()
        {
            IntVector3 orig = new IntVector3(-2, -3, -4);
            Assert.AreEqual(orig, new IntVector3(2, 3, 4).Translate(-4, -6, -8));
            Assert.AreEqual(orig.Translate(new IntVector3(1, -2, 3)), orig.Translate(1, -2, 3));
        }
    }
}