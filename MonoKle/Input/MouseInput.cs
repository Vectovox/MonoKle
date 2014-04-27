namespace MonoKle.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Xna.Framework.Input;

    using MonoKle.Core;
    using MonoKle.Graphics;

    /// <summary>
    /// Mouse input class.
    /// </summary>
    public class MouseInput
    {
        private MouseScrollDirection currentScrollDirection;
        private Dictionary<MouseButton, double> heldTimeByButton;
        private Vector2Int32 mousePosition;
        private HashSet<MouseButton> previousButtons;
        private int previousScrollValue;

        internal MouseInput()
        {
            heldTimeByButton = new Dictionary<MouseButton, double>();
            previousButtons = new HashSet<MouseButton>();
            currentScrollDirection = MouseScrollDirection.None;
            previousScrollValue = 0;
            mousePosition = new Vector2Int32(0, 0);
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
            if (heldTimeByButton.ContainsKey(button))
            {
                return heldTimeByButton[button];
            }
            return -1;
        }

        /// <summary>
        /// Returns whether a specified mouse button is down.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is down; false otherwise.</returns>
        public bool IsButtonDown(MouseButton button)
        {
            return heldTimeByButton.ContainsKey(button);
        }

        /// <summary>
        /// Returns whether a specified mouse button is held down.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is held down; false otherwise.</returns>
        public bool IsButtonHeld(MouseButton button)
        {
            return IsButtonDown(button) && previousButtons.Contains(button);
        }

        /// <summary>
        /// Returns whether a specified mouse button is held down for at least a specified time.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <param name="heldTime">Double value specifying the time to query for.</param>
        /// <returns>true if the button specified by button is held down; false otherwise.</returns>
        public bool IsButtonHeld(MouseButton button, double heldTime)
        {
            return IsButtonHeld(button) && GetButtonHeldTime(button) >= heldTime;
        }

        /// <summary>
        /// Returns whether a specified mouse button is pressed.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is pressed; false otherwise.</returns>
        public bool IsButtonPressed(MouseButton button)
        {
            return previousButtons.Contains(button) == false && IsButtonDown(button);
        }

        /// <summary>
        /// Returns whether a specified mouse button is released.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is being released; false otherwise.</returns>
        public bool IsButtonReleased(MouseButton button)
        {
            return previousButtons.Contains(button) && IsButtonUp(button);
        }

        /// <summary>
        /// Returns whether a specified mouse button is up.
        /// </summary>
        /// <param name="button">Enumerated value specifying the button to query.</param>
        /// <returns>true if the button specified by button is up; false otherwise.</returns>
        public bool IsButtonUp(MouseButton button)
        {
            return heldTimeByButton.ContainsKey(button) == false;
        }

        /// <summary>
        /// Returns the <see cref="Vector2Int32"/> representation of the current mouse position.
        /// </summary>
        /// <returns>Current mouse position.</returns>
        public Vector2Int32 GetMousePosition()
        {
            return mousePosition;
        }

        /// <summary>
        /// Returns whether the mouse was scrolled in the specified direction.
        /// </summary>
        /// <param name="direction">Enumerated value specifying the direction to query.</param>
        /// <returns>True if the specified direction was scrolled; false otherwise.</returns>
        public bool IsMouseScrolled(MouseScrollDirection direction)
        {
            return direction == currentScrollDirection;
        }

        internal void Update(double seconds, Vector2Int32 screenSize)
        {
            MouseState currentState = Mouse.GetState();

            // Buttons
            previousButtons.Clear();
            previousButtons.UnionWith(heldTimeByButton.Keys);
            UpdateButton(MouseButton.Left, currentState.LeftButton == ButtonState.Pressed, seconds);
            UpdateButton(MouseButton.Middle, currentState.MiddleButton == ButtonState.Pressed, seconds);
            UpdateButton(MouseButton.Right, currentState.RightButton == ButtonState.Pressed, seconds);
            UpdateButton(MouseButton.XButton1, currentState.XButton1 == ButtonState.Pressed, seconds);
            UpdateButton(MouseButton.XButton2, currentState.XButton2 == ButtonState.Pressed, seconds);

            // Scroll wheel
            if (currentState.ScrollWheelValue > previousScrollValue)
            {
                currentScrollDirection = MouseScrollDirection.Up;
            }
            else if (currentState.ScrollWheelValue < previousScrollValue)
            {
                currentScrollDirection = MouseScrollDirection.Down;
            }
            else
            {
                currentScrollDirection = MouseScrollDirection.None;
            }
            previousScrollValue = currentState.ScrollWheelValue;

            // Mouse position
            if (VirtualMouseEnabled)
            {
                mousePosition += new Vector2Int32(currentState.X, currentState.Y);
                Mouse.SetPosition(screenSize.X / 2, screenSize.Y / 2);
                ClampMousePosition(screenSize);
            }
            else
            {
                mousePosition = new Vector2Int32(currentState.X, currentState.Y);
            }
        }

        private void ClampMousePosition(Vector2Int32 screenSize)
        {
            this.mousePosition = this.mousePosition.Crop(screenSize);
        }

        private void UpdateButton(MouseButton button, bool pressed, double time)
        {
            if (pressed)
            {
                if (heldTimeByButton.ContainsKey(button))
                {
                    heldTimeByButton[button] += time;
                }
                else
                {
                    heldTimeByButton.Add(button, 0);
                }
            }
            else
            {
                heldTimeByButton.Remove(button);
            }
        }
    }
}