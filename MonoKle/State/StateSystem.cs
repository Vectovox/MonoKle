using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MonoKle.State
{
    /// <summary>
    /// Class maintaining game states. States are switched out as needed upon calling <see cref="Update(TimeSpan)"/>.
    /// </summary>
    public class StateSystem : IStateSystem, IUpdateable, IDrawable
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, GameState> _stateByString = new();
        private readonly Queue<StateSwitch> _switchQueue = new();

        public StateSystem(ILogger<StateSystem> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public ICollection<string> Identifiers => _stateByString.Keys;

        public string Current { get; private set; } = string.Empty;

        public bool AddState(string identifier, GameState state, bool transient = true)
        {
            if (identifier is null)
            {
                throw new ArgumentNullException(nameof(identifier));
            }
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            state.ServiceProvider = _serviceProvider;
            state.IsTransient = transient;

            if (_stateByString.ContainsKey(identifier))
            {
                _logger.LogError("Could not add state with identifier '{STATE}' as one with the same name already exists.", identifier);
                return false;
            }

            _stateByString.Add(identifier, state);
            return true;
        }

        public void Draw(TimeSpan timeDelta) => GetStateWithIdentifier(Current).Draw(timeDelta);

        public bool RemoveState(string identifier)
        {
            if (_stateByString.ContainsKey(identifier))
            {
                if (identifier == Current)
                {
                    Current = string.Empty;
                }
                _stateByString[identifier].Removed();
                _stateByString.Remove(identifier);
                return true;
            }

            _logger.LogError("Could not remove state with identifier '{STATE}' as it does not exist.", identifier);
            return false;
        }

        public void SwitchState(string stateIdentifier) =>
            SwitchState(new StateSwitchData(stateIdentifier, Current));

        public void SwitchState(string stateIdentifier, object data) =>
            SwitchState(new StateSwitchData(stateIdentifier, Current, data));

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
            GetStateWithIdentifier(Current).Update(timeDelta);

            // Run activation logic on state updates
            while (_switchQueue.TryDequeue(out var switchData))
            {
                switchData.From.Deactivated(switchData.Data);
                if (switchData.From.IsTransient && _stateByString.ContainsKey(switchData.Data.PreviousState))
                {
                    RemoveState(switchData.Data.PreviousState);
                }

                Current = switchData.Data.NextState;
                switchData.To.Activate(switchData.Data);

                if (!_stateByString.ContainsKey(Current))
                {
                    _logger.LogWarning("Switched to state '{STATE}' but it does not exist.", Current);
                }
            }
        }

        private GameState GetStateWithIdentifier(string identifier) =>
            _stateByString.TryGetValue(identifier, out var state)
                ? state
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
