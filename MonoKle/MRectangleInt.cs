using Microsoft.Xna.Framework;
using System;
using System.Text;

namespace MonoKle
{
    /// <summary>
    /// Struct for storing an immutable, serializable, normalized (non-negative width and height), integer-based area.
    /// </summary>
    [Serializable]
    public struct MRectangleInt : IEquatable<MRectangleInt>
    {
        /// <summary>
        /// The bottom right coordinate of the <see cref="MRectangleInt"/>.
        /// </summary>
        public readonly MPoint2 BottomRight;

        /// <summary>
        /// The top left coordinate of the <see cref="MRectangleInt"/>.
        /// </summary>
        public readonly MPoint2 TopLeft;

        /// <summary>
        /// Creates a new instance of <see cref="MRectangleInt"/> around (0, 0) with the given width and height.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public MRectangleInt(int width, int height)
        {
            int xLeft = Math.Min(0, width);
            int xRight = Math.Max(0, width);
            int yTop = Math.Min(0, height);
            int yBottom = Math.Max(0, height);
            TopLeft = new MPoint2(xLeft, yTop);
            BottomRight = new MPoint2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangleInt"/> by the provided coordinate and the given width and height.
        /// </summary>
        /// <param name="x">X-coordinate of a corner.</param>
        /// <param name="y">Y-coordinate of a corner.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public MRectangleInt(int x, int y, int width, int height)
        {
            int xLeft = Math.Min(x, x + width);
            int xRight = Math.Max(x, x + width);
            int yTop = Math.Min(y, y + height);
            int yBottom = Math.Max(y, y + height);
            TopLeft = new MPoint2(xLeft, yTop);
            BottomRight = new MPoint2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangleInt"/> around (0, 0) with the given size.
        /// </summary>
        /// <param name="size">The size.</param>
        public MRectangleInt(MPoint2 size)
        {
            int xLeft = Math.Min(0, size.X);
            int xRight = Math.Max(0, size.X);
            int yTop = Math.Min(0, size.Y);
            int yBottom = Math.Max(0, size.Y);
            TopLeft = new MPoint2(xLeft, yTop);
            BottomRight = new MPoint2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangleInt"/> bound by the provided coordinates.
        /// </summary>
        /// <param name="coordA">The first coordinate.</param>
        /// <param name="coordB">The second coordinate.</param>
        public MRectangleInt(MPoint2 coordA, MPoint2 coordB)
        {
            int xLeft = Math.Min(coordA.X, coordB.X);
            int xRight = Math.Max(coordA.X, coordB.X);
            int yTop = Math.Min(coordA.Y, coordB.Y);
            int yBottom = Math.Max(coordA.Y, coordB.Y);
            TopLeft = new MPoint2(xLeft, yTop);
            BottomRight = new MPoint2(xRight, yBottom);
        }

        /// <summary>
        /// Gets the Y-coordinate of the bottom border.
        /// </summary>
        public int Bottom => BottomRight.Y;

        /// <summary>
        /// Gets the bottom left corner.
        /// </summary>
        public MPoint2 BottomLeft => new MPoint2(TopLeft.X, BottomRight.Y);

        /// <summary>
        /// Gets the center of the <see cref="MRectangleInt"/>.
        /// </summary>
        public MVector2 Center => new MVector2(TopLeft.X + Width * 0.5f, TopLeft.Y + Height * 0.5f);

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Height => BottomRight.Y - TopLeft.Y;

        /// <summary>
        /// Gets the X-coordinate of the left border.
        /// </summary>
        public int Left => TopLeft.X;

        /// <summary>
        /// Gets the X-coordinate of the right border.
        /// </summary>
        public int Right => BottomRight.X;

        /// <summary>
        /// Gets the Y-coordinate of the top border.
        /// </summary>
        public int Top => TopLeft.Y;

        /// <summary>
        /// Gets the top right corner.
        /// </summary>
        public MPoint2 TopRight => new MPoint2(BottomRight.X, TopLeft.Y);

        /// <summary>
        /// Gets the <see cref="MPoint2"/> representing width and height.
        /// </summary>
        public MPoint2 Dimensions => new MPoint2(Width, Height);

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Width => BottomRight.X - TopLeft.X;

        /// <summary>
        /// Non-equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="MRectangleInt"/>.</param>
        /// <param name="b">Right <see cref="MRectangleInt"/>.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(MRectangleInt a, MRectangleInt b) => a.TopLeft != b.TopLeft || a.BottomRight != b.BottomRight;

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="MRectangleInt"/>.</param>
        /// <param name="b">Right <see cref="MRectangleInt"/>.</param>
        /// <returns>True if are equal, else false.</returns>
        public static bool operator ==(MRectangleInt a, MRectangleInt b) => a.TopLeft == b.TopLeft && a.BottomRight == b.BottomRight;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Rectangle"/> to <see cref="MRectangleInt"/>.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator MRectangleInt(Rectangle r) => new MRectangleInt(r.X, r.Y, r.Width, r.Height);

        /// <summary>
        /// Performs an implicit conversion from <see cref="MRectangleInt"/> to <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Rectangle(MRectangleInt r) => new Rectangle(r.TopLeft.X, r.TopLeft.Y, r.Width, r.Height);

        /// <summary>
        /// Clamps the provided <see cref="MRectangleInt"/> to fit within.
        /// </summary>
        /// <param name="area">The <see cref="MRectangleInt"/> to clamp.</param>
        public MRectangleInt Clamp(MRectangleInt area)
        {
            MPoint2 topLeft = Clamp(area.TopLeft);
            MPoint2 bottomRight = Clamp(area.BottomRight);
            return new MRectangleInt(topLeft, bottomRight);
        }

        /// <summary>
        /// Clamps the provided <see cref="MRectangle"/> to fit within.
        /// </summary>
        /// <param name="area">The <see cref="MRectangle"/> to clamp.</param>
        public MRectangle Clamp(MRectangle area)
        {
            MVector2 topLeft = Clamp(area.TopLeft);
            MVector2 bottomRight = Clamp(area.BottomRight);
            return new MRectangle(topLeft, bottomRight);
        }

        /// <summary>
        /// Clamps the provided <see cref="MPoint2"/> to be positioned within this.
        /// </summary>
        /// <param name="coordinate">The <see cref="MPoint2"/> to clamp.</param>
        public MPoint2 Clamp(MPoint2 coordinate)
        {
            int x = coordinate.X;
            int y = coordinate.Y;

            if (x < TopLeft.X)
            {
                x = TopLeft.X;
            }
            else if (x > BottomRight.X)
            {
                x = BottomRight.X;
            }

            if (y < TopLeft.Y)
            {
                y = TopLeft.Y;
            }
            else if (y > BottomRight.Y)
            {
                y = BottomRight.Y;
            }

            return new MPoint2(x, y);
        }

        /// <summary>
        /// Clamps the provided <see cref="MVector2"/> to be positioned within this.
        /// </summary>
        /// <param name="coordinate">The <see cref="MVector2"/> to clamp.</param>
        public MVector2 Clamp(MVector2 coordinate)
        {
            float x = coordinate.X;
            float y = coordinate.Y;

            if (x < TopLeft.X)
            {
                x = TopLeft.X;
            }
            else if (x > BottomRight.X)
            {
                x = BottomRight.X;
            }

            if (y < TopLeft.Y)
            {
                y = TopLeft.Y;
            }
            else if (y > BottomRight.Y)
            {
                y = BottomRight.Y;
            }

            return new MVector2(x, y);
        }

        /// <summary>
        /// Checks if contains the provided <see cref="MRectangleInt"/>.
        /// </summary>
        /// <param name="area">The <see cref="MRectangleInt"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="MRectangleInt"/> is contained, otherwise false.</returns>
        public bool Contains(MRectangleInt area) => Contains(area.TopLeft) && Contains(area.TopRight)
                && Contains(area.BottomLeft) && Contains(area.BottomRight);

        /// <summary>
        /// Checks if contains the provided <see cref="MRectangle"/>.
        /// </summary>
        /// <param name="area">The <see cref="MRectangle"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="MRectangle"/> is contained, otherwise false.</returns>
        public bool Contains(MRectangle area) => Contains(area.TopLeft) && Contains(area.TopRight)
                && Contains(area.BottomLeft) && Contains(area.BottomRight);

        /// <summary>
        /// Checks if contains the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate to check if it is contained.</param>
        /// <returns>True if the specified coordinate is contained, otherwise false.</returns>
        public bool Contains(MVector2 coordinate) => coordinate.X >= Left && coordinate.X <= Right
                && coordinate.Y >= Top && coordinate.Y <= Bottom;

        /// <summary>
        /// Checks if contains the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate to check if it is contained.</param>
        /// <returns>True if the specified coordinate is contained, otherwise false.</returns>
        public bool Contains(MPoint2 coordinate) => coordinate.X >= Left && coordinate.X <= Right
                && coordinate.Y >= Top && coordinate.Y <= Bottom;

        /// <summary>
        /// Returns whether this is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public override bool Equals(object obj) => obj is MRectangleInt rect && this == rect;

        /// <summary>
        /// Returns whether this is equal to the provided <see cref="MRectangleInt"/>.
        /// </summary>
        /// <param name="other">The <see cref="MRectangleInt"/> to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(MRectangleInt other) => this == other;

        /// <summary>
        /// Returns the hash representation.
        /// </summary>
        /// <returns>Hash representation.</returns>
        public override int GetHashCode()
        {
            const int HASH_CODE_MULTIPLIER = 23;
            unchecked
            {
                int hash = 17;
                hash = hash * HASH_CODE_MULTIPLIER + TopLeft.GetHashCode();
                hash = hash * HASH_CODE_MULTIPLIER + BottomRight.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns the <see cref="string"/> representation.
        /// </summary>
        /// <returns><see cref="string"/> representation.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{Top Left: ");
            sb.Append(TopLeft.ToString());
            sb.Append(", Bottom Right: ");
            sb.Append(BottomRight.ToString());
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// Translates the <see cref="MRectangleInt"/> with the given translation and returns the result.
        /// </summary>
        /// <param name="translation">The translation to make.</param>
        /// <returns>Translated <see cref="MRectangleInt"/>.</returns>
        public MRectangleInt Translate(MPoint2 translation) => Translate(translation.X, translation.Y);

        /// <summary>
        /// Translates the <see cref="MRectangleInt"/> with the given X-translation and returns the result.
        /// </summary>
        /// <param name="dx">The translation along the X-axis.</param>
        /// <returns>Translated <see cref="MRectangleInt"/>.</returns>
        public MRectangleInt TranslateX(int dx) => Translate(dx, 0);

        /// <summary>
        /// Translates the <see cref="MRectangleInt"/> with the given Y-translation and returns the result.
        /// </summary>
        /// <param name="dy">The translation along the Y-axis.</param>
        /// <returns>Translated <see cref="MRectangleInt"/>.</returns>
        public MRectangleInt TranslateY(int dy) => Translate(0, dy);

        /// <summary>
        /// Translates the <see cref="MRectangleInt"/> with the given translation and returns the result.
        /// </summary>
        /// <param name="dx">The translation along the X-axis.</param>
        /// <param name="dy">The translation along the Y-axis.</param>
        /// <returns>Translated <see cref="MRectangleInt"/>.</returns>
        public MRectangleInt Translate(int dx, int dy) =>
            new MRectangleInt(TopLeft.X + dx, TopLeft.Y + dy, BottomRight.X - TopLeft.X, BottomRight.Y - TopLeft.Y);
    }
}
