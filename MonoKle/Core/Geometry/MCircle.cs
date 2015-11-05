namespace MonoKle.Core.Geometry
{
    using System;

    /// <summary>
    /// Immutable, serializable, and normalized (positive radius), circle data-structure.
    /// </summary>
    [Serializable()]
    public struct MCircle
    {
        /// <summary>
        /// The origin of the <see cref="MCircle"/>.
        /// </summary>
        public readonly MVector2 Origin;

        /// <summary>
        /// The radius of the <see cref="MCircle"/>.
        /// </summary>
        public readonly float Radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="MCircle" /> struct.
        /// </summary>
        /// <param name="origin">The circle origin.</param>
        /// <param name="radius">The circle radius.</param>
        public MCircle(MVector2 origin, float radius)
        {
            this.Origin = origin;
            this.Radius = radius < 0f ? -radius : radius;
        }

        /// <summary>
        /// Gets the area of the <see cref="MCircle"/>.
        /// </summary>
        public float Area => this.Radius * this.Radius * (float)Math.PI;

        /// <summary>
        /// Gets the bounding rectangle of the <see cref="MCircle"/>.
        /// </summary>
        /// <value>
        /// The bounding rectangle.
        /// </value>
        public MRectangle BoundingRectangle => new MRectangle(this);

        /// <summary>
        /// Gets the circumference of the <see cref="MCircle"/>.
        /// </summary>
        public float Circumference => this.Radius * 2 * (float)Math.PI;

        /// <summary>
        /// Determines whether the <see cref="MCircle"/> contains the specified <see cref="MVector2"/> coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>True if the coordinate is contained; otherwise false.</returns>
        public bool Contains(MVector2 coordinate) => (coordinate - this.Origin).LengthSquared() <= this.Radius * this.Radius;

        /// <summary>
        /// Checks if the the specified <see cref="MRectangle"/> intersects the <see cref="MCircle"/>.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>True if it intersects; otherwise false.</returns>
        public bool Intersects(MRectangle rectangle)
        {
            MVector2 circleDistance = (this.Origin - rectangle.Center).Absolute;

            if (circleDistance.X > (rectangle.Width / 2 + this.Radius)) { return false; }
            if (circleDistance.Y > (rectangle.Height / 2 + this.Radius)) { return false; }

            if (circleDistance.X <= (rectangle.Width / 2)) { return true; }
            if (circleDistance.Y <= (rectangle.Height / 2)) { return true; }

            float cornerDistanceSquared = circleDistance.Translate(rectangle.Width * -0.5f, rectangle.Height * -0.5f).LengthSquared();

            return cornerDistanceSquared <= this.Radius * this.Radius;
        }
    }
}