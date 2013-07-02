namespace MonoKle.State
{
    /// <summary>
    /// Abstract class for a game state.
    /// </summary>
    public abstract class GameState
    {
        /// <summary>
        /// Called when the state is being activated.
        /// </summary>
        /// <param name="data">State data to receive.</param>
        public virtual void Activated(StateSwitchData data)
        {
        }

        /// <summary>
        /// Called when the state is being deactivated.
        /// </summary>
        /// <param name="data">State data which will be sent.</param>
        public virtual void Deactivated(StateSwitchData data)
        {
        }

        /// <summary>
        /// Called when the state should draw itself.
        /// </summary>
        /// <param name="seconds">Seconds since last called.</param>
        public abstract void Draw(double seconds);

        /// <summary>
        /// Called when the state is removed.
        /// </summary>
        public virtual void Removed()
        {
        }

        /// <summary>
        /// Called when the state should update itself.
        /// </summary>
        /// <param name="seconds">Seconds since last called.</param>
        public abstract void Update(double seconds);
    }
}