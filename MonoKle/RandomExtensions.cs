using Microsoft.Xna.Framework;
using System;

namespace MonoKle
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a random <see cref="MVector2"/> with components within the given range.
        /// </summary>
        /// <remarks>Resulting component values will be integers</remarks>
        /// <param name="random">The random instance to use.</param>
        /// <param name="min">The minimum values.</param>
        /// <param name="max">The maximum values.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the maximum value is not greater than the minimum.</exception>
        public static MVector2 NextVector(this Random random, MVector2 min, MVector2 max) =>
            NextPoint(random, min.ToMPoint2(), max.ToMPoint2()).ToMVector2();

        /// <summary>
        /// Returns a random <see cref="MPoint2"/> with components within the given inclusive range.
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <param name="min">The inclusive minimum values.</param>
        /// <param name="max">The inclusive maximum values.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the maximum value is not greater than the minimum.</exception>
        public static MPoint2 NextPoint(this Random random, MPoint2 min, MPoint2 max) =>
            new(random.Next(min.X, max.X + 1), random.Next(min.Y, max.Y + 1));

        /// <summary>
        /// Returns a random <see cref="MPoint2"/> located inside the provided inclusive space.
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <param name="randomSpace">The available space to generate random numbers for.</param>
        public static MPoint2 NextPoint(this Random random, MDiscreteRectangle randomSpace) =>
            random.NextPoint(randomSpace.TopLeft, randomSpace.BottomRight);

        /// <summary>
        /// Returns a random value between the provided min and max values.
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <param name="min">The inclusive min value.</param>
        /// <param name="max">The inclusive max value.</param>
        public static double NextDouble(this Random random, double min, double max) => min <= max
            ? min + random.NextDouble() * (max - min)
            : throw new ArgumentException("Minimum value must be smaller than maximum value");

        /// <summary>
        /// Returns either the provided value (v) or the negated value (-v).
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <param name="value">The value to potentially negate.</param>
        public static double NextNegate(this Random random, double value)
            => random.Next(0, 2) == 0 ? value : -value;

        /// <summary>
        /// Returns a random timespan between the provided min- and max-values.
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <param name="min">The inclusive minimum timespan boundary.</param>
        /// <param name="max">The inclusive maximum timespan boundary.</param>
        public static TimeSpan NextTimeSpan(this Random random, TimeSpan min, TimeSpan max)
        {
            var ticks = random.Next((int)(max.Ticks - min.Ticks));
            return new TimeSpan(min.Ticks + ticks);
        }

        /// <summary>
        /// Returns a random unit-vector direction.
        /// </summary>
        public static MVector2 NextDirection(this Random random)
        {
            var direction = random.NextDouble() * MathHelper.TwoPi;
            return new MVector2((float)Math.Cos(direction), (float)Math.Sin(direction));
        }
    }
}
