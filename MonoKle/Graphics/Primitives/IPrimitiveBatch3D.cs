namespace MonoKle.Graphics.Primitives
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines an interface for a 3D primitive drawer.
    /// </summary>
    public interface IPrimitiveBatch3D : IPrimitiveBatch
    {
        /// <summary>
        /// Draws a line.
        /// </summary>
        /// <param name="start">Start coordinate.</param>
        /// <param name="end">End coordinate.</param>
        /// <param name="color">Color of line.</param>
        void DrawLine(Vector3 start, Vector3 end, Color color);

        /// <summary>
        /// Draws a line.
        /// </summary>
        /// <param name="start">Start coordinate.</param>
        /// <param name="end">End coordinate.</param>
        /// <param name="startColor">Color of line on starting coordinate.</param>
        /// <param name="endColor">Color of line on ending coordinate.</param>
        void DrawLine(Vector3 start, Vector3 end, Color startColor, Color endColor);
    }
}