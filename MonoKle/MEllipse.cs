using System;

namespace MonoKle
{
    /// <summary>
    /// Immutable, serializable, and normalized (positive radius) ellipse.
    /// </summary>
    [Serializable]
    public readonly struct MEllipse
    {
        public readonly MVector2 HalfDimensions;
        public readonly MVector2 Position;

        public MVector2 Dimensions => HalfDimensions * 2;

        public MEllipse(float halfWidth, float halfHeight) : this(MVector2.Zero, halfWidth, halfHeight) { }

        public MEllipse(MVector2 position, float halfWidth, float halfHeight)
        {
            Position = position;
            HalfDimensions = new MVector2(halfWidth, halfHeight);
        }

        /// <summary>
        /// Clamps the given coordinate to the edge of the <see cref="MEllipse"/>.
        /// </summary>
        /// <param name="point">The coordinate to clamp.</param>
        /// <returns>Coordinate clamped to the edge.</returns>
        public MVector2 ClampEdge(MVector2 point)
        {
            // Coordinate of the point in the ellipse unit circle frame
            var r = (point - Position) / HalfDimensions;
            // Normalize coordinate to be the unit circle
            var rp = r / r.Length;
            // Transform clamped back into original frame
            return Position + (rp * HalfDimensions);
        }

        /// <summary>
        /// Clamps the given coordinate inside of the <see cref="MEllipse"/>.
        /// </summary>
        /// <param name="point">The coordinate to clamp.</param>
        /// <returns>Coordinate clamped inside.</returns>
        public MVector2 Clamp(MVector2 point) => !Inside(point)
            ? ClampEdge(point)
            : point;

        /// <summary>
        /// Returns whether the provided point is within the ellipse.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns>True if within, otherwise false.</returns>
        public bool Inside(MVector2 point) =>
            Math.Pow(point.X - Position.X, 2) / Math.Pow(HalfDimensions.X, 2) +
            Math.Pow(point.Y - Position.Y, 2) / Math.Pow(HalfDimensions.Y, 2) <= 1;
    }
}
