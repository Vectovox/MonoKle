namespace MonoKle.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;

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
            effect = new BasicEffect(graphicsDevice);
            effect.VertexColorEnabled = true;
            Grow();
        }

        /// <summary>
        /// Begins a batch of primitives.
        /// </summary>
        public void Begin() => Begin(Matrix.Identity);

        /// <summary>
        /// Begins a batch of primitives, using a transformation matrix to apply to each primitive.
        /// </summary>
        /// <param name="transformMatrix">Transformation matrix to apply.</param>
        public void Begin(Matrix transformMatrix)
        {
            if (hasBegun)
            {
                throw new InvalidOperationException(AbstractPrimitiveBatch.EXCEPTION_MSG_ALREADY_BEGUN);
            }
            else
            {
                effect.Projection = transformMatrix * GetPostTransformationMatrix(graphicsDevice.Viewport);
                hasBegun = true;
            }
        }

        /// <summary>
        /// Ends a batch of primitives.
        /// </summary>
        public void End()
        {
            if (hasBegun == false)
            {
                throw new InvalidOperationException(AbstractPrimitiveBatch.EXCEPTION_MSG_NOT_BEGUN);
            }
            else
            {
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList,
                        vertexArray,
                        0,
                        nVertices,
                        indexArray,
                        0,
                        (int)(nVertices * 0.5f));
                }

                nVertices = 0;
                hasBegun = false;
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
            if (hasBegun == false)
            {
                throw new InvalidOperationException(AbstractPrimitiveBatch.EXCEPTION_MSG_NOT_BEGUN);
            }
            else
            {
                if (nVertices >= vertexArray.Length)
                {
                    Grow();
                }
                vertexArray[nVertices] = new VertexPositionColor(start, startColor);
                vertexArray[nVertices + 1] = new VertexPositionColor(end, endColor);
                nVertices += 2;
            }
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
            var newVertexArray = new VertexPositionColor[newIndexArray.Length];

            for (short i = 0; i < newIndexArray.Length; i++)
            {
                newIndexArray[i] = i;
            }
            for (int i = 0; i < vertexArray.Length; i++)
            {
                newVertexArray[i] = vertexArray[i];
            }

            indexArray = newIndexArray;
            vertexArray = newVertexArray;
        }
    }
}