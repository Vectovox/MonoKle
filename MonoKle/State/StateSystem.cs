namespace MonoKle.State {
    using System;
    using System.Collections.Generic;
    using MonoKle.Logging;

    /// <summary>
    /// Class maintaining game states.
    /// </summary>
    public class StateSystem : IStateSystem, IUpdateable, IDrawable {
        private GameState currentState;
        private Dictionary<string, GameState> stateByString = new Dictionary<string, GameState>();
        private StateSwitchData switchData;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateSystem"/> class.
        /// </summary>
        public StateSystem() { }

        /// <summary>
        /// Gets a collection of the identifiers for the existing states.
        /// </summary>
        public ICollection<string> StateIdentifiers => stateByString.Keys;

        /// <summary>
        /// Adds a state.
        /// </summary>
        /// <param name="state">State to add.</param>
        public bool AddState(GameState state) {
            if (state == null) {
                throw new ArgumentNullException("State must not be null.");
            }

            if (stateByString.ContainsKey(state.Identifier) == false) {
                stateByString.Add(state.Identifier, state);
                return true;
            }

            Logger.Global.Log("Could not add state. Existing state exists with the identifier: " + state.Identifier, LogLevel.Error);
            return false;
        }

        public void Draw(TimeSpan timeDelta) => currentState?.Draw(timeDelta);

        /// <summary>
        /// Removes the state with the specified identifier.
        /// </summary>
        /// <param name="identifier">String identifier of the state to remove.</param>
        public bool RemoveState(string identifier) {
            if (stateByString.ContainsKey(identifier)) {
                stateByString[identifier].Remove();
                stateByString.Remove(identifier);
                return true;
            }

            Logger.Global.Log("Could not remove state. There is no state with the identifier: " + identifier, LogLevel.Error);
            return false;
        }

        /// <summary>
        /// Prepares for a state switch without data.
        /// </summary>
        /// <param name="stateIdentifier">The identifier of the state to switch to.</param>
        public void SwitchState(string stateIdentifier) => SwitchState(stateIdentifier, null);

        /// <summary>
        /// Prepares for a state switch using the provided data.
        /// </summary>
        /// <param name="stateIdentifier">The identifier of the state to switch to.</param>
        /// <param name="data">Data to send with the switch. May be null.</param>
        public void SwitchState(string stateIdentifier, object data) => switchData = new StateSwitchData(stateIdentifier, currentState == null ? null : currentState.Identifier, data);

        public void Update(TimeSpan timeDelta) {
            // Switch state
            if (switchData != null && stateByString.ContainsKey(switchData.NextState)) {
                if (currentState != null) {
                    currentState.Deactivate(switchData);
                    if (currentState.IsTemporary) {
                        RemoveState(currentState.Identifier);
                    }
                }

                currentState = stateByString[switchData.NextState];
                currentState.Activate(switchData);
                switchData = null;
            }

            // Update current state
            if (currentState != null) {
                currentState.Update(timeDelta);
            }
        }
    }
}
