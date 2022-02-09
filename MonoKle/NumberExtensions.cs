using System;

namespace MonoKle
{
    public static class NumberExtensions
    {
        /// <summary>
        /// Wraps the value if it reaches below or above the provided minimum or maximum value, respectively.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="min">The inclusive minimum value to wrap around.</param>
        /// <param name="max">The inclusive maximum value to wrap around.</param>
        public static int Wrap(this int value, int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("Max value must be greater or equal to min value.");
            }

            if (value > max)
            {
                return min;
            }
            else if (value < min)
            {
                return max;
            }

            return value;
        }

        /// <summary>
        /// Returns true if the value is between the provided inclusive bounds.
        /// </summary>
        public static bool Between(this int value, int min, int max) => value >= min && value <= max;

        /// <summary>
        /// Returning the clamped value. See <see cref="Math.Clamp(int, int, int)"/> for details.
        /// </summary>
        public static int Clamp(this int value, int min, int max) => Math.Clamp(value, min, max);

        /// <summary>
        /// Returns true if the value is between the provided inclusive bounds.
        /// </summary>
        public static bool Between(this double value, double min, double max) => value >= min && value <= max;

        /// <summary>
        /// Returning the clamped value. See <see cref="Math.Clamp(double, double, double)"/> for details.
        /// </summary>
        public static double Clamp(this double value, double min, double max) => Math.Clamp(value, min, max);
    }
}
