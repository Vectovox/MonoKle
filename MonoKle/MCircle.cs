using System;

namespace MonoKle
{
    /// <summary>
    /// Immutable, serializable, and normalized (positive radius) circle type.
    /// </summary>
    [Serializable]
    public readonly struct MCircle
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
            Origin = origin;
            Radius = radius < 0f ? -radius : radius;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MCircle"/> struct with the zero vector as origin.
        /// </summary>
        /// <param name="radius">The circle radius.</param>
        public MCircle(float radius) : this(MVector2.Zero, radius) { }

        /// <summary>
        /// Gets the area of the <see cref="MCircle"/>.
        /// </summary>
        public float Area => Radius * Radius * (float)Math.PI;

        /// <summary>
        /// Gets the bounding rectangle of the <see cref="MCircle"/>.
        /// </summary>
        /// <value>
        /// The bounding rectangle.
        /// </value>
        public MRectangle BoundingRectangle => new(this);

        /// <summary>
        /// Gets the circumference of the <see cref="MCircle"/>.
        /// </summary>
        public float Circumference => Radius * 2 * (float)Math.PI;

        /// <summary>
        /// Determines whether the <see cref="MCircle"/> contains the specified <see cref="MVector2"/> coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>True if the coordinate is contained; otherwise false.</returns>
        public bool Contains(MVector2 coordinate) => (coordinate - Origin).LengthSquared <= Radius * Radius;

        /// <summary>
        /// Checks if the the <see cref="MCircle"/> intersects the specified <see cref="MRectangle"/>.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>True if it intersects; otherwise false.</returns>
        public bool Intersects(MRectangle rectangle)
        {
            MVector2 circleDistance = (Origin - rectangle.Center).Absolute;

            if (circleDistance.X > (rectangle.Width / 2 + Radius)) { return false; }
            if (circleDistance.Y > (rectangle.Height / 2 + Radius)) { return false; }

            if (circleDistance.X <= (rectangle.Width / 2)) { return true; }
            if (circleDistance.Y <= (rectangle.Height / 2)) { return true; }

            float cornerDistanceSquared = circleDistance.Translate(rectangle.Width * -0.5f, rectangle.Height * -0.5f).LengthSquared;

            return cornerDistanceSquared <= Radius * Radius;
        }

        /// <summary>
        /// Moves the <see cref="MCircle"/> origin to the specified position.
        /// </summary>
        /// <param name="position">The position to move to.</param>
        /// <returns></returns>
        public MCircle MoveTo(MVector2 position) => new(position, Radius);

        /// <summary>
        /// Resizes the <see cref="MCircle"/> to the specified radius.
        /// </summary>
        /// <param name="radius">The radius to resize to.</param>
        /// <returns></returns>
        public MCircle Resize(float radius) => new(Origin, radius);

        /// <summary>
        /// Scales the <see cref="MCircle"/> with the specified factor.
        /// </summary>
        /// <param name="factor">The factor to scale with.</param>
        /// <returns></returns>
        public MCircle Scale(float factor) => new(Origin, Radius * factor);

        /// <summary>
        /// Separates the <see cref="MCircle"/> from the specified <see cref="MCircle"/>, translating it with the shortest distance that makes them not intersect.
        /// </summary>
        /// <param name="circle">The circle to separate from.</param>
        /// <returns></returns>
        public MCircle Separate(MCircle circle) => Translate(SeparationVector(circle));

        /// <summary>
        /// Separates the <see cref="MCircle"/> from the specified <see cref="MVector2"/>, translating it with the shortest distance that makes them not intersect.
        /// </summary>
        /// <param name="point">The point to separate from.</param>
        /// <returns></returns>
        public MCircle Separate(MVector2 point) => Translate(SeparationVector(point));

        /// <summary>
        /// Separates the <see cref="MCircle"/> from the specified <see cref="MPoint2"/>, translating it with the shortest distance that makes them not intersect.
        /// </summary>
        /// <param name="point">The point to separate from.</param>
        /// <returns></returns>
        public MCircle Separate(MPoint2 point) => Translate(SeparationVector(point));

        /// <summary>
        /// Separates the <see cref="MCircle"/> from the specified <see cref="MRectangle"/>, translating it with the shortest distance that makes them not intersect.
        /// </summary>
        /// <param name="rectangle">The rectangle to separate from.</param>
        /// <returns></returns>
        public MCircle Separate(MRectangle rectangle) => Translate(SeparationVector(rectangle));

        /// <summary>
        /// Returns the shortest vector to translate the <see cref="MCircle"/> with in order to separate from the provided <see cref="MCircle"/>.
        /// </summary>
        /// <param name="circle">The circle to separate from.</param>
        /// <returns></returns>
        public MVector2 SeparationVector(MCircle circle)
        {
            MVector2 deltaVector = (circle.Origin - Origin);
            float minDistance = circle.Radius + Radius;
            float toMove = deltaVector.Length - minDistance;

            if (toMove < 0)
            {
                return deltaVector.Normalized * toMove;
            }
            else
            {
                return MVector2.Zero;
            }
        }

        /// <summary>
        /// Returns the shortest vector to translate the <see cref="MCircle"/> with in order to separate from the provided <see cref="MRectangle"/>.
        /// </summary>
        /// <param name="rectangle">The rectangle to separate from.</param>
        /// <returns></returns>
        public MVector2 SeparationVector(MRectangle rectangle)
        {
            var left = new MVector2(rectangle.TopLeft.X - (Origin.X + Radius), 0);
            var right = new MVector2(rectangle.BottomRight.X - (Origin.X - Radius), 0);
            var up = new MVector2(0, rectangle.TopLeft.Y - (Origin.Y + Radius));
            var down = new MVector2(0, rectangle.BottomRight.Y - (Origin.Y - Radius));

            if (left.X < 0 && right.X > 0 || up.Y < 0 && down.Y > 0)
            {
                var shortest = new MVector2(float.MaxValue, float.MaxValue);

                if (Origin.X < rectangle.Left && Origin.Y < rectangle.Top)
                {
                    shortest = SeparationVector(rectangle.TopLeft);
                }
                else if (Origin.X > rectangle.Right && Origin.Y < rectangle.Top)
                {
                    shortest = SeparationVector(rectangle.TopRight);
                }
                else if (Origin.X < rectangle.Left && Origin.Y > rectangle.Bottom)
                {
                    shortest = SeparationVector(rectangle.BottomLeft);
                }
                else if (Origin.X > rectangle.Right && Origin.Y > rectangle.Bottom)
                {
                    shortest = SeparationVector(rectangle.BottomRight);
                }

                if (left.X < 0 && left.LengthSquared < shortest.LengthSquared)
                {
                    shortest = left;
                }
                if (right.X > 0 && right.LengthSquared < shortest.LengthSquared)
                {
                    shortest = right;
                }
                if (up.Y < 0 && up.LengthSquared < shortest.LengthSquared)
                {
                    shortest = up;
                }
                if (down.Y > 0 && down.LengthSquared < shortest.LengthSquared)
                {
                    shortest = down;
                }

                return shortest;
            }

            return MVector2.Zero;
        }

        /// <summary>
        /// Returns the shortest vector to translate the <see cref="MCircle"/> with in order to separate from the provided <see cref="MVector2"/>.
        /// </summary>
        /// <param name="point">The point to separate from.</param>
        /// <returns></returns>
        public MVector2 SeparationVector(MVector2 point) => SeparationVector(new MCircle(point, 0));

        /// <summary>
        /// Returns the shortest vector to translate the <see cref="MCircle"/> with in order to separate from the provided <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="point">The point to separate from.</param>
        /// <returns></returns>
        public MVector2 SeparationVector(MPoint2 point) => SeparationVector(new MCircle(point.ToMVector2(), 0));

        /// <summary>
        /// Translates the <see cref="MCircle"/> with the specified translation.
        /// </summary>
        /// <param name="translation">The translation to perform.</param>
        /// <returns></returns>
        public MCircle Translate(MVector2 translation) => new(Origin.Translate(translation), Radius);
    }
}
