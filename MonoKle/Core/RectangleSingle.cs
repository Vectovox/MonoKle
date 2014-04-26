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
        /// Checks if the rectangle contains the provided rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to check if it is contained.</param>
        /// <returns>True if the specified rectangle is contained, otherwise false.</returns>
        public bool Contains(Rectangle rectangle)
        {
            return this.Contains(rectangle.GetTopLeft()) && this.Contains(rectangle.GetBottomRight());
        }

        /// <summary>
        /// Checks if the rectangle contains the provided rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to check if it is contained.</param>
        /// <returns>True if the specified rectangle is contained, otherwise false.</returns>
        public bool Contains(RectangleSingle rectangle)
        {
            return this.Contains(rectangle.GetTopLeft()) && this.Contains(rectangle.GetBottomRight());
        }

        /// <summary>
        /// Checks if the rectangle contains the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate to check if it is contained.</param>
        /// <returns>True if the specified coordinate is contained, otherwise false.</returns>
        public bool Contains(Vector2 coordinate)
        {
            return coordinate.X >= this.GetLeft() && coordinate.X <= this.GetRight()
                && coordinate.Y >= this.GetTop() && coordinate.Y <= this.GetBottom();
        }

        /// <summary>
        /// Checks if the rectangle contains the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate to check if it is contained.</param>
        /// <returns>True if the specified coordinate is contained, otherwise false.</returns>
        public bool Contains(Vector2Int32 coordinate)
        {
            return coordinate.X >= this.GetLeft() && coordinate.X <= this.GetRight()
                && coordinate.Y >= this.GetTop() && coordinate.Y <= this.GetBottom();
        }

        /// <summary>
        /// Crops the rectangle to fit into the given bounds.
        /// </summary>
        /// <param name="bounds">The bounds to fit into.</param>
        public void Crop(RectangleSingle bounds)
        {
            if(this.X < bounds.X)
            {
                this.X = bounds.X;
            }
            else if(this.X > bounds.GetRight())
            {
                this.X = bounds.GetRight();
            }

            float drl = this.GetRight() - bounds.GetLeft();
            float drr = this.GetRight() - bounds.GetRight();
            if(drl < 0)
            {
                this.Width += drl;
            }
            else if(drr > 0)
            {
                this.Width -= drr;
            }

            if(this.Y < bounds.Y)
            {
                this.Y = bounds.Y;
            }
            else if(this.Y > bounds.GetBottom())
            {
                this.Y = bounds.GetBottom();
            }

            float dbt = this.GetBottom() - bounds.GetTop();
            float dbb = this.GetBottom() - bounds.GetBottom();
            if(dbt < 0)
            {
                this.Height += dbt;
            }
            else if(dbb > 0)
            {
                this.Height -= dbb;
            }
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