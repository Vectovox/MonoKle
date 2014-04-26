namespace MonoKle.Core
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Struct for storing a float-based rectangle.
    /// </summary>
    public struct RectangleSingle
    {
        /// <summary>
        /// Height of the rectangle.
        /// </summary>
        public float Height;

        /// <summary>
        /// Width of the rectangle.
        /// </summary>
        public float Width;

        /// <summary>
        /// Top left X-coordinate.
        /// </summary>
        public float X;

        /// <summary>
        /// Top left Y-coordinate.
        /// </summary>
        public float Y;

        /// <summary>
        /// Creates a new instance of <see cref="RectangleSingle"/> from an integer based rectangle.
        /// </summary>
        /// <param name="rectangle">Integer based rectangle.</param>
        public RectangleSingle(Rectangle rectangle)
        {
            this.X = rectangle.X;
            this.Y = rectangle.Y;
            this.Width = rectangle.Width;
            this.Height = rectangle.Height;
        }

        /// <summary>
        /// Creates a new instance of <see cref="RectangleSingle"/> with the given width and height.
        /// </summary>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        public RectangleSingle(float width, float height)
        {
            this.X = 0;
            this.Y = 0;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Creates a new instance of <see cref="RectangleSingle"/> on the provided coordinate and with the given width and height.
        /// </summary>
        /// <param name="x">X-coordinate of the top left corner.</param>
        /// <param name="y">Y-coordinate of the top left corner.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        public RectangleSingle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Creates a new instance of <see cref="RectangleSingle"/> by the provided top left- and bottom right coordinates.
        /// </summary>
        /// <param name="topLeft">Top left corner coordinate.</param>
        /// <param name="bottomRight">Bottom right corner coordinate.</param>
        public RectangleSingle(Vector2 topLeft, Vector2 bottomRight)
        {
            this.X = topLeft.X;
            this.Y = topLeft.Y;
            this.Width = bottomRight.X - topLeft.X;
            this.Height = bottomRight.Y - topLeft.Y;
        }

        /// <summary>
        /// Returns the bottom of the rectangle.
        /// </summary>
        /// <returns></returns>
        public float GetBottom()
        {
            return this.Y + this.Height;
        }

        /// <summary>
        /// Returns the bottom left corner.
        /// </summary>
        /// <returns>Bottom left corner.</returns>
        public Vector2 GetBottomLeft()
        {
            return new Vector2(this.X, this.Y + this.Height);
        }

        /// <summary>
        /// Returns the bottom right corner.
        /// </summary>
        /// <returns>Bottom right corner.</returns>
        public Vector2 GetBottomRight()
        {
            return new Vector2(this.X + this.Width, this.Y + this.Height);
        }

        /// <summary>
        /// Returns the left of the rectangle.
        /// </summary>
        /// <returns></returns>
        public float GetLeft()
        {
            return this.X;
        }

        /// <summary>
        /// Returns the right of the rectangle.
        /// </summary>
        /// <returns></returns>
        public float GetRight()
        {
            return this.X + this.Width;
        }

        /// <summary>
        /// Returns the top of the rectangle.
        /// </summary>
        /// <returns></returns>
        public float GetTop()
        {
            return this.Y;
        }

        /// <summary>
        /// Returns the top left corner.
        /// </summary>
        /// <returns>Top left corner.</returns>
        public Vector2 GetTopLeft()
        {
            return new Vector2(this.X, this.Y);
        }

        /// <summary>
        /// Returns the top right corner.
        /// </summary>
        /// <returns>Top right corner.</returns>
        public Vector2 GetTopRight()
        {
            return new Vector2(this.X + this.Width, this.Y);
        }

        /// <summary>
        /// Returns an integer based rectangle, cutting off decimals.
        /// </summary>
        /// <returns>Integer based rectangle.</returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle((int)this.X, (int)this.Y, (int)this.Width, (int)this.Height);
        }
    }
}