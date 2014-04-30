namespace MonoKle.Core.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xna.Framework;

    [TestClass]
    public class Vector3DIntegerTest
    {
        [TestMethod]
        public void TestConstructors()
        {
            int x = 27, y = -39, z = 12;
            Vector3DInteger v = new Vector3DInteger(x, y, z);
            Assert.AreEqual(v.X, x);
            Assert.AreEqual(v.Y, y);
            Assert.AreEqual(v.Z, z);

            Vector3 xyz = new Vector3(-57.28f, 23f, 19.87f);
            Vector3DInteger v2 = new Vector3DInteger(xyz);
            Assert.AreEqual(v2.X, (int)xyz.X);
            Assert.AreEqual(v2.Y, (int)xyz.Y);
            Assert.AreEqual(v2.Z, (int)xyz.Z);

            Vector2DInteger xy = new Vector2DInteger(-6, 5);
            Vector3DInteger v3 = new Vector3DInteger(xy, z);
            Assert.AreEqual(v3.X, xy.X);
            Assert.AreEqual(v3.Y, xy.Y);
            Assert.AreEqual(v3.Z, z);
        }

        [TestMethod]
        public void TestEquals()
        {
            Vector3DInteger a = new Vector3DInteger(5, 7, -1);
            Vector3DInteger b = new Vector3DInteger(5, 7, -1);
            Vector3DInteger c = new Vector3DInteger(5, 7, -2);
            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
        }

        [TestMethod]
        public void TestLength()
        {
            int x = 3, y = 7, z = -5;
            Assert.AreEqual(Math.Abs(x), new Vector3DInteger(x, 0, 0).Length());
            Assert.AreEqual(Math.Abs(y), new Vector3DInteger(0, y, 0).Length());
            Assert.AreEqual(Math.Abs(z), new Vector3DInteger(0, 0, z).Length());
            Assert.AreEqual(Math.Sqrt(x * x + y * y + z * z), new Vector3DInteger(x, y, z).Length());
        }

        [TestMethod]
        public void TestLengthSquared()
        {
            Vector3DInteger v = new Vector3DInteger(23, -19, 7);
            Assert.AreEqual(v.Length(), Math.Sqrt(v.LengthSquared()));
        }

        [TestMethod]
        public void TestOne()
        {
            Assert.AreEqual(Vector3DInteger.One, new Vector3DInteger(1, 1, 1));
        }

        [TestMethod]
        public void TestOperatorDivide()
        {
            Assert.AreEqual(new Vector3DInteger(3, 2, 5), new Vector3DInteger(15, 10, 25) / 5);
        }

        [TestMethod]
        public void TestOperatorEquals()
        {
            Assert.IsTrue(new Vector3DInteger(3, 2, -8) == new Vector3DInteger(3, 2, -8));
        }

        [TestMethod]
        public void TestOperatorMinus()
        {
            Assert.AreEqual(new Vector3DInteger(-1, 10, 4), new Vector3DInteger(1, 3, 0) - new Vector3DInteger(2, -7, -4));
        }

        [TestMethod]
        public void TestOperatorMultiply()
        {
            Assert.AreEqual(new Vector3DInteger(5, 15, 10), new Vector3DInteger(1, 3, 2) * 5);
            Assert.AreEqual(new Vector3DInteger(-5, 3, 7) * 5, 5 * new Vector3DInteger(-5, 3, 7));
        }

        [TestMethod]
        public void TestOperatorNotEquals()
        {
            Assert.IsTrue(new Vector3DInteger(1, 3, -19) != new Vector3DInteger(1, 3, 19));
        }

        [TestMethod]
        public void TestOperatorPlus()
        {
            Assert.AreEqual(new Vector3DInteger(3, -4, 0), new Vector3DInteger(1, 3, 17) + new Vector3DInteger(2, -7, -17));
        }

        [TestMethod]
        public void TestToVector3()
        {
            int x = 23, y = -19, z = -3;
            Vector3DInteger v = new Vector3DInteger(x, y, z);
            Assert.AreEqual(new Vector3(x, y, z), v.ToVector3());
        }

        [TestMethod]
        public void TestZero()
        {
            Assert.AreEqual(Vector3DInteger.Zero, new Vector3DInteger(0, 0, 0));
        }
    }
}