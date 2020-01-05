namespace MonoKle
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MVector2Test
    {
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
    }
}
