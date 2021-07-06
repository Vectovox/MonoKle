using Microsoft.Xna.Framework;

namespace MonoKle
{
    public static class LerpHelper
    {
        private const float ByteToFloatFactor = 1f / 255f;

        /// <summary>
        /// Lerps between two provided <see cref="Color"/> values. Each component is lerped individually.
        /// </summary>
        /// <remarks>
        /// Backed by <see cref="MathHelper.Lerp(float, float, float)"/>.
        /// </remarks>
        /// <param name="from">The value to interpolate from.</param>
        /// <param name="to">The value to interpolate to.</param>
        /// <param name="amount">The amount to interpolate with [0, 1].</param>
        /// <returns>Interpolated <see cref="Color"/>.</returns>
        public static Color Lerp(Color from, Color to, float amount) =>
            new Color(MathHelper.Lerp(from.R, to.R, amount) * ByteToFloatFactor,
                MathHelper.Lerp(from.G, to.G, amount) * ByteToFloatFactor,
                MathHelper.Lerp(from.B, to.B, amount) * ByteToFloatFactor,
                MathHelper.Lerp(from.A, to.A, amount) * ByteToFloatFactor);
    }
}
