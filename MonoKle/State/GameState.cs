﻿namespace MonoKle.State
{
    /// <summary>
    /// Abstract class for a game state.
    /// </summary>
    public abstract class GameState
    {
        private bool isActive;

        /// <summary>
        /// Called when the state is being activated.
        /// </summary>
        /// <param name="data">State data to receive.</param>
        public void Activate(StateSwitchData data)
        {
            if(this.isActive == false)
            {
                this.BeforeFirstActivation(data);
                this.isActive = true;
            }

            this.Activated(data);
        }

        /// <summary>
        /// Called when the state is being activated.
        /// </summary>
        /// <param name="data">State data to receive.</param>
        protected virtual void Activated(StateSwitchData data)
        {
        }

        /// <summary>
        /// Call when the state is being activated.
        /// </summary>
        /// <param name="data">State data to receive.</param>
        public void Deactivate(StateSwitchData data)
        {
            this.Deactivated(data);
        }

        /// <summary>
        /// Call when the state is being deactivated.
        /// </summary>
        /// <param name="data">State data which will be sent.</param>
        protected virtual void Deactivated(StateSwitchData data)
        {
        }

        /// <summary>
        /// Called when the state should draw itself.
        /// </summary>
        /// <param name="seconds">Seconds since last called.</param>
        public abstract void Draw(double seconds);

        /// <summary>
        /// Called before the state is activated for the first time.
        /// </summary>
        /// <param name="data">State data to receive.</param>
        protected virtual void BeforeFirstActivation(StateSwitchData data)
        {
        }

        /// <summary>
        /// Call when the state is removed.
        /// </summary>
        public void Remove()
        {
            this.Removed();
        }

        /// <summary>
        /// Called when the state is removed.
        /// </summary>
        protected virtual void Removed()
        {
        }

        /// <summary>
        /// Called when the state should update itself.
        /// </summary>
        /// <param name="seconds">Seconds since last called.</param>
        public abstract void Update(double seconds);
    }
}