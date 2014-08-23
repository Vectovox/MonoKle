namespace MonoKle.Graphics.Primitives
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Abstract class for drawing primitives.
    /// </summary>
    public abstract class AbstractPrimitiveBatch : IPrimitiveBatch
    {
        private const string EXCEPTION_MSG_ALREADY_BEGUN = "Begin has already been called.";
        private const string EXCEPTION_MSG_NOT_BEGUN = "Begin has not been called.";
        private const int INITIAL_VERTEX_AMOUNT = 1;

        private BasicEffect effect;
        private GraphicsDevice graphicsDevice;
        private bool hasBegun;
        private short[] indexArray = new short[AbstractPrimitiveBatch.INITIAL_VERTEX_AMOUNT];
        private int nVertices = 0;
        private VertexPositionColor[] vertexArray = new VertexPositionColor[AbstractPrimitiveBatch.INITIAL_VERTEX_AMOUNT];

        /// <summary>
        /// Abstract constructor for <see cref="AbstractPrimitiveBatch"/>.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to draw with.</param>
        public AbstractPrimitiveBatch(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.effect = new BasicEffect(graphicsDevice);
            this.effect.VertexColorEnabled = true;
            this.Grow();
        }

        /// <summary>
        /// Begins a batch of primitives.
        /// </summary>
        public void Begin()
        {
            this.Begin(Matrix.Identity);
        }

        /// <summary>
        /// Begins a batch of primitives, using a transformation matrix to apply to each primitive.
        /// </summary>
        /// <param name="transformMatrix">Transformation matrix to apply.</param>
        public void Begin(Matrix transformMatrix)
        {
            if(this.hasBegun)
            {
                throw new InvalidOperationException(AbstractPrimitiveBatch.EXCEPTION_MSG_ALREADY_BEGUN);
            }
            else
            {
                this.effect.Projection = transformMatrix * GetPostTransformationMatrix(this.graphicsDevice.Viewport);
                this.hasBegun = true;
            }
        }

        /// <summary>
        /// Ends a batch of primitives.
        /// </summary>
        public void End()
        {
            if(this.hasBegun == false)
            {
                throw new InvalidOperationException(AbstractPrimitiveBatch.EXCEPTION_MSG_NOT_BEGUN);
            }
            else
            {
                foreach(EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    this.graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList,
                        this.vertexArray,
                        0,
                        this.nVertices,
                        this.indexArray,
                        0,
                        (int)(this.nVertices * 0.5f));
                }

                this.nVertices = 0;
                this.hasBegun = false;
            }
        }

        /// <summary>
        /// Adds a line to draw.
        /// </summary>
        /// <param name="start">Start coordinate.</param>
        /// <param name="end">End coordinate.</param>
        /// <param name="startColor">Color of line on starting coordinate.</param>
        /// <param name="endColor">Color of line on ending coordinate.</param>
        protected void AddLine(Vector3 start, Vector3 end, Color startColor, Color endColor)
        {
            if(this.nVertices >= this.vertexArray.Length)
            {
                this.Grow();
            }
            this.vertexArray[nVertices] = new VertexPositionColor(start, startColor);
            this.vertexArray[nVertices + 1] = new VertexPositionColor(end, endColor);
            this.nVertices += 2;
        }

        /// <summary>
        /// Returns transformation matrix to apply after the transform matrix provided in Begin().
        /// </summary>
        /// <param name="viewport">The current viewport.</param>
        /// <returns>Transformation matrix.</returns>
        protected abstract Matrix GetPostTransformationMatrix(Viewport viewport);

        private void Grow()
        {
            short[] newIndexArray = new short[indexArray.Length * 2];
            VertexPositionColor[] newVertexArray = new VertexPositionColor[newIndexArray.Length];

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