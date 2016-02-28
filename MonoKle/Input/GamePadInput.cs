namespace MonoKle.Input
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class providing polling functionality for keyboard input.
    /// </summary>
    public class GamePadInput : IGamePadInput, MonoKle.IUpdateable
    {
        private HashSet<Buttons>[] currentButtons;

        //private static GamePadState[] previousStateArray;
        private GamePadState[] currentState;

        private HashSet<Buttons>[] previousButtons;
        private Dictionary<Buttons, double>[] timeHeldByButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePadInput"/> class.
        /// </summary>
        public GamePadInput()
        {
            //previousStateArray = new GamePadState[4];
            currentState = new GamePadState[4];
            currentButtons = new HashSet<Buttons>[4];
            previousButtons = new HashSet<Buttons>[4];
            timeHeldByButton = new Dictionary<Buttons, double>[4];
            for (int i = 0; i < 4; i++)
            {
                currentState[i] = new GamePadState();
                currentButtons[i] = new HashSet<Buttons>();
                previousButtons[i] = new HashSet<Buttons>();
                timeHeldByButton[i] = new Dictionary<Buttons, double>();
            }
        }

        /// <summary>
        /// Gets the time the specified button has been held.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Player index must be in the interval 0-3.</exception>
        public double GetButtonHeldTime(Buttons button, byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            double ret = 0;
            timeHeldByButton[playerIndex].TryGetValue(button, out ret);
            return ret;
        }

        /// <summary>
        /// Gets the left thumbstick position.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Player index must be in the interval 0-3.</exception>
        public Vector2 GetLeftThumbstick(byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentState[playerIndex].ThumbSticks.Left;
        }

        /// <summary>
        /// Gets the left trigger position.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Player index must be in the interval 0-3.</exception>
        public float GetLeftTrigger(byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentState[playerIndex].Triggers.Left;
        }

        /// <summary>
        /// Gets the right thumbstick position.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Player index must be in the interval 0-3.</exception>
        public Vector2 GetRightThumbstick(byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentState[playerIndex].ThumbSticks.Right;
        }

        /// <summary>
        /// Gets the right trigger position.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Player index must be in the interval 0-3.</exception>
        public float GetRightTrigger(byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentState[playerIndex].Triggers.Right;
        }

        /// <summary>
        /// Determines whether the specified button is down.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Player index must be in the interval 0-3.</exception>
        public bool IsButtonDown(Buttons button, byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentButtons[playerIndex].Contains(button);
        }

        /// <summary>
        /// Determines whether the specified button is held.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Player index must be in the interval 0-3.</exception>
        public bool IsButtonHeld(Buttons button, byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return IsButtonDown(button, playerIndex) && previousButtons[playerIndex].Contains(button);
        }

        /// <summary>
        /// Determines whether the specified button has been held for at least the given time.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <param name="timeHeld">The time held.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Player index must be in the interval 0-3.</exception>
        public bool IsButtonHeld(Buttons button, byte playerIndex, double timeHeld)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return IsButtonHeld(button, playerIndex) && GetButtonHeldTime(button, playerIndex) >= timeHeld;
        }

        /// <summary>
        /// Determines whether the specified button is pressed.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Player index must be in the interval 0-3.</exception>
        public bool IsButtonPressed(Buttons button, byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return IsButtonDown(button, playerIndex) && previousButtons[playerIndex].Contains(button) == false;
        }

        /// <summary>
        /// Determines whether the specified button is released.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Player index must be in the interval 0-3.</exception>
        public bool IsButtonReleased(Buttons button, byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return IsButtonUp(button, playerIndex) && previousButtons[playerIndex].Contains(button);
        }

        /// <summary>
        /// Determines whether the specified button is up.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Player index must be in the interval 0-3.</exception>
        public bool IsButtonUp(Buttons button, byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentButtons[playerIndex].Contains(button) == false;
        }

        /// <summary>
        /// Determines whether the gamepad with the specified index is connected.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Player index must be in the interval 0-3.</exception>
        public bool IsConnected(byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentState[playerIndex].IsConnected;
        }

        /// <summary>
        /// Updates the component with the specified seconds since last update.
        /// </summary>
        /// <param name="seconds">The amount of seconds since last update.</param>
        public void Update(double seconds)
        {
            for (int i = 0; i < 4; i++)
            {
                //previousStateArray[i] = currentStateArray[i];
                currentState[i] = GamePad.GetState((PlayerIndex)i);

                previousButtons[i] = currentButtons[i];
                currentButtons[i] = GetButtonsDown(currentState[i]);

                // Remove and update old held buttons
                ICollection<Buttons> c = new LinkedList<Buttons>(timeHeldByButton[i].Keys);
                foreach (Buttons b in c)
                {
                    if (currentButtons[i].Contains(b))
                    {
                        timeHeldByButton[i][b] += seconds;
                    }
                    else
                    {
                        timeHeldByButton[i].Remove(b);
                    }
                }

                // Add new held buttons
                foreach (Buttons b in currentButtons[i])
                {
                    if (timeHeldByButton[i].ContainsKey(b) == false)
                    {
                        timeHeldByButton[i].Add(b, 0);
                    }
                }
            }
        }

        private HashSet<Buttons> GetButtonsDown(GamePadState state)
        {
            HashSet<Buttons> ret = new HashSet<Buttons>();
            foreach (Buttons b in Enum.GetValues(typeof(Buttons)))
            {
                if (state.IsButtonDown(b))
                {
                    ret.Add(b);
                }
            }
            return ret;
        }
    }
}