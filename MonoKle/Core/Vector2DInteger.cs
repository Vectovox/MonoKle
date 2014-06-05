namespace MonoKle.Core
{
    using System;
    using System.Text;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// Two-dimensional, immutable, serializable, integer-based vector.
    /// </summary>
    [Serializable()]
    public struct Vector2DInteger : IEquatable<Vector2DInteger>
    {
        private const int HASH_CODE_INITIAL = 73;
        private const int HASH_CODE_MULTIPLIER = 101;

        private int x;
        private int y;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="x">X-coordinate.</param>
        /// <param name="y">Y-coordinate.</param>
        public Vector2DInteger(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Creates a new instance from a floating point vector, rounding down the composants to integer values.
        /// </summary>
        /// <param name="vector">The vector to copy values from.</param>
        public Vector2DInteger(Vector2 vector)
        {
            this.x = (int)vector.X;
            this.y = (int)vector.Y;
        }

        /// <summary>
        /// Gets a vector with all composant set to 1.
        /// </summary>
        public static Vector2DInteger One
        {
            get { return new Vector2DInteger(1, 1); }
        }

        /// <summary>
        /// Gets a vector with all composant set to 0.
        /// </summary>
        public static Vector2DInteger Zero
        {
            get { return new Vector2DInteger(0, 0); }
        }

        /// <summary>
        /// X-coordinate.
        /// </summary>
        public int X
        {
            get { return this.x; }
        }

        /// <summary>
        /// Y-coordinate.
        /// </summary>
        public int Y
        {
            get { return this.y; }
        }

        /// <summary>
        /// Logic operator for non-equality.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(Vector2DInteger a, Vector2DInteger b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        /// <summary>
        /// Operator for multiplication with scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static Vector2DInteger operator *(Vector2DInteger a, int b)
        {
            return new Vector2DInteger(a.X * b, a.Y * b);
        }

        /// <summary>
        /// Operator for multiplication with scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static Vector2DInteger operator *(int b, Vector2DInteger a)
        {
            return new Vector2DInteger(a.X * b, a.Y * b);
        }

        /// <summary>
        /// Operator for addition with another <see cref="Vector2DInteger"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Added result.</returns>
        public static Vector2DInteger operator +(Vector2DInteger a, Vector2DInteger b)
        {
            return new Vector2DInteger(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Operator for subtraction with another <see cref="Vector2DInteger"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Added result.</returns>
        public static Vector2DInteger operator -(Vector2DInteger a, Vector2DInteger b)
        {
            return new Vector2DInteger(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Operator for division with a scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Divided result.</returns>
        public static Vector2DInteger operator /(Vector2DInteger a, int b)
        {
            return new Vector2DInteger(a.X / b, a.Y / b);
        }

        /// <summary>
        /// Logic operator for equality.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>True if equal, else false.</returns>
        public static bool operator ==(Vector2DInteger a, Vector2DInteger b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        /// <summary>
        /// Returns whether the <see cref="Vector2DInteger"/> is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if they are equal, else false.</returns>
        public override bool Equals(object obj)
        {
            if(obj is Vector2DInteger)
            {
                return this == (Vector2DInteger)obj;
            }
            return false;
        }

        /// <summary>
        /// Returns equality to another <see cref="Vector2DInteger"/>.
        /// </summary>
        /// <param name="other">Another <see cref="Vector2DInteger"/>.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(Vector2DInteger other)
        {
            return this == other;
        }

        /// <summary>
        /// Returns the hash code representation.
        /// </summary>
        /// <returns>Hash code representation.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = Vector2DInteger.HASH_CODE_INITIAL;
                hash = hash * Vector2DInteger.HASH_CODE_MULTIPLIER + this.X;
                hash = hash * Vector2DInteger.HASH_CODE_MULTIPLIER + this.Y;
                return hash;
            }
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