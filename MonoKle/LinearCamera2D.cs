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
        public float DesiredRotation { get; private set; }
        private float _rotationSpeed;

        public float DesiredScale { get; private set; }
        private float _scalingSpeed;

        public MVector2 DesiredPosition { get; private set; }
        private float _translationSpeed;

        /// <summary>
        /// Initiates a new instance of <see cref="LinearCamera2D"/>.
        /// </summary>
        /// <param name="size">The <see cref="MPoint2"/> represenetation of the camera size.</param>
        public LinearCamera2D(MPoint2 size) : base(size) { }

        /// <summary>
        /// Moves the current camera center position to the given coordinate
        /// over time with the provided speed.
        /// </summary>
        /// <param name="position">The coordinate to set to.</param>
        /// <param name="speed">The delta translation per second.</param>
        public void MoveTo(MVector2 position, float speed)
        {
            DesiredPosition = position;
            _translationSpeed = speed;
        }

        /// <summary>
        /// Rotates the camera, in radians, to the given value over time with the provided speed.
        /// </summary>
        /// <param name="rotation">The rotation, in radians, to set to.</param>
        /// <param name="speed">The delta rotation, in radians, per second.</param>
        public void RotateTo(float rotation, float speed)
        {
            DesiredRotation = MathHelper.WrapAngle(rotation);
            if (DesiredRotation != Rotation)
            {
                float diff = DesiredRotation - Rotation;

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
        /// Scales the camera to the given value over time with the provided speed.
        /// </summary>
        /// <param name="scale">The scale factor to set to.</param>
        /// <param name="speed">The delta scale per second.</param>
        public void ScaleTo(float scale, float speed)
        {
            DesiredScale = scale;
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
            if (_translationSpeed != 0)
            {
                var direction = DesiredPosition - Position;
                var delta = direction.Normalized * (float)timeDelta.TotalSeconds * _translationSpeed;

                if (float.IsNaN(delta.X) || direction.LengthSquared < delta.LengthSquared)
                {
                    _translationSpeed = 0;
                    Position = DesiredPosition;
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
                double remainingDistance = Math.Atan2(Math.Sin(DesiredRotation - Rotation), Math.Cos(DesiredRotation - Rotation));
                if (Math.Abs(remainingDistance) < Math.Abs(delta))
                {
                    _rotationSpeed = 0f;
                    Rotation = DesiredRotation;
                }
            }
        }

        private void UpdateScale(TimeSpan timeDelta)
        {
            if (_scalingSpeed != 0)
            {
                var deltaScaling = _scalingSpeed * (float)timeDelta.TotalSeconds;
                var nextScale = Scale + deltaScaling;
                var remainingScaling = Scale - DesiredScale;
                if (Math.Abs(remainingScaling) < Math.Abs(deltaScaling) || nextScale < MinScale)
                {
                    _scalingSpeed = 0;
                    Scale = DesiredScale;
                }
                else
                {
                    Scale = nextScale;
                }
            }
        }
    }
}
