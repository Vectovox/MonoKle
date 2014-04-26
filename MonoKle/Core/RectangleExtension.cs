namespace MonoKle.Core
{
    using Microsoft.Xna.Framework;

    /*
     * NOTE: Hopefully Microsoft will add property extensions sooner rather than later to C#.
     * This would be nice to have since that would follow the original Rectangle format of
     * Top/Bottom/Left/Right-properties.
     */
    public static class RectangleExtension
    {
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