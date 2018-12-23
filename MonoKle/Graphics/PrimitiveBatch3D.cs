namespace MonoKle.Graphics {
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Class for drawing primitives in 3D space.
    /// </summary>
    public class PrimitiveBatch3D : AbstractPrimitiveBatch, IPrimitiveBatch3D {
        /// <summary>
        /// Creates a new instance of <see cref="PrimitiveBatch3D"/>.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to draw with.</param>
        public PrimitiveBatch3D(GraphicsDevice graphicsDevice)
            : base(graphicsDevice) {
        }

        /// <summary>
        /// Draws a line.
        /// </summary>
        /// <param name="start">Start coordinate.</param>
        /// <param name="end">End coordinate.</param>
        /// <param name="color">Color of line.</param>
        public void DrawLine(Vector3 start, Vector3 end, Color color) => DrawLine(start, end, color, color);

        /// <summary>
        /// Draws a line.
        /// </summary>
        /// <param name="start">Start coordinate.</param>
        /// <param name="end">End coordinate.</param>
        /// <param name="startColor">Color of line on starting coordinate.</param>
        /// <param name="endColor">Color of line on ending coordinate.</param>
        public void DrawLine(Vector3 start, Vector3 end, Color startColor, Color endColor) => base.AddLine(start, end, startColor, endColor);

        protected override Matrix GetPostTransformationMatrix(Viewport viewport) => Matrix.Identity;
    }
}