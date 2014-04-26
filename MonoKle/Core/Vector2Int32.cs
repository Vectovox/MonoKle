namespace MonoKle.Core
{
    using System;
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

#pragma warning disable 1591
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

        public static bool operator ==(Vector2Int32 a, Vector2Int32 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns whether the <see cref="Vector2Int32"/> is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if they are equal, else false.</returns>
        public override bool Equals(object obj)
        {
            if(obj is Vector2Int32)
            {
                return this == (Vector2Int32)obj;
            }
            return false;
        }

        /// <summary>
        /// Returns wether the vector, as a coordinate, is within the area created by (0, 0) and the given coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>True if within the area, else false.</returns>
        public bool IsWithin(Vector2Int32 coordinate)
        {
            return this.IsWithin(Vector2Int32.Zero, coordinate);
        }

        /// <summary>
        /// Returns wether the vector, as a coordinate, is within the area created by (0, 0) and the given coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>True if within the area, else false.</returns>
        public bool IsWithin(Vector2 coordinate)
        {
            return this.IsWithin(Vector2.Zero, coordinate);
        }

        /// <summary>
        /// Returns wether the vector, as a coordinate, is within the area created by the given coordinates.
        /// </summary>
        /// <param name="coordinateA">The first coordinate.</param>
        /// <param name="coordinateB">The second coordinate.</param>
        /// <returns>True if within the area, else false.</returns>
        public bool IsWithin(Vector2Int32 coordinateA, Vector2Int32 coordinateB)
        {
            int left = Math.Min(coordinateA.X, coordinateB.X);
            int right = Math.Max(coordinateA.X, coordinateB.X);
            int top = Math.Min(coordinateA.Y, coordinateB.Y);
            int bottom = Math.Max(coordinateA.Y, coordinateB.Y);

            return this.X >= left && this.X <= right && this.Y >= top && this.Y <= bottom;
        }

        /// <summary>
        /// Returns wether the vector, as a coordinate, is within the area created by the given coordinates.
        /// </summary>
        /// <param name="coordinateA">The first coordinate.</param>
        /// <param name="coordinateB">The second coordinate.</param>
        /// <returns>True if within the area, else false.</returns>
        public bool IsWithin(Vector2 coordinateA, Vector2 coordinateB)
        {
            float left = Math.Min(coordinateA.X, coordinateB.X);
            float right = Math.Max(coordinateA.X, coordinateB.X);
            float top = Math.Min(coordinateA.Y, coordinateB.Y);
            float bottom = Math.Max(coordinateA.Y, coordinateB.Y);

            return this.X >= left && this.X <= right && this.Y >= top && this.Y <= bottom;
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

        /// <summary>
        /// Returns wether the vector, as a coordinate, is within the given area.
        /// </summary>
        /// <param name="area">The area to check for.</param>
        /// <returns>True if within the area, else false.</returns>
        public bool IsWithin(RectangleSingle area)
        {
            return this.IsWithin(area.GetTopLeft(), area.GetBottomRight());
        }

        /// <summary>
        /// Returns the length of the vector.
        /// </summary>
        /// <returns>Length of the vector.</returns>
        public double Length()
        {
            return Math.Sqrt(this.LengthSquared());
        }

        /// <summary>
        /// Returns the squared length of the vector. Faster than <see cref="Length()"/>.
        /// </summary>
        /// <returns>Squared length of the vector.</returns>
        public double LengthSquared()
        {
            return this.X * this.X + this.Y * this.Y;
        }

        /// <summary>
        /// Returns the string representation.
        /// </summary>
        /// <returns>String representation.</returns>
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