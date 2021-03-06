using Microsoft.Xna.Framework;
using System;

namespace MonoKle
{
    /// <summary>
    /// Serializable class representing a 2D environment camera.
    /// </summary>
    [Serializable]
    public class Camera2D
    {
        private MPoint2 _size;

        private float _rotation;
        private MVector2 _position;

        private float _scale = 1f;
        private float _minScale = 0.5f;
        private float _maxScale = 2f;

        private Matrix _transformMatrix;
        private Matrix _transformMatrixInv;
        private bool _matrixNeedsUpdate = true;

        /// <summary>
        /// Initiates a new instance of <see cref="Camera2D"/>.
        /// </summary>
        /// <param name="size">The size of the camera.</param>
        public Camera2D(MPoint2 size) => Size = size;

        /// <summary>
        /// Gets the size of the camera. 
        /// </summary>
        public MPoint2 Size
        {
            get => _size;
            set
            {
                _size = value;
                _matrixNeedsUpdate = true;
            }
        }

        /// <summary>
        /// Gets or sets the current camera center position.
        /// </summary>
        public MVector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _matrixNeedsUpdate = true;
            }
        }

        /// <summary>
        /// Gets or sets the current camera rotation (in radians).
        /// </summary>
        public float Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                _matrixNeedsUpdate = true;
            }
        }

        /// <summary>
        /// Gets or sets the current scaling factor.
        /// </summary>
        public float Scale
        {
            get => _scale;
            set
            {
                _scale = ClampScale(value);
                _matrixNeedsUpdate = true;
            }
        }
        private float ClampScale(float scale) => Math.Clamp(scale, MinScale, MaxScale);

        /// <summary>
        /// Gets or sets the minimum allowed scaling.
        /// </summary>
        public float MinScale
        {
            get => _minScale;
            set
            {
                _minScale = value;
                Scale = Scale;  // Update clamping
                _matrixNeedsUpdate = true;
            }
        }

        /// <summary>
        /// Gets or sets the maximum allowed scaling.
        /// </summary>
        public float MaxScale
        {
            get => _maxScale;
            set
            {
                _maxScale = value;
                Scale = Scale;  // Update clamping
                _matrixNeedsUpdate = true;
            }
        }

        /// <summary>
        /// Gets the camera transformation matrix, going from world -> camera space.
        /// </summary>
        public Matrix TransformMatrix => _transformMatrix;

        /// <summary>
        /// Gets the inverse transformation matrix, going from camera -> world space.
        /// </summary>
        public Matrix TransformMatrixInv => _transformMatrixInv;

        /// <summary>
        /// Scales seamlessly towards the given coordinate by keeping it in the same spot in
        /// camera space.
        /// </summary>
        /// <param name="worldCoordinate">The camera space coordiante to scale towards.</param>
        /// <param name="deltaScaling">The amount of scaling to add.</param>
        public void ScaleAround(MVector2 worldCoordinate, float deltaScaling) =>
            (Scale, Position) = GetScaleAroundTranslation(worldCoordinate, deltaScaling);

        protected (float Scale, MVector2 Position) GetScaleAroundTranslation(MVector2 worldCoordinate, float deltaScaling)
        {
            // Store the camera space of the world coordinate
            var cameraCoordinate = Transform(worldCoordinate);
            // Apply scaling
            var newScale = ClampScale(Scale + deltaScaling);
            // Terminate early if no scaling was done
            if (Scale == newScale)
            {
                return (Scale, Position);
            }
            // Calculate where in the world space the camera coordinate is now
            MVector2 newWorldCoordinate = Vector2.Transform(cameraCoordinate, CalculateMatrices(Position, Rotation, newScale, Size).TransformInv);
            // Move camera with how much the scaling changed the world coordinate location
            var newPosition = Position - (newWorldCoordinate - worldCoordinate);
            return (newScale, newPosition);
        }

        /// <summary>
        /// Zooms seamlessly towards/from the given coordinate by keeping it in the same spot in
        /// camera space.
        /// </summary>
        /// <param name="worldCoordinate">The camera space coordiante to scale towards.</param>
        /// <param name="zoomFactor">The zoom factor, either negative or positive, to apply.</param>
        public void ZoomAround(MVector2 worldCoordinate, float zoomFactor) => ScaleAround(worldCoordinate, Scale * zoomFactor);

        /// <summary>
        /// Transforms a given coordinate from world space to camera space.
        /// </summary>
        /// <param name="coordinate">The coordinate to transform.</param>
        /// <returns>Transformed coordinate.</returns>
        public MVector2 Transform(MVector2 coordinate) => Vector2.Transform(coordinate, _transformMatrix);

        /// <summary>
        /// Inversely transforms a given coordinate, effectively untransforming camera space to world space.
        /// </summary>
        /// <param name="coordinate">The coordinate to transform.</param>
        /// <returns>Transformed coordinate.</returns>
        public MVector2 TransformInv(MVector2 coordinate) => Vector2.Transform(coordinate, _transformMatrixInv);

        /// <summary>
        /// Updates camera composition with the given amount of delta time.
        /// </summary>
        /// <param name="timeDelta">Delta time.</param>
        public virtual void Update(TimeSpan timeDelta)
        {
            if (_matrixNeedsUpdate)
            {
                (_transformMatrix, _transformMatrixInv) = CalculateMatrices(_position, _rotation, _scale, _size);
            }
        }

        private static (Matrix Transform, Matrix TransformInv) CalculateMatrices(MVector2 position, float rotation, float scale, MPoint2 size)
        {
            MVector2 center = size.ToMVector2() * 0.5f;
            var transform = Matrix.CreateTranslation(-new Vector3(position - center, 0f))
            * Matrix.CreateTranslation(-new Vector3(center, 0f))
            * Matrix.CreateRotationZ(-rotation)
            * Matrix.CreateScale(scale)
            * Matrix.CreateTranslation(new Vector3(center, 0f));
            return (Transform: transform, TransformInv: Matrix.Invert(transform));
        }
    }
}
