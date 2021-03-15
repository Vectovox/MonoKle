using MonoKle.Logging;
using System;
using System.Collections.Generic;

namespace MonoKle.State
{
    /// <summary>
    /// Class maintaining game states.
    /// </summary>
    public class StateSystem : IStateSystem, IUpdateable, IDrawable
    {
        private GameState _currentState;
        private Dictionary<string, GameState> _stateByString = new Dictionary<string, GameState>();
        private StateSwitchData _switchData;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateSystem"/> class.
        /// </summary>
        public StateSystem()
        {

        }

        /// <summary>
        /// Gets a collection of the identifiers for the existing states.
        /// </summary>
        public ICollection<string> StateIdentifiers => _stateByString.Keys;

        /// <summary>
        /// Adds a state.
        /// </summary>
        /// <param name="state">State to add.</param>
        public bool AddState(GameState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("State must not be null.");
            }

            if (_stateByString.ContainsKey(state.Identifier) == false)
            {
                _stateByString.Add(state.Identifier, state);
                return true;
            }

            Logger.Global.Log("Could not add state. Existing state exists with the identifier: " + state.Identifier, LogLevel.Error);
            return false;
        }

        public void Draw(TimeSpan timeDelta) => _currentState?.Draw(timeDelta);

        /// <summary>
        /// Removes the state with the specified identifier.
        /// </summary>
        /// <param name="identifier">String identifier of the state to remove.</param>
        public bool RemoveState(string identifier)
        {
            if (_stateByString.ContainsKey(identifier))
            {
                _stateByString[identifier].Remove();
                _stateByString.Remove(identifier);
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
        public void SwitchState(string stateIdentifier, object data) => _switchData = new StateSwitchData(stateIdentifier, _currentState == null ? null : _currentState.Identifier, data);

        public void Update(TimeSpan timeDelta)
        {
            // Switch state
            if (_switchData != null && _stateByString.ContainsKey(_switchData.NextState))
            {
                if (_currentState != null)
                {
                    _currentState.Deactivate(_switchData);
                    if (_currentState.IsTemporary)
                    {
                        RemoveState(_currentState.Identifier);
                    }
                }

                _currentState = _stateByString[_switchData.NextState];
                _currentState.Activate(_switchData);
                _switchData = null;
            }

            // Update current state
            if (_currentState != null)
            {
                _currentState.Update(timeDelta);
            }
        }
    }
}
