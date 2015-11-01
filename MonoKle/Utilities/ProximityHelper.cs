namespace MonoKle.Utilities
{
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Helper for calculating closest values.
    /// </summary>
    public static class ProximityHelper
    {
        /// <summary>
        /// Calculates the point in a collection that is the closest to the provided point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="compared">The compared points.</param>
        /// <returns>The closest point.</returns>
        public static Vector2 ClosestPoint2D(Vector2 point, IEnumerable<Vector2> compared)
        {
            float distanceSqrd = float.MaxValue;
            Vector2 closest = Vector2.Zero;
            foreach (Vector2 v in compared)
            {
                float dx = point.X - v.X;
                float dy = point.Y - v.Y;
                float d = dx * dx + dy * dy;
                if (d < distanceSqrd)
                {
                    distanceSqrd = d;
                    closest = v;
                }
            }
            return closest;
        }

        /// <summary>
        /// Calculates the point in a collection that is the closest to the provided point, providing the distance as an out parameter.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="compared">The compared points.</param>
        /// <param name="distance">The distance to the closest point.</param>
        /// <returns>The closest point.</returns>
        public static Vector2 ClosestPoint2D(Vector2 point, IEnumerable<Vector2> compared, out float distance)
        {
            Vector2 closest = ProximityHelper.ClosestPoint2D(point, compared);
            float dx = point.X - closest.X;
            float dy = point.Y - closest.Y;
            distance = (float)Math.Sqrt(dx * dx + dy * dy);
            return closest;
        }

        /// <summary>
        /// Calculates the point in a collection that is the closest to the provided point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="compared">The compared points.</param>
        /// <returns>The closest point.</returns>
        public static Vector3 ClosestPoint3D(Vector3 point, IEnumerable<Vector3> compared)
        {
            float distanceSqrd = float.MaxValue;
            Vector3 closest = Vector3.Zero;
            foreach (Vector3 v in compared)
            {
                float dx = point.X - v.X;
                float dy = point.Y - v.Y;
                float dz = point.Z - v.Z;
                float d = dx * dx + dy * dy + dz * dz;
                if (d < distanceSqrd)
                {
                    distanceSqrd = d;
                    closest = v;
                }
            }
            return closest;
        }

        /// <summary>
        /// Calculates the point in a collection that is the closest to the provided point, providing the distance as an out parameter.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="compared">The compared points.</param>
        /// <param name="distance">The distance to the closest point.</param>
        /// <returns>The closest point.</returns>
        public static Vector3 ClosestPoint3D(Vector3 point, IEnumerable<Vector3> compared, out float distance)
        {
            Vector3 closest = ProximityHelper.ClosestPoint3D(point, compared);
            float dx = point.X - closest.X;
            float dy = point.Y - closest.Y;
            float dz = point.Z - closest.Z;
            distance = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
            return closest;
        }
    }
}