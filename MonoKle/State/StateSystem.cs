using MonoKle.Logging;
using System;
using System.Collections.Generic;

namespace MonoKle.State
{
    /// <summary>
    /// Class maintaining game states. States are switched out as needed upon calling <see cref="Update(TimeSpan)"/>.
    /// </summary>
    public class StateSystem : IStateSystem, IUpdateable, IDrawable
    {
        private readonly Logger _logger;

        private string _currentState = string.Empty;

        private readonly Dictionary<string, GameState> _stateByString = new Dictionary<string, GameState>();

        private readonly Queue<StateSwitch> _switchQueue = new Queue<StateSwitch>();

        public ICollection<string> StateIdentifiers => _stateByString.Keys;

        public StateSystem(Logger logger)
        {
            _logger = logger;
        }

        public bool AddState(string identifier, GameState state)
        {
            if (state == null || identifier == null)
            {
                throw new ArgumentNullException("State must not be null.");
            }

            if (_stateByString.ContainsKey(identifier))
            {
                _logger.Log($"Could not add state with identifier '{identifier}' as one with the same name already exists.", LogLevel.Error);
                return false;
            }

            _stateByString.Add(identifier, state);
            return true;
        }

        public void Draw(TimeSpan timeDelta) => GetStateWithIdentifier(_currentState).Draw(timeDelta);

        public bool RemoveState(string identifier)
        {
            if (_stateByString.ContainsKey(identifier))
            {
                if (identifier == _currentState)
                {
                    _currentState = string.Empty;
                }
                _stateByString[identifier].Removed();
                _stateByString.Remove(identifier);
                return true;
            }

            _logger.Log($"Could not remove state with identifier '{identifier}' as it does not exist.", LogLevel.Error);
            return false;
        }

        public void SwitchState(string stateIdentifier) =>
            SwitchState(new StateSwitchData(stateIdentifier, _currentState));

        public void SwitchState(string stateIdentifier, object data) =>
            SwitchState(new StateSwitchData(stateIdentifier, _currentState, data));

        private void ReplaceState(string stateIdentifier, GameState state)
        {
            if (_stateByString.ContainsKey(stateIdentifier))
            {
                RemoveState(stateIdentifier);
            }
            AddState(stateIdentifier, state);
        }

        public void SwitchState(string stateIdentifier, GameState state)
        {
            ReplaceState(stateIdentifier, state);
            SwitchState(stateIdentifier);
        }

        public void SwitchState(string stateIdentifier, GameState state, object data)
        {
            ReplaceState(stateIdentifier, state);
            SwitchState(stateIdentifier, data);
        }

        private void SwitchState(StateSwitchData data)
        {
            // Only allow one switch for now so clear previous one
            if (_switchQueue.Count > 0)
            {
                _switchQueue.Clear();
            }

            var previous = GetStateWithIdentifier(data.PreviousState);
            var next = GetStateWithIdentifier(data.NextState);
            _switchQueue.Enqueue(new StateSwitch(previous, next, data));
        }

        public void Update(TimeSpan timeDelta)
        {
            // Update current state
            GetStateWithIdentifier(_currentState).Update(timeDelta);

            // Run activation logic on state updates
            while (_switchQueue.TryDequeue(out var switchData))
            {
                switchData.From.Deactivated(switchData.Data);
                switchData.To.Activate(switchData.Data);
                _currentState = switchData.Data.NextState;

                if (!_stateByString.ContainsKey(_currentState))
                {
                    _logger.Log($"Switched to state '{_currentState}' but it does not exist.", LogLevel.Warning);
                }
            }
        }

        private GameState GetStateWithIdentifier(string identifier) =>
            _stateByString.ContainsKey(identifier)
                ? _stateByString[identifier]
                : Void;

        private static GameState Void { get; } = new VoidState();

        /// <summary>
        /// NOOP game state.
        /// </summary>
        private class VoidState : GameState
        {
            public override void Draw(TimeSpan timeDelta) { }

            public override void Update(TimeSpan timeDelta) { }
        }

        /// <summary>
        /// State switching information.
        /// </summary>
        private class StateSwitch
        {
            public StateSwitch(GameState from, GameState to, StateSwitchData data)
            {
                From = from;
                To = to;
                Data = data;
            }

            public GameState From { get; }
            public GameState To { get; }
            public StateSwitchData Data { get; }
        }
    }
}
