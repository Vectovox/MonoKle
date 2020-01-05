namespace MonoKle
{
    using Microsoft.Xna.Framework;
    using System;

    /// <summary>
    /// Serializable class representing a 2D environment camera providing transformation capabilities.
    /// </summary>
    [Serializable]
    public class Camera2D
    {
        // TODO: Add desired position and a method that travels to a given position from the current one. private Vector2 desiredPosition;
        // TODO: Calculate matrixes when properties are read to not have to call Update + get lazy evaluation
        // TODO: Movable camera as a subclass?
        private float desiredRotation;
        private float desiredRotationSpeed = 0;
        private float desiredScale;
        private float desiredScaleSpeed = 0;
        private bool matrixNeedsUpdate = true;
        private MVector2 position;
        private float rotation;
        private float scale = 1f;
        private MPoint2 size;
        private Matrix transformMatrix;
        private Matrix transformMatrixInv;

        /// <summary>
        /// Initiates a new instance of <see cref="Camera2D"/>.
        /// </summary>
        /// <param name="size">The <see cref="MPoint2"/> represenetation of the camera size.</param>
        public Camera2D(MPoint2 size) => this.size = size;

        /// <summary>
        /// Gets the size of the camera. 
        /// </summary>
        public MPoint2 Size
        {
            get => size;
            set { size = value; matrixNeedsUpdate = true; }
        }

        /// <summary>
        /// Returns the current camera center position.
        /// </summary>
        public MVector2 Position => position;

        /// <summary>
        /// Returns the current rotation.
        /// </summary>
        public float Rotation => rotation;

        /// <summary>
        /// Returns the current scale factor.
        /// </summary>
        public float Scale => scale;

        /// <summary>
        /// Gets the transformation matrix representation.
        /// </summary>
        public Matrix TransformMatrix => transformMatrix;

        /// <summary>
        /// Gets the inverse transformation matrix representation.
        /// </summary>
        public Matrix TransformMatrixInv => transformMatrixInv;

        /// <summary>
        /// Sets the current camera center position to the given coordinate.
        /// </summary>
        /// <param name="position">The Vector2 coordinate to set to.</param>
        public void SetPosition(MVector2 position)
        {
            this.position = position;
            matrixNeedsUpdate = true;
        }

        /// <summary>
        /// Sets the current rotation to the given value.
        /// </summary>
        /// <param name="rotation">The value to set rotation to.</param>
        public void SetRotation(float rotation)
        {
            this.rotation = MathHelper.WrapAngle(rotation);
            desiredRotationSpeed = 0;
            matrixNeedsUpdate = true;
        }

        /// <summary>
        /// Sets the current rotation, in radians, to the given value over a period of time determined by the given speed.
        /// </summary>
        /// <param name="rotation">The rotation, in radians, to set to.</param>
        /// <param name="speed">The delta rotation, in radians, per second.</param>
        public void SetRotation(float rotation, float speed)
        {
            desiredRotation = MathHelper.WrapAngle(rotation);
            if (desiredRotation != this.rotation)
            {
                float a = desiredRotation - this.rotation;
                if (a > Math.PI)
                    a -= 2 * (float)Math.PI;
                if (a < -Math.PI)
                    a += 2 * (float)Math.PI;

                desiredRotationSpeed = a < 0 ? -speed : speed;
            }
        }

        /// <summary>
        /// Sets the current scale factor to the given value.
        /// </summary>
        /// <param name="scale">The scale factor to set to.</param>
        public void SetScale(float scale)
        {
            this.scale = scale;
            desiredScaleSpeed = 0;
            matrixNeedsUpdate = true;
        }

        /// <summary>
        /// Sets the current scale factor to the given value over a period of time determined by the given speed.
        /// </summary>
        /// <param name="scale">The scale factor to set to.</param>
        /// <param name="speed">The delta scale per second.</param>
        public void SetScale(float scale, float speed)
        {
            desiredScale = scale;
            desiredScaleSpeed = (scale - this.scale) < 0 ? -speed : speed;
        }

        /// <summary>
        /// Transforms a given coordinate from world space to camera space.
        /// </summary>
        /// <param name="coordinate">The coordinate to transform.</param>
        /// <returns>Transformed coordinate.</returns>
        public MVector2 Transform(MVector2 coordinate) => Vector2.Transform(coordinate, transformMatrix);

        /// <summary>
        /// Inversely transforms a given coordinate, effectively untransforming camera space to world space.
        /// </summary>
        /// <param name="coordinate">The coordinate to transform.</param>
        /// <returns>Transformed coordinate.</returns>
        public MVector2 TransformInv(MVector2 coordinate) => Vector2.Transform(coordinate, transformMatrixInv);

        /// <summary>
        /// Updates camera composition with the given amount of delta time.
        /// </summary>
        /// <param name="span">Delta time.</param>
        public void Update(TimeSpan span) => Update(span.TotalSeconds);

        private void Update(double seconds)
        {
            UpdateScale(seconds);
            UpdateRotation(seconds);

            if (matrixNeedsUpdate)
            {
                MVector2 center = size.ToMVector2() * 0.5f;
                transformMatrix = Matrix.CreateTranslation(-new Vector3(position - center, 0f))
                * Matrix.CreateTranslation(-new Vector3(center, 0f))
                * Matrix.CreateRotationZ(-rotation)
                * Matrix.CreateScale(scale)
                * Matrix.CreateTranslation(new Vector3(center, 0f));

                transformMatrixInv = Matrix.Invert(transformMatrix);
            }
        }

        private void UpdateRotation(double seconds)
        {
            if (desiredRotationSpeed != 0)
            {
                float delta = (float)(desiredRotationSpeed * seconds);
                rotation = MathHelper.WrapAngle(rotation + delta);
                double distance = Math.Atan2(Math.Sin(desiredRotation - rotation), Math.Cos(desiredRotation - rotation));

                // Check if we turned past the desired rotation
                if (Math.Abs(distance) < Math.Abs(delta))
                {
                    desiredRotationSpeed = 0f;
                    rotation = desiredRotation;
                }

                matrixNeedsUpdate = true;
            }
        }

        private void UpdateScale(double seconds)
        {
            if (desiredScaleSpeed != 0)
            {
                float delta = (float)(desiredScaleSpeed * seconds);
                if (Math.Abs(scale - desiredScale) < Math.Abs(delta))
                {
                    desiredScaleSpeed = 0;
                    scale = desiredScale;
                }
                else
                {
                    scale += delta;
                }

                matrixNeedsUpdate = true;
            }
        }
    }
}
