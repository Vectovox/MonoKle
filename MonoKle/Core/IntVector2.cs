namespace MonoKle.Core
{
    using Microsoft.Xna.Framework;
    using System;
    using System.Text;

    /// <summary>
    /// Two-dimensional, immutable, serializable, integer-based vector.
    /// </summary>
    [Serializable()]
    public struct IntVector2 : IEquatable<IntVector2>
    {
        /// <summary>
        /// The x coordinate.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// The y coordinate.
        /// </summary>
        public readonly int Y;

        private const int HASH_CODE_INITIAL = 73;
        private const int HASH_CODE_MULTIPLIER = 101;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="x">X-coordinate.</param>
        /// <param name="y">Y-coordinate.</param>
        public IntVector2(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Creates a new instance from a floating point vector, rounding down the composants to integer values.
        /// </summary>
        /// <param name="vector">The vector to copy values from.</param>
        public IntVector2(Vector2 vector)
        {
            this.X = (int)vector.X;
            this.Y = (int)vector.Y;
        }

        /// <summary>
        /// Gets a vector with all composant set to 1.
        /// </summary>
        public static IntVector2 One
        {
            get { return new IntVector2(1, 1); }
        }

        /// <summary>
        /// Gets a vector with all composant set to 0.
        /// </summary>
        public static IntVector2 Zero
        {
            get { return new IntVector2(0, 0); }
        }

        /// <summary>
        /// Operator for subtraction with another <see cref="IntVector2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Added result.</returns>
        public static IntVector2 operator -(IntVector2 a, IntVector2 b)
        {
            return new IntVector2(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Logic operator for non-equality.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(IntVector2 a, IntVector2 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        /// <summary>
        /// Operator for multiplication with scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static IntVector2 operator *(IntVector2 a, int b)
        {
            return new IntVector2(a.X * b, a.Y * b);
        }

        /// <summary>
        /// Operator for multiplication with scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static IntVector2 operator *(int b, IntVector2 a)
        {
            return new IntVector2(a.X * b, a.Y * b);
        }

        /// <summary>
        /// Operator for division with a scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Divided result.</returns>
        public static IntVector2 operator /(IntVector2 a, int b)
        {
            return new IntVector2(a.X / b, a.Y / b);
        }

        /// <summary>
        /// Operator for addition with another <see cref="IntVector2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Added result.</returns>
        public static IntVector2 operator +(IntVector2 a, IntVector2 b)
        {
            return new IntVector2(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Logic operator for equality.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>True if equal, else false.</returns>
        public static bool operator ==(IntVector2 a, IntVector2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        /// <summary>
        /// Returns whether the <see cref="IntVector2"/> is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if they are equal, else false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is IntVector2)
            {
                return this == (IntVector2)obj;
            }
            return false;
        }

        /// <summary>
        /// Returns equality to another <see cref="IntVector2"/>.
        /// </summary>
        /// <param name="other">Another <see cref="IntVector2"/>.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(IntVector2 other)
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
                int hash = IntVector2.HASH_CODE_INITIAL;
                hash = hash * IntVector2.HASH_CODE_MULTIPLIER + this.X;
                hash = hash * IntVector2.HASH_CODE_MULTIPLIER + this.Y;
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

        /// <summary>
        /// Translates with the specified delta.
        /// </summary>
        /// <param name="dx">The x translation.</param>
        /// <param name="dy">The y translation.</param>
        /// <returns></returns>
        public IntVector2 Translate(int dx, int dy)
        {
            return new IntVector2(this.X + dx, this.Y + dy);
        }

        /// <summary>
        /// Translates with the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns></returns>
        public IntVector2 Translate(IntVector2 delta)
        {
            return new IntVector2(this.X + delta.X, this.Y + delta.Y);
        }
    }
}