using System.Collections.Generic;

namespace MonoKle.State
{
    /// <summary>
    /// Interface for maintaining game states.
    /// </summary>
    public interface IStateSystem
    {
        /// <summary>
        /// Gets a collection of the identifiers for the existing states.
        /// </summary>
        ICollection<string> Identifiers { get; }

        /// <summary>
        /// Gets the current state identifier.
        /// </summary>
        string Current { get; }

        /// <summary>
        /// Adds a state with the given identifier.
        /// </summary>
        /// <param name="identifier">Identifier to use.</param>
        /// <param name="state">State to add.</param>
        /// <param name="transient">If true, state will be removed once switched away from.</param>
        bool AddState(string identifier, GameState state, bool transient = true);

        /// <summary>
        /// Removes the state with the specified identifier.
        /// </summary>
        /// <param name="identifier">String identifier of the state to remove.</param>
        bool RemoveState(string identifier);

        /// <summary>
        /// Prepares for a state switch without data.
        /// </summary>
        /// <param name="stateIdentifier">The identifier of the state to switch to.</param>
        void SwitchState(string stateIdentifier);

        /// <summary>
        /// Prepares for a state switch using the provided data.
        /// </summary>
        /// <param name="stateIdentifier">The identifier of the state to switch to.</param>
        /// <param name="data">Data to send with the switch.</param>
        void SwitchState(string stateIdentifier, object data);

        /// <summary>
        /// Switches to the provided state by adding it. Should it already exist it is replaced.
        /// </summary>
        /// <param name="stateIdentifier">The identifier to use for the state.</param>
        /// <param name="state">The state to add.</param>
        void SwitchState(string stateIdentifier, GameState state);

        /// <summary>
        /// Switches to the provided state by adding it. Should it already exist it is replaced.
        /// </summary>
        /// <param name="stateIdentifier">The identifier to use for the state.</param>
        /// <param name="state">The state to add.</param>
        /// <param name="data">Data to send with the switch.</param>
        void SwitchState(string stateIdentifier, GameState state, object data);
    }
}