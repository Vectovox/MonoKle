using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonoKle
{
    /// <summary>
    /// Struct for storing an immutable, serializable, normalized (non-negative width and height), discrete rectangle.
    /// </summary>
    /// <remarks>
    /// As this rectangle is discrete it differs from intuitive mathematics at times. E.g. if the <see cref="TopLeft"/> and <see cref="BottomRight"/>
    /// coordinates are equivalent, the <see cref="Width"/> and <see cref="Height"/> will be 1.
    /// </remarks>
    [Serializable]
    public struct MDiscreteRectangle : IEquatable<MDiscreteRectangle>
    {
        /// <summary>
        /// The bottom right coordinate of the <see cref="MDiscreteRectangle"/>.
        /// </summary>
        public readonly MPoint2 BottomRight;

        /// <summary>
        /// The top left coordinate of the <see cref="MDiscreteRectangle"/>.
        /// </summary>
        public readonly MPoint2 TopLeft;

        /// <summary>
        /// Creates a new instance of <see cref="MDiscreteRectangle"/> from (0, 0) with the given width and height.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public MDiscreteRectangle(int width, int height) : this(0, 0, width, height)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="MDiscreteRectangle"/> by the provided coordinate and the given width and height.
        /// </summary>
        /// <param name="x">X-coordinate of a corner.</param>
        /// <param name="y">Y-coordinate of a corner.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public MDiscreteRectangle(int x, int y, int width, int height)
        {
            if (width == 0 || height == 0)
            {
                throw new ArgumentException("Width and height must be non-zero.");
            }

            int x2 = x + width + (width > 0 ? -1 : 1);
            int y2 = y + height + (height > 0 ? -1 : 1);

            int xLeft = Math.Min(x, x2);
            int xRight = Math.Max(x, x2);
            int yTop = Math.Min(y, y2);
            int yBottom = Math.Max(y, y2);

            TopLeft = new MPoint2(xLeft, yTop);
            BottomRight = new MPoint2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MDiscreteRectangle"/> from (0, 0) with the given size.
        /// </summary>
        /// <param name="size">The size.</param>
        public MDiscreteRectangle(MPoint2 size) : this(size.X, size.Y)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="MDiscreteRectangle"/> bound by the provided coordinates.
        /// </summary>
        /// <param name="coordA">The first coordinate.</param>
        /// <param name="coordB">The second coordinate.</param>
        public MDiscreteRectangle(MPoint2 coordA, MPoint2 coordB)
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
        /// Gets the center of the <see cref="MDiscreteRectangle"/>.
        /// </summary>
        public MVector2 Center => new MVector2(TopLeft.X + (Width - 1) * 0.5f, TopLeft.Y + (Height - 1) * 0.5f);

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height => BottomRight.Y - TopLeft.Y + 1;

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
        public int Width => BottomRight.X - TopLeft.X + 1;

        /// <summary>
        /// Non-equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="MDiscreteRectangle"/>.</param>
        /// <param name="b">Right <see cref="MDiscreteRectangle"/>.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(MDiscreteRectangle a, MDiscreteRectangle b) => a.TopLeft != b.TopLeft || a.BottomRight != b.BottomRight;

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="MDiscreteRectangle"/>.</param>
        /// <param name="b">Right <see cref="MDiscreteRectangle"/>.</param>
        /// <returns>True if are equal, else false.</returns>
        public static bool operator ==(MDiscreteRectangle a, MDiscreteRectangle b) => a.TopLeft == b.TopLeft && a.BottomRight == b.BottomRight;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Rectangle"/> to <see cref="MDiscreteRectangle"/>.
        /// </summary>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator MDiscreteRectangle(Rectangle r) => new MDiscreteRectangle(r.X, r.Y, r.Width, r.Height);

        /// <summary>
        /// Performs an implicit conversion from <see cref="MDiscreteRectangle"/> to <see cref="Rectangle"/>.
        /// </summary>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Rectangle(MDiscreteRectangle r) => new Rectangle(r.TopLeft.X, r.TopLeft.Y, r.Width, r.Height);

        /// <summary>
        /// Clamps the provided <see cref="MDiscreteRectangle"/> to be within.
        /// </summary>
        /// <param name="area">The <see cref="MDiscreteRectangle"/> to clamp.</param>
        public MDiscreteRectangle Clamp(MDiscreteRectangle area)
        {
            MPoint2 topLeft = Clamp(area.TopLeft);
            MPoint2 bottomRight = Clamp(area.BottomRight);
            return new MDiscreteRectangle(topLeft, bottomRight);
        }

        /// <summary>
        /// Clamps the provided <see cref="MPoint2"/> to be positioned within the <see cref="MDiscreteRectangle"/>.
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
        /// Checks if contains the provided <see cref="MDiscreteRectangle"/>.
        /// </summary>
        /// <param name="area">The <see cref="MDiscreteRectangle"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="MDiscreteRectangle"/> is contained, otherwise false.</returns>
        public bool Contains(MDiscreteRectangle area) => Contains(area.TopLeft) && Contains(area.TopRight)
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
        /// Intersects the <see cref="MDiscreteRectangle"/> with another <see cref="MDiscreteRectangle"/>,
        /// returning the intersecting area.
        /// </summary>
        /// <param name="other">The rectangle to intersect with.</param>
        public MDiscreteRectangle Intersect(MDiscreteRectangle other)
        {
            var left = Math.Max(Left, other.Left);
            var top = Math.Max(Top, other.Top);
            var right = Math.Min(Right, other.Right);
            var bottom = Math.Min(Bottom, other.Bottom);
            return new MDiscreteRectangle(new MPoint2(left, top), new MPoint2(right, bottom));
        }

        /// <summary>
        /// Returns whether this is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public override bool Equals(object obj) => obj is MDiscreteRectangle rect && this == rect;

        /// <summary>
        /// Returns whether this is equal to the provided <see cref="MDiscreteRectangle"/>.
        /// </summary>
        /// <param name="other">The <see cref="MDiscreteRectangle"/> to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(MDiscreteRectangle other) => this == other;

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
        /// Sets the <see cref="MDiscreteRectangle"/> dimensions to the provided values.
        /// </summary>
        /// <param name="dimensions">The new width and height.</param>
        public MDiscreteRectangle Redimension(MPoint2 dimensions) => Redimension(dimensions.X, dimensions.Y);

        /// <summary>
        /// Sets the width of the <see cref="MDiscreteRectangle"/> to the provided value.
        /// </summary>
        /// <param name="width">The new width.</param>
        public MDiscreteRectangle RedimensionWidth(int width) => Redimension(width, Height);

        /// <summary>
        /// Sets the height of the <see cref="MDiscreteRectangle"/> to the provided value.
        /// </summary>
        /// <param name="height">The new height.</param>
        public MDiscreteRectangle RedimensionHeight(int height) => Redimension(Width, height);

        /// <summary>
        /// Sets the <see cref="MDiscreteRectangle"/> dimensions to the provided values.
        /// </summary>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        public MDiscreteRectangle Redimension(int width, int height) => new MDiscreteRectangle(TopLeft.X, TopLeft.Y, width, height);

        /// <summary>
        /// Resizes the <see cref="MDiscreteRectangle"/> width with the provide delta values.
        /// </summary>
        /// <param name="delta">The change in width and height.</param>
        public MDiscreteRectangle Resize(MPoint2 delta) => Resize(delta.X, delta.Y);

        /// <summary>
        /// Resizes the width of the <see cref="MDiscreteRectangle"/>.
        /// </summary>
        /// <param name="deltaWidth">The change in width.</param>
        public MDiscreteRectangle ResizeWidth(int deltaWidth) => Resize(deltaWidth, 0);

        /// <summary>
        /// Resizes the height of the <see cref="MDiscreteRectangle"/>.
        /// </summary>
        /// <param name="deltaHeight">The change in height.</param>
        public MDiscreteRectangle ResizeHeight(int deltaHeight) => Resize(0, deltaHeight);

        /// <summary>
        /// Resizes the <see cref="MDiscreteRectangle"/> width the provide delta values.
        /// </summary>
        /// <param name="deltaWidth">The change in width.</param>
        /// <param name="deltaHeight">The change in height.</param>
        public MDiscreteRectangle Resize(int deltaWidth, int deltaHeight) =>
            new MDiscreteRectangle(TopLeft.X, TopLeft.Y, Width + deltaWidth, Height + deltaHeight);

        /// <summary>
        /// Translates the <see cref="MDiscreteRectangle"/> with the given translation and returns the result.
        /// </summary>
        /// <param name="translation">The translation to make.</param>
        /// <returns>Translated <see cref="MDiscreteRectangle"/>.</returns>
        public MDiscreteRectangle Translate(MPoint2 translation) => Translate(translation.X, translation.Y);

        /// <summary>
        /// Translates the <see cref="MDiscreteRectangle"/> with the given X-translation and returns the result.
        /// </summary>
        /// <param name="dx">The translation along the X-axis.</param>
        /// <returns>Translated <see cref="MDiscreteRectangle"/>.</returns>
        public MDiscreteRectangle TranslateX(int dx) => Translate(dx, 0);

        /// <summary>
        /// Translates the <see cref="MDiscreteRectangle"/> with the given Y-translation and returns the result.
        /// </summary>
        /// <param name="dy">The translation along the Y-axis.</param>
        /// <returns>Translated <see cref="MDiscreteRectangle"/>.</returns>
        public MDiscreteRectangle TranslateY(int dy) => Translate(0, dy);

        /// <summary>
        /// Translates the <see cref="MDiscreteRectangle"/> with the given translation and returns the result.
        /// </summary>
        /// <param name="dx">The translation along the X-axis.</param>
        /// <param name="dy">The translation along the Y-axis.</param>
        /// <returns>Translated <see cref="MDiscreteRectangle"/>.</returns>
        public MDiscreteRectangle Translate(int dx, int dy) =>
            new MDiscreteRectangle(TopLeft.Translate(dx, dy), BottomRight.Translate(dx, dy));

        /// <summary>
        /// Iterates all the points contained within the <see cref="MDiscreteRectangle"/>.
        /// </summary>
        public IEnumerable<MPoint2> Iterate()
        {
            for (int y = Top; y <= Bottom; y++)
            {
                for (int x = Left; x <= Right; x++)
                {
                    yield return new MPoint2(x, y);
                }
            }
        }
    }
}
