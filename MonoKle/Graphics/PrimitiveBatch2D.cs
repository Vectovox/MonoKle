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

        public void DrawLine(MVector2 start, MVector2 end, Color color) => DrawLine(start, end, color, color);

        public void DrawLine(MVector2 start, MVector2 end, Color startColor, Color endColor) => AddLine(new Vector3(start, 0f), new Vector3(end, 0f), startColor, endColor);

        public void DrawRectangle(MRectangle rectangle, Color color)
        {
            DrawLine(rectangle.TopLeft, rectangle.TopRight, color);
            DrawLine(rectangle.TopLeft, rectangle.BottomLeft, color);
            DrawLine(rectangle.BottomRight, rectangle.TopRight, color);
            DrawLine(rectangle.BottomRight, rectangle.BottomLeft, color);
        }

        public void DrawRenderingArea(RenderingArea2D renderingArea)
        {
            DrawRectangle(renderingArea.Desired.PositionCenter(renderingArea.Display), Color.White);
            DrawRectangle(renderingArea.Render.PositionCenter(renderingArea.Display), Color.Yellow);
            DrawRectangle(renderingArea.Display.PositionCenter(renderingArea.Display), Color.Red);
            DrawRectangle(renderingArea.DisplayLogic.PositionCenter(renderingArea.Display), Color.Green);
        }

        public void DrawCross(MVector2 position, float size, Color color)
        {
            DrawLine(position.Translate(-size / 2f, 0), position.Translate(size / 2f, 0), color);
            DrawLine(position.Translate(0, -size / 2f), position.Translate(0, size / 2f), color);
        }

        protected override Matrix GetPostTransformationMatrix(Viewport viewport) =>
            Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
    }
}
