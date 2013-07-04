namespace MonoKle.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Xna.Framework;

    // TODO: Implement GetHashCode()...
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