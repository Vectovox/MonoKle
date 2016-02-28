//namespace MonoKle
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;
//    using System.Threading.Tasks;

        //[Serializable()]
//    public struct MVector3
//    {
//        // TODO: Fill in

//        ///// <summary>
//        ///// Calculates the point in a collection that is the closest to the provided point.
//        ///// </summary>
//        ///// <param name="point">The point.</param>
//        ///// <param name="compared">The compared points.</param>
//        ///// <returns>The closest point.</returns>
//        //public static Vector3 ClosestPoint3D(Vector3 point, IEnumerable<Vector3> compared)
//        //{
//        //    float distanceSqrd = float.MaxValue;
//        //    Vector3 closest = Vector3.Zero;
//        //    foreach (Vector3 v in compared)
//        //    {
//        //        float dx = point.X - v.X;
//        //        float dy = point.Y - v.Y;
//        //        float dz = point.Z - v.Z;
//        //        float d = dx * dx + dy * dy + dz * dz;
//        //        if (d < distanceSqrd)
//        //        {
//        //            distanceSqrd = d;
//        //            closest = v;
//        //        }
//        //    }
//        //    return closest;
//        //}

//        ///// <summary>
//        ///// Calculates the point in a collection that is the closest to the provided point, providing the distance as an out parameter.
//        ///// </summary>
//        ///// <param name="point">The point.</param>
//        ///// <param name="compared">The compared points.</param>
//        ///// <param name="distance">The distance to the closest point.</param>
//        ///// <returns>The closest point.</returns>
//        //public static Vector3 ClosestPoint3D(Vector3 point, IEnumerable<Vector3> compared, out float distance)
//        //{
//        //    Vector3 closest = ProximityHelper.ClosestPoint3D(point, compared);
//        //    float dx = point.X - closest.X;
//        //    float dy = point.Y - closest.Y;
//        //    float dz = point.Z - closest.Z;
//        //    distance = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
//        //    return closest;
//        //}

//        //[TestMethod]
//        //public void TestClosest3D()
//        //{
//        //    float distance;
//        //    Vector3 point = new Vector3(-5, 5, 5);
//        //    Vector3[] points = new Vector3[] {
//        //        new Vector3(100, 100, 27),
//        //        new Vector3(0, 0, 0),
//        //        new Vector3(-1, 3, 2),
//        //        new Vector3(-101, -100, 5),
//        //        new Vector3(float.MaxValue, float.MaxValue, float.MaxValue),
//        //        new Vector3(float.MinValue, float.MinValue, float.MinValue)
//        //    };
//        //    Vector3 expected = new Vector3(-1, 3, 2);
//        //    Assert.AreEqual(expected, ProximityHelper.ClosestPoint3D(point, points));
//        //    Assert.AreEqual(ProximityHelper.ClosestPoint3D(point, points), ProximityHelper.ClosestPoint3D(point, points, out distance));
//        //    Assert.AreEqual((float)(point - expected).Length(), distance);
//        //}
//    }
//}