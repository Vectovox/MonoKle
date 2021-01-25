using System;

namespace MonoKle
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a random <see cref="MVector2"/> with components within the given range.
        /// </summary>
        /// <remarks>Resulting component values will be integers</remarks>
        /// <param name="random">The random object to use.</param>
        /// <param name="min">The minimum values.</param>
        /// <param name="max">The maximum values.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the maximum value is not greater than the minimum.</exception>
        public static MVector2 NextVector(this Random random, MVector2 min, MVector2 max) =>
            NextPoint(random, min.ToMPoint2(), max.ToMPoint2()).ToMVector2();

        /// <summary>
        /// Returns a random <see cref="MPoint2"/> with components within the given range.
        /// </summary>
        /// <param name="random">The random object to use.</param>
        /// <param name="min">The minimum values.</param>
        /// <param name="max">The maximum values.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the maximum value is not greater than the minimum.</exception>
        public static MPoint2 NextPoint(this Random random, MPoint2 min, MPoint2 max) =>
            new MPoint2(random.Next(min.X, max.X + 1), random.Next(min.Y, max.Y + 1));
    }
}
