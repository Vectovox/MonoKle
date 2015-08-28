namespace MonoKle.State
{
    using MonoKle.Logging;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class maintaining game states.
    /// </summary>
    public class StateManager
    {
        private GameState currentState;
        private Dictionary<string, GameState> stateByString;
        private StateSwitchData switchData;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateManager"/> class.
        /// </summary>
        public StateManager()
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
        /// Adds a state.
        /// </summary>
        /// <param name="state">State to add.</param>
        public bool AddState(GameState state)
        {
            if(state == null)
            {
                throw new ArgumentNullException("State must not be null.");
            }

            if (this.stateByString.ContainsKey(state.Identifier) == false)
            {
                this.stateByString.Add(state.Identifier, state);
                return true;
            }
            
            Logger.Global.Log("Could not add state. Existing state exists with the identifier: " + state.Identifier, LogLevel.Error);
            return false;
        }

        /// <summary>
        /// Removes the state with the specified identifier.
        /// </summary>
        /// <param name="identifier">String identifier of the state to remove.</param>
        public bool RemoveState(string identifier)
        {
            if (this.stateByString.ContainsKey(identifier))
            {
                this.stateByString[identifier].Remove();
                this.stateByString.Remove(identifier);
                return true;
            }

            Logger.Global.Log("Could not remove state. There is no state with the identifier: " + identifier, LogLevel.Error);
            return false;
        }

        /// <summary>
        /// Prepares for a state switch without data.
        /// </summary>
        /// <param name="stateIdentifier">The identifier of the state to switch to.</param>
        public void SwitchState(string stateIdentifier)
        {
            this.SwitchState(stateIdentifier, null);
        }

        /// <summary>
        /// Prepares for a state switch using the provided data.
        /// </summary>
        /// <param name="stateIdentifier">The identifier of the state to switch to.</param>
        /// <param name="data">Data to send with the switch. May be null.</param>
        public void SwitchState(string stateIdentifier, object data)
        {
            this.switchData = new StateSwitchData(stateIdentifier, this.currentState == null ? null : this.currentState.Identifier, data);
        }

        public void Draw(double seconds)
        {
            if (this.currentState != null)
            {
                this.currentState.Draw(seconds);
            }
        }

        public void Update(double seconds)
        {
            // Switch state
            if (this.switchData != null && this.stateByString.ContainsKey(this.switchData.NextState))
            {
                if (this.currentState != null)
                {
                    this.currentState.Deactivate(this.switchData);
                    if(this.currentState.IsTemporary)
                    {
                        this.RemoveState(this.currentState.Identifier);
                    }
                }

                this.currentState = this.stateByString[this.switchData.NextState];
                this.currentState.Activate(this.switchData);
                this.switchData = null;
            }

            // Update current state
            if (this.currentState != null)
            {
                this.currentState.Update(seconds);
            }
        }
    }
}