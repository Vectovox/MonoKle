using Microsoft.Xna.Framework;
using System;

namespace MonoKle
{
    /// <summary>
    /// Serializable class representing a 2D environment camera providing transformation capabilities.
    /// </summary>
    [Serializable]
    public class Camera2D
    {
        private MPoint2 _size;

        private float _rotation;
        private float _desiredRotation;
        private float _desiredRotationSpeed = 0;

        private float _scale = 1f;
        private float _desiredScale;
        private float _desiredScaleSpeed = 0;

        private MVector2 _position;
        private MVector2 _desiredPosition;
        private float _desiredPositionSpeed;

        private Matrix _transformMatrix;
        private Matrix _transformMatrixInv;
        private bool _matrixNeedsUpdate = true;

        /// <summary>
        /// Initiates a new instance of <see cref="Camera2D"/>.
        /// </summary>
        /// <param name="size">The <see cref="MPoint2"/> represenetation of the camera size.</param>
        public Camera2D(MPoint2 size) => _size = size;

        /// <summary>
        /// Gets the size of the camera. 
        /// </summary>
        public MPoint2 Size
        {
            get => _size;
            set { _size = value; _matrixNeedsUpdate = true; }
        }

        /// <summary>
        /// Returns the current camera center position.
        /// </summary>
        public MVector2 Position => _position;

        /// <summary>
        /// Returns the current rotation.
        /// </summary>
        public float Rotation => _rotation;

        /// <summary>
        /// Returns the current scale factor.
        /// </summary>
        public float Scale => _scale;

        /// <summary>
        /// Gets or sets the minimum allowed scaling.
        /// </summary>
        public float MinScale { get; set; } = 0.5f;

        /// <summary>
        /// Gets or sets the maximum allowed scaling.
        /// </summary>
        public float MaxScale { get; set; } = 2f;

        /// <summary>
        /// Gets the transformation matrix representation.
        /// </summary>
        public Matrix TransformMatrix => _transformMatrix;

        /// <summary>
        /// Gets the inverse transformation matrix representation.
        /// </summary>
        public Matrix TransformMatrixInv => _transformMatrixInv;

        /// <summary>
        /// Moves the current camera center by the provided delta vector.
        /// </summary>
        /// <param name="delta">The amount to move the camera by.</param>
        public void Translate(MVector2 delta) => SetPosition(_position + delta);

        /// <summary>
        /// Moves the current camera center in world-space by the provided camera-space delta vector.
        /// Useful for movement based on screen-dragging.
        /// </summary>
        /// <param name="cameraSpaceDelta">The amount to move the camera by.</param>
        public void TranslateCameraSpace(MVector2 cameraSpaceDelta)
        {
            var currentCameraSpacePosition = Transform(_position);
            var nextCameraSpacePosition = currentCameraSpacePosition + cameraSpaceDelta;
            var nextWorldSpacePosition = TransformInv(nextCameraSpacePosition);
            SetPosition(nextWorldSpacePosition);
        }

        /// <summary>
        /// Sets the current camera center position to the given coordinate.
        /// </summary>
        /// <param name="position">The coordinate to set to.</param>
        public void SetPosition(MVector2 position)
        {
            _position = position;
            _desiredPositionSpeed = 0;
            _matrixNeedsUpdate = true;
        }

        /// <summary>
        /// Sets the current camera center position to the given coordinate, moving it there
        /// over time with the provided speed.
        /// </summary>
        /// <param name="position">The coordinate to set to.</param>
        /// <param name="speed">The delta translation per second.</param>
        public void SetPosition(MVector2 position, float speed)
        {
            _desiredPosition = position;
            _desiredPositionSpeed = speed;
        }

        /// <summary>
        /// Sets the current rotation to the given value.
        /// </summary>
        /// <param name="rotation">The value to set rotation to.</param>
        public void SetRotation(float rotation)
        {
            _rotation = MathHelper.WrapAngle(rotation);
            _desiredRotationSpeed = 0;
            _matrixNeedsUpdate = true;
        }

        /// <summary>
        /// Sets the current rotation, in radians, to the given value over a period of time determined by the given speed.
        /// </summary>
        /// <param name="rotation">The rotation, in radians, to set to.</param>
        /// <param name="speed">The delta rotation, in radians, per second.</param>
        public void SetRotation(float rotation, float speed)
        {
            _desiredRotation = MathHelper.WrapAngle(rotation);
            if (_desiredRotation != _rotation)
            {
                float a = _desiredRotation - _rotation;
                if (a > Math.PI)
                    a -= 2 * (float)Math.PI;
                if (a < -Math.PI)
                    a += 2 * (float)Math.PI;

                _desiredRotationSpeed = a < 0 ? -speed : speed;
            }
        }

        /// <summary>
        /// Sets the current scale factor to the given value.
        /// </summary>
        /// <param name="scale">The scale factor to set to.</param>
        public void SetScale(float scale)
        {
            AssignScale(scale);
            _desiredScaleSpeed = 0;
            _matrixNeedsUpdate = true;
        }

        /// <summary>
        /// Sets the current scale factor to the given value over a period of time determined by the given speed.
        /// </summary>
        /// <param name="scale">The scale factor to set to.</param>
        /// <param name="speed">The delta scale per second.</param>
        public void SetScale(float scale, float speed)
        {
            _desiredScale = scale;
            _desiredScaleSpeed = (scale - _scale) < 0 ? -speed : speed;
        }

        /// <summary>
        /// Scales seamlessly towards to given coordinate by keeping the given camera coordinate
        /// in the same world coordinate after the scaling.
        /// </summary>
        /// <param name="cameraCoordinate">The coordiante to scale towards.</param>
        /// <param name="scaling">The amount of scaling to add.</param>
        public void ScaleTo(MVector2 cameraCoordinate, float scaling)
        {
            // Calculate where in the game plane to scale towards
            var previousWorldCoordinate = TransformInv(cameraCoordinate);
            // Update to the new scale
            SetScale(_scale + scaling);
            // Calculate where in the game plane the camera coordinate is now
            var matrix = CalculateMatrices(_position, _rotation, _scale, _size).TransformInv;
            MVector2 newWorldCoordinate = Vector2.Transform(cameraCoordinate, matrix);
            // Move the camera back such that the camera coordinate is back on the same world coordinate
            Translate(previousWorldCoordinate - newWorldCoordinate);
        }

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
        /// <param name="span">Delta time.</param>
        public void Update(TimeSpan span) => Update(span.TotalSeconds);

        private void Update(double seconds)
        {
            UpdateScale(seconds);
            UpdateRotation(seconds);
            UpdatePosition(seconds);

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

        private void UpdatePosition(double seconds)
        {
            if (_desiredPositionSpeed != 0)
            {
                var direction = _desiredPosition - _position;
                var delta = direction.Normalized * (float)seconds * _desiredPositionSpeed;

                if (float.IsNaN(delta.X) || direction.LengthSquared < delta.LengthSquared)
                {
                    _desiredPositionSpeed = 0;
                    _position = _desiredPosition;
                }
                else
                {
                    _position += delta;
                }

                _matrixNeedsUpdate = true;
            }
        }

        private void UpdateRotation(double seconds)
        {
            if (_desiredRotationSpeed != 0)
            {
                float delta = (float)(_desiredRotationSpeed * seconds);
                _rotation = MathHelper.WrapAngle(_rotation + delta);
                double distance = Math.Atan2(Math.Sin(_desiredRotation - _rotation), Math.Cos(_desiredRotation - _rotation));

                // Check if we turned past the desired rotation
                if (Math.Abs(distance) < Math.Abs(delta))
                {
                    _desiredRotationSpeed = 0f;
                    _rotation = _desiredRotation;
                }

                _matrixNeedsUpdate = true;
            }
        }

        private void UpdateScale(double seconds)
        {
            if (_desiredScaleSpeed != 0)
            {
                var delta = (float)(_desiredScaleSpeed * seconds);
                var scalingLeft = _scale - _desiredScale;
                var nextScale = _scale + delta;
                if (Math.Abs(scalingLeft) < Math.Abs(delta) || nextScale < MinScale)
                {
                    _desiredScaleSpeed = 0;
                    AssignScale(_desiredScale);
                }
                else
                {
                    AssignScale(nextScale);
                }

                _matrixNeedsUpdate = true;
            }
        }

        private void AssignScale(float scale) => _scale = Math.Clamp(scale, MinScale, MaxScale);
    }
}
