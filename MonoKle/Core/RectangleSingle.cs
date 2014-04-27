namespace MonoKle.Core
{
    using System;
    using System.Text;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// Struct for storing an immutable, normalized (non-negative width and height), float-based rectangle.
    /// </summary>
    public struct RectangleSingle : IEquatable<RectangleSingle>
    {
        private const int HASH_CODE_INITIAL = 17;
        private const int HASH_CODE_MULTIPLIER = 23;

        private Vector2 bottomRight;
        private Vector2 topLeft;

        /// <summary>
        /// Creates a new instance of <see cref="RectangleSingle"/> from an integer based rectangle.
        /// </summary>
        /// <param name="rectangle">Integer based rectangle.</param>
        public RectangleSingle(Rectangle rectangle)
        {
            float xLeft = Math.Min(rectangle.X, rectangle.X + rectangle.Width);
            float xRight = Math.Max(rectangle.X, rectangle.X + rectangle.Width);
            float yTop = Math.Min(rectangle.Y, rectangle.Y + rectangle.Height);
            float yBottom = Math.Max(rectangle.Y, rectangle.Y + rectangle.Height);
            this.topLeft = new Vector2(xLeft, yTop);
            this.bottomRight = new Vector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="RectangleSingle"/> around (0, 0) with the given width and height.
        /// </summary>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        public RectangleSingle(float width, float height)
        {
            float xLeft = Math.Min(0, width);
            float xRight = Math.Max(0, width);
            float yTop = Math.Min(0, height);
            float yBottom = Math.Max(0, height);
            this.topLeft = new Vector2(xLeft, yTop);
            this.bottomRight = new Vector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="RectangleSingle"/> by the provided coordinate and the given width and height.
        /// </summary>
        /// <param name="x">X-coordinate of a corner.</param>
        /// <param name="y">Y-coordinate of a corner.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        public RectangleSingle(float x, float y, float width, float height)
        {
            float xLeft = Math.Min(x, x + width);
            float xRight = Math.Max(x, x + width);
            float yTop = Math.Min(y, y + height);
            float yBottom = Math.Max(y, y + height);
            this.topLeft = new Vector2(xLeft, yTop);
            this.bottomRight = new Vector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="RectangleSingle"/> bound by the provided coordinates.
        /// </summary>
        /// <param name="coordA">The first coordinate.</param>
        /// <param name="coordB">The second coordinate.</param>
        public RectangleSingle(Vector2 coordA, Vector2 coordB)
        {
            float xLeft = Math.Min(coordA.X, coordB.X);
            float xRight = Math.Max(coordA.X, coordB.X);
            float yTop = Math.Min(coordA.Y, coordB.Y);
            float yBottom = Math.Max(coordA.Y, coordB.Y);
            this.topLeft = new Vector2(xLeft, yTop);
            this.bottomRight = new Vector2(xRight, yBottom);
        }

        /// <summary>
        /// Non-equality operator.
        /// </summary>
        /// <param name="a">Left rectangle.</param>
        /// <param name="b">Right rectangle.</param>
        /// <returns>True if rectangles are not equal, else false.</returns>
        public static bool operator !=(RectangleSingle a, RectangleSingle b)
        {
            return a.topLeft != b.topLeft || a.bottomRight != b.bottomRight;
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="a">Left rectangle.</param>
        /// <param name="b">Right rectangle.</param>
        /// <returns>True if rectangles are equal, else false.</returns>
        public static bool operator ==(RectangleSingle a, RectangleSingle b)
        {
            return a.topLeft == b.topLeft && a.bottomRight == b.bottomRight;
        }

        /// <summary>
        /// Checks if the rectangle contains the provided rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to check if it is contained.</param>
        /// <returns>True if the specified rectangle is contained, otherwise false.</returns>
        public bool Contains(Rectangle rectangle)
        {
            return this.Contains(rectangle.GetTopLeft()) && this.Contains(rectangle.GetTopRight())
                && this.Contains(rectangle.GetBottomLeft()) && this.Contains(rectangle.GetBottomRight());
        }

        /// <summary>
        /// Checks if the rectangle contains the provided rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to check if it is contained.</param>
        /// <returns>True if the specified rectangle is contained, otherwise false.</returns>
        public bool Contains(RectangleSingle rectangle)
        {
            return this.Contains(rectangle.GetTopLeft()) && this.Contains(rectangle.GetTopRight())
                && this.Contains(rectangle.GetBottomLeft()) && this.Contains(rectangle.GetBottomRight());
        }

        /// <summary>
        /// Checks if the rectangle contains the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate to check if it is contained.</param>
        /// <returns>True if the specified coordinate is contained, otherwise false.</returns>
        public bool Contains(Vector2 coordinate)
        {
            return coordinate.X >= this.GetLeft() && coordinate.X <= this.GetRight()
                && coordinate.Y >= this.GetTop() && coordinate.Y <= this.GetBottom();
        }

        /// <summary>
        /// Checks if the rectangle contains the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate to check if it is contained.</param>
        /// <returns>True if the specified coordinate is contained, otherwise false.</returns>
        public bool Contains(Vector2Int32 coordinate)
        {
            return coordinate.X >= this.GetLeft() && coordinate.X <= this.GetRight()
                && coordinate.Y >= this.GetTop() && coordinate.Y <= this.GetBottom();
        }

        /// <summary>
        /// Returns the rectangle cropped to fit into the given bounds.
        /// </summary>
        /// <param name="bounds">The bounds to fit into.</param>
        public RectangleSingle Crop(RectangleSingle bounds)
        {
            float x = this.topLeft.X;
            float y = this.topLeft.Y;
            float x2 = this.bottomRight.X;
            float y2 = this.bottomRight.Y;

            if(x < bounds.topLeft.X)
            {
                x = bounds.topLeft.X;
            }
            else if(x > bounds.bottomRight.X)
            {
                x = bounds.bottomRight.X;
            }

            if(x2 < bounds.topLeft.X)
            {
                x2 = bounds.topLeft.X;
            }
            else if(x2 > bounds.bottomRight.X)
            {
                x2 = bounds.bottomRight.X;
            }

            if(y < bounds.topLeft.Y)
            {
                y = bounds.topLeft.Y;
            }
            else if(y > bounds.bottomRight.Y)
            {
                y = bounds.bottomRight.Y;
            }

            if(y2 < bounds.topLeft.Y)
            {
                y2 = bounds.topLeft.Y;
            }
            else if(y2 > bounds.bottomRight.Y)
            {
                y2 = bounds.bottomRight.Y;
            }

            return new RectangleSingle(x, y, x2 - x, y2 - y);
        }

        /// <summary>
        /// Returns whether this is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public override bool Equals(object obj)
        {
            if(obj is RectangleSingle)
            {
                return this == ((RectangleSingle)obj);
            }
            return false;
        }

        /// <summary>
        /// Returns whether this is equal to the provided <see cref="RectangleSingle"/>.
        /// </summary>
        /// <param name="other">The <see cref="RectangleSingle"/> to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(RectangleSingle other)
        {
            return this == other;
        }

        /// <summary>
        /// Returns the bottom of the rectangle.
        /// </summary>
        /// <returns></returns>
        public float GetBottom()
        {
            return this.bottomRight.Y;
        }

        /// <summary>
        /// Returns the bottom left corner.
        /// </summary>
        /// <returns>Bottom left corner.</returns>
        public Vector2 GetBottomLeft()
        {
            return new Vector2(this.topLeft.X, this.bottomRight.Y);
        }

        /// <summary>
        /// Returns the bottom right corner.
        /// </summary>
        /// <returns>Bottom right corner.</returns>
        public Vector2 GetBottomRight()
        {
            return bottomRight;
        }

        /// <summary>
        /// Returns the hash representation.
        /// </summary>
        /// <returns>Hash representation.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = RectangleSingle.HASH_CODE_INITIAL;
                hash = hash * RectangleSingle.HASH_CODE_MULTIPLIER + this.topLeft.GetHashCode();
                hash = hash * RectangleSingle.HASH_CODE_MULTIPLIER + this.bottomRight.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns the left of the rectangle.
        /// </summary>
        /// <returns></returns>
        public float GetLeft()
        {
            return this.topLeft.X;
        }

        /// <summary>
        /// Returns the right of the rectangle.
        /// </summary>
        /// <returns></returns>
        public float GetRight()
        {
            return this.bottomRight.X;
        }

        /// <summary>
        /// Returns the top of the rectangle.
        /// </summary>
        /// <returns></returns>
        public float GetTop()
        {
            return this.topLeft.Y;
        }

        /// <summary>
        /// Returns the top left corner.
        /// </summary>
        /// <returns>Top left corner.</returns>
        public Vector2 GetTopLeft()
        {
            return this.topLeft;
        }

        /// <summary>
        /// Returns the top right corner.
        /// </summary>
        /// <returns>Top right corner.</returns>
        public Vector2 GetTopRight()
        {
            return new Vector2(this.bottomRight.X, this.topLeft.Y);
        }

        /// <summary>
        /// Returns an integer based rectangle, rounded down by cutting of decimals.
        /// </summary>
        /// <returns>Integer based rectangle.</returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle((int)this.topLeft.X,
                (int)this.topLeft.Y,
                (int)(this.bottomRight.X - this.topLeft.X),
                (int)(this.bottomRight.Y - this.topLeft.Y)
                );
        }

        /// <summary>
        /// Returns the <see cref="string"/> representation.
        /// </summary>
        /// <returns><see cref="string"/> representation.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[Top Left: ");
            sb.Append(this.topLeft.ToString());
            sb.Append(", Bottom Right: ");
            sb.Append(this.bottomRight.ToString());
            sb.Append("]");
            return sb.ToString();
        }
    }
}