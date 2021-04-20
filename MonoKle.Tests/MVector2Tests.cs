using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MonoKle.Tests
{
    [TestClass]
    public class MVector2Tests
    {
        [TestMethod]
        public void ImplicitOperatorMPoint2_CorrectlyAssigned()
        {
            int x = 27;
            int y = -11;
            MVector2 sut = new MPoint2(x, y);
            Assert.AreEqual(new MVector2(x, y), sut);
        }

        [TestMethod]
        public void Parse_TryParse_Equal()
        {
            string s = nameof(MVector2) + "(-15.2, 0.1)";
            var v2 = MVector2.Parse(s);
            MVector2 v3 = MVector2.Zero;
            MVector2.TryParse(s, out v3);
            Assert.AreEqual(v2, v3);
        }

        [TestMethod]
        public void Parsing_Spaces_StillWorks()
        {
            string s = nameof(MVector2) + "  (  -15.2  ,   0.1  )  ";
            var v = new MVector2(-15.2f, 0.1f);
            Assert.AreEqual(v, MVector2.Parse(s));
        }

        [TestMethod]
        public void Parsing_ToString_Parse_Equal()
        {
            var v = new MVector2(-5.3f, 17f);
            var v2 = MVector2.Parse(v.ToString());
            Assert.AreEqual(v, v2);
        }

        [TestMethod]
        public void TestClosestPoint()
        {
            float distance;
            var point = new MVector2(-5, 5);
            var points = new MVector2[] {
                new MVector2(100, 100),
                new MVector2(0,0),
                new MVector2(-1,3),
                new MVector2(-101, -100),
                new MVector2(float.MaxValue, float.MaxValue),
                new MVector2(float.MinValue, float.MinValue)
            };
            var expected = new MVector2(-1, 3);
            Assert.AreEqual(expected, point.ClosestPoint(points));
            Assert.AreEqual(point.ClosestPoint(points), point.ClosestPoint(points, out distance));
            Assert.AreEqual((float)(point - expected).Length, distance);
        }

        [TestMethod]
        public void UnaryOperatorMinus_Correct() => Assert.AreEqual(new MVector2(1, -3), -new MVector2(-1, 3));

        [TestMethod]
        public void MemberwiseProduct_Correct() => Assert.AreEqual(new MVector2(2, -6), new MVector2(1, -2) * new MVector2(2, 3));

        [TestMethod]
        public void MemberwiseDivision_Correct() => Assert.AreEqual(new MVector2(5, -2), new MVector2(10, -6) / new MVector2(2, 3));

        [TestMethod]
        public void MemberwiseAddition_Correct() => Assert.AreEqual(new MVector2(3, -6), new MVector2(1, 5) + new MVector2(2, -11));

        [TestMethod]
        public void MemberwiseSubtraction_LikeNegativeAddition()
        {
            MVector2 a = new MVector2(10, -7);
            MVector2 b = new MVector2(-3, 28);
            Assert.AreEqual(a + -b, a - b);
        }
    }
}
