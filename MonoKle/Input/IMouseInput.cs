namespace MonoKle.Input
{
    using MonoKle.Core;

    /// <summary>
    /// Interface for mouse input.
    /// </summary>
    public interface IMouseInput
    {
        /// <summary>
        /// Gets or sets whether the virtual mouse is enabled.
        /// </summary>
        bool VirtualMouseEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the time a specified mouse button has been held down; -1 if it is not held.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>The amount of time the specified button has been held down, -1 if it is not held.</returns>
        double GetButtonHeldTime(MouseButton button);

        /// <summary>
        /// Returns the <see cref="Vector2DInteger"/> representation of the mouse movement delta.
        /// </summary>
        /// <returns>Mouse movement delta.</returns>
        Vector2DInteger GetDeltaPosition();

        /// <summary>
        /// Returns the <see cref="Vector2DInteger"/> representation of the current mouse position.
        /// </summary>
        /// <returns>Current mouse position.</returns>
        Vector2DInteger GetPosition();

        /// <summary>
        /// Returns whether a specified mouse button is down.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is down; false otherwise.</returns>
        bool IsButtonDown(MouseButton button);

        /// <summary>
        /// Returns whether a specified mouse button is held down.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is held down; false otherwise.</returns>
        bool IsButtonHeld(MouseButton button);

        /// <summary>
        /// Returns whether a specified mouse button is held down for at least a specified time.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <param name="heldTime">Double value specifying the time to query for.</param>
        /// <returns>true if the button specified by button is held down; false otherwise.</returns>
        bool IsButtonHeld(MouseButton button, double heldTime);

        /// <summary>
        /// Returns whether a specified mouse button is pressed.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is pressed; false otherwise.</returns>
        bool IsButtonPressed(MouseButton button);

        /// <summary>
        /// Returns whether a specified mouse button is released.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is being released; false otherwise.</returns>
        bool IsButtonReleased(MouseButton button);

        /// <summary>
        /// Returns whether a specified mouse button is up.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is up; false otherwise.</returns>
        bool IsButtonUp(MouseButton button);

        /// <summary>
        /// Returns whether the mouse was scrolled in the specified direction.
        /// </summary>
        /// <param name="direction">Enumerated value specifying the direction to query.</param>
        /// <returns>True if the specified direction was scrolled; false otherwise.</returns>
        bool IsMouseScrolled(MouseScrollDirection direction);
    }
}