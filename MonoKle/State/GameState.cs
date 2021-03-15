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
        /// Gets the identifier of the state.
        /// </summary>
        /// <value>
        /// The identifier of the state.
        /// </value>
        public string Identifier { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is temporary, meaning it should be removed after it is switched away from.
        /// </summary>
        /// <value>
        /// True if this instance is temporary; otherwise false.
        /// </value>
        public bool IsTemporary { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameState"/> class.
        /// </summary>
        /// <param name="identifier">The identifier of the state.</param>
        public GameState(string identifier) => Identifier = identifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameState"/> class.
        /// </summary>
        /// <param name="identifier">The identifier of the state.</param>
        /// <param name="isTemporary">If set to <c>true</c>, the state [is temporary].</param>
        public GameState(string identifier, bool isTemporary) : this(identifier) => IsTemporary = isTemporary;

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
        /// Call when the state is being activated.
        /// </summary>
        /// <param name="data">State data to receive.</param>
        public void Deactivate(StateSwitchData data) => Deactivated(data);

        /// <summary>
        /// Call when the state is being deactivated.
        /// </summary>
        /// <param name="data">State data which will be sent.</param>
        protected virtual void Deactivated(StateSwitchData data) { }

        public abstract void Draw(TimeSpan timeDelta);

        /// <summary>
        /// Called before the state is activated for the first time.
        /// </summary>
        /// <param name="data">State data to receive.</param>
        protected virtual void BeforeFirstActivation(StateSwitchData data) { }

        /// <summary>
        /// Call when the state is removed.
        /// </summary>
        public void Remove() => Removed();

        /// <summary>
        /// Called when the state is removed.
        /// </summary>
        protected virtual void Removed() { }

        public abstract void Update(TimeSpan timeDelta);
    }
}
