using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MonoKle
{
    /// <summary>
    /// Two-dimensional, immutable, serializable, floating-point vector. Has implicit operator to <see cref="Vector2"/>.
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MVector2"/> struct.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        public MVector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MVector2"/> struct, with both components set to the provided value.
        /// </summary>
        /// <param name="value">The value.</param>
        public MVector2(float value) : this(value, value) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MVector2"/> struct.
        /// </summary>
        /// <param name="point">The point to intialize from.</param>
        public MVector2(MPoint2 point) : this(point.X, point.Y) { }

        /// <summary>
        /// Gets the <see cref="MVector2"/> with both components being positive.
        /// </summary>
        public MVector2 Absolute => new MVector2(Math.Abs(X), Math.Abs(Y));

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
                return v;
            }
        }

        /// <summary>
        /// Gets a <see cref="MVector2"/> with all components set to 1, the identity vector.
        /// </summary>
        public static MVector2 One => new MVector2(1f);

        /// <summary>
        /// Gets a <see cref="MVector2"/> with all components set to 0.
        /// </summary>
        public static MVector2 Zero => new MVector2(0f);

        /// <summary>
        /// Operator for subtraction with another <see cref="MVector2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Added result.</returns>
        public static MVector2 operator -(MVector2 a, MVector2 b) => new MVector2(a.X - b.X, a.Y - b.Y);

        /// <summary>
        /// Unary operator for negating the components of the <see cref="MVector2"/>.
        /// </summary>
        /// <param name="a">The operand.</param>
        /// <returns>Negated result.</returns>
        public static MVector2 operator -(MVector2 a) => Zero - a;

        /// <summary>
        /// Logic operator for non-equality.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(MVector2 a, MVector2 b) => a.X != b.X || a.Y != b.Y;

        /// <summary>
        /// Operator for multiplication with scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static MVector2 operator *(MVector2 a, float b) => new MVector2(a.X * b, a.Y * b);

        /// <summary>
        /// Operator for multiplication with scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static MVector2 operator *(float b, MVector2 a) => new MVector2(a.X * b, a.Y * b);

        /// <summary>
        /// Operator for division with a scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Divided result.</returns>
        public static MVector2 operator /(MVector2 a, float b) => new MVector2(a.X / b, a.Y / b);

        /// <summary>
        /// Operator for addition with another <see cref="MVector2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Added result.</returns>
        public static MVector2 operator +(MVector2 a, MVector2 b) => new MVector2(a.X + b.X, a.Y + b.Y);

        /// <summary>
        /// Operator for memberwise multiplication with another <see cref="MVector2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static MVector2 operator *(MVector2 a, MVector2 b) => new MVector2(a.X * b.X, a.Y * b.Y);

        /// <summary>
        /// Operator for memberwise division with another <see cref="MVector2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Division result.</returns>
        public static MVector2 operator /(MVector2 a, MVector2 b) => new MVector2(a.X / b.X, a.Y / b.Y);

        /// <summary>
        /// Logic operator for equality.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>True if equal, else false.</returns>
        public static bool operator ==(MVector2 a, MVector2 b) => a.X == b.X && a.Y == b.Y;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Vector2"/> to <see cref="MVector2"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public static implicit operator MVector2(Vector2 vector) => new MVector2(vector.X, vector.Y);

        /// <summary>
        /// Performs an implicit conversion from <see cref="MVector2"/> to <see cref="Vector2"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public static implicit operator Vector2(MVector2 vector) => new Vector2(vector.X, vector.Y);

        /// <summary>
        /// Performs an implicit conversion from <see cref="MPoint2"/> to <see cref="MVector2"/>.
        /// </summary>
        /// <param name="p">The point.</param>
        public static implicit operator MVector2(MPoint2 p) => new MVector2(p);

        /// <summary>
        /// Parses the specified string.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>Parsed value</returns>
        public static MVector2 Parse(string s)
        {
            if (TryParse(s, out MVector2 result))
            {
                return result;
            }
            throw new FormatException("String format not correctly defined.");
        }

        /// <summary>
        /// Tries to parse the specified string.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="result">The out parameter result of parsing.</param>
        /// <returns>True if parsing was successful; otherwise false.</returns>
        public static bool TryParse(string s, out MVector2 result)
        {
            const string parseRegex = "^" + nameof(MVector2) + "\\((-?[0-9]+(\\.[0-9]+)?),(-?[0-9]+(\\.[0-9]+)?)\\)$";

            result = Zero;

            Match match = Regex.Match(s.Replace(" ", ""), parseRegex, RegexOptions.Compiled);
            if (match.Success)
            {
                float x = float.Parse(match.Groups[1].Value, NumberFormatInfo.InvariantInfo);
                float y = float.Parse(match.Groups[3].Value, NumberFormatInfo.InvariantInfo);
                result = new MVector2(x, y);
            }

            return match.Success;
        }

        /// <summary>
        /// Interpreting <see cref="MVector2"/> as a point, calculates which <see cref="MVector2"/> in a collection that is the closest.
        /// </summary>
        /// <param name="comparisonPoints">The compared points.</param>
        /// <returns>The closest point.</returns>
        public MVector2 ClosestPoint(IEnumerable<MVector2> comparisonPoints)
        {
            float closestDistance = float.MaxValue;
            MVector2 closest = this;
            foreach (MVector2 point in comparisonPoints)
            {
                var delta = this - point;
                var distance = delta.LengthSquared;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = point;
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
            MVector2 closest = ClosestPoint(compared);
            float dx = X - closest.X;
            float dy = Y - closest.Y;
            distance = (float)Math.Sqrt(dx * dx + dy * dy);
            return closest;
        }

        /// <summary>
        /// Returns whether the <see cref="MVector2"/> is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if they are equal, else false.</returns>
        public override bool Equals(object obj) => obj is MVector2 other && Equals(other);

        /// <summary>
        /// Returns equality to another <see cref="MVector2"/>.
        /// </summary>
        /// <param name="other">Another <see cref="MVector2"/>.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(MVector2 other) => this == other;

        /// <summary>
        /// Returns the hash code representation.
        /// </summary>
        /// <returns>Hash code representation.</returns>
        public override int GetHashCode()
        {
            const int HASH_CODE_MULTIPLIER = 101;
            unchecked
            {
                int hash = 73;
                hash = hash * HASH_CODE_MULTIPLIER + X.GetHashCode();
                hash = hash * HASH_CODE_MULTIPLIER + Y.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns the length of the vector.
        /// </summary>
        /// <returns>Length of the vector.</returns>
        public float Length => (float)Math.Sqrt(LengthSquared);

        /// <summary>
        /// Returns the squared length of the vector. Computationally faster than <see cref="Length"/>.
        /// </summary>
        /// <returns>Squared length of the vector.</returns>
        public float LengthSquared => X * X + Y * Y;

        /// <summary>
        /// Returns the <see cref="MPoint2"/> representation, rounding down each component to closest integer.
        /// </summary>
        public MPoint2 ToMPoint2() => new MPoint2(this);

        /// <summary>
        /// Returns the <see cref="MPoint2"/> representation, rounding up each component to closest integer.
        /// </summary>
        public MPoint2 ToMPoint2RoundUp() => new MPoint2((int)Math.Ceiling(X), (int)Math.Ceiling(Y));

        /// <summary>
        /// Returns the string representation.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(nameof(MVector2));
            sb.Append('(');
            sb.Append(X.ToString(NumberFormatInfo.InvariantInfo));
            sb.Append(',');
            sb.Append(Y.ToString(NumberFormatInfo.InvariantInfo));
            sb.Append(')');
            return sb.ToString();
        }

        /// <summary>
        /// Translates the <see cref="MVector2"/> with the specified delta.
        /// </summary>
        /// <param name="dx">The x translation.</param>
        /// <param name="dy">The y translation.</param>
        public MVector2 Translate(float dx, float dy) => new MVector2(X + dx, Y + dy);

        /// <summary>
        /// Translates the <see cref="MVector2"/> with the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        public MVector2 Translate(MVector2 delta) => this + delta;
    }
}
