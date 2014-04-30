﻿namespace MonoKle.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using MonoKle.Core;

    /// <summary>
    /// Class to easily draw simple primitives.
    /// </summary>
    public class PrimitiveDrawer
    {
        private const int MAX_VERTICES = 128;

        private BasicEffect effect;
        private GraphicsDevice graphicsDevice;
        private short[] indexArray = new short[MAX_VERTICES];
        private int nVertices = 0;
        private VertexPositionColor[] vertexArray = new VertexPositionColor[MAX_VERTICES];
        private VertexBuffer vertexBuffer;

        internal PrimitiveDrawer(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.vertexBuffer = new VertexBuffer(graphicsDevice,
                VertexPositionColor.VertexDeclaration,
                MAX_VERTICES,
                BufferUsage.WriteOnly);
            this.effect = new BasicEffect(graphicsDevice);
            this.effect.VertexColorEnabled = true;
            for (short i = 0; i < MAX_VERTICES; i++)
            {
                indexArray[i] = i;
            }
            this.Camera = new Camera2D(new Vector2DInteger(this.graphicsDevice.Viewport.Width, this.graphicsDevice.Viewport.Height));
            this.Camera.Update(0);
        }

        /// <summary>
        /// Gets or sets the camera transforming primitive rendering.
        /// </summary>
        public Camera2D Camera
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
        public void Draw2DLine(Vector2 start, Vector2 end, Color color)
        {
            this.Draw2DLine(start, end, color, color);
        }

        /// <summary>
        /// Draws a 2D line to screen.
        /// </summary>
        /// <param name="start">Start coordinate.</param>
        /// <param name="end">End coordinate.</param>
        /// <param name="startColor">Color of line on starting coordinate.</param>
        /// <param name="endColor">Color of line on ending coordinate.</param>
        public void Draw2DLine(Vector2 start, Vector2 end, Color startColor, Color endColor)
        {
            vertexArray[nVertices] = new VertexPositionColor(new Vector3(start, 0f), startColor);
            vertexArray[nVertices + 1] = new VertexPositionColor(new Vector3(end, 0f), endColor);
            nVertices += 2;
        }

        // TODO: More shapes and stuff
        //public void Draw2DCircle(Vector2 topLeft, Vector2 bottomRight, Color color)
        //{
        //}
        internal void Render()
        {
            this.effect.Projection = this.Camera.GetTransformMatrix() * Matrix.CreateOrthographicOffCenter(0,
                this.graphicsDevice.Viewport.Width,
                this.graphicsDevice.Viewport.Height,
                0,
                0,
                1);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList,
                    vertexArray,
                    0,
                    nVertices,
                    indexArray,
                    0,
                    nVertices / 2);
            }

            nVertices = 0;
        }
    }
}