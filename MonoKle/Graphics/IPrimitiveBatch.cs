﻿namespace MonoKle.Graphics
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Interface for classes drawing primitives.
    /// </summary>
    public interface IPrimitiveBatch
    {
        /// <summary>
        /// Begins a batch of primitives.
        /// </summary>
        void Begin();

        /// <summary>
        /// Begins a batch of primitives, using a transformation matrix to apply to each primitive.
        /// </summary>
        /// <param name="transformMatrix">Transformation matrix to apply.</param>
        void Begin(Matrix transformMatrix);

        /// <summary>
        /// Ends a batch of primitives.
        /// </summary>
        void End();
    }
}