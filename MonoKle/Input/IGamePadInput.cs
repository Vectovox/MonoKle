namespace MonoKle.Input
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Interface providing polling functionality for gamepad input.
    /// </summary>
    public interface IGamePadInput
    {
        /// <summary>
        /// Gets the time the specified button has been held.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        double GetButtonHeldTime(Buttons button, byte playerIndex);

        /// <summary>
        /// Gets the left thumbstick position.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        Vector2 GetLeftThumbstick(byte playerIndex);

        /// <summary>
        /// Gets the left trigger position.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        float GetLeftTrigger(byte playerIndex);

        /// <summary>
        /// Gets the right thumbstick position.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        Vector2 GetRightThumbstick(byte playerIndex);

        /// <summary>
        /// Gets the right trigger position.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        float GetRightTrigger(byte playerIndex);

        /// <summary>
        /// Determines whether the specified button is down.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        bool IsButtonDown(Buttons button, byte playerIndex);

        /// <summary>
        /// Determines whether the specified button is held.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        bool IsButtonHeld(Buttons button, byte playerIndex);

        /// <summary>
        /// Determines whether the specified button has been held for at least the given time.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <param name="timeHeld">The time held.</param>
        /// <returns></returns>
        bool IsButtonHeld(Buttons button, byte playerIndex, double timeHeld);

        /// <summary>
        /// Determines whether the specified button is pressed.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        bool IsButtonPressed(Buttons button, byte playerIndex);

        /// <summary>
        /// Determines whether the specified button is released.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        bool IsButtonReleased(Buttons button, byte playerIndex);

        /// <summary>
        /// Determines whether the specified button is up.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        bool IsButtonUp(Buttons button, byte playerIndex);

        /// <summary>
        /// Determines whether the gamepad with the specified index is connected.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        bool IsConnected(byte playerIndex);
    }
}