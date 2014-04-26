namespace MonoKle.Core
{
    using Microsoft.Xna.Framework;

    /*
     * NOTE: Hopefully Microsoft will add property extensions sooner rather than later to C#.
     * This would be nice to have since that would follow the original Rectangle format of
     * Top/Bottom/Left/Right-properties.
     */
    /// <summary>
    /// Extension class for <see cref="Rectangle"/>.
    /// </summary>
    public static class RectangleExtension
    {
        /// <summary>
        /// Checks if the rectangle contains the provided coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate to check if it is contained.</param>
        /// <returns>True if the specified coordinate is contained, otherwise false.</returns>
        public static bool Contains(this Rectangle rect, Vector2Int32 coordinate)
        {
            return rect.Contains(coordinate.ToVector2());
        }

        /// <summary>
        /// Checks if the rectangle contains the provided rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to check if it is contained.</param>
        /// <returns>True if the specified rectangle is contained, otherwise false.</returns>
        public static bool Contains(this Rectangle rect, RectangleSingle rectangle)
        {
            return rect.Contains(rectangle.GetTopLeft()) && rect.Contains(rectangle.GetBottomRight());
        }

        /// <summary>
        /// Crops the rectangle to fit into the given bounds.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="bounds">The bounds to fit into.</param>
        public static void Crop(this Rectangle rectangle, Rectangle bounds)
        {
            if(rectangle.X < bounds.X)
            {
                rectangle.X = bounds.X;
            }
            else if(rectangle.X > bounds.Right)
            {
                rectangle.X = bounds.Right;
            }

            int drl = rectangle.Right - bounds.Left;
            int drr = rectangle.Right - bounds.Right;
            if(drl < 0)
            {
                rectangle.Width += drl;
            } else if(drr > 0)
            {
                rectangle.Width -= drr;
            }

            if(rectangle.Y < bounds.Y)
            {
                rectangle.Y = bounds.Y;
            }
            else if(rectangle.Y > bounds.Bottom)
            {
                rectangle.Y = bounds.Bottom;
            }

            int dbt = rectangle.Bottom - bounds.Top;
            int dbb = rectangle.Bottom - bounds.Bottom;
            if(dbt < 0)
            {
                rectangle.Height += dbt;
            }
            else if(dbb > 0)
            {
                rectangle.Height -= dbb;
            }
        }

        /// <summary>
        /// Gets the bottom left coordinate.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Vector2Int32 GetBottomLeft(this Rectangle rectangle)
        {
            return new Vector2Int32(rectangle.Left, rectangle.Bottom);
        }

        /// <summary>
        /// Gets the bottom right coordinate.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Vector2Int32 GetBottomRight(this Rectangle rectangle)
        {
            return new Vector2Int32(rectangle.Right, rectangle.Bottom);
        }

        /// <summary>
        /// Gets the top left coordinate.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Vector2Int32 GetTopLeft(this Rectangle rectangle)
        {
            return new Vector2Int32(rectangle.Left, rectangle.Top);
        }

        /// <summary>
        /// Gets the top right coordinate.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Vector2Int32 GetTopRight(this Rectangle rectangle)
        {
            return new Vector2Int32(rectangle.Right, rectangle.Top);
        }
    }
}