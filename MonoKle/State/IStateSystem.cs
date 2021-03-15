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
        ICollection<string> StateIdentifiers { get; }

        /// <summary>
        /// Adds a state with the given identifier.
        /// </summary>
        /// <param name="identifier">Identifier to use.</param>
        /// <param name="state">State to add.</param>
        bool AddState(string identifier, GameState state);

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
        /// <param name="data">Data to send with the switch. May be null.</param>
        void SwitchState(string stateIdentifier, object data);
    }
}