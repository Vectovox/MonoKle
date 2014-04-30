namespace MonoKle.Core
{
    using System;
    using System.Text;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// Struct for storing an immutable, normalized (non-negative width and height), integer-based area.
    /// </summary>
    public struct AreaInteger : IEquatable<AreaInteger>
    {
        private const int HASH_CODE_INITIAL = 17;
        private const int HASH_CODE_MULTIPLIER = 23;

        private Vector2DInteger bottomRight;
        private Vector2DInteger topLeft;

        /// <summary>
        /// Creates a new instance of <see cref="AreaInteger"/> around (0, 0) with the given width and height.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public AreaInteger(int width, int height)
        {
            int xLeft = Math.Min(0, width);
            int xRight = Math.Max(0, width);
            int yTop = Math.Min(0, height);
            int yBottom = Math.Max(0, height);
            this.topLeft = new Vector2DInteger(xLeft, yTop);
            this.bottomRight = new Vector2DInteger(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="AreaInteger"/> by the provided coordinate and the given width and height.
        /// </summary>
        /// <param name="x">X-coordinate of a corner.</param>
        /// <param name="y">Y-coordinate of a corner.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public AreaInteger(int x, int y, int width, int height)
        {
            int xLeft = Math.Min(x, x + width);
            int xRight = Math.Max(x, x + width);
            int yTop = Math.Min(y, y + height);
            int yBottom = Math.Max(y, y + height);
            this.topLeft = new Vector2DInteger(xLeft, yTop);
            this.bottomRight = new Vector2DInteger(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="AreaInteger"/> around (0, 0) with the given size.
        /// </summary>
        /// <param name="coord">The coordinate.</param>
        public AreaInteger(Vector2DInteger size)
        {
            int xLeft = Math.Min(0, size.X);
            int xRight = Math.Max(0, size.X);
            int yTop = Math.Min(0, size.Y);
            int yBottom = Math.Max(0, size.Y);
            this.topLeft = new Vector2DInteger(xLeft, yTop);
            this.bottomRight = new Vector2DInteger(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="AreaInteger"/> bound by the provided coordinates.
        /// </summary>
        /// <param name="coordA">The first coordinate.</param>
        /// <param name="coordB">The second coordinate.</param>
        public AreaInteger(Vector2DInteger coordA, Vector2DInteger coordB)
        {
            int xLeft = Math.Min(coordA.X, coordB.X);
            int xRight = Math.Max(coordA.X, coordB.X);
            int yTop = Math.Min(coordA.Y, coordB.Y);
            int yBottom = Math.Max(coordA.Y, coordB.Y);
            this.topLeft = new Vector2DInteger(xLeft, yTop);
            this.bottomRight = new Vector2DInteger(xRight, yBottom);
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
        public Vector2DInteger BottomLeft
        {
            get { return new Vector2DInteger(this.topLeft.X, this.bottomRight.Y); }
        }

        /// <summary>
        /// Gets the bottom right corner.
        /// </summary>
        public Vector2DInteger BottomRight
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
        public Vector2DInteger TopLeft
        {
            get { return this.topLeft; }
        }

        /// <summary>
        /// Gets the top right corner.
        /// </summary>
        public Vector2DInteger TopRight
        {
            get { return new Vector2DInteger(this.bottomRight.X, this.topLeft.Y); }
        }

        /// <summary>
        /// Non-equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="AreaInteger"/>.</param>
        /// <param name="b">Right <see cref="AreaInteger"/>.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(AreaInteger a, AreaInteger b)
        {
            return a.topLeft != b.topLeft || a.bottomRight != b.bottomRight;
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="AreaInteger"/>.</param>
        /// <param name="b">Right <see cref="AreaInteger"/>.</param>
        /// <returns>True if are equal, else false.</returns>
        public static bool operator ==(AreaInteger a, AreaInteger b)
        {
            return a.topLeft == b.topLeft && a.bottomRight == b.bottomRight;
        }

        /// <summary>
        /// Clamps the provided <see cref="AreaInteger"/> to fit within.
        /// </summary>
        /// <param name="area">The <see cref="AreaInteger"/> to clamp.</param>
        public AreaInteger Clamp(AreaInteger area)
        {
            Vector2DInteger topLeft = this.Clamp(area.topLeft);
            Vector2DInteger bottomRight = this.Clamp(area.bottomRight);
            return new AreaInteger(topLeft, bottomRight);
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
        /// Clamps the provided <see cref="Vector2DInteger"/> to be positioned within this.
        /// </summary>
        /// <param name="coordinate">The <see cref="Vector2DInteger"/> to clamp.</param>
        public Vector2DInteger Clamp(Vector2DInteger coordinate)
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

            return new Vector2DInteger(x, y);
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
        /// Checks if contains the provided <see cref="AreaInteger"/>.
        /// </summary>
        /// <param name="area">The <see cref="AreaInteger"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="AreaInteger"/> is contained, otherwise false.</returns>
        public bool Contains(AreaInteger area)
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
        public bool Contains(Vector2DInteger coordinate)
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
            if(obj is AreaInteger)
            {
                return this == ((AreaInteger)obj);
            }
            return false;
        }

        /// <summary>
        /// Returns whether this is equal to the provided <see cref="AreaInteger"/>.
        /// </summary>
        /// <param name="other">The <see cref="AreaInteger"/> to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(AreaInteger other)
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
                int hash = AreaInteger.HASH_CODE_INITIAL;
                hash = hash * AreaInteger.HASH_CODE_MULTIPLIER + this.topLeft.GetHashCode();
                hash = hash * AreaInteger.HASH_CODE_MULTIPLIER + this.bottomRight.GetHashCode();
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