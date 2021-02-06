using Microsoft.Xna.Framework;
using System;

namespace MonoKle
{
    /// <summary>
    /// Serializable class representing a 2D environment camera with capabilities over time.
    /// </summary>
    [Serializable]
    public class DynamicCamera2D : Camera2D
    {
        public float DesiredRotation { get; private set; }
        private float _rotationSpeed;

        public float DesiredScale { get; private set; }
        private float _scalingSpeed;

        public MVector2 DesiredPosition { get; private set; }
        private float _translationSpeed;

        /// <summary>
        /// Initiates a new instance of <see cref="DynamicCamera2D"/>.
        /// </summary>
        /// <param name="size">The <see cref="MPoint2"/> represenetation of the camera size.</param>
        public DynamicCamera2D(MPoint2 size) : base(size) { }

        /// <summary>
        /// Moves the current camera center position to the given coordinate
        /// over time with the provided speed.
        /// </summary>
        /// <param name="position">The coordinate to set to.</param>
        /// <param name="speed">The delta translation per second.</param>
        public void MoveTo(MVector2 position, float speed)
        {
            if (speed < 0)
            {
                throw new ArgumentException($"{nameof(speed)} may not be negative");
            }

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
            if (speed < 0)
            {
                throw new ArgumentException($"{nameof(speed)} may not be negative");
            }

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
            if (speed < 0)
            {
                throw new ArgumentException($"{nameof(speed)} may not be negative");
            }

            DesiredScale = scale;
            _scalingSpeed = (scale - Scale) < 0 ? -speed : speed;
        }

        /// <summary>
        /// Scales seamlessly towards the given coordinate over time with the provided speed.
        /// </summary>
        /// <param name="worldCoordinate">The camera space coordiante to scale towards.</param>
        /// <param name="deltaScaling">The amount of scaling to add.</param>
        /// <param name="duration">The duration of the scaling.</param>
        public void ScaleAroundTo(MVector2 worldCoordinate, float deltaScaling, TimeSpan duration)
        {
            var (newScale, newPosition) = GetScaleAroundTranslation(worldCoordinate, deltaScaling);
            ScaleTo(newScale, Math.Abs(deltaScaling) / (float)duration.TotalSeconds);
            MoveTo(newPosition, (newPosition - Position).Length / (float)duration.TotalSeconds);
        }

        /// <summary>
        /// Scales seamlessly towards the given coordinate over time with the provided speed.
        /// </summary>
        /// <param name="worldCoordinate">The camera space coordiante to scale towards.</param>
        /// <param name="zoomFactor">The zoom factor, either negative or positive, to apply.</param>
        /// <param name="duration">The duration of the zooming.</param>
        public void ZoomAroundTo(MVector2 worldCoordinate, float zoomFactor, TimeSpan duration) =>
            ScaleAroundTo(worldCoordinate, Scale * zoomFactor, duration);

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
