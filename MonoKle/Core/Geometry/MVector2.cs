namespace MonoKle.Core.Geometry
{
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Two-dimensional, immutable, serializable, floating-point vector. Has implicit operator to <see cref="Microsoft.Xna.Framework.Vector2"/>.
    /// </summary>
    [Serializable()]
    public struct MVector2 : IEquatable<MVector2>
    {
        /// <summary>
        /// The X component of the <see cref="MVector2"/>.
        /// </summary>
        public readonly float X;

        /// <summary>
        /// The Y component of the <see cref="MVector2"/>.
        /// </summary>
        public readonly float Y;

        private const int HASH_CODE_INITIAL = 73;
        private const int HASH_CODE_MULTIPLIER = 101;

        /// <summary>
        /// Initializes a new instance of the <see cref="MVector2"/> struct.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        public MVector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MVector2"/> struct, with both components set to the provided value.
        /// </summary>
        /// <param name="value">The value.</param>
        public MVector2(float value)
        {
            this.X = value;
            this.Y = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MVector2"/> struct.
        /// </summary>
        /// <param name="point">The point to intialize from.</param>
        public MVector2(MPoint2 point)
        {
            this.X = point.X;
            this.Y = point.Y;
        }

        /// <summary>
        /// Gets the absolute <see cref="MVector2"/>, both components being positive.
        /// </summary>
        /// <value>
        /// The absolute vector.
        /// </value>
        public MVector2 Absolute => new MVector2(Math.Abs(this.X), Math.Abs(this.Y));

        /// <summary>
        /// Gets the normalized <see cref="MVector2"/>.
        /// </summary>
        /// <value>
        /// The normalized vector.
        /// </value>
        public MVector2 Normalized
        {
            get
            {
                Vector2 v = this;
                v.Normalize();
                return this;
            }
        }

        /// <summary>
        /// Gets a <see cref="MVector2"/> with all components set to 1, the identity vector.
        /// </summary>
        public static MVector2 One => new MVector2(1f, 1f);

        /// <summary>
        /// Gets the <see cref="Microsoft.Xna.Framework.Vector2"/> representation.
        /// </summary>
        /// <value>
        /// The Vector2 representation.
        /// </value>
        public Vector2 Vector2
        {
            get { return this; }
        }

        /// <summary>
        /// Gets a <see cref="MVector2"/> with all components set to 0.
        /// </summary>
        public static MVector2 Zero => new MVector2(0f, 0f);

        /// <summary>
        /// Operator for subtraction with another <see cref="MVector2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Added result.</returns>
        public static MVector2 operator -(MVector2 a, MVector2 b)
        {
            return new MVector2(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Logic operator for non-equality.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(MVector2 a, MVector2 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        /// <summary>
        /// Operator for multiplication with scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static MVector2 operator *(MVector2 a, float b)
        {
            return new MVector2(a.X * b, a.Y * b);
        }

        /// <summary>
        /// Operator for multiplication with scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static MVector2 operator *(float b, MVector2 a)
        {
            return new MVector2(a.X * b, a.Y * b);
        }

        /// <summary>
        /// Operator for division with a scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Divided result.</returns>
        public static MVector2 operator /(MVector2 a, float b)
        {
            return new MVector2(a.X / b, a.Y / b);
        }

        /// <summary>
        /// Operator for addition with another <see cref="MVector2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Added result.</returns>
        public static MVector2 operator +(MVector2 a, MVector2 b)
        {
            return new MVector2(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Logic operator for equality.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>True if equal, else false.</returns>
        public static bool operator ==(MVector2 a, MVector2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Microsoft.Xna.Framework.Vector2"/> to <see cref="MVector2"/>.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator MVector2(Vector2 v)
        {
            return new MVector2(v.X, v.Y);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="MVector2"/> to <see cref="Microsoft.Xna.Framework.Vector2"/>.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Vector2(MVector2 v)
        {
            return new Vector2(v.X, v.Y);
        }

        /// <summary>
        /// Interpreting <see cref="MVector2"/> as a point, calculates which <see cref="MVector2"/> in a collection that is the closest.
        /// </summary>
        /// <param name="compared">The compared points.</param>
        /// <returns>The closest point.</returns>
        public MVector2 ClosestPoint(IEnumerable<MVector2> compared)
        {
            float distanceSqrd = float.MaxValue;
            MVector2 closest = MVector2.Zero;
            foreach (MVector2 v in compared)
            {
                float dx = this.X - v.X;
                float dy = this.Y - v.Y;
                float d = dx * dx + dy * dy;
                if (d < distanceSqrd)
                {
                    distanceSqrd = d;
                    closest = v;
                }
            }
            return closest;
        }

        /// <summary>
        /// Interpreting <see cref="MVector2"/> as a point, calculates which <see cref="MVector2"/> in a collection that is the closest.
        /// </summary>
        /// <param name="compared">The compared points.</param>
        /// <param name="distance">The distance to the closest point.</param>
        /// <returns>The closest point.</returns>
        public MVector2 ClosestPoint(IEnumerable<MVector2> compared, out float distance)
        {
            MVector2 closest = this.ClosestPoint(compared);
            float dx = this.X - closest.X;
            float dy = this.Y - closest.Y;
            distance = (float)Math.Sqrt(dx * dx + dy * dy);
            return closest;
        }

        /// <summary>
        /// Returns whether the <see cref="MVector2"/> is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if they are equal, else false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is MVector2)
            {
                return this == (MVector2)obj;
            }
            return false;
        }

        /// <summary>
        /// Returns equality to another <see cref="MVector2"/>.
        /// </summary>
        /// <param name="other">Another <see cref="MVector2"/>.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(MVector2 other)
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
                int hash = MVector2.HASH_CODE_INITIAL;
                hash = hash * MVector2.HASH_CODE_MULTIPLIER + this.X.GetHashCode();
                hash = hash * MVector2.HASH_CODE_MULTIPLIER + this.Y.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns the length of the vector.
        /// </summary>
        /// <returns>Length of the vector.</returns>
        public float Length()
        {
            return (float)Math.Sqrt(this.LengthSquared());
        }

        /// <summary>
        /// Returns the squared length of the vector. Computationally faster than <see cref="Length()"/>.
        /// </summary>
        /// <returns>Squared length of the vector.</returns>
        public float LengthSquared()
        {
            return this.X * this.X + this.Y * this.Y;
        }

        /// <summary>
        /// Returns the <see cref="MPoint2"/> representation, round down each component to closest integer value.
        /// </summary>
        /// <returns></returns>
        public MPoint2 ToMPoint2()
        {
            return new MPoint2(this);
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
        /// Translates the <see cref="MVector2"/> with the specified delta.
        /// </summary>
        /// <param name="dx">The x translation.</param>
        /// <param name="dy">The y translation.</param>
        /// <returns></returns>
        public MVector2 Translate(float dx, float dy)
        {
            return new MVector2(this.X + dx, this.Y + dy);
        }

        /// <summary>
        /// Translates the <see cref="MVector2"/> with the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns></returns>
        public MVector2 Translate(MVector2 delta)
        {
            return new MVector2(this.X + delta.X, this.Y + delta.Y);
        }
    }
}