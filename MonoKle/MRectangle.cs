namespace MonoKle
{
    using System;
    using System.Text;

    /// <summary>
    /// Struct for storing an immutable, serializable, normalized (positive width and height), floating point precision area.
    /// </summary>
    [Serializable()]
    public struct MRectangle : IEquatable<MRectangle>
    {
        /// <summary>
        /// The bottom right coordinate of the <see cref="MRectangle"/>.
        /// </summary>
        public readonly MVector2 BottomRight;

        /// <summary>
        /// The top left coordinate of the <see cref="MRectangle"/>.
        /// </summary>
        public readonly MVector2 TopLeft;

        private const int HASH_CODE_INITIAL = 17;
        private const int HASH_CODE_MULTIPLIER = 23;

        /// <summary>
        /// Creates a new instance of <see cref="MRectangle"/> from the given <see cref="MRectangleInt"/>.
        /// <param name="area">The area to instantiate from.</param>
        /// </summary>
        public MRectangle(MRectangleInt area)
        {
            this.TopLeft = area.TopLeft.ToMVector2();
            this.BottomRight = area.BottomRight.ToMVector2();
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangle"/> around (0, 0) with the given width and height.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public MRectangle(float width, float height)
        {
            float xLeft = Math.Min(0, width);
            float xRight = Math.Max(0, width);
            float yTop = Math.Min(0, height);
            float yBottom = Math.Max(0, height);
            this.TopLeft = new MVector2(xLeft, yTop);
            this.BottomRight = new MVector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangle"/> around (0, 0) with the given size.
        /// </summary>
        /// <param name="size">The size.</param>
        public MRectangle(MVector2 size)
        {
            float xLeft = Math.Min(0, size.X);
            float xRight = Math.Max(0, size.X);
            float yTop = Math.Min(0, size.Y);
            float yBottom = Math.Max(0, size.Y);
            this.TopLeft = new MVector2(xLeft, yTop);
            this.BottomRight = new MVector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangle"/> by the provided coordinate and the given width and height.
        /// </summary>
        /// <param name="x">X-coordinate of a corner.</param>
        /// <param name="y">Y-coordinate of a corner.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public MRectangle(float x, float y, float width, float height)
        {
            float xLeft = Math.Min(x, x + width);
            float xRight = Math.Max(x, x + width);
            float yTop = Math.Min(y, y + height);
            float yBottom = Math.Max(y, y + height);
            this.TopLeft = new MVector2(xLeft, yTop);
            this.BottomRight = new MVector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangle"/> bound by the provided coordinates.
        /// </summary>
        /// <param name="coordA">The first coordinate.</param>
        /// <param name="coordB">The second coordinate.</param>
        public MRectangle(MVector2 coordA, MVector2 coordB)
        {
            float xLeft = Math.Min(coordA.X, coordB.X);
            float xRight = Math.Max(coordA.X, coordB.X);
            float yTop = Math.Min(coordA.Y, coordB.Y);
            float yBottom = Math.Max(coordA.Y, coordB.Y);
            this.TopLeft = new MVector2(xLeft, yTop);
            this.BottomRight = new MVector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangle"/> bound by the provided coordinates.
        /// </summary>
        /// <param name="coordA">The first coordinate.</param>
        /// <param name="coordB">The second coordinate.</param>
        public MRectangle(MPoint2 coordA, MPoint2 coordB)
        {
            float xLeft = Math.Min(coordA.X, coordB.X);
            float xRight = Math.Max(coordA.X, coordB.X);
            float yTop = Math.Min(coordA.Y, coordB.Y);
            float yBottom = Math.Max(coordA.Y, coordB.Y);
            this.TopLeft = new MVector2(xLeft, yTop);
            this.BottomRight = new MVector2(xRight, yBottom);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MRectangle"/> struct, equating to the bounding box of the provided <see cref="MCircle"/>.
        /// </summary>
        /// <param name="circle">The circle.</param>
        public MRectangle(MCircle circle)
        {
            this.TopLeft = new MVector2(circle.Origin.X - circle.Radius, circle.Origin.Y - circle.Radius);
            this.BottomRight = new MVector2(circle.Origin.X + circle.Radius, circle.Origin.Y + circle.Radius);
        }

        /// <summary>
        /// Gets the Y-coordinate of the bottom border.
        /// </summary>
        public float Bottom
        {
            get { return this.BottomRight.Y; }
        }

        /// <summary>
        /// Gets the bottom left corner.
        /// </summary>
        public MVector2 BottomLeft
        {
            get { return new MVector2(this.TopLeft.X, this.BottomRight.Y); }
        }

        /// <summary>
        /// Gets the center of the <see cref="MRectangle"/>.
        /// </summary>
        public MVector2 Center => new MVector2(this.TopLeft.X + this.Width * 0.5f, this.TopLeft.Y + this.Height * 0.5f);

        /// <summary>
        /// Gets the width.
        /// </summary>
        public float Height
        {
            get { return this.BottomRight.Y - this.TopLeft.Y; }
        }

        /// <summary>
        /// Gets the X-coordinate of the left border.
        /// </summary>
        public float Left
        {
            get { return this.TopLeft.X; }
        }

        /// <summary>
        /// Gets the X-coordinate of the right border.
        /// </summary>
        public float Right
        {
            get { return this.BottomRight.X; }
        }

        /// <summary>
        /// Gets the Y-coordinate of the top border.
        /// </summary>
        public float Top
        {
            get { return this.TopLeft.Y; }
        }

        /// <summary>
        /// Gets the top right corner.
        /// </summary>
        public MVector2 TopRight
        {
            get { return new MVector2(this.BottomRight.X, this.TopLeft.Y); }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public float Width
        {
            get { return this.BottomRight.X - this.TopLeft.X; }
        }

        /// <summary>
        /// Non-equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="MRectangle"/>.</param>
        /// <param name="b">Right <see cref="MRectangle"/>.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(MRectangle a, MRectangle b)
        {
            return a.TopLeft != b.TopLeft || a.BottomRight != b.BottomRight;
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="MRectangle"/>.</param>
        /// <param name="b">Right <see cref="MRectangle"/>.</param>
        /// <returns>True if are equal, else false.</returns>
        public static bool operator ==(MRectangle a, MRectangle b)
        {
            return a.TopLeft == b.TopLeft && a.BottomRight == b.BottomRight;
        }

        /// <summary>
        /// Clamps the provided <see cref="MRectangle"/> to fit within this.
        /// </summary>
        /// <param name="area">The <see cref="MRectangle"/> to clamp.</param>
        public MRectangle Clamp(MRectangle area)
        {
            return new MRectangle(this.Clamp(area.TopLeft), this.Clamp(area.BottomRight));
        }

        /// <summary>
        /// Clamps the provided <see cref="MVector2"/> to be positioned within this.
        /// </summary>
        /// <param name="coordinate">The <see cref="MVector2"/> to clamp.</param>
        public MVector2 Clamp(MVector2 coordinate)
        {
            float x = coordinate.X;
            float y = coordinate.Y;

            if (x < this.TopLeft.X)
            {
                x = this.TopLeft.X;
            }
            else if (x > this.BottomRight.X)
            {
                x = this.BottomRight.X;
            }

            if (y < this.TopLeft.Y)
            {
                y = this.TopLeft.Y;
            }
            else if (coordinate.Y > this.BottomRight.Y)
            {
                y = this.BottomRight.Y;
            }

            return new MVector2(x, y);
        }

        /// <summary>
        /// Checks if contains the provided <see cref="MRectangle"/>.
        /// </summary>
        /// <param name="area">The <see cref="MRectangle"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="MRectangle"/> is contained, otherwise false.</returns>
        public bool Contains(MRectangle area)
        {
            return this.Contains(area.TopLeft) && this.Contains(area.TopRight)
                && this.Contains(area.BottomLeft) && this.Contains(area.BottomRight);
        }

        /// <summary>
        /// Checks if contains the provided <see cref="MRectangleInt"/>.
        /// </summary>
        /// <param name="area">The <see cref="MRectangleInt"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="MRectangleInt"/> is contained, otherwise false.</returns>
        public bool Contains(MRectangleInt area)
        {
            return this.Contains(area.TopLeft) && this.Contains(area.TopRight)
                && this.Contains(area.BottomLeft) && this.Contains(area.BottomRight);
        }

        /// <summary>
        /// Checks if the <see cref="MRectangle"/> contains the provided <see cref="MVector2"/>.
        /// </summary>
        /// <param name="coordinate">The <see cref="MVector2"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="MVector2"/> is contained, otherwise false.</returns>
        public bool Contains(MVector2 coordinate)
        {
            return coordinate.X >= this.TopLeft.X && coordinate.X <= this.BottomRight.X
                && coordinate.Y >= this.TopLeft.Y && coordinate.Y <= this.BottomRight.Y;
        }

        /// <summary>
        /// Checks if the <see cref="MRectangle"/> contains the provided <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="coordinate">The <see cref="MPoint2"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="MPoint2"/> is contained, otherwise false.</returns>
        public bool Contains(MPoint2 coordinate)
        {
            return coordinate.X >= this.TopLeft.X && coordinate.X <= this.BottomRight.X
                && coordinate.Y >= this.TopLeft.Y && coordinate.Y <= this.BottomRight.Y;
        }

        /// <summary>
        /// Checks if envelops, contains with a marigin to all borders, the provided <see cref="MRectangle"/>.
        /// </summary>
        /// <param name="area">The <see cref="MRectangle"/> to check if enveloped.</param>
        /// <returns>True if the specified <see cref="MRectangle"/> is enveloped, otherwise false.</returns>
        public bool Envelops(MRectangle area) =>
            this.Envelops(area.TopLeft) && this.Envelops(area.TopRight)
                && this.Envelops(area.BottomLeft) && this.Envelops(area.BottomRight);

        /// <summary>
        /// Checks if envelops, contains with a marigin to all borders, the provided <see cref="MRectangleInt"/>.
        /// </summary>
        /// <param name="area">The <see cref="MRectangleInt"/> to check if enveloped.</param>
        /// <returns>True if the specified <see cref="MRectangleInt"/> is enveloped, otherwise false.</returns>
        public bool Envelops(MRectangleInt area) =>
            this.Envelops(area.TopLeft) && this.Envelops(area.TopRight)
                && this.Envelops(area.BottomLeft) && this.Envelops(area.BottomRight);

        /// <summary>
        /// Checks if envelops, contains with a marigin to all borders, the provided <see cref="MVector2"/>.
        /// </summary>
        /// <param name="coordinate">The <see cref="MVector2"/> to check if enveloped.</param>
        /// <returns>True if the <see cref="MVector2"/> is enveloped, otherwise false.</returns>
        public bool Envelops(MVector2 coordinate) =>
            coordinate.X > this.TopLeft.X && coordinate.X < this.BottomRight.X
                && coordinate.Y > this.TopLeft.Y && coordinate.Y < this.BottomRight.Y;

        /// <summary>
        /// Checks if envelops, contains with a marigin to all borders, the provided <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="coordinate">The <see cref="MPoint2"/> to check if enveloped.</param>
        /// <returns>True if the <see cref="MPoint2"/> is enveloped, otherwise false.</returns>
        public bool Envelops(MPoint2 coordinate) =>
            coordinate.X > this.TopLeft.X && coordinate.X < this.BottomRight.X
                && coordinate.Y > this.TopLeft.Y && coordinate.Y < this.BottomRight.Y;

        /// <summary>
        /// Returns whether this is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is MRectangle)
            {
                return this == ((MRectangle)obj);
            }
            return false;
        }

        /// <summary>
        /// Returns whether this is equal to the provided <see cref="MRectangle"/>.
        /// </summary>
        /// <param name="other">The <see cref="MRectangle"/> to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(MRectangle other)
        {
            return this == other;
        }

        /// <summary>
        /// Returns the hash representation.
        /// </summary>
        /// <returns>Hash representation.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = MRectangle.HASH_CODE_INITIAL;
                hash = hash * MRectangle.HASH_CODE_MULTIPLIER + this.TopLeft.GetHashCode();
                hash = hash * MRectangle.HASH_CODE_MULTIPLIER + this.BottomRight.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns the <see cref="String"/> representation.
        /// </summary>
        /// <returns><see cref="String"/> representation.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{Top Left: ");
            sb.Append(this.TopLeft.ToString());
            sb.Append(", Bottom Right: ");
            sb.Append(this.BottomRight.ToString());
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// Translates the <see cref="MRectangle"/> with the given translation and returns the result.
        /// </summary>
        /// <param name="translation">The translation to make.</param>
        /// <returns>Translated <see cref="MRectangle"/></returns>
        public MRectangle Translate(MVector2 translation)
        {
            return new MRectangle(this.TopLeft.X + translation.X, this.TopLeft.Y + translation.Y, this.BottomRight.X - this.TopLeft.X, this.BottomRight.Y - this.TopLeft.Y);
        }

        /// <summary>
        /// Translates the <see cref="MRectangle"/> with the given X-translation and returns the result.
        /// </summary>
        /// <param name="x">The translation along the X-axis.</param>
        /// <returns>Translated <see cref="MRectangle"/></returns>
        public MRectangle TranslateX(float x)
        {
            return new MRectangle(this.TopLeft.X + x, this.TopLeft.Y, this.BottomRight.X - this.TopLeft.X, this.BottomRight.Y - this.TopLeft.Y);
        }

        /// <summary>
        /// Translates the <see cref="MRectangle"/> with the given Y-translation and returns the result.
        /// </summary>
        /// <param name="y">The translation along the Y-axis.</param>
        /// <returns>Translated <see cref="MRectangle"/></returns>
        public MRectangle TranslateY(float y)
        {
            return new MRectangle(this.TopLeft.X, this.TopLeft.Y + y, this.BottomRight.X - this.TopLeft.X, this.BottomRight.Y - this.TopLeft.Y);
        }
    }
}