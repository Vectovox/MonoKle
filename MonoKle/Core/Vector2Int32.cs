namespace MonoKle.Core
{
    using System.Text;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// Two-dimensional Int32-based vector.
    /// </summary>
    public struct Vector2Int32
    {
        /// <summary>
        /// X-coordinate.
        /// </summary>
        public int X;

        /// <summary>
        /// Y-coordinate.
        /// </summary>
        public int Y;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="x">X-coordinate.</param>
        /// <param name="y">Y-coordinate.</param>
        public Vector2Int32(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Creates a new instance from a floating point vector, rounding down the composants to integer values.
        /// </summary>
        /// <param name="vector">The vector to copy values from.</param>
        public Vector2Int32(Vector2 vector)
        {
            this.X = (int)vector.X;
            this.Y = (int)vector.Y;
        }

        /// <summary>
        /// Gets a vector with all composant set to 1.
        /// </summary>
        public static Vector2Int32 One
        {
            get { return new Vector2Int32(1, 1); }
        }

        /// <summary>
        /// Gets a vector with all composant set to 0.
        /// </summary>
        public static Vector2Int32 Zero
        {
            get { return new Vector2Int32(0, 0); }
        }

        public static bool operator !=(Vector2Int32 a, Vector2Int32 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public static Vector2Int32 operator *(Vector2Int32 a, int b)
        {
            return new Vector2Int32(a.X * b, a.Y * b);
        }

        public static Vector2Int32 operator *(int b, Vector2Int32 a)
        {
            return new Vector2Int32(a.X * b, a.Y * b);
        }

        public static Vector2Int32 operator +(Vector2Int32 a, Vector2Int32 b)
        {
            return new Vector2Int32(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2Int32 operator -(Vector2Int32 a, Vector2Int32 b)
        {
            return new Vector2Int32(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2Int32 operator /(Vector2Int32 a, int b)
        {
            return new Vector2Int32(a.X / b, a.Y / b);
        }

        public static Vector2Int32 operator /(int b, Vector2Int32 a)
        {
            return new Vector2Int32(a.X / b, a.Y / b);
        }

        public static bool operator ==(Vector2Int32 a, Vector2Int32 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2Int32)
            {
                return this == (Vector2Int32)obj;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode() * 7;
        }

        /// <summary>
        /// Returns wether the vector, as a coordinate, is within the area created by (0, 0) and the given bottom right corner.
        /// </summary>
        /// <param name="bottomRight">The bottom right corner.</param>
        /// <returns>True if within the area, else false.</returns>
        public bool IsWithin(Vector2Int32 bottomRight)
        {
            return this.IsWithin(Vector2Int32.Zero, bottomRight);
        }

        /// <summary>
        /// Returns wether the vector, as a coordinate, is within the area created by the given top left and bottom right corner.
        /// </summary>
        /// <param name="bottomRight">The bottom right corner.</param>
        /// <param name="topLeft">The top left corner.</param>
        /// <returns>True if within the area, else false.</returns>
        public bool IsWithin(Vector2Int32 topLeft, Vector2Int32 bottomRight)
        {
            return this.X >= topLeft.X && this.X <= bottomRight.X && this.Y >= topLeft.Y && this.Y <= bottomRight.Y;
        }

        /// <summary>
        /// Returns wether the vector, as a coordinate, is within the given area.
        /// </summary>
        /// <param name="area">The area to check for.</param>
        /// <returns>True if within the area, else false.</returns>
        public bool IsWithin(Rectangle area)
        {
            return this.IsWithin(new Vector2Int32(area.Left, area.Top), new Vector2Int32(area.Right, area.Bottom));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("( ");
            sb.Append(this.X);
            sb.Append(", ");
            sb.Append(this.Y);
            sb.Append(" )");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the Vector2 representation.
        /// </summary>
        /// <returns>Vector2 representation.</returns>
        public Vector2 ToVector2()
        {
            return new Vector2(this.X, this.Y);
        }
    }
}