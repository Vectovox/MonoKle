namespace MonoKle.Graphics
{
    using Microsoft.Xna.Framework;

    using MonoKle.Core;

    /// <summary>
    /// Defines an interface for a primitive drawer.
    /// </summary>
    public interface IPrimitiveDrawer
    {
        /// <summary>
        /// Gets or sets the camera transforming primitive rendering.
        /// </summary>
        Camera2D Camera
        {
            get;
            set;
        }

        /// <summary>
        /// Draws a 2D line to screen.
        /// </summary>
        /// <param name="start">Start coordinate.</param>
        /// <param name="end">End coordinate.</param>
        /// <param name="color">Color of line.</param>
        void Draw2DLine(Vector2 start, Vector2 end, Color color);

        /// <summary>
        /// Draws a 2D line to screen.
        /// </summary>
        /// <param name="start">Start coordinate.</param>
        /// <param name="end">End coordinate.</param>
        /// <param name="startColor">Color of line on starting coordinate.</param>
        /// <param name="endColor">Color of line on ending coordinate.</param>
        void Draw2DLine(Vector2 start, Vector2 end, Color startColor, Color endColor);
    }
}