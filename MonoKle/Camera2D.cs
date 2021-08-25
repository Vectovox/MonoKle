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
        private bool _matrixOutdated = true;

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
                _matrixOutdated = true;
            }
        }

        /// <summary>
        /// Gets the current world-coordinate viewing frustum of the camera.
        /// </summary>
        public MRectangle View => new MRectangle(TransformInv(MPoint2.Zero), TransformInv(Size));

        /// <summary>
        /// Gets or sets the current camera center position.
        /// </summary>
        public MVector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _matrixOutdated = true;
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
                _matrixOutdated = true;
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
                _matrixOutdated = true;
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
                _matrixOutdated = true;
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
                _matrixOutdated = true;
            }
        }

        /// <summary>
        /// Gets the ratio of how much scaling is applied.
        /// </summary>
        /// <value>Number in the range [0, 1]. 0 means <see cref="Scale"/> is <see cref="MinScale"/> and 1 <see cref="MaxScale"/>.</value>
        /// <remarks>Returns 1 when <see cref="MinScale"/> is equivalent to <see cref="MaxScale"/>.</remarks>
        public float ScaleRatio => _maxScale == _minScale
            ? 1f
            : (_scale - _minScale) / (_maxScale - _minScale);

        /// <summary>
        /// Gets the camera transformation matrix, going from world -> camera space.
        /// </summary>
        public Matrix TransformMatrix
        {
            get
            {
                if (_matrixOutdated)
                {
                    (_transformMatrix, _transformMatrixInv) = CalculateMatrices(_position, _rotation, _scale, _size);
                    _matrixOutdated = false;
                }
                return _transformMatrix;
            }
        }

        /// <summary>
        /// Gets the inverse transformation matrix, going from camera -> world space.
        /// </summary>
        public Matrix TransformMatrixInv
        {
            get
            {
                if (_matrixOutdated)
                {
                    (_transformMatrix, _transformMatrixInv) = CalculateMatrices(_position, _rotation, _scale, _size);
                    _matrixOutdated = false;
                }
                return _transformMatrixInv;
            }
        }

        /// <summary>
        /// Scales seamlessly towards the given coordinate by keeping it in the same spot in
        /// camera space.
        /// </summary>
        /// <param name="worldCoordinate">The camera space coordiante to scale towards.</param>
        /// <param name="newScale">The new scale to set.</param>
        public void ScaleAround(MVector2 worldCoordinate, float newScale) =>
            (Scale, Position) = GetScaleAroundTranslation(worldCoordinate, newScale);

        /// <summary>
        /// Scales seamlessly towards the given coordinate by keeping it in the same spot in
        /// camera space.
        /// </summary>
        /// <param name="worldCoordinate">The camera space coordiante to scale towards.</param>
        /// <param name="deltaScaling">The amount of scaling to add.</param>
        public void ScaleAroundRelative(MVector2 worldCoordinate, float deltaScaling) =>
            (Scale, Position) = GetScaleAroundTranslationRelative(worldCoordinate, deltaScaling);

        /// <summary>
        /// Helper method to get new scale and world position when scaling towards a coordinate.
        /// </summary>
        /// <param name="worldCoordinate">The world coodinate to scale to.</param>
        /// <param name="newScale">The new scale to set to.</param>
        /// <returns>Absolute scale and position tuple.</returns>
        protected (float Scale, MVector2 Position) GetScaleAroundTranslation(MVector2 worldCoordinate, float newScale)
        {
            // Store the camera space of the world coordinate
            var cameraCoordinate = Transform(worldCoordinate);
            // Apply scaling
            var clampedScale = ClampScale(newScale);
            // Terminate early if no scaling was done
            if (Scale == clampedScale)
            {
                return (Scale, Position);
            }
            // Calculate where in the world space the camera coordinate is now
            MVector2 newWorldCoordinate = Vector2.Transform(cameraCoordinate, CalculateMatrices(Position, Rotation, clampedScale, Size).TransformInv);
            // Move camera with how much the scaling changed the world coordinate location
            var newPosition = Position - (newWorldCoordinate - worldCoordinate);
            return (clampedScale, newPosition);
        }

        /// <summary>
        /// Helper method to get new scale and world position when scaling towards a coordinate.
        /// </summary>
        /// <param name="worldCoordinate">The world coodinate to scale to.</param>
        /// <param name="deltaScaling">The relative amount of scaling to add.</param>
        /// <returns>Absolute scale and position tuple.</returns>
        protected (float Scale, MVector2 Position) GetScaleAroundTranslationRelative(MVector2 worldCoordinate, float deltaScaling)
            => GetScaleAroundTranslation(worldCoordinate, Scale + deltaScaling);

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
