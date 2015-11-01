namespace MonoKle.Core
{
    using System;
    using System.Text;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// Struct for storing an immutable, serializable, normalized (non-negative width and height), integer-based area.
    /// </summary>
    [Serializable()]
    public struct IntArea : IEquatable<IntArea>
    {
        private const int HASH_CODE_INITIAL = 17;
        private const int HASH_CODE_MULTIPLIER = 23;

        private IntVector2 bottomRight;
        private IntVector2 topLeft;

        /// <summary>
        /// Creates a new instance of <see cref="IntArea"/> around (0, 0) with the given width and height.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public IntArea(int width, int height)
        {
            int xLeft = Math.Min(0, width);
            int xRight = Math.Max(0, width);
            int yTop = Math.Min(0, height);
            int yBottom = Math.Max(0, height);
            this.topLeft = new IntVector2(xLeft, yTop);
            this.bottomRight = new IntVector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="IntArea"/> by the provided coordinate and the given width and height.
        /// </summary>
        /// <param name="x">X-coordinate of a corner.</param>
        /// <param name="y">Y-coordinate of a corner.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public IntArea(int x, int y, int width, int height)
        {
            int xLeft = Math.Min(x, x + width);
            int xRight = Math.Max(x, x + width);
            int yTop = Math.Min(y, y + height);
            int yBottom = Math.Max(y, y + height);
            this.topLeft = new IntVector2(xLeft, yTop);
            this.bottomRight = new IntVector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="IntArea"/> around (0, 0) with the given size.
        /// </summary>
        /// <param name="coord">The coordinate.</param>
        public IntArea(IntVector2 size)
        {
            int xLeft = Math.Min(0, size.X);
            int xRight = Math.Max(0, size.X);
            int yTop = Math.Min(0, size.Y);
            int yBottom = Math.Max(0, size.Y);
            this.topLeft = new IntVector2(xLeft, yTop);
            this.bottomRight = new IntVector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="IntArea"/> bound by the provided coordinates.
        /// </summary>
        /// <param name="coordA">The first coordinate.</param>
        /// <param name="coordB">The second coordinate.</param>
        public IntArea(IntVector2 coordA, IntVector2 coordB)
        {
            int xLeft = Math.Min(coordA.X, coordB.X);
            int xRight = Math.Max(coordA.X, coordB.X);
            int yTop = Math.Min(coordA.Y, coordB.Y);
            int yBottom = Math.Max(coordA.Y, coordB.Y);
            this.topLeft = new IntVector2(xLeft, yTop);
            this.bottomRight = new IntVector2(xRight, yBottom);
        }

        /// <summary>
        /// Translates the <see cref="IntArea"/> with the given translation and returns the result.
        /// </summary>
        /// <param name="translation">The translation to make.</param>
        /// <returns>Translated <see cref="IntArea"/></returns>
        public IntArea Translate(IntVector2 translation)
        {
            return new IntArea(this.topLeft.X + translation.X, this.topLeft.Y + translation.Y, this.bottomRight.X - this.topLeft.X, this.bottomRight.Y - this.topLeft.Y);
        }

        /// <summary>
        /// Translates the <see cref="IntArea"/> with the given X-translation and returns the result.
        /// </summary>
        /// <param name="x">The translation along the X-axis.</param>
        /// <returns>Translated <see cref="IntArea"/></returns>
        public IntArea TranslateX(int x)
        {
            return new IntArea(this.topLeft.X + x, this.topLeft.Y, this.bottomRight.X - this.topLeft.X, this.bottomRight.Y - this.topLeft.Y);
        }

        /// <summary>
        /// Translates the <see cref="IntArea"/> with the given Y-translation and returns the result.
        /// </summary>
        /// <param name="x">The translation along the Y-axis.</param>
        /// <returns>Translated <see cref="IntArea"/></returns>
        public IntArea TranslateY(int y)
        {
            return new IntArea(this.topLeft.X, this.topLeft.Y + y, this.bottomRight.X - this.topLeft.X, this.bottomRight.Y - this.topLeft.Y);
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Width
        {
            get { return this.bottomRight.X - this.topLeft.X; }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Height
        {
            get { return this.bottomRight.Y - this.topLeft.Y; }
        }

        /// <summary>
        /// Gets the Y-coordinate of the bottom border.
        /// </summary>
        public int Bottom
        {
            get { return this.bottomRight.Y; }
        }

        /// <summary>
        /// Gets the bottom left corner.
        /// </summary>
        public IntVector2 BottomLeft
        {
            get { return new IntVector2(this.topLeft.X, this.bottomRight.Y); }
        }

        /// <summary>
        /// Gets the bottom right corner.
        /// </summary>
        public IntVector2 BottomRight
        {
            get { return bottomRight; }
        }

        /// <summary>
        /// Gets the X-coordinate of the left border.
        /// </summary>
        public int Left
        {
            get { return this.topLeft.X; }
        }

        /// <summary>
        /// Gets the X-coordinate of the right border.
        /// </summary>
        public int Right
        {
            get { return this.bottomRight.X; }
        }

        /// <summary>
        /// Gets the Y-coordinate of the top border.
        /// </summary>
        public int Top
        {
            get { return this.topLeft.Y; }
        }

        /// <summary>
        /// Gets the top left corner.
        /// </summary>
        public IntVector2 TopLeft
        {
            get { return this.topLeft; }
        }

        /// <summary>
        /// Gets the top right corner.
        /// </summary>
        public IntVector2 TopRight
        {
            get { return new IntVector2(this.bottomRight.X, this.topLeft.Y); }
        }

        /// <summary>
        /// Non-equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="IntArea"/>.</param>
        /// <param name="b">Right <see cref="IntArea"/>.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(IntArea a, IntArea b)
        {
            return a.topLeft != b.topLeft || a.bottomRight != b.bottomRight;
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="IntArea"/>.</param>
        /// <param name="b">Right <see cref="IntArea"/>.</param>
        /// <returns>True if are equal, else false.</returns>
        public static bool operator ==(IntArea a, IntArea b)
        {
            return a.topLeft == b.topLeft && a.bottomRight == b.bottomRight;
        }

        /// <summary>
        /// Clamps the provided <see cref="IntArea"/> to fit within.
        /// </summary>
        /// <param name="area">The <see cref="IntArea"/> to clamp.</param>
        public IntArea Clamp(IntArea area)
        {
            IntVector2 topLeft = this.Clamp(area.topLeft);
            IntVector2 bottomRight = this.Clamp(area.bottomRight);
            return new IntArea(topLeft, bottomRight);
        }

        /// <summary>
        /// Clamps the provided <see cref="Area"/> to fit within.
        /// </summary>
        /// <param name="area">The <see cref="Area"/> to clamp.</param>
        public Area Clamp(Area area)
        {
            Vector2 topLeft = this.Clamp(area.TopLeft);
            Vector2 bottomRight = this.Clamp(area.BottomRight);
            return new Area(topLeft, bottomRight);
        }

        /// <summary>
        /// Clamps the provided <see cref="IntVector2"/> to be positioned within this.
        /// </summary>
        /// <param name="coordinate">The <see cref="IntVector2"/> to clamp.</param>
        public IntVector2 Clamp(IntVector2 coordinate)
        {
            int x = coordinate.X;
            int y = coordinate.Y;

            if(x < this.topLeft.X)
            {
                x = this.topLeft.X;
            }
            else if(x > this.bottomRight.X)
            {
                x = this.bottomRight.X;
            }

            if(y < this.topLeft.Y)
            {
                y = this.topLeft.Y;
            }
            else if(y > this.bottomRight.Y)
            {
                y = this.bottomRight.Y;
            }

            return new IntVector2(x, y);
        }

        /// <summary>
        /// Clamps the provided <see cref="Vector2"/> to be positioned within this.
        /// </summary>
        /// <param name="coordinate">The <see cref="Vector2"/> to clamp.</param>
        public Vector2 Clamp(Vector2 coordinate)
        {
            float x = coordinate.X;
            float y = coordinate.Y;

            if(x < this.topLeft.X)
            {
                x = this.topLeft.X;
            }
            else if(x > this.bottomRight.X)
            {
                x = this.bottomRight.X;
            }

            if(y < this.topLeft.Y)
            {
                y = this.topLeft.Y;
            }
            else if(y > this.bottomRight.Y)
            {
                y = this.bottomRight.Y;
            }

            return new Vector2(x, y);
        }

        /// <summary>
        /// Checks if contains the provided <see cref="IntArea"/>.
        /// </summary>
        /// <param name="area">The <see cref="IntArea"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="IntArea"/> is contained, otherwise false.</returns>
        public bool Contains(IntArea area)
        {
            return this.Contains(area.TopLeft) && this.Contains(area.TopRight)
                && this.Contains(area.BottomLeft) && this.Contains(area.BottomRight);
        }

        /// <summary>
        /// Checks if contains the provided <see cref="Area"/>.
        /// </summary>
        /// <param name="area">The <see cref="Area"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="Area"/> is contained, otherwise false.</returns>
        public bool Contains(Area area)
        {
            return this.Contains(area.TopLeft) && this.Contains(area.TopRight)
                && this.Contains(area.BottomLeft) && this.Contains(area.BottomRight);
        }

        /// <summary>
        /// Checks if contains the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate to check if it is contained.</param>
        /// <returns>True if the specified coordinate is contained, otherwise false.</returns>
        public bool Contains(Vector2 coordinate)
        {
            return coordinate.X >= this.Left && coordinate.X <= this.Right
                && coordinate.Y >= this.Top && coordinate.Y <= this.Bottom;
        }

        /// <summary>
        /// Checks if contains the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate to check if it is contained.</param>
        /// <returns>True if the specified coordinate is contained, otherwise false.</returns>
        public bool Contains(IntVector2 coordinate)
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
            if(obj is IntArea)
            {
                return this == ((IntArea)obj);
            }
            return false;
        }

        /// <summary>
        /// Returns whether this is equal to the provided <see cref="IntArea"/>.
        /// </summary>
        /// <param name="other">The <see cref="IntArea"/> to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(IntArea other)
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
                int hash = IntArea.HASH_CODE_INITIAL;
                hash = hash * IntArea.HASH_CODE_MULTIPLIER + this.topLeft.GetHashCode();
                hash = hash * IntArea.HASH_CODE_MULTIPLIER + this.bottomRight.GetHashCode();
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
            sb.Append(this.topLeft.ToString());
            sb.Append(", Bottom Right: ");
            sb.Append(this.bottomRight.ToString());
            sb.Append("}");
            return sb.ToString();
        }
    }
}