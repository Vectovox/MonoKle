using System;
using System.Text;

namespace MonoKle
{
    /// <summary>
    /// Struct for storing an immutable, serializable, normalized (positive width and height), floating point precision area.
    /// </summary>
    [Serializable]
    public struct MRectangle : IEquatable<MRectangle>
    {
        /// <summary>
        /// The bottom right coordinate of the <see cref="MRectangle"/>.
        /// </summary>
        public readonly MVector2 BottomRight;

        /// <summary>
        /// The top left coordinate of the <see cref="MRectangle"/>.
        /// </summary>
        public readonly MVector2 TopLeft;

        /// <summary>
        /// Creates a new instance of <see cref="MRectangle"/> from the given <see cref="MRectangleInt"/>.
        /// <param name="area">The area to instantiate from.</param>
        /// </summary>
        public MRectangle(MRectangleInt area)
        {
            TopLeft = area.TopLeft.ToMVector2();
            BottomRight = area.BottomRight.ToMVector2();
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangle"/> around (0, 0) with the given width and height.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public MRectangle(float width, float height) : this(0, 0, width, height)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangle"/> around (0, 0) with the given size.
        /// </summary>
        /// <param name="size">The size.</param>
        public MRectangle(MVector2 size) : this(size.X, size.Y)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangle"/> by the provided coordinate and the given width and height.
        /// </summary>
        /// <param name="x">X-coordinate of a corner.</param>
        /// <param name="y">Y-coordinate of a corner.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public MRectangle(float x, float y, float width, float height)
        {
            float xLeft = Math.Min(x, x + width);
            float xRight = Math.Max(x, x + width);
            float yTop = Math.Min(y, y + height);
            float yBottom = Math.Max(y, y + height);
            TopLeft = new MVector2(xLeft, yTop);
            BottomRight = new MVector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangle"/> bound by the provided coordinates.
        /// </summary>
        /// <param name="coordA">The first coordinate.</param>
        /// <param name="coordB">The second coordinate.</param>
        public MRectangle(MVector2 coordA, MVector2 coordB)
        {
            float xLeft = Math.Min(coordA.X, coordB.X);
            float xRight = Math.Max(coordA.X, coordB.X);
            float yTop = Math.Min(coordA.Y, coordB.Y);
            float yBottom = Math.Max(coordA.Y, coordB.Y);
            TopLeft = new MVector2(xLeft, yTop);
            BottomRight = new MVector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MRectangle"/> bound by the provided coordinates.
        /// </summary>
        /// <param name="coordA">The first coordinate.</param>
        /// <param name="coordB">The second coordinate.</param>
        public MRectangle(MPoint2 coordA, MPoint2 coordB)
        {
            float xLeft = Math.Min(coordA.X, coordB.X);
            float xRight = Math.Max(coordA.X, coordB.X);
            float yTop = Math.Min(coordA.Y, coordB.Y);
            float yBottom = Math.Max(coordA.Y, coordB.Y);
            TopLeft = new MVector2(xLeft, yTop);
            BottomRight = new MVector2(xRight, yBottom);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MRectangle"/> struct, equating to the bounding box of the provided <see cref="MCircle"/>.
        /// </summary>
        /// <param name="circle">The circle.</param>
        public MRectangle(MCircle circle)
        {
            TopLeft = new MVector2(circle.Origin.X - circle.Radius, circle.Origin.Y - circle.Radius);
            BottomRight = new MVector2(circle.Origin.X + circle.Radius, circle.Origin.Y + circle.Radius);
        }

        /// <summary>
        /// Gets the Y-coordinate of the bottom border.
        /// </summary>
        public float Bottom => BottomRight.Y;

        /// <summary>
        /// Gets the bottom left corner.
        /// </summary>
        public MVector2 BottomLeft => new MVector2(TopLeft.X, BottomRight.Y);

        /// <summary>
        /// Gets the center of the <see cref="MRectangle"/>.
        /// </summary>
        public MVector2 Center => new MVector2(TopLeft.X + Width * 0.5f, TopLeft.Y + Height * 0.5f);

        /// <summary>
        /// Gets the width.
        /// </summary>
        public float Height => BottomRight.Y - TopLeft.Y;

        /// <summary>
        /// Gets the width divided by the height.
        /// </summary>
        /// <exception cref="DivideByZeroException">Thrown if height is zero.</exception>
        public float AspectRatio => Width / Height;

        /// <summary>
        /// Gets the X-coordinate of the left border.
        /// </summary>
        public float Left => TopLeft.X;

        /// <summary>
        /// Gets the X-coordinate of the right border.
        /// </summary>
        public float Right => BottomRight.X;

        /// <summary>
        /// Gets the Y-coordinate of the top border.
        /// </summary>
        public float Top => TopLeft.Y;

        /// <summary>
        /// Gets the top right corner.
        /// </summary>
        public MVector2 TopRight => new MVector2(BottomRight.X, TopLeft.Y);

        /// <summary>
        /// Gets the <see cref="MVector2"/> representing width and height.
        /// </summary>
        public MVector2 Dimensions => new MVector2(Width, Height);

        /// <summary>
        /// Gets the height.
        /// </summary>
        public float Width => BottomRight.X - TopLeft.X;

        /// <summary>
        /// Non-equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="MRectangle"/>.</param>
        /// <param name="b">Right <see cref="MRectangle"/>.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(MRectangle a, MRectangle b) => a.TopLeft != b.TopLeft || a.BottomRight != b.BottomRight;

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="MRectangle"/>.</param>
        /// <param name="b">Right <see cref="MRectangle"/>.</param>
        /// <returns>True if are equal, else false.</returns>
        public static bool operator ==(MRectangle a, MRectangle b) => a.TopLeft == b.TopLeft && a.BottomRight == b.BottomRight;

        /// <summary>
        /// Clamps the provided <see cref="MRectangle"/> to fit within this.
        /// </summary>
        /// <param name="area">The <see cref="MRectangle"/> to clamp.</param>
        public MRectangle Clamp(MRectangle area) => new MRectangle(Clamp(area.TopLeft), Clamp(area.BottomRight));

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
            else if (coordinate.Y > BottomRight.Y)
            {
                y = BottomRight.Y;
            }

            return new MVector2(x, y);
        }

        /// <summary>
        /// Scales the <see cref="MRectangle"/> to a size that fits the given bounding rectangle as well
        /// as possible without affecting the aspect ratio. Position is left unaltered.
        /// </summary>
        /// <param name="boundingRectangle">The bounding rectangle to fit to.</param>
        public MRectangle ScaleToFit(MRectangle boundingRectangle)
        {
            float thisRatio = AspectRatio;
            float boundingRatio = boundingRectangle.AspectRatio;
            if (thisRatio > boundingRatio)
            {
                // Letterbox top-bottom
                return new MRectangle(TopLeft.X, TopLeft.Y, boundingRectangle.Width, boundingRectangle.Width / thisRatio);
            }
            else if(thisRatio < boundingRatio)
            {
                // Letterbox left-right
                return new MRectangle(TopLeft.X, TopLeft.Y, boundingRectangle.Height * thisRatio, boundingRectangle.Height);
            }
            return boundingRectangle;
        }

        /// <summary>
        /// Scales the <see cref="MRectangle"/> to a minimal size that fills the given bounding rectangle
        /// without affecting the aspect ratio. Position is left unaltered.
        /// </summary>
        /// <param name="boundingRectangle">The bounding rectangle to fit to.</param>
        public MRectangle ScaleToFill(MRectangle boundingRectangle)
        {
            float thisRatio = AspectRatio;
            float boundingRatio = boundingRectangle.AspectRatio;
            if (thisRatio > boundingRatio)
            {
                return new MRectangle(TopLeft.X, TopLeft.Y, boundingRectangle.Height * thisRatio, boundingRectangle.Height);
            }
            else if (thisRatio < boundingRatio)
            {
                return new MRectangle(TopLeft.X, TopLeft.Y, boundingRectangle.Width, boundingRectangle.Width / thisRatio);
            }
            return boundingRectangle;
        }

        /// <summary>
        /// Centers the <see cref="MRectangle"/> such that its center aligns with the center
        /// of the provided <see cref="MRectangle"/>.
        /// </summary>
        /// <param name="boundingRectangle">The rectangle to align to.</param>
        public MRectangle PositionCenter(MRectangle boundingRectangle) => PositionCenter(boundingRectangle.Center);

        /// <summary>
        /// Centers the <see cref="MRectangle"/> such that its center aligns with the provided
        /// <see cref="MVector2"/>.
        /// </summary>
        /// <param name="position">The point to align to.</param>
        public MRectangle PositionCenter(MVector2 position) => new MRectangle(position.X - Width * 0.5f, position.Y - Height * 0.5f, Width, Height);

        /// <summary>
        /// Checks if contains the provided <see cref="MRectangle"/>.
        /// </summary>
        /// <param name="area">The <see cref="MRectangle"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="MRectangle"/> is contained, otherwise false.</returns>
        public bool Contains(MRectangle area) => Contains(area.TopLeft) && Contains(area.TopRight)
                && Contains(area.BottomLeft) && Contains(area.BottomRight);

        /// <summary>
        /// Checks if contains the provided <see cref="MRectangleInt"/>.
        /// </summary>
        /// <param name="area">The <see cref="MRectangleInt"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="MRectangleInt"/> is contained, otherwise false.</returns>
        public bool Contains(MRectangleInt area) => Contains(area.TopLeft) && Contains(area.TopRight)
                && Contains(area.BottomLeft) && Contains(area.BottomRight);

        /// <summary>
        /// Checks if the <see cref="MRectangle"/> contains the provided <see cref="MVector2"/>.
        /// </summary>
        /// <param name="coordinate">The <see cref="MVector2"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="MVector2"/> is contained, otherwise false.</returns>
        public bool Contains(MVector2 coordinate) => coordinate.X >= TopLeft.X && coordinate.X <= BottomRight.X
                && coordinate.Y >= TopLeft.Y && coordinate.Y <= BottomRight.Y;

        /// <summary>
        /// Checks if the <see cref="MRectangle"/> contains the provided <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="coordinate">The <see cref="MPoint2"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="MPoint2"/> is contained, otherwise false.</returns>
        public bool Contains(MPoint2 coordinate) => coordinate.X >= TopLeft.X && coordinate.X <= BottomRight.X
                && coordinate.Y >= TopLeft.Y && coordinate.Y <= BottomRight.Y;

        /// <summary>
        /// Checks if envelops, contains with a margin to all borders, the provided <see cref="MRectangle"/>.
        /// </summary>
        /// <param name="area">The <see cref="MRectangle"/> to check if enveloped.</param>
        /// <returns>True if the specified <see cref="MRectangle"/> is enveloped, otherwise false.</returns>
        public bool Envelops(MRectangle area) =>
            Envelops(area.TopLeft) && Envelops(area.TopRight)
                && Envelops(area.BottomLeft) && Envelops(area.BottomRight);

        /// <summary>
        /// Checks if envelops, contains with a margin to all borders, the provided <see cref="MRectangleInt"/>.
        /// </summary>
        /// <param name="area">The <see cref="MRectangleInt"/> to check if enveloped.</param>
        /// <returns>True if the specified <see cref="MRectangleInt"/> is enveloped, otherwise false.</returns>
        public bool Envelops(MRectangleInt area) =>
            Envelops(area.TopLeft) && Envelops(area.TopRight)
                && Envelops(area.BottomLeft) && Envelops(area.BottomRight);

        /// <summary>
        /// Checks if envelops, contains with a margin to all borders, the provided <see cref="MVector2"/>.
        /// </summary>
        /// <param name="coordinate">The <see cref="MVector2"/> to check if enveloped.</param>
        /// <returns>True if the <see cref="MVector2"/> is enveloped, otherwise false.</returns>
        public bool Envelops(MVector2 coordinate) =>
            coordinate.X > TopLeft.X && coordinate.X < BottomRight.X
                && coordinate.Y > TopLeft.Y && coordinate.Y < BottomRight.Y;

        /// <summary>
        /// Checks if envelops, contains with a margin to all borders, the provided <see cref="MPoint2"/>.
        /// </summary>
        /// <param name="coordinate">The <see cref="MPoint2"/> to check if enveloped.</param>
        /// <returns>True if the <see cref="MPoint2"/> is enveloped, otherwise false.</returns>
        public bool Envelops(MPoint2 coordinate) =>
            coordinate.X > TopLeft.X && coordinate.X < BottomRight.X
                && coordinate.Y > TopLeft.Y && coordinate.Y < BottomRight.Y;

        /// <summary>
        /// Intersects the <see cref="MRectangle"/> with another <see cref="MRectangle"/>,
        /// returning the intersecting area.
        /// </summary>
        /// <param name="other">The rectangle to intersect with.</param>
        public MRectangle Intersect(MRectangle other)
        {
            var left = Math.Max(Left, other.Left);
            var top = Math.Max(Top, other.Top);
            var right = Math.Min(Right, other.Right);
            var bottom = Math.Min(Bottom, other.Bottom);
            return new MRectangle(left, top, Math.Max(right - left, 0), Math.Max(bottom - top, 0));
        }

        /// <summary>
        /// Returns whether this is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to check for equality with.</param>
        /// <returns>True if equal; else false.</returns>
        public override bool Equals(object obj) => obj is MRectangle rectangle && this == rectangle;

        /// <summary>
        /// Returns whether this is equal to the provided <see cref="MRectangle"/>.
        /// </summary>
        /// <param name="other">The <see cref="MRectangle"/> to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(MRectangle other) => this == other;

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
        /// Resizes the <see cref="MRectangle"/> dimensions to the provided values.
        /// </summary>
        /// <param name="dimensions">The new width and height.</param>
        public MRectangle Redimension(MVector2 dimensions) => Redimension(dimensions.X, dimensions.Y);

        /// <summary>
        /// Resizes the width of the <see cref="MRectangle"/> to the provided value.
        /// </summary>
        /// <param name="width">The new width.</param>
        public MRectangle RedimensionWidth(float width) => Redimension(width, Height);

        /// <summary>
        /// Resizes the height of the <see cref="MRectangle"/> to the provided value.
        /// </summary>
        /// <param name="height">The new height.</param>
        public MRectangle RedimensionHeight(float height) => Redimension(Width, height);

        /// <summary>
        /// Resizes the <see cref="MRectangle"/> dimensions to the provided values.
        /// </summary>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        public MRectangle Redimension(float width, float height) => new MRectangle(TopLeft.X, TopLeft.Y, width, height);

        /// <summary>
        /// Resizes the <see cref="MRectangle"/> with the provide delta values.
        /// </summary>
        /// <param name="delta">The change in width and height.</param>
        public MRectangle Resize(MVector2 delta) => Resize(delta.X, delta.Y);

        /// <summary>
        /// Resizes the width of the <see cref="MRectangle"/>.
        /// </summary>
        /// <param name="deltaWidth">The change in width.</param>
        public MRectangle ResizeWidth(float deltaWidth) => Resize(deltaWidth, 0);

        /// <summary>
        /// Resizes the height of the <see cref="MRectangle"/>.
        /// </summary>
        /// <param name="deltaHeight">The change in height.</param>
        public MRectangle ResizeHeight(float deltaHeight) => Resize(0, deltaHeight);

        /// <summary>
        /// Resizes the <see cref="MRectangle"/> with the provide delta values.
        /// </summary>
        /// <param name="deltaWidth">The change in width.</param>
        /// <param name="deltaHeight">The change in height.</param>
        public MRectangle Resize(float deltaWidth, float deltaHeight) =>
            new MRectangle(TopLeft.X, TopLeft.Y, Width + deltaWidth, Height + deltaHeight);

        /// <summary>
        /// Uniformly scales the <see cref="MRectangle"/> around <see cref="Center"/> with the given factor.
        /// </summary>
        /// <param name="factor">The factor with which to scale.</param>
        /// <returns>Scaled rectangle positioned such that the origin is the same as the initial rectangle.</returns>
        public MRectangle Scale(float factor) => new MRectangle(Dimensions * factor).PositionCenter(this);

        /// <summary>
        /// Scales the <see cref="MRectangle"/> around the provided coordinate with the given factor.
        /// </summary>
        /// <param name="factor">The factor with which to scale.</param>
        /// <param name="coordinate">The coordinate to scale around.</param>
        public MRectangle Scale(float factor, MVector2 coordinate)
        {
            var translated = Translate(-coordinate);
            var scaled = new MRectangle(new MVector2(translated.Left * factor, translated.Top * factor), new MVector2(translated.Right * factor, translated.Bottom * factor));
            return scaled.Translate(coordinate);
        }

        /// <summary>
        /// Translates the <see cref="MRectangle"/> with the given translation and returns the result.
        /// </summary>
        /// <param name="translation">The translation to make.</param>
        /// <returns>Translated <see cref="MRectangle"/>.</returns>
        public MRectangle Translate(MVector2 translation) => Translate(translation.X, translation.Y);

        /// <summary>
        /// Translates the <see cref="MRectangle"/> with the given X-translation and returns the result.
        /// </summary>
        /// <param name="dx">The translation along the X-axis.</param>
        /// <returns>Translated <see cref="MRectangle"/>.</returns>
        public MRectangle TranslateX(float dx) => Translate(dx, 0);

        /// <summary>
        /// Translates the <see cref="MRectangle"/> with the given Y-translation and returns the result.
        /// </summary>
        /// <param name="dy">The translation along the Y-axis.</param>
        /// <returns>Translated <see cref="MRectangle"/>.</returns>
        public MRectangle TranslateY(float dy) => Translate(0, dy);

        /// <summary>
        /// Translates the <see cref="MRectangle"/> with the given translation and returns the result.
        /// </summary>
        /// <param name="dx">The translation along the X-axis.</param>
        /// <param name="dy">The translation along the Y-axis.</param>
        /// <returns>Translated <see cref="MRectangle"/>.</returns>
        public MRectangle Translate(float dx, float dy) =>
            new MRectangle(TopLeft.X + dx, TopLeft.Y + dy, BottomRight.X - TopLeft.X, BottomRight.Y - TopLeft.Y);

        /// <summary>
        /// Converts the <see cref="MRectangle"/> to an <see cref="MRectangleInt"/> by cutting of the factions,
        /// basically a float -> int conversion.
        /// </summary>
        public MRectangleInt ToMRectangleInt() => new MRectangleInt(TopLeft.ToMPoint2(), BottomRight.ToMPoint2());
    }
}
