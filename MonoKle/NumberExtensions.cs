using System;

namespace MonoKle
{
    public static class NumberExtensions
    {
        /// <summary>
        /// Wraps the value if it reaches below or above the provided minimum or maximum value, respectively.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="min">The minimum value to wrap around.</param>
        /// <param name="max">The maximum value to wrap around.</param>
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
    }
}
