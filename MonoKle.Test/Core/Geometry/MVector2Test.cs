namespace MonoKle.Core.Geometry
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MVector2Test
    {
        [TestMethod]
        public void TestClosestPoint()
        {
            float distance;
            MVector2 point = new MVector2(-5, 5);
            MVector2[] points = new MVector2[] {
                new MVector2(100, 100),
                new MVector2(0,0),
                new MVector2(-1,3),
                new MVector2(-101, -100),
                new MVector2(float.MaxValue, float.MaxValue),
                new MVector2(float.MinValue, float.MinValue)
            };
            MVector2 expected = new MVector2(-1, 3);
            Assert.AreEqual(expected, point.ClosestPoint(points));
            Assert.AreEqual(point.ClosestPoint(points), point.ClosestPoint(points, out distance));
            Assert.AreEqual((float)(point - expected).Length(), distance);
        }
    }
}