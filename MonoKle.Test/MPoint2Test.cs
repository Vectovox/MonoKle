namespace MonoKle
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xna.Framework;
    using System;

    [TestClass]
    public class MPoint2Test
    {
        [TestMethod]
        public void Parse_TryParse_Equal()
        {
            string s = nameof(MPoint2) + "(-15, 0)";
            var v2 = MPoint2.Parse(s);
            MPoint2 v3 = MPoint2.Zero;
            MPoint2.TryParse(s, out v3);
            Assert.AreEqual(v2, v3);
        }

        [TestMethod]
        public void Parsing_Spaces_StillWorks()
        {
            string s = nameof(MPoint2) + "  (  -15  ,   0  )  ";
            var v = new MPoint2(-15, 0);
            Assert.AreEqual(v, MPoint2.Parse(s));
        }

        [TestMethod]
        public void Parsing_ToString_Parse_Equal()
        {
            var v = new MPoint2(-5, 17);
            var v2 = MPoint2.Parse(v.ToString());
            Assert.AreEqual(v, v2);
        }

        [TestMethod]
        public void TestConstructors()
        {
            int x = 27, y = -39;
            var v = new MPoint2(x, y);
            Assert.AreEqual(v.X, x);
            Assert.AreEqual(v.Y, y);

            var xy = new Vector2(-57.28f, 23f);
            var v2 = new MPoint2(xy);
            Assert.AreEqual(v2.X, (int)xy.X);
            Assert.AreEqual(v2.Y, (int)xy.Y);
        }

        [TestMethod]
        public void TestEqualsObject()
        {
            var a = new MPoint2(5, 7);
            var b = new MPoint2(5, 7);
            var c = new MPoint2(4, 7);
            Assert.IsTrue(a.Equals((object)b));
            Assert.IsFalse(a.Equals((object)c));
        }

        [TestMethod]
        public void TestEqualsVector()
        {
            var a = new MPoint2(5, 7);
            var b = new MPoint2(5, 7);
            var c = new MPoint2(4, 7);
            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
        }

        [TestMethod]
        public void TestLength()
        {
            int x = 3, y = -7;
            Assert.AreEqual(Math.Abs(x), new MPoint2(x, 0).Length);
            Assert.AreEqual(Math.Abs(y), new MPoint2(0, y).Length);
            Assert.AreEqual(Math.Sqrt(x * x + y * y), new MPoint2(x, y).Length);
        }

        [TestMethod]
        public void TestLengthSquared()
        {
            var v = new MPoint2(23, -19);
            Assert.AreEqual(v.Length, Math.Sqrt(v.LengthSquared));
        }

        [TestMethod]
        public void TestOne() => Assert.AreEqual(MPoint2.One, new MPoint2(1, 1));

        [TestMethod]
        public void TestOperatorDivide() => Assert.AreEqual(new MPoint2(3, 2), new MPoint2(15, 10) / 5);

        [TestMethod]
        public void TestOperatorEquals() => Assert.IsTrue(new MPoint2(3, 2) == new MPoint2(3, 2));

        [TestMethod]
        public void TestOperatorMinus() => Assert.AreEqual(new MPoint2(-1, 10), new MPoint2(1, 3) - new MPoint2(2, -7));

        [TestMethod]
        public void TestOperatorMultiply()
        {
            Assert.AreEqual(new MPoint2(5, 15), new MPoint2(1, 3) * 5);
            Assert.AreEqual(new MPoint2(1, 3) * 5, 5 * new MPoint2(1, 3));
        }

        [TestMethod]
        public void TestOperatorNotEquals() => Assert.IsTrue(new MPoint2(1, 3) != new MPoint2(1, 4));

        [TestMethod]
        public void TestOperatorPlus() => Assert.AreEqual(new MPoint2(3, -4), new MPoint2(1, 3) + new MPoint2(2, -7));

        [TestMethod]
        public void TestToMVector2()
        {
            int x = 23, y = -19;
            var v = new MPoint2(x, y);
            Assert.AreEqual(new MVector2(x, y), v.ToMVector2());
        }

        [TestMethod]
        public void TestTranslate()
        {
            var orig = new MPoint2(-2, -3);
            Assert.AreEqual(orig, new MPoint2(2, 3).Translate(-4, -6));
            Assert.AreEqual(orig.Translate(new MPoint2(1, -2)), orig.Translate(1, -2));
        }

        [TestMethod]
        public void TestZero() => Assert.AreEqual(MPoint2.Zero, new MPoint2(0, 0));
    }
}
