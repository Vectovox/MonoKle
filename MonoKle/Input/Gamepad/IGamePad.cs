using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoKle.Input.Gamepad
{
    /// <summary>
    /// Interface for a gamepad.
    /// </summary>
    public interface IGamePad : IActivatableInput
    {
        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        bool IsConnected { get; }

        /// <summary>
        /// Gets the packet number.
        /// </summary>
        /// <value>
        /// The packet number.
        /// </value>
        int PacketNumber { get; }

        /// <summary>
        /// Gets the index of the player.
        /// </summary>
        /// <value>
        /// The index of the player.
        /// </value>
        PlayerIndex PlayerIndex { get; }

        /// <summary>
        /// Gets the A button.
        /// </summary>
        /// <value>
        /// A button.
        /// </value>
        IPressable A { get; }

        /// <summary>
        /// Gets the B button.
        /// </summary>
        /// <value>
        /// B button.
        /// </value>
        IPressable B { get; }

        /// <summary>
        /// Gets the Back button.
        /// </summary>
        /// <value>
        /// Back button.
        /// </value>
        IPressable Back { get; }

        /// <summary>
        /// Gets the Big button.
        /// </summary>
        /// <value>
        /// Big button.
        /// </value>
        IPressable Big { get; }

        /// <summary>
        /// Gets the Left Shoulder button.
        /// </summary>
        /// <value>
        /// Left Shoulder button.
        /// </value>
        IPressable LeftShoulder { get; }

        /// <summary>
        /// Gets the Right Shoulder button.
        /// </summary>
        /// <value>
        /// Right Shoulder button.
        /// </value>
        IPressable RightShoulder { get; }

        /// <summary>
        /// Gets the Start button.
        /// </summary>
        /// <value>
        /// Start button.
        /// </value>
        IPressable Start { get; }

        /// <summary>
        /// Gets the X button.
        /// </summary>
        /// <value>
        /// X button.
        /// </value>
        IPressable X { get; }

        /// <summary>
        /// Gets the Y button.
        /// </summary>
        /// <value>
        /// Y button.
        /// </value>
        IPressable Y { get; }

        /// <summary>
        /// Gets the D-pad.
        /// </summary>
        /// <value>
        /// The D-pad.
        /// </value>
        IDPad DPad { get; }

        /// <summary>
        /// Gets the left trigger.
        /// </summary>
        /// <value>
        /// The left trigger.
        /// </value>
        ITrigger LeftTrigger { get; }

        /// <summary>
        /// Gets the right trigger.
        /// </summary>
        /// <value>
        /// The right trigger.
        /// </value>
        ITrigger RightTrigger { get; }

        /// <summary>
        /// Gets the left thumbstick.
        /// </summary>
        /// <value>
        /// The left thumbstick.
        /// </value>
        IPressableStick LeftThumbstick { get; }

        /// <summary>
        /// Gets the right thumbstick.
        /// </summary>
        /// <value>
        /// The right thumbstick.
        /// </value>
        IPressableStick RightThumbstick { get; }

        /// <summary>
        /// Gets whether the gamepad was disconnected.
        /// </summary>
        bool WasDisconnected { get; }

        /// <summary>
        /// Gets the enum value for all available pressable buttons.
        /// </summary>
        Span<Buttons> GetPressableButtons();

        /// <summary>
        /// Gets the corresponding pressable button.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if the button is not a supported pressable button.</exception>
        /// <param name="button">The button to get.</param>
        IPressable GetPressableButton(Buttons button);
    }
}