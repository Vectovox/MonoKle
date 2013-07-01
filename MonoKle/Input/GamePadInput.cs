namespace MonoKle.Input
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework;

    public class GamePadInput
    {
        //private static GamePadState[] previousStateArray;
        private GamePadState[] currentState;
        private HashSet<Buttons>[] currentButtons;
        private Dictionary<Buttons, double>[] timeHeldByButton;
        private HashSet<Buttons>[] previousButtons;

        internal GamePadInput()
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

        public bool IsButtonDown(Buttons button, byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentButtons[playerIndex].Contains(button);
        }

        public bool IsButtonUp(Buttons button, byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentButtons[playerIndex].Contains(button) == false;
        }

        public bool IsButtonHeld(Buttons button, byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return IsButtonDown(button, playerIndex) && previousButtons[playerIndex].Contains(button);
        }

        public bool IsButtonHeld(Buttons button, byte playerIndex, double timeHeld)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return IsButtonHeld(button, playerIndex) && GetButtonHeldTime(button, playerIndex) >= timeHeld;
        }

        public bool IsButtonPressed(Buttons button, byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return IsButtonDown(button, playerIndex) && previousButtons[playerIndex].Contains(button) == false;
        }

        public bool IsButtonReleased(Buttons button, byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return IsButtonUp(button, playerIndex) && previousButtons[playerIndex].Contains(button);
        }

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

        public bool IsConnected(byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentState[playerIndex].IsConnected;
        }

        public Vector2 GetLeftThumbstick(byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentState[playerIndex].ThumbSticks.Left;
        }

        public Vector2 GetRightThumbstick(byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentState[playerIndex].ThumbSticks.Right;
        }

        public float GetLeftTrigger(byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentState[playerIndex].Triggers.Left;
        }

        public float GetRightTrigger(byte playerIndex)
        {
            if (playerIndex > 3)
            {
                throw new ArgumentOutOfRangeException("Player index must be in the interval 0-3.");
            }
            return currentState[playerIndex].Triggers.Right;
        }

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

        private static HashSet<Buttons> GetButtonsDown(GamePadState state)
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
