using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoKle.Graphics
{
    /// <summary>
    /// Class for drawing primitives in 2D space.
    /// </summary>
    public class PrimitiveBatch2D : AbstractPrimitiveBatch, IPrimitiveBatch2D
    {
        /// <summary>
        /// Creates a new instance of <see cref="PrimitiveBatch2D"/>.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to draw with.</param>
        public PrimitiveBatch2D(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
        }

        /// <summary>
        /// Draws a line to screen.
        /// </summary>
        /// <param name="start">Start coordinate.</param>
        /// <param name="end">End coordinate.</param>
        /// <param name="color">Color of line.</param>
        public void DrawLine(Vector2 start, Vector2 end, Color color) => DrawLine(start, end, color, color);

        /// <summary>
        /// Draws a line to screen.
        /// </summary>
        /// <param name="start">Start coordinate.</param>
        /// <param name="end">End coordinate.</param>
        /// <param name="startColor">Color of line on starting coordinate.</param>
        /// <param name="endColor">Color of line on ending coordinate.</param>
        public void DrawLine(Vector2 start, Vector2 end, Color startColor, Color endColor) => base.AddLine(new Vector3(start, 0f), new Vector3(end, 0f), startColor, endColor);

        protected override Matrix GetPostTransformationMatrix(Viewport viewport) => Matrix.CreateOrthographicOffCenter(0,
                    viewport.Width,
                    viewport.Height,
                    0,
                    0,
                    1);
    }
}