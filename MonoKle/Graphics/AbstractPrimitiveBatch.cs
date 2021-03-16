using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoKle.Graphics
{
    /// <summary>
    /// Abstract class for drawing primitives.
    /// </summary>
    /// <remarks>
    /// Intended purpose is debugging. Should not be run in production.
    /// </remarks>
    public abstract class AbstractPrimitiveBatch : IPrimitiveBatch, IDisposable
    {
        private const string BeginNotCalledExceptionMessage = "Begin has not been called.";
        private const int InitialVertexAmount = 1;

        private readonly BasicEffect _effect;
        private readonly GraphicsDevice _graphicsDevice;
        private bool _hasBegun;
        private short[] _indexArray = new short[InitialVertexAmount];
        private int _nVertices = 0;
        private VertexPositionColor[] _vertexArray = new VertexPositionColor[InitialVertexAmount];

        /// <summary>
        /// Abstract constructor for <see cref="AbstractPrimitiveBatch"/>.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to draw with.</param>
        public AbstractPrimitiveBatch(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _effect = new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = true
            };
            Grow();
        }

        public void Dispose()
        {
            _effect.Dispose();
        }

        public void Begin() => Begin(Matrix.Identity);

        public void Begin(Matrix transformMatrix)
        {
            if (_hasBegun)
            {
                throw new InvalidOperationException("Begin has already been called.");
            }
            else
            {
                _effect.Projection = transformMatrix * GetPostTransformationMatrix(_graphicsDevice.Viewport);
                _hasBegun = true;
            }
        }

        public void End()
        {
            if (!_hasBegun)
            {
                throw new InvalidOperationException(BeginNotCalledExceptionMessage);
            }
            else
            {
                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    _graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList,
                        _vertexArray,
                        0,
                        _nVertices,
                        _indexArray,
                        0,
                        (int)(_nVertices * 0.5f));
                }

                _nVertices = 0;
                _hasBegun = false;
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
            if (!_hasBegun)
            {
                throw new InvalidOperationException(BeginNotCalledExceptionMessage);
            }
            else
            {
                if (_nVertices >= _vertexArray.Length)
                {
                    Grow();
                }
                _vertexArray[_nVertices] = new VertexPositionColor(start, startColor);
                _vertexArray[_nVertices + 1] = new VertexPositionColor(end, endColor);
                _nVertices += 2;
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
            short[] newIndexArray = new short[_indexArray.Length * 2];
            var newVertexArray = new VertexPositionColor[newIndexArray.Length];

            for (short i = 0; i < newIndexArray.Length; i++)
            {
                newIndexArray[i] = i;
            }
            for (int i = 0; i < _vertexArray.Length; i++)
            {
                newVertexArray[i] = _vertexArray[i];
            }

            _indexArray = newIndexArray;
            _vertexArray = newVertexArray;
        }
    }
}