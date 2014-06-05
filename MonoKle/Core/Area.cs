namespace MonoKle.Core
{
    using System;
    using System.Text;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// Struct for storing an immutable, serializable, normalized (non-negative width and height), float-based area.
    /// </summary>
    [Serializable()]
    public struct Area : IEquatable<Area>
    {
        private const int HASH_CODE_INITIAL = 17;
        private const int HASH_CODE_MULTIPLIER = 23;

        private Vector2 bottomRight;
        private Vector2 topLeft;

        /// <summary>
        /// Creates a new instance of <see cref="Area"/> from the given <see cref="AreaInteger"/>.
        /// <param name="area">The area to instantiate from.</param>
        /// </summary>
        public Area(AreaInteger area)
        {
            this.topLeft = area.TopLeft.ToVector2();
            this.bottomRight = area.BottomRight.ToVector2();
        }

        /// <summary>
        /// Creates a new instance of <see cref="Area"/> around (0, 0) with the given width and height.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public Area(float width, float height)
        {
            float xLeft = Math.Min(0, width);
            float xRight = Math.Max(0, width);
            float yTop = Math.Min(0, height);
            float yBottom = Math.Max(0, height);
            this.topLeft = new Vector2(xLeft, yTop);
            this.bottomRight = new Vector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Area"/> around (0, 0) with the given size.
        /// </summary>
        /// <param name="size">The size.</param>
        public Area(Vector2 size)
        {
            float xLeft = Math.Min(0, size.X);
            float xRight = Math.Max(0, size.X);
            float yTop = Math.Min(0, size.Y);
            float yBottom = Math.Max(0, size.Y);
            this.topLeft = new Vector2(xLeft, yTop);
            this.bottomRight = new Vector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Area"/> by the provided coordinate and the given width and height.
        /// </summary>
        /// <param name="x">X-coordinate of a corner.</param>
        /// <param name="y">Y-coordinate of a corner.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public Area(float x, float y, float width, float height)
        {
            float xLeft = Math.Min(x, x + width);
            float xRight = Math.Max(x, x + width);
            float yTop = Math.Min(y, y + height);
            float yBottom = Math.Max(y, y + height);
            this.topLeft = new Vector2(xLeft, yTop);
            this.bottomRight = new Vector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Area"/> bound by the provided coordinates.
        /// </summary>
        /// <param name="coordA">The first coordinate.</param>
        /// <param name="coordB">The second coordinate.</param>
        public Area(Vector2 coordA, Vector2 coordB)
        {
            float xLeft = Math.Min(coordA.X, coordB.X);
            float xRight = Math.Max(coordA.X, coordB.X);
            float yTop = Math.Min(coordA.Y, coordB.Y);
            float yBottom = Math.Max(coordA.Y, coordB.Y);
            this.topLeft = new Vector2(xLeft, yTop);
            this.bottomRight = new Vector2(xRight, yBottom);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Area"/> bound by the provided coordinates.
        /// </summary>
        /// <param name="coordA">The first coordinate.</param>
        /// <param name="coordB">The second coordinate.</param>
        public Area(Vector2DInteger coordA, Vector2DInteger coordB)
        {
            float xLeft = Math.Min(coordA.X, coordB.X);
            float xRight = Math.Max(coordA.X, coordB.X);
            float yTop = Math.Min(coordA.Y, coordB.Y);
            float yBottom = Math.Max(coordA.Y, coordB.Y);
            this.topLeft = new Vector2(xLeft, yTop);
            this.bottomRight = new Vector2(xRight, yBottom);
        }

        /// <summary>
        /// Returns the Y-coordinate of the bottom border.
        /// </summary>
        public float Bottom
        {
            get { return this.bottomRight.Y; }
        }

        /// <summary>
        /// Returns the bottom left corner.
        /// </summary>
        public Vector2 BottomLeft
        {
            get { return new Vector2(this.topLeft.X, this.bottomRight.Y); }
        }

        /// <summary>
        /// Returns the bottom right corner.
        /// </summary>
        public Vector2 BottomRight
        {
            get { return this.bottomRight; }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public float Height
        {
            get { return this.bottomRight.Y - this.topLeft.Y; }
        }

        /// <summary>
        /// Returns the X-coordinate of the left border.
        /// </summary>
        public float Left
        {
            get { return this.topLeft.X; }
        }

        /// <summary>
        /// Returns the X-coordinate of the right border.
        /// </summary>
        public float Right
        {
            get { return this.bottomRight.X; }
        }

        /// <summary>
        /// Returns the Y-coordinate of the top border.
        /// </summary>
        public float Top
        {
            get { return this.topLeft.Y; }
        }

        /// <summary>
        /// Returns the top left corner.
        /// </summary>
        public Vector2 TopLeft
        {
            get { return this.topLeft; }
        }

        /// <summary>
        /// Returns the top right corner.
        /// </summary>
        public Vector2 TopRight
        {
            get { return new Vector2(this.bottomRight.X, this.topLeft.Y); }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public float Width
        {
            get { return this.bottomRight.X - this.topLeft.X; }
        }

        /// <summary>
        /// Non-equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="Area"/>.</param>
        /// <param name="b">Right <see cref="Area"/>.</param>
        /// <returns>True if not equal, else false.</returns>
        public static bool operator !=(Area a, Area b)
        {
            return a.topLeft != b.topLeft || a.bottomRight != b.bottomRight;
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="a">Left <see cref="Area"/>.</param>
        /// <param name="b">Right <see cref="Area"/>.</param>
        /// <returns>True if are equal, else false.</returns>
        public static bool operator ==(Area a, Area b)
        {
            return a.topLeft == b.topLeft && a.bottomRight == b.bottomRight;
        }

        /// <summary>
        /// Clamps the provided <see cref="Area"/> to fit within this.
        /// </summary>
        /// <param name="area">The <see cref="Area"/> to clamp.</param>
        public Area Clamp(Area area)
        {
            return new Area(this.Clamp(area.topLeft), this.Clamp(area.bottomRight));
        }

        /// <summary>
        /// Clamps the provided <see cref="Vector2"/> to be positioned within this.
        /// </summary>
        /// <param name="coordinate">The <see cref="Vector2"/> to clamp.</param>
        public Vector2 Clamp(Vector2 coordinate)
        {
            if(coordinate.X < this.topLeft.X)
            {
                coordinate.X = this.topLeft.X;
            }
            else if(coordinate.X > this.bottomRight.X)
            {
                coordinate.X = this.bottomRight.X;
            }

            if(coordinate.Y < this.topLeft.Y)
            {
                coordinate.Y = this.topLeft.Y;
            }
            else if(coordinate.Y > this.bottomRight.Y)
            {
                coordinate.Y = this.bottomRight.Y;
            }

            return coordinate;
        }

        /// <summary>
        /// Checks if contains the provided <see cref="Area"/>.
        /// </summary>
        /// <param name="area">The <see cref="Area"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="Area"/> is contained, otherwise false.</returns>
        public bool Contains(Area area)
        {
            return this.Contains(area.topLeft) && this.Contains(area.TopRight)
                && this.Contains(area.BottomLeft) && this.Contains(area.bottomRight);
        }

        /// <summary>
        /// Checks if contains the provided <see cref="AreaInteger"/>.
        /// </summary>
        /// <param name="area">The <see cref="AreaInteger"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="AreaInteger"/> is contained, otherwise false.</returns>
        public bool Contains(AreaInteger area)
        {
            return this.Contains(area.TopLeft) && this.Contains(area.TopRight)
                && this.Contains(area.BottomLeft) && this.Contains(area.BottomRight);
        }

        /// <summary>
        /// Checks if the <see cref="Area"/> contains the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The <see cref="Vector2"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="Vector2"/> is contained, otherwise false.</returns>
        public bool Contains(Vector2 coordinate)
        {
            return coordinate.X >= this.topLeft.X && coordinate.X <= this.bottomRight.X
                && coordinate.Y >= this.topLeft.Y && coordinate.Y <= this.bottomRight.Y;
        }

        /// <summary>
        /// Checks if the <see cref="Area"/> contains the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The <see cref="Vector2DInteger"/> to check if contained.</param>
        /// <returns>True if the specified <see cref="Vector2DInteger"/> is contained, otherwise false.</returns>
        public bool Contains(Vector2DInteger coordinate)
        {
            return coordinate.X >= this.topLeft.X && coordinate.X <= this.bottomRight.X
                && coordinate.Y >= this.topLeft.Y && coordinate.Y <= this.bottomRight.Y;
        }

        /// <summary>
        /// Checks if envelops, contains with a marigin to all borders, the provided <see cref="Area"/>.
        /// </summary>
        /// <param name="area">The <see cref="Area"/> to check if enveloped.</param>
        /// <returns>True if the specified <see cref="Area"/> is enveloped, otherwise false.</returns>
        public bool Envelops(Area area)
        {
            return this.Envelops(area.topLeft) && this.Envelops(area.TopRight)
                && this.Envelops(area.BottomLeft) && this.Envelops(area.bottomRight);
        }

        /// <summary>
        /// Checks if envelops, contains with a marigin to all borders, the provided <see cref="AreaInteger"/>.
        /// </summary>
        /// <param name="area">The <see cref="AreaInteger"/> to check if enveloped.</param>
        /// <returns>True if the specified <see cref="AreaInteger"/> is enveloped, otherwise false.</returns>
        public bool Envelops(AreaInteger area)
        {
            return this.Envelops(area.TopLeft) && this.Envelops(area.TopRight)
                && this.Envelops(area.BottomLeft) && this.Envelops(area.BottomRight);
        }

        /// <summary>
        /// Checks if envelops, contains with a marigin to all borders, the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The <see cref="Vector2"/> to check if enveloped.</param>
        /// <returns>True if the <see cref="Vector2"/> is enveloped, otherwise false.</returns>
        public bool Envelops(Vector2 coordinate)
        {
            return coordinate.X > this.topLeft.X && coordinate.X < this.bottomRight.X
                && coordinate.Y > this.topLeft.Y && coordinate.Y < this.bottomRight.Y;
        }

        /// <summary>
        /// Checks if envelops, contains with a marigin to all borders, the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The <see cref="Vector2DInteger"/> to check if enveloped.</param>
        /// <returns>True if the <see cref="Vector2DInteger"/> is enveloped, otherwise false.</returns>
        public bool Envelops(Vector2DInteger coordinate)
        {
            return coordinate.X > this.topLeft.X && coordinate.X < this.bottomRight.X
                && coordinate.Y > this.topLeft.Y && coordinate.Y < this.bottomRight.Y;
        }

        /// <summary>
        /// Returns whether this is equal to the provided object.
        /// </summary>
        /// <param name="obj">The object to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public override bool Equals(object obj)
        {
            if(obj is Area)
            {
                return this == ((Area)obj);
            }
            return false;
        }

        /// <summary>
        /// Returns whether this is equal to the provided <see cref="Area"/>.
        /// </summary>
        /// <param name="other">The <see cref="Area"/> to check for equality with.</param>
        /// <returns>True if equal, else false.</returns>
        public bool Equals(Area other)
        {
            return this == other;
        }

        /// <summary>
        /// Returns the hash representation.
        /// </summary>
        /// <returns>Hash representation.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = Area.HASH_CODE_INITIAL;
                hash = hash * Area.HASH_CODE_MULTIPLIER + this.topLeft.GetHashCode();
                hash = hash * Area.HASH_CODE_MULTIPLIER + this.bottomRight.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns the <see cref="String"/> representation.
        /// </summary>
        /// <returns><see cref="String"/> representation.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{Top Left: ");
            sb.Append(this.topLeft.ToString());
            sb.Append(", Bottom Right: ");
            sb.Append(this.bottomRight.ToString());
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// Translates the <see cref="Area"/> with the given translation and returns the result.
        /// </summary>
        /// <param name="translation">The translation to make.</param>
        /// <returns>Translated <see cref="Area"/></returns>
        public Area Translate(Vector2 translation)
        {
            return new Area(this.topLeft.X + translation.X, this.topLeft.Y + translation.Y, this.bottomRight.X - this.topLeft.X, this.bottomRight.Y - this.topLeft.Y);
        }

        /// <summary>
        /// Translates the <see cref="Area"/> with the given X-translation and returns the result.
        /// </summary>
        /// <param name="x">The translation along the X-axis.</param>
        /// <returns>Translated <see cref="Area"/></returns>
        public Area TranslateX(float x)
        {
            return new Area(this.topLeft.X + x, this.topLeft.Y, this.bottomRight.X - this.topLeft.X, this.bottomRight.Y - this.topLeft.Y);
        }

        /// <summary>
        /// Translates the <see cref="Area"/> with the given Y-translation and returns the result.
        /// </summary>
        /// <param name="x">The translation along the Y-axis.</param>
        /// <returns>Translated <see cref="Area"/></returns>
        public Area TranslateY(float y)
        {
            return new Area(this.topLeft.X, this.topLeft.Y + y, this.bottomRight.X - this.topLeft.X, this.bottomRight.Y - this.topLeft.Y);
        }
    }
}