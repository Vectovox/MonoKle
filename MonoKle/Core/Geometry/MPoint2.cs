namespace MonoKle.Core.Geometry
{
    using Microsoft.Xna.Framework;
    using System;
    using System.Text;

    /// <summary>
    /// Two-dimensional, immutable, serializable, integer-based point.
    /// </summary>
    [Serializable()]
    public struct MPoint2 : IEquatable<MPoint2>
    {
        /// <summary>
        /// The X component of the <see cref="MPoint2"/>.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// The Y component of the <see cref="MPoint2"/>.
        /// </summary>
        public readonly int Y;

        private const int HASH_CODE_INITIAL = 73;
        private const int HASH_CODE_MULTIPLIER = 101;

        /// <summary>
        /// Initializes a new instance of the <see cref="MPoint2"/> struct.
        /// </summary>
        /// <param name="x">X component.</param>
        /// <param name="y">Y component.</param>
        public MPoint2(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MPoint2"/> struct from the <see cref="MVector2"/> vector, rounding down the components to integer values.
        /// </summary>
        /// <param name="vector">The original vector.</param>
        public MPoint2(MVector2 vector)
        {
            this.X = (int)vector.X;
            this.Y = (int)vector.Y;
        }

        /// <summary>
        /// Gets a <see cref="MPoint2"/> with all components set to 1.
        /// </summary>
        public static MPoint2 One
        {
            get { return new MPoint2(1, 1); }
        }

        /// <summary>
        /// Gets a <see cref="MPoint2"/> with all components set to 0.
        /// </summary>
        public static MPoint2 Zero
        {
            get { return new MPoint2(0, 0); }
        }

        /// <summary>
        /// Gets the <see cref="Microsoft.Xna.Framework.Point"/> representation of the <see cref="MPoint2"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Microsoft.Xna.Framework.Point"/> representation.
        /// </value>
        public Point Point { get { return this; } }

        /// <summary>
        /// Operator for subtraction with another <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Added result.</returns>
        public static MPoint2 operator -(MPoint2 a, MPoint2 b)
        {
            return new MPoint2(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Logic operator for non-equality.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(MPoint2 a, MPoint2 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        /// <summary>
        /// Operator for multiplication with scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static MPoint2 operator *(MPoint2 a, int b)
        {
            return new MPoint2(a.X * b, a.Y * b);
        }

        /// <summary>
        /// Operator for multiplication with scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static MPoint2 operator *(int b, MPoint2 a)
        {
            return new MPoint2(a.X * b, a.Y * b);
        }

        /// <summary>
        /// Operator for division with a scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Divided result.</returns>
        public static MPoint2 operator /(MPoint2 a, int b)
        {
            return new MPoint2(a.X / b, a.Y / b);
        }

        /// <summary>
        /// Operator for addition with another <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Added result.</returns>
        public static MPoint2 operator +(MPoint2 a, MPoint2 b)
        {
            return new MPoint2(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Logic operator for equality.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>True if equal, else false.</returns>
        public static bool operator ==(MPoint2 a, MPoint2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Point"/> to <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator MPoint2(Point p)
        {
            return new MPoint2(p.X, p.Y);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="MPoint2"/> to <see cref="Point"/>.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Point(MPoint2 p)
        {
            return new Point(p.X, p.Y);
        }

        /// <summary>
        /// Returns whether the <see cref="MPoint2"/> is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if they are equal, else false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is MPoint2)
            {
                return this == (MPoint2)obj;
            }
            return false;
        }

        /// <summary>
        /// Returns equality to another <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="other">Another <see cref="MPoint2"/>.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(MPoint2 other)
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
                int hash = MPoint2.HASH_CODE_INITIAL;
                hash = hash * MPoint2.HASH_CODE_MULTIPLIER + this.X;
                hash = hash * MPoint2.HASH_CODE_MULTIPLIER + this.Y;
                return hash;
            }
        }

        /// <summary>
        /// Returns the length of the <see cref="MPoint2"/>, the distance to <see cref="Zero"/>.
        /// </summary>
        /// <returns>Length of the <see cref="MPoint2"/>.</returns>
        public double Length()
        {
            return Math.Sqrt(this.LengthSquared());
        }

        /// <summary>
        /// Returns the squared length of the <see cref="MPoint2"/>, the distance to <see cref="Zero"/>. Faster than <see cref="Length()"/>.
        /// </summary>
        /// <returns>Squared length of the <see cref="MPoint2"/>.</returns>
        public double LengthSquared()
        {
            return this.X * this.X + this.Y * this.Y;
        }

        /// <summary>
        /// Returns the <see cref="MVector2"/> representation.
        /// </summary>
        /// <returns><see cref="MVector2"/> representation.</returns>
        public MVector2 ToMVector2()
        {
            return new MVector2(this);
        }

        /// <summary>
        /// Returns the string representation of the <see cref="MPoint2"/>.
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
        /// Translates the <see cref="MPoint2"/> with the specified delta.
        /// </summary>
        /// <param name="dx">The x translation.</param>
        /// <param name="dy">The y translation.</param>
        /// <returns></returns>
        public MPoint2 Translate(int dx, int dy)
        {
            return new MPoint2(this.X + dx, this.Y + dy);
        }

        /// <summary>
        /// Translates the <see cref="MPoint2"/> with the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns></returns>
        public MPoint2 Translate(MPoint2 delta)
        {
            return new MPoint2(this.X + delta.X, this.Y + delta.Y);
        }
    }
}