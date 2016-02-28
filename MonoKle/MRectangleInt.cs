namespace MonoKle
{
    using Microsoft.Xna.Framework;
    using System;
    using System.Text;

    /// <summary>
    /// Struct for storing an immutable, serializable, normalized (non-negative width and height), integer-based area.
    /// </summary>
    [Serializable()]
    public struct MRectangleInt : IEquatable<MRectangleInt>
    {
        /// <summary>
        /// The bottom right coordinate of the <see cref="MRectangleInt"/>.
        /// </summary>
        public readonly MPoint2 BottomRight;

        /// <summary>
        /// The top left coordinate of the <see cref="MRectangleInt"/>.
        /// </summary>
        public readonly MPoint2 TopLeft;

        private const int HASH_CODE_INITIAL = 17;
        private const int HASH_CODE_MULTIPLIER = 23;

        /// <summary>
        /// Creates a new instance of <see cref="MRectangleInt"/> around (0, 0) with the given width and height.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public MRectangleInt(int width, int height)
        {
            int xLeft = Math.Min(0, width);
            int xRight = Math.Max(0, width);
            int yTop = Math.Min(0, height);
            int yBottom = Math.Max(0, height);
            this.TopLeft = new MPoint2(xLeft, yTop);
            this.BottomRight = new MPoint2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangleInt"/> by the provided coordinate and the given width and height.
        /// </summary>
        /// <param name="x">X-coordinate of a corner.</param>
        /// <param name="y">Y-coordinate of a corner.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public MRectangleInt(int x, int y, int width, int height)
        {
            int xLeft = Math.Min(x, x + width);
            int xRight = Math.Max(x, x + width);
            int yTop = Math.Min(y, y + height);
            int yBottom = Math.Max(y, y + height);
            this.TopLeft = new MPoint2(xLeft, yTop);
            this.BottomRight = new MPoint2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangleInt"/> around (0, 0) with the given size.
        /// </summary>
        /// <param name="size">The size.</param>
        public MRectangleInt(MPoint2 size)
        {
            int xLeft = Math.Min(0, size.X);
            int xRight = Math.Max(0, size.X);
            int yTop = Math.Min(0, size.Y);
            int yBottom = Math.Max(0, size.Y);
            this.TopLeft = new MPoint2(xLeft, yTop);
            this.BottomRight = new MPoint2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangleInt"/> bound by the provided coordinates.
        /// </summary>
        /// <param name="coordA">The first coordinate.</param>
        /// <param name="coordB">The second coordinate.</param>
        public MRectangleInt(MPoint2 coordA, MPoint2 coordB)
        {
            int xLeft = Math.Min(coordA.X, coordB.X);
            int xRight = Math.Max(coordA.X, coordB.X);
            int yTop = Math.Min(coordA.Y, coordB.Y);
            int yBottom = Math.Max(coordA.Y, coordB.Y);
            this.TopLeft = new MPoint2(xLeft, yTop);
            this.BottomRight = new MPoint2(xRight, yBottom);
        }

        /// <summary>
        /// Gets the Y-coordinate of the bottom border.
        /// </summary>
        public int Bottom
        {
            get { return this.BottomRight.Y; }
        }

        /// <summary>
        /// Gets the bottom left corner.
        /// </summary>
        public MPoint2 BottomLeft
        {
            get { return new MPoint2(this.TopLeft.X, this.BottomRight.Y); }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Height
        {
            get { return this.BottomRight.Y - this.TopLeft.Y; }
        }

        /// <summary>
        /// Gets the X-coordinate of the left border.
        /// </summary>
        public int Left
        {
            get { return this.TopLeft.X; }
        }

        /// <summary>
        /// Gets the X-coordinate of the right border.
        /// </summary>
        public int Right
        {
            get { return this.BottomRight.X; }
        }

        /// <summary>
        /// Gets the Y-coordinate of the top border.
        /// </summary>
        public int Top
        {
            get { return this.TopLeft.Y; }
        }

        /// <summary>
        /// Gets the top right corner.
        /// </summary>
        public MPoint2 TopRight
        {
            get { return new MPoint2(this.BottomRight.X, this.TopLeft.Y); }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Width
        {
            get { return this.BottomRight.X - this.TopLeft.X; }
        }

        /// <summary>
        /// Non-equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="MRectangleInt"/>.</param>
        /// <param name="b">Right <see cref="MRectangleInt"/>.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(MRectangleInt a, MRectangleInt b)
        {
            return a.TopLeft != b.TopLeft || a.BottomRight != b.BottomRight;
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="MRectangleInt"/>.</param>
        /// <param name="b">Right <see cref="MRectangleInt"/>.</param>
        /// <returns>True if are equal, else false.</returns>
        public static bool operator ==(MRectangleInt a, MRectangleInt b)
        {
            return a.TopLeft == b.TopLeft && a.BottomRight == b.BottomRight;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="MRectangleInt"/> to <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Rectangle(MRectangleInt r)
        {
            return new Rectangle(r.TopLeft.X, r.TopLeft.Y, r.Width, r.Height);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Rectangle"/> to <see cref="MRectangleInt"/>.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator MRectangleInt(Rectangle r)
        {
            return new MRectangleInt(r.X, r.Y, r.Width, r.Height);
        }

        /// <summary>
        /// Clamps the provided <see cref="MRectangleInt"/> to fit within.
        /// </summary>
        /// <param name="area">The <see cref="MRectangleInt"/> to clamp.</param>
        public MRectangleInt Clamp(MRectangleInt area)
        {
            MPoint2 topLeft = this.Clamp(area.TopLeft);
            MPoint2 bottomRight = this.Clamp(area.BottomRight);
            return new MRectangleInt(topLeft, bottomRight);
        }

        /// <summary>
        /// Clamps the provided <see cref="MRectangle"/> to fit within.
        /// </summary>
        /// <param name="area">The <see cref="MRectangle"/> to clamp.</param>
        public MRectangle Clamp(MRectangle area)
        {
            MVector2 topLeft = this.Clamp(area.TopLeft);
            MVector2 bottomRight = this.Clamp(area.BottomRight);
            return new MRectangle(topLeft, bottomRight);
        }

        /// <summary>
        /// Clamps the provided <see cref="MPoint2"/> to be positioned within this.
        /// </summary>
        /// <param name="coordinate">The <see cref="MPoint2"/> to clamp.</param>
        public MPoint2 Clamp(MPoint2 coordinate)
        {
            int x = coordinate.X;
            int y = coordinate.Y;

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
            else if (y > this.BottomRight.Y)
            {
                y = this.BottomRight.Y;
            }

            return new MPoint2(x, y);
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
            else if (y > this.BottomRight.Y)
            {
                y = this.BottomRight.Y;
            }

            return new MVector2(x, y);
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
        /// Checks if contains the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate to check if it is contained.</param>
        /// <returns>True if the specified coordinate is contained, otherwise false.</returns>
        public bool Contains(MVector2 coordinate)
        {
            return coordinate.X >= this.Left && coordinate.X <= this.Right
                && coordinate.Y >= this.Top && coordinate.Y <= this.Bottom;
        }

        /// <summary>
        /// Checks if contains the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate to check if it is contained.</param>
        /// <returns>True if the specified coordinate is contained, otherwise false.</returns>
        public bool Contains(MPoint2 coordinate)
        {
            return coordinate.X >= this.Left && coordinate.X <= this.Right
                && coordinate.Y >= this.Top && coordinate.Y <= this.Bottom;
        }

        /// <summary>
        /// Returns whether this is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is MRectangleInt)
            {
                return this == ((MRectangleInt)obj);
            }
            return false;
        }

        /// <summary>
        /// Returns whether this is equal to the provided <see cref="MRectangleInt"/>.
        /// </summary>
        /// <param name="other">The <see cref="MRectangleInt"/> to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(MRectangleInt other)
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
                int hash = MRectangleInt.HASH_CODE_INITIAL;
                hash = hash * MRectangleInt.HASH_CODE_MULTIPLIER + this.TopLeft.GetHashCode();
                hash = hash * MRectangleInt.HASH_CODE_MULTIPLIER + this.BottomRight.GetHashCode();
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
        /// Translates the <see cref="MRectangleInt"/> with the given translation and returns the result.
        /// </summary>
        /// <param name="translation">The translation to make.</param>
        /// <returns>Translated <see cref="MRectangleInt"/></returns>
        public MRectangleInt Translate(MPoint2 translation)
        {
            return new MRectangleInt(this.TopLeft.X + translation.X, this.TopLeft.Y + translation.Y, this.BottomRight.X - this.TopLeft.X, this.BottomRight.Y - this.TopLeft.Y);
        }

        /// <summary>
        /// Translates the <see cref="MRectangleInt"/> with the given X-translation and returns the result.
        /// </summary>
        /// <param name="x">The translation along the X-axis.</param>
        /// <returns>Translated <see cref="MRectangleInt"/></returns>
        public MRectangleInt TranslateX(int x)
        {
            return new MRectangleInt(this.TopLeft.X + x, this.TopLeft.Y, this.BottomRight.X - this.TopLeft.X, this.BottomRight.Y - this.TopLeft.Y);
        }

        /// <summary>
        /// Translates the <see cref="MRectangleInt"/> with the given Y-translation and returns the result.
        /// </summary>
        /// <param name="x">The translation along the Y-axis.</param>
        /// <returns>Translated <see cref="MRectangleInt"/></returns>
        public MRectangleInt TranslateY(int y)
        {
            return new MRectangleInt(this.TopLeft.X, this.TopLeft.Y + y, this.BottomRight.X - this.TopLeft.X, this.BottomRight.Y - this.TopLeft.Y);
        }
    }
}