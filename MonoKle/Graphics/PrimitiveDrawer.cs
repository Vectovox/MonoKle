namespace MonoKle.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using MonoKle.Core;
    using System;

    /// <summary>
    /// Class to easily draw simple primitives.
    /// </summary>
    public class PrimitiveDrawer : IPrimitiveDrawer
    {
        private const int INITIAL_VERTEX_AMOUNT = 1;

        private BasicEffect effect;
        private GraphicsDevice graphicsDevice;
        private short[] indexArray = new short[PrimitiveDrawer.INITIAL_VERTEX_AMOUNT];
        private int nVertices = 0;
        private VertexPositionColor[] vertexArray = new VertexPositionColor[PrimitiveDrawer.INITIAL_VERTEX_AMOUNT];
        private VertexBuffer vertexBuffer;

        /// <summary>
        /// Creates a new instance of <see cref="PrimitiveDrawer"/>.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to draw with.</param>
        public PrimitiveDrawer(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.effect = new BasicEffect(graphicsDevice);
            this.effect.VertexColorEnabled = true;
            
            //this.vertexBuffer = new VertexBuffer(graphicsDevice,
            //    VertexPositionColor.VertexDeclaration,
            //    INITIAL_VERTEX_AMOUNT,
            //    BufferUsage.WriteOnly);
            //for (short i = 0; i < INITIAL_VERTEX_AMOUNT; i++)
            //{
            //    this.indexArray[i] = i;
            //}
            this.Grow();
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
            if(this.nVertices >= this.vertexArray.Length)
            {
                this.Grow();
            }
            this.vertexArray[nVertices] = new VertexPositionColor(new Vector3(start, 0f), startColor);
            this.vertexArray[nVertices + 1] = new VertexPositionColor(new Vector3(end, 0f), endColor);
            this.nVertices += 2;
        }

        // TODO: More shapes and stuff
        //public void Draw2DCircle(Vector2 topLeft, Vector2 bottomRight, Color color)
        //{
        //}
        /// <summary>
        /// Renders the currently drawn primitives and clears them.
        /// </summary>
        public void Render()
        {
            if(this.Camera == null)
            {
                this.effect.Projection = Matrix.CreateOrthographicOffCenter(0,
                this.graphicsDevice.Viewport.Width,
                this.graphicsDevice.Viewport.Height,
                0,
                0,
                1);
            }
            else
            {
                this.effect.Projection = this.Camera.GetTransformMatrix() * Matrix.CreateOrthographicOffCenter(0,
                this.graphicsDevice.Viewport.Width,
                this.graphicsDevice.Viewport.Height,
                0,
                0,
                1);
            }
            

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList,
                    this.vertexArray,
                    0,
                    this.nVertices,
                    this.indexArray,
                    0,
                    this.nVertices / 2);
            }

            this.nVertices = 0;
        }

        private void Grow()
        {
            short[] newIndexArray = new short[indexArray.Length * 2];
            VertexPositionColor[] newVertexArray = new VertexPositionColor[newIndexArray.Length];

            this.vertexBuffer = new VertexBuffer(this.graphicsDevice,
                VertexPositionColor.VertexDeclaration,
                newIndexArray.Length,
                BufferUsage.WriteOnly);

            for(short i = 0; i < newIndexArray.Length; i++)
            {
                newIndexArray[i] = i;
            }
            for(int i = 0; i < this.vertexArray.Length; i++)
            {
                newVertexArray[i] = this.vertexArray[i];
            }

            this.indexArray = newIndexArray;
            this.vertexArray = newVertexArray;
        }
    }
}