namespace MonoKle.Core
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Extension class for <see cref="Rectangle"/>.
    /// </summary>
    public static class RectangleExtension
    {
        /// <summary>
        /// Checks if the rectangle contains the provided coordinate.
        /// </summary>
        /// <param name="rect">The rectangle check if it contains the provided rectangle.</param>
        /// <param name="coordinate">The coordinate to check if it is contained.</param>
        /// <returns>True if the specified coordinate is contained, otherwise false.</returns>
        public static bool Contains(this Rectangle rect, Vector2Int32 coordinate)
        {
            rect = rect.Normalize();
            // TODO: This could use MonoGame's Contains if issue #2436 is fixed
            return coordinate.X >= rect.Left && coordinate.X <= rect.Right &&
                coordinate.Y >= rect.Top && coordinate.Y <= rect.Bottom;
        }

        /// <summary>
        /// Checks if the rectangle contains the provided rectangle.
        /// </summary>
        /// <param name="rect">The rectangle check if it contains the provided rectangle.</param>
        /// <param name="rectangle">The rectangle to check if it is contained.</param>
        /// <returns>True if the specified rectangle is contained, otherwise false.</returns>
        public static bool Contains(this Rectangle rect, RectangleSingle rectangle)
        {
            rect = rect.Normalize();
            // TODO: This fails tests for MonoGame issue #2436
            return rect.Contains(rectangle.GetTopLeft()) && rect.Contains(rectangle.GetTopRight())
                && rect.Contains(rectangle.GetBottomLeft()) && rect.Contains(rectangle.GetBottomRight());
        }

        /// <summary>
        /// Crops the rectangle to fit into the given bounds. The returned rectangle is normalized.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="coordinateA">First coordinate.</param>
        /// <param name="coordinateB">Seconds coordinate.</param>
        /// <returns>Cropped Rectangle.</returns>
        public static Rectangle Crop(this Rectangle rectangle, Vector2Int32 coordinateA, Vector2Int32 coordinateB)
        {
            return rectangle.Crop(new Rectangle(coordinateA.X, coordinateA.Y, coordinateB.X - coordinateA.X, coordinateB.Y - coordinateA.Y));
        }

        /// <summary>
        /// Crops the rectangle to fit into the given bounds. The returned rectangle is normalized.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="bounds">The bounds to fit into.</param>
        /// <returns>Cropped Rectangle.</returns>
        public static Rectangle Crop(this Rectangle rectangle, Rectangle bounds)
        {
            rectangle = rectangle.Normalize();
            bounds = bounds.Normalize();

            int x = rectangle.X;
            int y = rectangle.Y;
            int x2 = rectangle.X + rectangle.Width;
            int y2 = rectangle.Y + rectangle.Height;

            if(x < bounds.X)
            {
                x = bounds.X;
            }
            else if(x > bounds.Right)
            {
                x = bounds.Right;
            }

            if(x2 < bounds.X)
            {
                x2 = bounds.X;
            }
            else if(x2 > bounds.Right)
            {
                x2 = bounds.Right;
            }

            if(y < bounds.Y)
            {
                y = bounds.Y;
            }
            else if(y > bounds.Bottom)
            {
                y = bounds.Bottom;
            }

            if(y2 < bounds.Y)
            {
                y2 = bounds.Y;
            }
            else if(y2 > bounds.Bottom)
            {
                y2 = bounds.Bottom;
            }

            return new Rectangle(x, y, x2 - x, y2 - y);
        }

        /// <summary>
        /// Gets the bottom left coordinate.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>Bottom left coordinate.</returns>
        public static Vector2Int32 GetBottomLeft(this Rectangle rectangle)
        {
            return new Vector2Int32(rectangle.Left, rectangle.Bottom);
        }

        /// <summary>
        /// Gets the bottom right coordinate.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>Bottom right coordinate.</returns>
        public static Vector2Int32 GetBottomRight(this Rectangle rectangle)
        {
            return new Vector2Int32(rectangle.Right, rectangle.Bottom);
        }

        /// <summary>
        /// Gets the top left coordinate.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>Top left coordinate.</returns>
        public static Vector2Int32 GetTopLeft(this Rectangle rectangle)
        {
            return new Vector2Int32(rectangle.Left, rectangle.Top);
        }

        /// <summary>
        /// Gets the top right coordinate.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>Top right coordinate.</returns>
        public static Vector2Int32 GetTopRight(this Rectangle rectangle)
        {
            return new Vector2Int32(rectangle.Right, rectangle.Top);
        }

        /// <summary>
        /// Returns whether the rectangle is normalized, i.e. contains non-negative width and height.
        /// </summary>
        /// <param name="rectangle">Parameter to check.</param>
        /// <returns>True if normalized, else false.</returns>
        public static bool IsNormalized(this Rectangle rectangle)
        {
            return rectangle.Width >= 0 && rectangle.Height >= 0;
        }

        /// <summary>
        /// Normalizes the rectangle to have non-negative width and height.
        /// </summary>
        /// <param name="rectangle">The rectangle to normalize.</param>
        /// <returns>Normalized rectangle.</returns>
        public static Rectangle Normalize(this Rectangle rectangle)
        {
            if(rectangle.Width < 0)
            {
                rectangle.X += rectangle.Width;
                rectangle.Width = -rectangle.Width;
            }
            if(rectangle.Height < 0)
            {
                rectangle.Y += rectangle.Height;
                rectangle.Height = -rectangle.Height;
            }
            return rectangle;
        }
    }
}