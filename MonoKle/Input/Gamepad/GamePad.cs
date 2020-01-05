using Microsoft.Xna.Framework.Input;
using System;

namespace MonoKle.Input.Gamepad
{
    /// <summary>
    /// Class representing the current state of a gamepad.
    /// </summary>
    /// <seealso cref="IGamePad" />
    /// <seealso cref="IUpdateable" />
    public class GamePad : IGamePad, IUpdateable
    {
        private Button buttonA = new Button();
        private Button buttonB = new Button();
        private Button buttonBack = new Button();
        private Button buttonBig = new Button();
        private Button buttonLeftShoulder = new Button();
        private Button buttonRightShoulder = new Button();
        private Button buttonStart = new Button();
        private Button buttonX = new Button();
        private Button buttonY = new Button();
        private GamePadState currentState;
        private DPad dPad = new DPad();
        private Microsoft.Xna.Framework.PlayerIndex playerIndex;
        private PressableStick stickLeft = new PressableStick();
        private PressableStick stickRight = new PressableStick();
        private Trigger triggerLeft = new Trigger();
        private Trigger triggerRight = new Trigger();

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePad"/> class.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        public GamePad(Microsoft.Xna.Framework.PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
            currentState = new GamePadState();
        }

        /// <summary>
        /// Gets the A button.
        /// </summary>
        /// <value>
        /// A button.
        /// </value>
        public IPressable A => buttonA;

        /// <summary>
        /// Gets the B button.</summary>
        /// <value>
        /// B button.
        /// </value>
        public IPressable B => buttonB;

        /// <summary>
        /// Gets the Back button.
        /// </summary>
        /// <value>
        /// Back button.
        /// </value>
        public IPressable Back => buttonBack;

        /// <summary>
        /// Gets the Big button.
        /// </summary>
        /// <value>
        /// Big button.
        /// </value>
        public IPressable Big => buttonBig;

        /// <summary>
        /// Gets the D-pad.
        /// </summary>
        /// <value>
        /// The D-pad.
        /// </value>
        public IDPad DPad => dPad;

        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected => currentState.IsConnected;

        /// <summary>
        /// Gets the Left Shoulder button.
        /// </summary>
        /// <value>
        /// Left Shoulder button.
        /// </value>
        public IPressable LeftShoulder => buttonLeftShoulder;

        /// <summary>
        /// Gets the left thumbstick.
        /// </summary>
        /// <value>
        /// The left thumbstick.
        /// </value>
        public IPressableStick LeftThumbstick => stickLeft;

        /// <summary>
        /// Gets the left trigger.
        /// </summary>
        /// <value>
        /// The left trigger.
        /// </value>
        public ITrigger LeftTrigger => triggerLeft;

        /// <summary>
        /// Gets the packet number.
        /// </summary>
        /// <value>
        /// The packet number.
        /// </value>
        public int PacketNumber => currentState.PacketNumber;

        /// <summary>
        /// Gets the index of the player.
        /// </summary>
        /// <value>
        /// The index of the player.
        /// </value>
        public Microsoft.Xna.Framework.PlayerIndex PlayerIndex => playerIndex;

        /// <summary>
        /// Gets the Right Shoulder button.
        /// </summary>
        /// <value>
        /// Right Shoulder button.
        /// </value>
        public IPressable RightShoulder => buttonRightShoulder;

        /// <summary>
        /// Gets the right thumbstick.
        /// </summary>
        /// <value>
        /// The right thumbstick.
        /// </value>
        public IPressableStick RightThumbstick => stickRight;

        /// <summary>
        /// Gets the right trigger.
        /// </summary>
        /// <value>
        /// The right trigger.
        /// </value>
        public ITrigger RightTrigger => triggerRight;

        /// <summary>
        /// Gets the Start button.
        /// </summary>
        /// <value>
        /// Start button.
        /// </value>
        public IPressable Start => buttonStart;

        /// <summary>
        /// Gets the X button.</summary>
        /// <value>
        /// X button.
        /// </value>
        public IPressable X => buttonX;

        /// <summary>
        /// Gets the Y button.
        /// </summary>
        /// <value>
        /// Y button.
        /// </value>
        public IPressable Y => buttonY;

        public void Update(TimeSpan timeDelta)
        {
            currentState = Microsoft.Xna.Framework.Input.GamePad.GetState(playerIndex);

            if (currentState.IsConnected)
            {
                buttonA.Update(currentState.Buttons.A == ButtonState.Pressed, timeDelta);
                buttonB.Update(currentState.Buttons.B == ButtonState.Pressed, timeDelta);
                buttonBack.Update(currentState.Buttons.Back == ButtonState.Pressed, timeDelta);
                buttonBig.Update(currentState.Buttons.BigButton == ButtonState.Pressed, timeDelta);
                buttonLeftShoulder.Update(currentState.Buttons.LeftShoulder == ButtonState.Pressed, timeDelta);
                buttonRightShoulder.Update(currentState.Buttons.RightShoulder == ButtonState.Pressed, timeDelta);
                buttonStart.Update(currentState.Buttons.Start == ButtonState.Pressed, timeDelta);
                buttonX.Update(currentState.Buttons.X == ButtonState.Pressed, timeDelta);
                buttonY.Update(currentState.Buttons.Y == ButtonState.Pressed, timeDelta);

                stickLeft.Update(currentState.Buttons.LeftStick == ButtonState.Pressed,
                    currentState.ThumbSticks.Left, timeDelta);
                stickRight.Update(currentState.Buttons.RightStick == ButtonState.Pressed,
                    currentState.ThumbSticks.Right, timeDelta);

                dPad.Update(currentState.DPad.Left == ButtonState.Pressed,
                                 currentState.DPad.Right == ButtonState.Pressed,
                                 currentState.DPad.Up == ButtonState.Pressed,
                                 currentState.DPad.Down == ButtonState.Pressed,
                                 timeDelta);

                triggerLeft.Update(currentState.Triggers.Left, timeDelta);
                triggerRight.Update(currentState.Triggers.Right, timeDelta);
            }
        }
    }
}
