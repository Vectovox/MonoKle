namespace MonoKle.Input
{
    using Microsoft.Xna.Framework.Input;
    using System.Collections.Generic;

    /// <summary>
    /// Class providing polling functionality for keyboard input.
    /// </summary>
    public class MouseInput : IMouseInput, IUpdateable
    {
        private MouseScrollDirection currentScrollDirection;
        private MPoint2 deltaPosition;
        private Dictionary<MouseButton, double> heldTimeByButton;
        private MPoint2 mousePosition;
        private HashSet<MouseButton> previousButtons;
        private int previousScrollValue;

        /// <summary>
        /// Creates a new instance of <see cref="MouseInput"/>.
        /// </summary>
        public MouseInput()
        {
            this.heldTimeByButton = new Dictionary<MouseButton, double>();
            this.previousButtons = new HashSet<MouseButton>();
            this.currentScrollDirection = MouseScrollDirection.None;
            this.previousScrollValue = 0;
            this.mousePosition = new MPoint2(0, 0);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MouseInput"/> with the given screen size.
        /// </summary>
        /// <param name="screenSize"></param>
        public MouseInput(MPoint2 screenSize) : this()
        {
            this.ScreenSize = screenSize;
        }

        /// <summary>
        /// Gets or sets the size of the screen used for the virtual mouse.
        /// </summary>
        /// <value>
        /// The size of the screen.
        /// </value>
        public MPoint2 ScreenSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the virtual mouse is enabled.
        /// </summary>
        public bool VirtualMouseEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the time a specified mouse button has been held down; -1 if it is not held.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>The amount of time the specified button has been held down, -1 if it is not held.</returns>
        public double GetButtonHeldTime(MouseButton button)
        {
            if (this.heldTimeByButton.ContainsKey(button))
            {
                return this.heldTimeByButton[button];
            }
            return -1;
        }

        /// <summary>
        /// Returns the <see cref="MPoint2"/> representation of the mouse movement delta since the last update call.
        /// </summary>
        /// <returns>Mouse movement delta.</returns>
        public MPoint2 GetDeltaPosition()
        {
            return this.deltaPosition;
        }

        /// <summary>
        /// Returns the <see cref="MPoint2"/> representation of the current mouse position.
        /// </summary>
        /// <returns>Current mouse position.</returns>
        public MPoint2 GetPosition()
        {
            return this.mousePosition;
        }

        /// <summary>
        /// Returns whether a specified mouse button is down.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is down; false otherwise.</returns>
        public bool IsButtonDown(MouseButton button)
        {
            return this.heldTimeByButton.ContainsKey(button);
        }

        /// <summary>
        /// Returns whether a specified mouse button is held down.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is held down; false otherwise.</returns>
        public bool IsButtonHeld(MouseButton button)
        {
            return this.IsButtonDown(button) && this.previousButtons.Contains(button);
        }

        /// <summary>
        /// Returns whether a specified mouse button is held down for at least a specified time.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <param name="heldTime">Double value specifying the time to query for.</param>
        /// <returns>true if the button specified by button is held down; false otherwise.</returns>
        public bool IsButtonHeld(MouseButton button, double heldTime)
        {
            return this.IsButtonHeld(button) && this.GetButtonHeldTime(button) >= heldTime;
        }

        /// <summary>
        /// Returns whether a specified mouse button is pressed.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is pressed; false otherwise.</returns>
        public bool IsButtonPressed(MouseButton button)
        {
            return this.previousButtons.Contains(button) == false && this.IsButtonDown(button);
        }

        /// <summary>
        /// Returns whether a specified mouse button is released.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is being released; false otherwise.</returns>
        public bool IsButtonReleased(MouseButton button)
        {
            return this.previousButtons.Contains(button) && this.IsButtonUp(button);
        }

        /// <summary>
        /// Returns whether a specified mouse button is up.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is up; false otherwise.</returns>
        public bool IsButtonUp(MouseButton button)
        {
            return this.heldTimeByButton.ContainsKey(button) == false;
        }

        /// <summary>
        /// Returns whether the mouse was scrolled in the specified direction.
        /// </summary>
        /// <param name="direction">Enumerated value specifying the direction to query.</param>
        /// <returns>True if the specified direction was scrolled; false otherwise.</returns>
        public bool IsMouseScrolled(MouseScrollDirection direction)
        {
            return direction == this.currentScrollDirection;
        }

        /// <summary>
        /// Updates the component with the specified seconds since last update.
        /// </summary>
        /// <param name="seconds">The amount of seconds since last update.</param>
        public void Update(double seconds)
        {
            MouseState currentState = Mouse.GetState();

            // Buttons
            this.previousButtons.Clear();
            this.previousButtons.UnionWith(heldTimeByButton.Keys);
            this.UpdateButton(MouseButton.Left, currentState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed, seconds);
            this.UpdateButton(MouseButton.Middle, currentState.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed, seconds);
            this.UpdateButton(MouseButton.Right, currentState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed, seconds);
            this.UpdateButton(MouseButton.XButton1, currentState.XButton1 == Microsoft.Xna.Framework.Input.ButtonState.Pressed, seconds);
            this.UpdateButton(MouseButton.XButton2, currentState.XButton2 == Microsoft.Xna.Framework.Input.ButtonState.Pressed, seconds);

            // Scroll wheel
            if (currentState.ScrollWheelValue > previousScrollValue)
            {
                this.currentScrollDirection = MouseScrollDirection.Up;
            }
            else if (currentState.ScrollWheelValue < previousScrollValue)
            {
                this.currentScrollDirection = MouseScrollDirection.Down;
            }
            else
            {
                this.currentScrollDirection = MouseScrollDirection.None;
            }
            this.previousScrollValue = currentState.ScrollWheelValue;

            // Mouse position
            this.deltaPosition = this.mousePosition;
            if (this.VirtualMouseEnabled)
            {
                this.mousePosition += new MPoint2(currentState.X, currentState.Y);
                Mouse.SetPosition(this.ScreenSize.X / 2, ScreenSize.Y / 2);
                this.mousePosition = new MRectangleInt(ScreenSize.X, ScreenSize.Y).Clamp(this.mousePosition);
            }
            else
            {
                this.mousePosition = new MPoint2(currentState.X, currentState.Y);
            }
            this.deltaPosition = this.mousePosition - this.deltaPosition;
        }

        private void UpdateButton(MouseButton button, bool pressed, double time)
        {
            if (pressed)
            {
                if (this.heldTimeByButton.ContainsKey(button))
                {
                    this.heldTimeByButton[button] += time;
                }
                else
                {
                    this.heldTimeByButton.Add(button, 0);
                }
            }
            else
            {
                this.heldTimeByButton.Remove(button);
            }
        }
    }
}