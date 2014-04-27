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
        public void TestCropRectangle()
        {
            Rectangle bounds = new Rectangle(-5, -5, 10, 10);
            Assert.AreEqual(new Vector2Int32(-5, -5), new Vector2Int32(-10, -5).Crop(bounds));
            Assert.AreEqual(new Vector2Int32(-5, -5), new Vector2Int32(-5, -10).Crop(bounds));
            Assert.AreEqual(new Vector2Int32(5, -5), new Vector2Int32(10, -5).Crop(bounds));
            Assert.AreEqual(new Vector2Int32(-5, 5), new Vector2Int32(-5, 10).Crop(bounds));
            Assert.AreEqual(new Vector2Int32(0, 0), new Vector2Int32(0, 0).Crop(bounds));

            Rectangle nonNormalBounds = new Rectangle(5, 5, -10, -10);
            Assert.AreEqual(new Vector2Int32(-5, -5), new Vector2Int32(-10, -5).Crop(nonNormalBounds));
            Assert.AreEqual(new Vector2Int32(-5, -5), new Vector2Int32(-5, -10).Crop(nonNormalBounds));
            Assert.AreEqual(new Vector2Int32(5, -5), new Vector2Int32(10, -5).Crop(nonNormalBounds));
            Assert.AreEqual(new Vector2Int32(-5, 5), new Vector2Int32(-5, 10).Crop(nonNormalBounds));
            Assert.AreEqual(new Vector2Int32(0, 0), new Vector2Int32(0, 0).Crop(nonNormalBounds));
        }

        [TestMethod]
        public void TestCropVector()
        {
            Vector2Int32 bounds = new Vector2Int32(5, 5);
            Assert.AreEqual(new Vector2Int32(0, 0), new Vector2Int32(-5, 0).Crop(bounds));
            Assert.AreEqual(new Vector2Int32(0, 0), new Vector2Int32(0, -5).Crop(bounds));
            Assert.AreEqual(new Vector2Int32(5, 0), new Vector2Int32(10, 0).Crop(bounds));
            Assert.AreEqual(new Vector2Int32(0, 5), new Vector2Int32(0, 10).Crop(bounds));
            Assert.AreEqual(new Vector2Int32(2, 2), new Vector2Int32(2, 2).Crop(bounds));

            Vector2Int32 nonNormalBounds = new Vector2Int32(-5, -5);
            Assert.AreEqual(new Vector2Int32(0, 0), new Vector2Int32(5, 0).Crop(nonNormalBounds));
            Assert.AreEqual(new Vector2Int32(0, 0), new Vector2Int32(0, 5).Crop(nonNormalBounds));
            Assert.AreEqual(new Vector2Int32(-5, 0), new Vector2Int32(-10, 0).Crop(nonNormalBounds));
            Assert.AreEqual(new Vector2Int32(0, -5), new Vector2Int32(0, -10).Crop(nonNormalBounds));
            Assert.AreEqual(new Vector2Int32(-2, -2), new Vector2Int32(-2, -2).Crop(nonNormalBounds));
        }

        [TestMethod]
        public void TestCropVectorVector()
        {
            Vector2Int32 boundsA = new Vector2Int32(-5, -5);
            Vector2Int32 boundsB = new Vector2Int32(5, 5);
            Assert.AreEqual(new Vector2Int32(-5, -5), new Vector2Int32(-10, -5).Crop(boundsA, boundsB));
            Assert.AreEqual(new Vector2Int32(-5, -5), new Vector2Int32(-5, -10).Crop(boundsA, boundsB));
            Assert.AreEqual(new Vector2Int32(5, -5), new Vector2Int32(10, -5).Crop(boundsA, boundsB));
            Assert.AreEqual(new Vector2Int32(-5, 5), new Vector2Int32(-5, 10).Crop(boundsA, boundsB));
            Assert.AreEqual(new Vector2Int32(0, 0), new Vector2Int32(0, 0).Crop(boundsA, boundsB));

            Vector2Int32 nonNormalBoundsA = new Vector2Int32(5, 5);
            Vector2Int32 nonNormalBoundsB = new Vector2Int32(-5, -5);
            Assert.AreEqual(new Vector2Int32(-5, -5), new Vector2Int32(-10, -5).Crop(nonNormalBoundsA, nonNormalBoundsB));
            Assert.AreEqual(new Vector2Int32(-5, -5), new Vector2Int32(-5, -10).Crop(nonNormalBoundsA, nonNormalBoundsB));
            Assert.AreEqual(new Vector2Int32(5, -5), new Vector2Int32(10, -5).Crop(nonNormalBoundsA, nonNormalBoundsB));
            Assert.AreEqual(new Vector2Int32(-5, 5), new Vector2Int32(-5, 10).Crop(nonNormalBoundsA, nonNormalBoundsB));
            Assert.AreEqual(new Vector2Int32(0, 0), new Vector2Int32(0, 0).Crop(nonNormalBoundsA, nonNormalBoundsB));
        }

        [TestMethod]
        public void TestEqualsObject()
        {
            Vector2Int32 a = new Vector2Int32(5, 7);
            Vector2Int32 b = new Vector2Int32(5, 7);
            Vector2Int32 c = new Vector2Int32(4, 7);
            Assert.IsTrue(a.Equals((object)b));
            Assert.IsFalse(a.Equals((object)c));
        }

        [TestMethod]
        public void TestEqualsVector()
        {
            Vector2Int32 a = new Vector2Int32(5, 7);
            Vector2Int32 b = new Vector2Int32(5, 7);
            Vector2Int32 c = new Vector2Int32(4, 7);
            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
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