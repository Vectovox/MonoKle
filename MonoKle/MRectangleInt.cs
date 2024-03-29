using Microsoft.Xna.Framework;
using System;
using System.Text;

namespace MonoKle
{
    /// <summary>
    /// Struct for storing an immutable, serializable, normalized (non-negative width and height), integer-based area.
    /// </summary>
    [Serializable]
    public readonly struct MRectangleInt : IEquatable<MRectangleInt>
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
        public MRectangleInt(int width, int height) : this(0, 0, width, height)
        {
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
        public MRectangleInt(MPoint2 size) : this(size.X, size.Y)
        {
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
        public MPoint2 BottomLeft => new(TopLeft.X, BottomRight.Y);

        /// <summary>
        /// Gets the center of the <see cref="MRectangleInt"/>.
        /// </summary>
        public MVector2 Center => new(TopLeft.X + Width * 0.5f, TopLeft.Y + Height * 0.5f);

        /// <summary>
        /// Gets the height.
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
        public MPoint2 TopRight => new(BottomRight.X, TopLeft.Y);

        /// <summary>
        /// Returns the <see cref="Right"/> coordinate aligned to the vertical center.
        /// </summary>
        public MPoint2 CenterRight => new(Right, (int)Center.Y);

        /// <summary>
        /// Returns the <see cref="Left"/> coordinate aligned to the vertical center.
        /// </summary>
        public MPoint2 CenterLeft => new(Left, (int)Center.Y);

        /// <summary>
        /// Returns the <see cref="Top"/> coordinate aligned to the horizontal center.
        /// </summary>
        public MPoint2 CenterTop => new((int)Center.X, Top);

        /// <summary>
        /// Returns the <see cref="Bottom"/> coordinate aligned to the horizontal center.
        /// </summary>
        public MPoint2 CenterBottom => new((int)Center.X, Bottom);

        /// <summary>
        /// Gets the <see cref="MPoint2"/> representing width and height.
        /// </summary>
        public MPoint2 Dimensions => new(Width, Height);

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Width => BottomRight.X - TopLeft.X ;

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
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator MRectangleInt(Rectangle r) => new(r.X, r.Y, r.Width, r.Height);

        /// <summary>
        /// Performs an implicit conversion from <see cref="MRectangleInt"/> to <see cref="Rectangle"/>.
        /// </summary>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Rectangle(MRectangleInt r) => new(r.TopLeft.X, r.TopLeft.Y, r.Width, r.Height);

        /// <summary>
        /// Clamps the provided <see cref="MRectangleInt"/> to be within.
        /// </summary>
        /// <param name="area">The <see cref="MRectangleInt"/> to clamp.</param>
        public MRectangleInt Clamp(MRectangleInt area)
        {
            MPoint2 topLeft = Clamp(area.TopLeft);
            MPoint2 bottomRight = Clamp(area.BottomRight);
            return new MRectangleInt(topLeft, bottomRight);
        }

        /// <summary>
        /// Clamps the provided <see cref="MRectangle"/> to be within.
        /// </summary>
        /// <param name="area">The <see cref="MRectangle"/> to clamp.</param>
        public MRectangle Clamp(MRectangle area)
        {
            MVector2 topLeft = Clamp(area.TopLeft);
            MVector2 bottomRight = Clamp(area.BottomRight);
            return new MRectangle(topLeft, bottomRight);
        }

        /// <summary>
        /// Clamps the provided <see cref="MPoint2"/> to be positioned within or on the edge of
        /// the <see cref="MRectangleInt"/>.
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
        /// Clamps the provided <see cref="MVector2"/> to be positioned within or on the edge of
        /// the <see cref="MRectangleInt"/>.
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
        /// Intersects the <see cref="MRectangleInt"/> with another <see cref="MRectangleInt"/>,
        /// returning the intersecting area.
        /// </summary>
        /// <param name="other">The rectangle to intersect with.</param>
        public MRectangleInt Intersect(MRectangleInt other)
        {
            var left = Math.Max(Left, other.Left);
            var top = Math.Max(Top, other.Top);
            var right = Math.Min(Right, other.Right);
            var bottom = Math.Min(Bottom, other.Bottom);
            return new MRectangleInt(left, top, Math.Max(right - left, 0), Math.Max(bottom - top, 0));
        }

        /// <summary>
        /// Returns whether this intersects the provided <see cref="MRectangleInt"/>.
        /// </summary>
        /// <param name="other">The rectangle to test intersection with.</param>
        /// <returns>True if intersecting; otherwise false.</returns>
        public bool Intersects(MRectangleInt other)
        {
            if (Left > other.Right || Top > other.Bottom || Right < other.Left || Bottom < other.Top)
            {
                return false;
            }
            return true;
        }

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
        /// Resizes the <see cref="MRectangleInt"/> dimensions to the provided values.
        /// </summary>
        /// <param name="dimensions">The new width and height.</param>
        public MRectangleInt Redimension(MPoint2 dimensions) => Redimension(dimensions.X, dimensions.Y);

        /// <summary>
        /// Resizes the width of the <see cref="MRectangleInt"/> to the provided value.
        /// </summary>
        /// <param name="width">The new width.</param>
        public MRectangleInt RedimensionWidth(int width) => Redimension(width, Height);

        /// <summary>
        /// Resizes the height of the <see cref="MRectangleInt"/> to the provided value.
        /// </summary>
        /// <param name="height">The new height.</param>
        public MRectangleInt RedimensionHeight(int height) => Redimension(Width, height);

        /// <summary>
        /// Resizes the <see cref="MRectangleInt"/> dimensions to the provided values.
        /// </summary>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        public MRectangleInt Redimension(int width, int height) => new(TopLeft.X, TopLeft.Y, width, height);

        /// <summary>
        /// Resizes the <see cref="MRectangleInt"/> width the provide delta values.
        /// </summary>
        /// <param name="delta">The change in width and height.</param>
        public MRectangleInt Resize(MPoint2 delta) => Resize(delta.X, delta.Y);

        /// <summary>
        /// Resizes the width of the <see cref="MRectangleInt"/>.
        /// </summary>
        /// <param name="deltaWidth">The change in width.</param>
        public MRectangleInt ResizeWidth(int deltaWidth) => Resize(deltaWidth, 0);

        /// <summary>
        /// Resizes the height of the <see cref="MRectangleInt"/>.
        /// </summary>
        /// <param name="deltaHeight">The change in height.</param>
        public MRectangleInt ResizeHeight(int deltaHeight) => Resize(0, deltaHeight);

        /// <summary>
        /// Repositions the <see cref="MRectangleInt"/> to the provided coordinate.
        /// </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        public MRectangleInt Reposition(int x, int y) => new MRectangleInt(Dimensions).Translate(x, y);

        /// <summary>
        /// Repositions the <see cref="MRectangleInt"/> to the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        public MRectangleInt Reposition(MPoint2 coordinate) => new MRectangleInt(Dimensions).Translate(coordinate);

        /// <summary>
        /// Resizes the <see cref="MRectangleInt"/> width the provide delta values.
        /// </summary>
        /// <param name="deltaWidth">The change in width.</param>
        /// <param name="deltaHeight">The change in height.</param>
        public MRectangleInt Resize(int deltaWidth, int deltaHeight) =>
            new(TopLeft.X, TopLeft.Y, Width + deltaWidth, Height + deltaHeight);

        /// <summary>
        /// Uniformly scales the <see cref="MRectangleInt"/> around <see cref="Center"/> with the given factor.
        /// </summary>
        /// <remarks>
        /// Will apply integer cast rounding for fractional results.
        /// </remarks>
        /// <param name="factor">The factor with which to scale.</param>
        /// <returns>Scaled rectangle positioned such that the origin is the same as the initial rectangle.</returns>
        public MRectangleInt Scale(float factor)
        {
            var scaled = new MRectangle(Left, Top, Width * factor, Height * factor).ToMRectangleInt();
            return scaled.Translate((Width - scaled.Width) / 2, (Height - scaled.Height) / 2);
        }

        /// <summary>
        /// Scales the <see cref="MRectangleInt"/> around the provided coordinate with the given factor.
        /// </summary>
        /// <param name="factor">The factor with which to scale.</param>
        /// <param name="coordinate">The coordinate to scale around.</param>
        public MRectangleInt Scale(float factor, MPoint2 coordinate) => ToMRectangle().Scale(factor, coordinate).ToMRectangleInt();

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
            new(TopLeft.X + dx, TopLeft.Y + dy, BottomRight.X - TopLeft.X, BottomRight.Y - TopLeft.Y);

        /// <summary>
        /// Converts the <see cref="MRectangleInt"/> to an <see cref="MRectangle"/> by int -> float conversion.
        /// </summary>
        public MRectangle ToMRectangle() => new(this);
    }
}
