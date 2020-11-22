using Microsoft.Xna.Framework;

namespace MonoKle.Graphics
{
    /// <summary>
    /// Defines an interface for a 2D primitive drawer.
    /// </summary>
    public interface IPrimitiveBatch2D : IPrimitiveBatch
    {
        /// <summary>
        /// Draws a line to screen.
        /// </summary>
        /// <param name="start">Start coordinate.</param>
        /// <param name="end">End coordinate.</param>
        /// <param name="color">Color of line.</param>
        void DrawLine(MVector2 start, MVector2 end, Color color);

        /// <summary>
        /// Draws a line to screen.
        /// </summary>
        /// <param name="start">Start coordinate.</param>
        /// <param name="end">End coordinate.</param>
        /// <param name="startColor">Color of line on starting coordinate.</param>
        /// <param name="endColor">Color of line on ending coordinate.</param>
        void DrawLine(MVector2 start, MVector2 end, Color startColor, Color endColor);

        /// <summary>
        /// Draws the given rectangle with the provided color.
        /// </summary>
        /// <param name="rectangle">The rectangle to draw.</param>
        /// <param name="color">The color to draw with.</param>
        void DrawRectangle(MRectangle rectangle, Color color);

        /// <summary>
        /// Draws the screen centered information contained in the given rendering area.
        /// </summary>
        /// <param name="renderingArea">The rendering area to draw.</param>
        /// <remarks>Intended usage is for deugging.</remarks>
        void DrawRenderingArea(RenderingArea2D renderingArea);

        /// <summary>
        /// Draws a cross to screen.
        /// </summary>
        /// <param name="position">The position of the cross.</param>
        /// <param name="size">The size of the cross, in pixels.</param>
        /// <param name="color">The color of the cross.</param>
        void DrawCross(MVector2 position, float size, Color color);
    }
}