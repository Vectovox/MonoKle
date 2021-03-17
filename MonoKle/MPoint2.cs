using Microsoft.Xna.Framework;
using MonoKle.Converters;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MonoKle
{
    /// <summary>
    /// Two-dimensional, immutable, serializable, integer-based point.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(MPoint2Converter))]
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

        private const string RegexParse = "^" + nameof(MPoint2) + "\\((-?[0-9]+),(-?[0-9]+)\\)$";

        /// <summary>
        /// Initializes a new instance of the <see cref="MPoint2"/> struct.
        /// </summary>
        /// <param name="x">X component.</param>
        /// <param name="y">Y component.</param>
        public MPoint2(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MPoint2"/> struct from the <see cref="MVector2"/> vector, rounding down the components to integer values.
        /// </summary>
        /// <param name="vector">The original vector.</param>
        public MPoint2(MVector2 vector)
        {
            X = (int)vector.X;
            Y = (int)vector.Y;
        }

        /// <summary>
        /// Gets a <see cref="MPoint2"/> with all components set to 1.
        /// </summary>
        public static MPoint2 One => new MPoint2(1, 1);

        /// <summary>
        /// Gets a <see cref="MPoint2"/> with all components set to 0.
        /// </summary>
        public static MPoint2 Zero => new MPoint2(0, 0);

        /// <summary>
        /// Gets the <see cref="Microsoft.Xna.Framework.Point"/> representation of the <see cref="MPoint2"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Microsoft.Xna.Framework.Point"/> representation.
        /// </value>
        public Point Point => this;

        /// <summary>
        /// Operator for subtraction with another <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Added result.</returns>
        public static MPoint2 operator -(MPoint2 a, MPoint2 b) => new MPoint2(a.X - b.X, a.Y - b.Y);

        /// <summary>
        /// Unary operator for negating the components of the <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="a">The operand.</param>
        /// <returns>Negated result.</returns>
        public static MPoint2 operator -(MPoint2 a) => Zero - a;

        /// <summary>
        /// Logic operator for non-equality.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(MPoint2 a, MPoint2 b) => a.X != b.X || a.Y != b.Y;

        /// <summary>
        /// Operator for multiplication with scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static MPoint2 operator *(MPoint2 a, int b) => new MPoint2(a.X * b, a.Y * b);

        /// <summary>
        /// Operator for multiplication with scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static MPoint2 operator *(int b, MPoint2 a) => new MPoint2(a.X * b, a.Y * b);

        /// <summary>
        /// Operator for division with a scalar.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Divided result.</returns>
        public static MPoint2 operator /(MPoint2 a, int b) => new MPoint2(a.X / b, a.Y / b);

        /// <summary>
        /// Operator for addition with another <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Added result.</returns>
        public static MPoint2 operator +(MPoint2 a, MPoint2 b) => new MPoint2(a.X + b.X, a.Y + b.Y);

        /// <summary>
        /// Operator for memberwise multiplication with another <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Multiplied result.</returns>
        public static MPoint2 operator *(MPoint2 a, MPoint2 b) => new MPoint2(a.X * b.X, a.Y * b.Y);

        /// <summary>
        /// Operator for memberwise division with another <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>Division result.</returns>
        public static MPoint2 operator /(MPoint2 a, MPoint2 b) => new MPoint2(a.X / b.X, a.Y / b.Y);

        /// <summary>
        /// Logic operator for equality.
        /// </summary>
        /// <param name="a">Left operand.</param>
        /// <param name="b">Right operand.</param>
        /// <returns>True if equal, else false.</returns>
        public static bool operator ==(MPoint2 a, MPoint2 b) => a.X == b.X && a.Y == b.Y;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Point"/> to <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator MPoint2(Point p) => new MPoint2(p.X, p.Y);

        /// <summary>
        /// Performs an implicit conversion from <see cref="MPoint2"/> to <see cref="Point"/>.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Point(MPoint2 p) => new Point(p.X, p.Y);

        /// <summary>
        /// Parses the specified string.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns></returns>
        public static MPoint2 Parse(string s)
        {
            if (TryParse(s, out MPoint2 res) == false)
            {
                throw new FormatException("String format not correctly defined.");
            }
            return res;
        }

        /// <summary>
        /// Tries to parse the specified string.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="result">The out parameter result of parsing.</param>
        /// <returns>True if parsing was successful; otherwise false.</returns>
        public static bool TryParse(string s, out MPoint2 result)
        {
            Match match = Regex.Match(s.Replace(" ", ""), RegexParse, RegexOptions.Compiled);
            result = Zero;
            if (match.Success)
            {
                int x = int.Parse(match.Groups[1].Value, NumberFormatInfo.InvariantInfo);
                int y = int.Parse(match.Groups[2].Value, NumberFormatInfo.InvariantInfo);
                result = new MPoint2(x, y);
            }
            return match.Success;
        }

        /// <summary>
        /// Returns whether the <see cref="MPoint2"/> is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if they are equal, else false.</returns>
        public override bool Equals(object obj) => obj is MPoint2 point && this == point;

        /// <summary>
        /// Returns equality to another <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="other">Another <see cref="MPoint2"/>.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(MPoint2 other) => this == other;

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
                hash = hash * HASH_CODE_MULTIPLIER + X;
                hash = hash * HASH_CODE_MULTIPLIER + Y;
                return hash;
            }
        }

        /// <summary>
        /// Returns the length of the <see cref="MPoint2"/>, the distance to <see cref="Zero"/>.
        /// </summary>
        /// <returns>Length of the <see cref="MPoint2"/>.</returns>
        public double Length => Math.Sqrt(LengthSquared);

        /// <summary>
        /// Returns the squared length of the <see cref="MPoint2"/>, the distance to <see cref="Zero"/>. Faster than <see cref="Length()"/>.
        /// </summary>
        /// <returns>Squared length of the <see cref="MPoint2"/>.</returns>
        public double LengthSquared => X * X + Y * Y;

        /// <summary>
        /// Returns the <see cref="MVector2"/> representation.
        /// </summary>
        /// <returns><see cref="MVector2"/> representation.</returns>
        public MVector2 ToMVector2() => new MVector2(this);

        /// <summary>
        /// Returns the string representation of the <see cref="MPoint2"/>.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(nameof(MPoint2));
            sb.Append('(');
            sb.Append(X);
            sb.Append(',');
            sb.Append(Y);
            sb.Append(')');
            return sb.ToString();
        }

        /// <summary>
        /// Translates the <see cref="MPoint2"/> with the specified delta.
        /// </summary>
        /// <param name="dx">The x translation.</param>
        /// <param name="dy">The y translation.</param>
        /// <returns></returns>
        public MPoint2 Translate(int dx, int dy) => new MPoint2(X + dx, Y + dy);

        /// <summary>
        /// Translates the <see cref="MPoint2"/> with the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns></returns>
        public MPoint2 Translate(MPoint2 delta) => new MPoint2(X + delta.X, Y + delta.Y);
    }
}
