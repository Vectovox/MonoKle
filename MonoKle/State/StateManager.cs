namespace MonoKle.State
{
    using MonoKle.Logging;
    using System.Collections.Generic;

    /// <summary>
    /// Class maintaining game states.
    /// </summary>
    public class StateManager
    {
        private GameState currentState;
        private Dictionary<string, GameState> stateByString;
        private StateSwitchData switchData;

        internal StateManager()
        {
            this.stateByString = new Dictionary<string, GameState>();
        }

        /// <summary>
        /// Gets a collection of the identifiers for the existing states.
        /// </summary>
        public ICollection<string> StateIdentifiers
        {
            get { return this.stateByString.Keys; }
        }

        /// <summary>
        /// Adds a state with the specified identifier.
        /// </summary>
        /// <param name="identifier">String identifier.</param>
        /// <param name="state">State to add.</param>
        public void AddState(string identifier, GameState state)
        {
            if (this.stateByString.ContainsKey(identifier) == false)
            {
                this.stateByString.Add(identifier, state);
            }
            else
            {
                Logger.Global.Log("Could not add state. Existing state exists with the identifier: " + identifier, LogLevel.Error);
            }
        }

        /// <summary>
        /// Removes the state with the specified identifier.
        /// </summary>
        /// <param name="identifier">String identifier of the state to remove.</param>
        public void RemoveState(string identifier)
        {
            if (this.stateByString.ContainsKey(identifier))
            {
                this.stateByString[identifier].Remove();
                this.stateByString.Remove(identifier);
            }
            else
            {
                Logger.Global.Log("Could not remove state. There is no state with the identifier: " + identifier, LogLevel.Error);
            }
        }

        /// <summary>
        /// Prepares for a state switch using the provided data.
        /// </summary>
        /// <param name="data">The data to utilize when switching.</param>
        public void SwitchState(StateSwitchData data)
        {
            this.switchData = data;
        }

        internal void Draw(double seconds)
        {
            if (this.currentState != null)
            {
                this.currentState.Draw(seconds);
            }
        }

        internal void Update(double seconds)
        {
            if (this.switchData != null && this.stateByString.ContainsKey(this.switchData.NextState))
            {
                if (this.currentState != null)
                {
                    this.currentState.Deactivate(this.switchData);
                }

                this.currentState = this.stateByString[this.switchData.NextState];
                this.currentState.Activate(this.switchData);
                this.switchData = null;
            }

            if (this.currentState != null)
            {
                this.currentState.Update(seconds);
            }
        }
    }
}