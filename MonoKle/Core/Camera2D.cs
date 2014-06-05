namespace MonoKle.Core
{
    using System;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// Serializable class representing a 2D environment camera providing transformation capabilities.
    /// </summary>
    [Serializable()]
    public class Camera2D
    {
        // TODO: Add desired position and a method that travels to a given position from the current one. private Vector2 desiredPosition;
        private float desiredRotation;
        private float desiredRotationSpeed = 0;
        private float desiredScale;
        private float desiredScaleSpeed = 0;
        private bool matrixNeedsUpdate = true;
        private Vector2 position;
        private float rotation;
        private float scale = 1f;
        private Vector2DInteger size;
        private Matrix transformMatrix;
        private Matrix transformMatrixInv;

        /// <summary>
        /// Initiates a new instance of <see cref="Camera2D"/>.
        /// </summary>
        /// <param name="size">The <see cref="Vector2DInteger"/> represenetation of the camera size.</param>
        public Camera2D(Vector2DInteger size)
        {
            this.size = size;
        }

        /// <summary>
        /// Gets the size of the camera. 
        /// </summary>
        public Vector2DInteger Size
        {
            get { return size; }
            set { size = value; matrixNeedsUpdate = true; }
        }

        /// <summary>
        /// Returns the current camera center position.
        /// </summary>
        /// <returns>Vector2 representation of the position.</returns>
        public Vector2 GetPosition()
        {
            return this.position;
        }

        /// <summary>
        /// Returns the current rotation.
        /// </summary>
        /// <returns>Float representation of rotation.</returns>
        public float GetRotation()
        {
            return this.rotation;
        }

        /// <summary>
        /// Returns the current scale factor.
        /// </summary>
        /// <returns>Float representation of the scale factor.</returns>
        public float GetScale()
        {
            return this.scale;
        }

        /// <summary>
        /// Gets the transformation matrix representation.
        /// </summary>
        /// <returns>Transformation matrix.</returns>
        public Matrix GetTransformMatrix()
        {
            return this.transformMatrix;
        }

        /// <summary>
        /// Gets the inverse transformation matrix representation.
        /// </summary>
        /// <returns>Transformation matrix.</returns>
        public Matrix GetTransformMatrixInv()
        {
            return this.transformMatrixInv;
        }

        /// <summary>
        /// Sets the current camera center position to the given coordinate.
        /// </summary>
        /// <param name="position">The Vector2 coordinate to set to.</param>
        public void SetPosition(Vector2 position)
        {
            this.position = position;
            this.matrixNeedsUpdate = true;
        }

        /// <summary>
        /// Sets the current rotation to the given value.
        /// </summary>
        /// <param name="rotation">The value to set rotation to.</param>
        public void SetRotation(float rotation)
        {
            this.rotation = MathHelper.WrapAngle(rotation);
            this.desiredRotationSpeed = 0;
            this.matrixNeedsUpdate = true;
        }

        /// <summary>
        /// Sets the current rotation, in radians, to the given value over a period of time determined by the given speed.
        /// </summary>
        /// <param name="rotation">The rotation, in radians, to set to.</param>
        /// <param name="speed">The delta rotation, in radians, per second.</param>
        public void SetRotation(float rotation, float speed)
        {
            this.desiredRotation = MathHelper.WrapAngle(rotation);
            if(this.desiredRotation != this.rotation)
            {
                float a = desiredRotation - this.rotation;
                if(a > Math.PI) a -= 2 * (float)Math.PI;
                if(a < -Math.PI) a += 2 * (float)Math.PI;

                this.desiredRotationSpeed = a < 0 ? -speed : speed;
            }
        }

        /// <summary>
        /// Sets the current scale factor to the given value.
        /// </summary>
        /// <param name="scale">The scale factor to set to.</param>
        public void SetScale(float scale)
        {
            this.scale = scale;
            this.desiredScaleSpeed = 0;
            this.matrixNeedsUpdate = true;
        }

        /// <summary>
        /// Sets the current scale factor to the given value over a period of time determined by the given speed.
        /// </summary>
        /// <param name="scale">The scale factor to set to.</param>
        /// <param name="speed">The delta scale per second.</param>
        public void SetScale(float scale, float speed)
        {
            this.desiredScale = scale;
            this.desiredScaleSpeed = (scale - this.scale) < 0 ? -speed : speed;
        }

        /// <summary>
        /// Transforms a given coordinate from world space to camera space.
        /// </summary>
        /// <param name="coordinate">The coordinate to transform.</param>
        /// <returns>Transformed coordinate.</returns>
        public Vector2 Transform(Vector2 coordinate)
        {
            return Vector2.Transform(coordinate, this.transformMatrix);
        }

        /// <summary>
        /// Inversely transforms a given coordinate, effectively untransforming camera space to world space.
        /// </summary>
        /// <param name="coordinate">The coordinate to transform.</param>
        /// <returns>Transformed coordinate.</returns>
        public Vector2 TransformInv(Vector2 coordinate)
        {
            return Vector2.Transform(coordinate, transformMatrixInv);
        }

        /// <summary>
        /// Updates camera composition with the given amount of delta time.
        /// </summary>
        /// <param name="span">Delta time.</param>
        public void Update(TimeSpan span)
        {
            this.Update(span.TotalSeconds);
        }

        /// <summary>
        /// Updates camera composition with the given amount of delta time.
        /// </summary>
        /// <param name="seconds">Delta time in seconds.</param>
        public void Update(double seconds)
        {
            this.UpdateScale(ref seconds);
            this.UpdateRotation(ref seconds);

            if(this.matrixNeedsUpdate)
            {
                Vector2 center = size.ToVector2() * 0.5f;
                this.transformMatrix = Matrix.CreateTranslation(-new Vector3(position - center, 0f))
                * Matrix.CreateTranslation(-new Vector3(center, 0f))
                * Matrix.CreateRotationZ(-rotation)
                * Matrix.CreateScale(scale)
                * Matrix.CreateTranslation(new Vector3(center, 0f));

                this.transformMatrixInv = Matrix.Invert(this.transformMatrix);
            }
        }

        private void UpdateRotation(ref double seconds)
        {
            if(desiredRotationSpeed != 0)
            {
                float delta = (float)(this.desiredRotationSpeed * seconds);
                this.rotation = MathHelper.WrapAngle(this.rotation + delta);
                double distance = Math.Atan2(Math.Sin(this.desiredRotation - this.rotation), Math.Cos(this.desiredRotation - this.rotation));

                // Check if we turned past the desired rotation
                if(Math.Abs(distance) < Math.Abs(delta))
                {
                    this.desiredRotationSpeed = 0f;
                    this.rotation = desiredRotation;
                }

                matrixNeedsUpdate = true;
            }
        }

        private void UpdateScale(ref double seconds)
        {
            if(this.desiredScaleSpeed != 0)
            {
                float delta = (float)(this.desiredScaleSpeed * seconds);
                if(Math.Abs(this.scale - this.desiredScale) < Math.Abs(delta))
                {
                    this.desiredScaleSpeed = 0;
                    this.scale = this.desiredScale;
                }
                else
                {
                    this.scale += delta;
                }

                this.matrixNeedsUpdate = true;
            }
        }
    }
}