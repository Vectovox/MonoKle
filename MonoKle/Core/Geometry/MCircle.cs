namespace MonoKle.Core.Geometry
{
    using System;

    /// <summary>
    /// Immutable, serializable, and normalized (positive radius) circle type.
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
        /// Initializes a new instance of the <see cref="MCircle"/> struct with the zero vector as origin.
        /// </summary>
        /// <param name="radius">The circle radius.</param>
        public MCircle(float radius) : this(MVector2.Zero, radius) { }

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
        /// Checks if the the <see cref="MCircle"/> intersects the specified <see cref="MRectangle"/>.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>True if it intersects; otherwise false.</returns>
        public bool Intersects(MRectangle rectangle)
        {
            MVector2 circleDistance = (this.Origin - rectangle.Center).AbsoluteComponents;

            if (circleDistance.X > (rectangle.Width / 2 + this.Radius)) { return false; }
            if (circleDistance.Y > (rectangle.Height / 2 + this.Radius)) { return false; }

            if (circleDistance.X <= (rectangle.Width / 2)) { return true; }
            if (circleDistance.Y <= (rectangle.Height / 2)) { return true; }

            float cornerDistanceSquared = circleDistance.Translate(rectangle.Width * -0.5f, rectangle.Height * -0.5f).LengthSquared();

            return cornerDistanceSquared <= this.Radius * this.Radius;
        }

        /// <summary>
        /// Moves the <see cref="MCircle"/> origin to the specified position.
        /// </summary>
        /// <param name="position">The position to move to.</param>
        /// <returns></returns>
        public MCircle MoveTo(MVector2 position) => new MCircle(position, this.Radius);

        /// <summary>
        /// Resizes the <see cref="MCircle"/> to the specified radius.
        /// </summary>
        /// <param name="radius">The radius to resize to.</param>
        /// <returns></returns>
        public MCircle Resize(float radius) => new MCircle(this.Origin, radius);

        /// <summary>
        /// Scales the <see cref="MCircle"/> with the specified factor.
        /// </summary>
        /// <param name="factor">The factor to scale with.</param>
        /// <returns></returns>
        public MCircle Scale(float factor) => new MCircle(this.Origin, this.Radius * factor);

        /// <summary>
        /// Separates the <see cref="MCircle"/> from the specified <see cref="MCircle"/>, translating it with the shortest distance that makes them not intersect.
        /// </summary>
        /// <param name="circle">The circle to separate from.</param>
        /// <returns></returns>
        public MCircle Separate(MCircle circle) => this.Translate(this.SeparationVector(circle));

        /// <summary>
        /// Separates the <see cref="MCircle"/> from the specified <see cref="MVector2"/>, translating it with the shortest distance that makes them not intersect.
        /// </summary>
        /// <param name="point">The point to separate from.</param>
        /// <returns></returns>
        public MCircle Separate(MVector2 point) => this.Translate(this.SeparationVector(point));

        /// <summary>
        /// Separates the <see cref="MCircle"/> from the specified <see cref="MPoint2"/>, translating it with the shortest distance that makes them not intersect.
        /// </summary>
        /// <param name="point">The point to separate from.</param>
        /// <returns></returns>
        public MCircle Separate(MPoint2 point) => this.Translate(this.SeparationVector(point));

        /// <summary>
        /// Separates the <see cref="MCircle"/> from the specified <see cref="MRectangle"/>, translating it with the shortest distance that makes them not intersect.
        /// </summary>
        /// <param name="rectangle">The rectangle to separate from.</param>
        /// <returns></returns>
        public MCircle Separate(MRectangle rectangle) => this.Translate(this.SeparationVector(rectangle));

        /// <summary>
        /// Returns the shortest vector to translate the <see cref="MCircle"/> with in order to separate from the provided <see cref="MCircle"/>.
        /// </summary>
        /// <param name="circle">The circle to separate from.</param>
        /// <returns></returns>
        public MVector2 SeparationVector(MCircle circle)
        {
            MVector2 deltaVector = (circle.Origin - this.Origin);
            float minDistance = circle.Radius + this.Radius;
            float toMove = deltaVector.Length() - minDistance;

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
            if(this.Intersects(rectangle))
            {
                MVector2?[] translationArray = new MVector2?[5];
                translationArray[0] = new MVector2(rectangle.Left - (this.Origin.X + this.Radius), 0);          // Left
                translationArray[1] = new MVector2(rectangle.Right - (this.Origin.X - this.Radius), 0);         // Right
                translationArray[2] = new MVector2(0, rectangle.Top - (this.Origin.Y + this.Radius));           // Up
                translationArray[3] = new MVector2(0, rectangle.Bottom - (this.Origin.Y - this.Radius));        // Down

                if(this.Origin.X < rectangle.Left && this.Origin.Y < rectangle.Top)
                {
                    translationArray[4] = this.SeparationVector(rectangle.TopLeft);                            // Top-left
                }

                if (this.Origin.X > rectangle.Right && this.Origin.Y < rectangle.Top)
                {
                    translationArray[4] = this.SeparationVector(rectangle.TopRight);                           // Top-right
                }

                if (this.Origin.X < rectangle.Left && this.Origin.Y > rectangle.Bottom)
                {
                    translationArray[4] = this.SeparationVector(rectangle.BottomLeft);                         // Bottom-left
                }

                if (this.Origin.X > rectangle.Right && this.Origin.Y > rectangle.Bottom)
                {
                    translationArray[4] = this.SeparationVector(rectangle.BottomRight);                        // Bottom-right
                }

                MVector2? shortest = translationArray[0];
                float length = shortest.Value.LengthSquared();

                for (byte i = 1; i < 5; i++)
                {
                    if(translationArray[i] != null)
                    {
                        float nLength = translationArray[i].Value.LengthSquared();
                        if(nLength < length)
                        {
                            shortest = translationArray[i].Value;
                            length = nLength;
                        }
                    }
                }

                return shortest.Value;
            }

            return MVector2.Zero;
        }

        /// <summary>
        /// Returns the shortest vector to translate the <see cref="MCircle"/> with in order to separate from the provided <see cref="MVector2"/>.
        /// </summary>
        /// <param name="point">The point to separate from.</param>
        /// <returns></returns>
        public MVector2 SeparationVector(MVector2 point) => this.SeparationVector(new MCircle(point, 0));

        /// <summary>
        /// Returns the shortest vector to translate the <see cref="MCircle"/> with in order to separate from the provided <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="point">The point to separate from.</param>
        /// <returns></returns>
        public MVector2 SeparationVector(MPoint2 point) => this.SeparationVector(new MCircle(point.ToMVector2(), 0));

        /// <summary>
        /// Translates the <see cref="MCircle"/> with the specified translation.
        /// </summary>
        /// <param name="translation">The translation to perform.</param>
        /// <returns></returns>
        public MCircle Translate(MVector2 translation) => new MCircle(this.Origin.Translate(translation), this.Radius);
    }
}