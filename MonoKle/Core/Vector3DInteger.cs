namespace MonoKle.Core
{
    using System;
    using System.Text;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// Three-dimensional Int32-based vector.
    /// </summary>
    public struct Vector3DInteger
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
        public Vector3DInteger(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Creates a new instance from a floating point vector, rounding down to composants to integer values.
        /// </summary>
        /// <param name="vector">The vector to copy values from.</param>
        public Vector3DInteger(Vector3 vector)
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
        public Vector3DInteger(Vector2DInteger xy, int z)
        {
            this.X = xy.X;
            this.Y = xy.Y;
            this.Z = z;
        }

        /// <summary>
        /// Gets a vector with all composant set to 1.
        /// </summary>
        public static Vector3DInteger One
        {
            get { return new Vector3DInteger(1, 1, 1); }
        }

        /// <summary>
        /// Gets a vector with all composant set to 0.
        /// </summary>
        public static Vector3DInteger Zero
        {
            get { return new Vector3DInteger(0, 0, 0); }
        }

#pragma warning disable 1591
        public static bool operator !=(Vector3DInteger a, Vector3DInteger b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public static Vector3DInteger operator *(Vector3DInteger a, int b)
        {
            return new Vector3DInteger(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3DInteger operator *(int b, Vector3DInteger a)
        {
            return new Vector3DInteger(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3DInteger operator +(Vector3DInteger a, Vector3DInteger b)
        {
            return new Vector3DInteger(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3DInteger operator -(Vector3DInteger a, Vector3DInteger b)
        {
            return new Vector3DInteger(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3DInteger operator /(Vector3DInteger a, int b)
        {
            return new Vector3DInteger(a.X / b, a.Y / b, a.Z / b);
        }

        public static Vector3DInteger operator /(int b, Vector3DInteger a)
        {
            return new Vector3DInteger(a.X / b, a.Y / b, a.Z / b);
        }

        public static bool operator ==(Vector3DInteger a, Vector3DInteger b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }
#pragma warning restore 1591

        /// <summary>
        /// Returns whether the <see cref="Vector3DInteger"/> is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if they are equal, else false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector3DInteger)
            {
                return this == (Vector3DInteger)obj;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code representation.
        /// </summary>
        /// <returns>Hash code representation.</returns>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode() * 7 + this.Z.GetHashCode() * 11;
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
            return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
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