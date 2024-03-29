﻿using Microsoft.Xna.Framework;

namespace MonoKle.Input.Gamepad
{
    /// <summary>
    /// Interface providing methods to get gamepads.
    /// </summary>
    public interface IGamePadHub : IActivatableInput
    {
        /// <summary>
        /// Gets the player four.
        /// </summary>
        /// <value>
        /// The player four.
        /// </value>
        IGamePad PlayerFour { get; }

        /// <summary>
        /// Gets the player one.
        /// </summary>
        /// <value>
        /// The player one.
        /// </value>
        IGamePad PlayerOne { get; }

        /// <summary>
        /// Gets the player three.
        /// </summary>
        /// <value>
        /// The player three.
        /// </value>
        IGamePad PlayerThree { get; }

        /// <summary>
        /// Gets the player two.
        /// </summary>
        /// <value>
        /// The player two.
        /// </value>
        IGamePad PlayerTwo { get; }

        /// <summary>
        /// Gets the game pad for the provided player.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns>A gamepad.</returns>
        IGamePad GetGamePad(PlayerIndex playerIndex);

        /// <summary>
        /// Gets whether any player was disconnected.
        /// </summary>
        bool AnyDisconnected { get; }
    }
}