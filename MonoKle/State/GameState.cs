using System;

namespace MonoKle.State
{
    /// <summary>
    /// Abstract class for a game state.
    /// </summary>
    public abstract class GameState : IUpdateable, IDrawable
    {
        private bool _hasBeenActivated;

        /// <summary>
        /// Called when the state is being activated.
        /// </summary>
        /// <param name="data">State data to receive.</param>
        public void Activate(StateSwitchData data)
        {
            if (_hasBeenActivated == false)
            {
                BeforeFirstActivation(data);
                _hasBeenActivated = true;
            }

            Activated(data);
        }

        /// <summary>
        /// Called when the state is being activated.
        /// </summary>
        /// <param name="data">State data to receive.</param>
        protected virtual void Activated(StateSwitchData data)
        {
        }

        /// <summary>
        /// Called when the state is being deactivated.
        /// </summary>
        /// <param name="data">State switch data.</param>
        public virtual void Deactivated(StateSwitchData data) { }

        /// <summary>
        /// Called when the state is being removed.
        /// </summary>
        public virtual void Removed() { }

        public abstract void Draw(TimeSpan timeDelta);

        /// <summary>
        /// Called before the state is activated for the first time.
        /// </summary>
        /// <param name="data">State data to receive.</param>
        protected virtual void BeforeFirstActivation(StateSwitchData data) { }

        public abstract void Update(TimeSpan timeDelta);
    }
}
