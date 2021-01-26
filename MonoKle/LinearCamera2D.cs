using Microsoft.Xna.Framework;
using System;

namespace MonoKle
{
    /// <summary>
    /// Serializable class representing a 2D environment camera providing transformation capabilities.
    /// </summary>
    [Serializable]
    public class LinearCamera2D : Camera2D
    {
        private float _desiredRotation;
        private float _rotationSpeed;

        private float _desiredScale;
        private float _scalingSpeed;

        private MVector2 _desiredPosition;
        private float _movementSpeed;

        /// <summary>
        /// Initiates a new instance of <see cref="LinearCamera2D"/>.
        /// </summary>
        /// <param name="size">The <see cref="MPoint2"/> represenetation of the camera size.</param>
        public LinearCamera2D(MPoint2 size) : base(size) { }

        /// <summary>
        /// Sets the current camera center position to the given coordinate, moving it there
        /// over time with the provided speed.
        /// </summary>
        /// <param name="position">The coordinate to set to.</param>
        /// <param name="speed">The delta translation per second.</param>
        public void MoveTo(MVector2 position, float speed)
        {
            _desiredPosition = position;
            _movementSpeed = speed;
        }

        /// <summary>
        /// Sets the current rotation, in radians, to the given value over a period of time determined by the given speed.
        /// </summary>
        /// <param name="rotation">The rotation, in radians, to set to.</param>
        /// <param name="speed">The delta rotation, in radians, per second.</param>
        public void RotateTo(float rotation, float speed)
        {
            _desiredRotation = MathHelper.WrapAngle(rotation);
            if (_desiredRotation != Rotation)
            {
                float diff = _desiredRotation - Rotation;

                if (diff > Math.PI)
                {
                    diff -= 2 * (float)Math.PI;
                }
                else if (diff < -Math.PI)
                {
                    diff += 2 * (float)Math.PI;
                }

                _rotationSpeed = diff < 0
                    ? -speed
                    : speed;
            }
        }

        /// <summary>
        /// Sets the current scale factor to the given value over a period of time determined by the given speed.
        /// </summary>
        /// <param name="scale">The scale factor to set to.</param>
        /// <param name="speed">The delta scale per second.</param>
        public void ScaleTo(float scale, float speed)
        {
            _desiredScale = scale;
            _scalingSpeed = (scale - Scale) < 0 ? -speed : speed;
        }

        /// <summary>
        /// Updates camera composition with the given amount of delta time.
        /// </summary>
        /// <param name="timeDelta">Delta time.</param>
        public override void Update(TimeSpan timeDelta)
        {
            UpdateScale(timeDelta);
            UpdateRotation(timeDelta);
            UpdatePosition(timeDelta);
            base.Update(timeDelta);
        }

        private void UpdatePosition(TimeSpan timeDelta)
        {
            if (_movementSpeed != 0)
            {
                var direction = _desiredPosition - Position;
                var delta = direction.Normalized * (float)timeDelta.TotalSeconds * _movementSpeed;

                if (float.IsNaN(delta.X) || direction.LengthSquared < delta.LengthSquared)
                {
                    _movementSpeed = 0;
                    Position = _desiredPosition;
                }
                else
                {
                    Position += delta;
                }
            }
        }

        private void UpdateRotation(TimeSpan timeDelta)
        {
            if (_rotationSpeed != 0)
            {
                // Rotate
                float delta = _rotationSpeed * (float)timeDelta.TotalSeconds;
                Rotation = MathHelper.WrapAngle(Rotation + delta);

                // Check if we turned past the desired rotation and therefore are done
                double remainingDistance = Math.Atan2(Math.Sin(_desiredRotation - Rotation), Math.Cos(_desiredRotation - Rotation));
                if (Math.Abs(remainingDistance) < Math.Abs(delta))
                {
                    _rotationSpeed = 0f;
                    Rotation = _desiredRotation;
                }
            }
        }

        private void UpdateScale(TimeSpan timeDelta)
        {
            if (_scalingSpeed != 0)
            {
                var deltaScaling = _scalingSpeed * (float)timeDelta.TotalSeconds;
                var nextScale = Scale + deltaScaling;
                var remainingScaling = Scale - _desiredScale;
                if (Math.Abs(remainingScaling) < Math.Abs(deltaScaling) || nextScale < MinScale)
                {
                    _scalingSpeed = 0;
                    Scale = _desiredScale;
                }
                else
                {
                    Scale = nextScale;
                }
            }
        }
    }
}
