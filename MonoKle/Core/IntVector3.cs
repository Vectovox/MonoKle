namespace MonoKle.Core
{
    using Microsoft.Xna.Framework;
    using System;
    using System.Text;

    /// <summary>
    /// Three-dimensional, immutable, serializable, integer-based vector.
    /// </summary>
    [Serializable()]
    public struct IntVector3
    {
        /// <summary>
        /// X-coordinate.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// Y-coordinate.
        /// </summary>
        public readonly int Y;

        /// <summary>
        /// Z-coordinate.
        /// </summary>
        public readonly int Z;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="x">X-coordinate.</param>
        /// <param name="y">Y-coordinate.</param>
        /// <param name="z">Z-coordinate.</param>
        public IntVector3(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Creates a new instance from a floating point vector, rounding down to composants to integer values.
        /// </summary>
        /// <param name="vector">The vector to copy values from.</param>
        public IntVector3(Vector3 vector)
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
        public IntVector3(IntVector2 xy, int z)
        {
            this.X = xy.X;
            this.Y = xy.Y;
            this.Z = z;
        }

        /// <summary>
        /// Gets a vector with all composant set to 1.
        /// </summary>
        public static IntVector3 One
        {
            get { return new IntVector3(1, 1, 1); }
        }

        /// <summary>
        /// Gets a vector with all composant set to 0.
        /// </summary>
        public static IntVector3 Zero
        {
            get { return new IntVector3(0, 0, 0); }
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IntVector3 operator -(IntVector3 a, IntVector3 b)
        {
            return new IntVector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(IntVector3 a, IntVector3 b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IntVector3 operator *(IntVector3 a, int b)
        {
            return new IntVector3(a.X * b, a.Y * b, a.Z * b);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="a">a.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IntVector3 operator *(int b, IntVector3 a)
        {
            return new IntVector3(a.X * b, a.Y * b, a.Z * b);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IntVector3 operator /(IntVector3 a, int b)
        {
            return new IntVector3(a.X / b, a.Y / b, a.Z / b);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="a">a.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IntVector3 operator /(int b, IntVector3 a)
        {
            return new IntVector3(a.X / b, a.Y / b, a.Z / b);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static IntVector3 operator +(IntVector3 a, IntVector3 b)
        {
            return new IntVector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(IntVector3 a, IntVector3 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        /// <summary>
        /// Returns whether the <see cref="IntVector3"/> is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if they are equal, else false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is IntVector3)
            {
                return this == (IntVector3)obj;
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

        /// <summary>
        /// Translates with the specified delta.
        /// </summary>
        /// <param name="dx">The x translation.</param>
        /// <param name="dy">The y translation.</param>
        /// <param name="dz">The z translation.</param>
        /// <returns></returns>
        public IntVector3 Translate(int dx, int dy, int dz)
        {
            return new IntVector3(this.X + dx, this.Y + dy, this.Z + dz);
        }

        /// <summary>
        /// Translates with the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns></returns>
        public IntVector3 Translate(IntVector3 delta)
        {
            return new IntVector3(this.X + delta.X, this.Y + delta.Y, this.Z + delta.Z);
        }
    }
}