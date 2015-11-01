namespace MonoKle.Utilities
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xna.Framework;

    [TestClass]
    public class ProximityHelperTest
    {
        [TestMethod]
        public void TestClosest2D()
        {
            float distance;
            Vector2 point = new Vector2(-5, 5);
            Vector2[] points = new Vector2[] {
                new Vector2(100, 100),
                new Vector2(0,0),
                new Vector2(-1,3),
                new Vector2(-101, -100),
                new Vector2(float.MaxValue, float.MaxValue),
                new Vector2(float.MinValue, float.MinValue)
            };
            Vector2 expected = new Vector2(-1, 3);
            Assert.AreEqual(expected, ProximityHelper.ClosestPoint2D(point, points));
            Assert.AreEqual(ProximityHelper.ClosestPoint2D(point, points), ProximityHelper.ClosestPoint2D(point, points, out distance));
            Assert.AreEqual((point - expected).Length(), distance);
        }

        [TestMethod]
        public void TestClosest3D()
        {
            float distance;
            Vector3 point = new Vector3(-5, 5, 5);
            Vector3[] points = new Vector3[] {
                new Vector3(100, 100, 27),
                new Vector3(0, 0, 0),
                new Vector3(-1, 3, 2),
                new Vector3(-101, -100, 5),
                new Vector3(float.MaxValue, float.MaxValue, float.MaxValue),
                new Vector3(float.MinValue, float.MinValue, float.MinValue)
            };
            Vector3 expected = new Vector3(-1, 3, 2);
            Assert.AreEqual(expected, ProximityHelper.ClosestPoint3D(point, points));
            Assert.AreEqual(ProximityHelper.ClosestPoint3D(point, points), ProximityHelper.ClosestPoint3D(point, points, out distance));
            Assert.AreEqual((point - expected).Length(), distance);
        }
    }
}