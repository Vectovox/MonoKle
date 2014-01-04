namespace MonoKle.Core
{
    using System.Text;

    using Microsoft.Xna.Framework;

    // TODO: Implement GetHashCode()...
    /// <summary>
    /// Three-dimensional Int32-based vector.
    /// </summary>
    public struct Vector3Int32
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
        /// Z-coordinate.
        /// </summary>
        public int Z;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="x">X-coordinate.</param>
        /// <param name="y">Y-coordinate.</param>
        /// <param name="z">Z-coordinate.</param>
        public Vector3Int32(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Creates a new instance from a floating point vector, rounding down to composants to integer values.
        /// </summary>
        /// <param name="vector">The vector to copy values from.</param>
        public Vector3Int32(Vector3 vector)
        {
            this.X = (int)vector.X;
            this.Y = (int)vector.Y;
            this.Z = (int)vector.Z;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="xy">The XY-coordinate.</param>
        /// <param name="z">Z-coordinate.</param>
        public Vector3Int32(Vector2Int32 xy, int z)
        {
            this.X = xy.X;
            this.Y = xy.Y;
            this.Z = z;
        }

        /// <summary>
        /// Gets a vector with all composant set to 1.
        /// </summary>
        public static Vector3Int32 One
        {
            get { return new Vector3Int32(1, 1, 1); }
        }

        /// <summary>
        /// Gets a vector with all composant set to 0.
        /// </summary>
        public static Vector3Int32 Zero
        {
            get { return new Vector3Int32(0, 0, 0); }
        }

        public static bool operator !=(Vector3Int32 a, Vector3Int32 b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public static Vector3Int32 operator *(Vector3Int32 a, int b)
        {
            return new Vector3Int32(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3Int32 operator *(int b, Vector3Int32 a)
        {
            return new Vector3Int32(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3Int32 operator +(Vector3Int32 a, Vector3Int32 b)
        {
            return new Vector3Int32(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3Int32 operator -(Vector3Int32 a, Vector3Int32 b)
        {
            return new Vector3Int32(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3Int32 operator /(Vector3Int32 a, int b)
        {
            return new Vector3Int32(a.X / b, a.Y / b, a.Z / b);
        }

        public static Vector3Int32 operator /(int b, Vector3Int32 a)
        {
            return new Vector3Int32(a.X / b, a.Y / b, a.Z / b);
        }

        public static bool operator ==(Vector3Int32 a, Vector3Int32 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3Int32)
            {
                return this == (Vector3Int32)obj;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode() * 7 + this.Z.GetHashCode() * 11;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("( ");
            sb.Append(this.X);
            sb.Append(", ");
            sb.Append(this.Y);
            sb.Append(", ");
            sb.Append(this.Z);
            sb.Append(" )");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the Vector3 representation.
        /// </summary>
        /// <returns>Vector3 representation.</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(this.X, this.Y, this.Z);
        }
    }
}