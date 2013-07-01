namespace MonoKle.State
{
    using System.Collections.Generic;

    public class StateManager
    {
        private GameState currentState;
        private Dictionary<string, GameState> stateByString;

        internal StateManager()
        {
            stateByString = new Dictionary<string, GameState>();
            currentState = null;
        }

        /// <summary>
        /// Gets or sets the state identifier for the next switched to state.
        /// If set to null, no state will be switched to.
        /// </summary>
        public string NextState
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a collection of the identifiers for the existing states.
        /// </summary>
        public ICollection<string> StateIdentifiers
        {
            get { return stateByString.Keys; }
        }

        /// <summary>
        /// Adds a state with the specified identifier.
        /// </summary>
        /// <param name="identifier">String identifier.</param>
        /// <param name="state">State to add.</param>
        public void AddState(string identifier, GameState state)
        {
            if (stateByString.ContainsKey(identifier) == false)
            {
                stateByString.Add(identifier, state);
            }
            else
            {
                // TODO: Log this
            }
        }

        /// <summary>
        /// Removes the state with the specified identifier.
        /// </summary>
        /// <param name="identifier">String identifier of the state to remove.</param>
        public void RemoveState(string identifier)
        {
            if (stateByString.ContainsKey(identifier))
            {
                stateByString[identifier].Removed();
                stateByString.Remove(identifier);
            }
            else
            {
                // TODO: Log this
            }
        }

        internal void Draw(double seconds)
        {
            if (currentState != null)
            {
                currentState.Draw(seconds);
            }
        }

        internal void Update(double seconds)
        {
            if (NextState != null && stateByString.ContainsKey(NextState))
            {
                if (currentState != null)
                {
                    currentState.Deactivated();
                }
                currentState = stateByString[NextState];
                NextState = null;
                currentState.Activated();
            }

            if (currentState != null)
            {
                currentState.Update(seconds);
            }
        }
    }
}