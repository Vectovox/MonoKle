namespace MonoKle.Input
{
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Class for polling gamepad input.
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
            this.currentState = new GamePadState();
        }

        /// <summary>
        /// Gets the A button.
        /// </summary>
        /// <value>
        /// A button.
        /// </value>
        public IPressable A => this.buttonA;

        /// <summary>
        /// Gets the B button.</summary>
        /// <value>
        /// B button.
        /// </value>
        public IPressable B => this.buttonB;

        /// <summary>
        /// Gets the Back button.
        /// </summary>
        /// <value>
        /// Back button.
        /// </value>
        public IPressable Back => this.buttonBack;

        /// <summary>
        /// Gets the Big button.
        /// </summary>
        /// <value>
        /// Big button.
        /// </value>
        public IPressable Big => this.buttonBig;

        /// <summary>
        /// Gets the D-pad.
        /// </summary>
        /// <value>
        /// The D-pad.
        /// </value>
        public IDPad DPad => this.dPad;

        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected => this.currentState.IsConnected;

        /// <summary>
        /// Gets the Left Shoulder button.
        /// </summary>
        /// <value>
        /// Left Shoulder button.
        /// </value>
        public IPressable LeftShoulder => this.buttonLeftShoulder;

        /// <summary>
        /// Gets the left thumbstick.
        /// </summary>
        /// <value>
        /// The left thumbstick.
        /// </value>
        public IPressableStick LeftThumbstick => this.stickLeft;

        /// <summary>
        /// Gets the left trigger.
        /// </summary>
        /// <value>
        /// The left trigger.
        /// </value>
        public ITrigger LeftTrigger => this.triggerLeft;

        /// <summary>
        /// Gets the packet number.
        /// </summary>
        /// <value>
        /// The packet number.
        /// </value>
        public int PacketNumber => this.currentState.PacketNumber;

        /// <summary>
        /// Gets the index of the player.
        /// </summary>
        /// <value>
        /// The index of the player.
        /// </value>
        public Microsoft.Xna.Framework.PlayerIndex PlayerIndex => this.playerIndex;

        /// <summary>
        /// Gets the Right Shoulder button.
        /// </summary>
        /// <value>
        /// Right Shoulder button.
        /// </value>
        public IPressable RightShoulder => this.buttonRightShoulder;

        /// <summary>
        /// Gets the right thumbstick.
        /// </summary>
        /// <value>
        /// The right thumbstick.
        /// </value>
        public IPressableStick RightThumbstick => this.stickRight;

        /// <summary>
        /// Gets the right trigger.
        /// </summary>
        /// <value>
        /// The right trigger.
        /// </value>
        public ITrigger RightTrigger => this.triggerRight;

        /// <summary>
        /// Gets the Start button.
        /// </summary>
        /// <value>
        /// Start button.
        /// </value>
        public IPressable Start => this.buttonStart;

        /// <summary>
        /// Gets the X button.</summary>
        /// <value>
        /// X button.
        /// </value>
        public IPressable X => this.buttonX;

        /// <summary>
        /// Gets the Y button.
        /// </summary>
        /// <value>
        /// Y button.
        /// </value>
        public IPressable Y => this.buttonY;

        /// <summary>
        /// Updates the component with the specified seconds since last update.
        /// </summary>
        /// <param name="seconds">The amount of seconds since last update.</param>
        public void Update(double seconds)
        {
            this.currentState = Microsoft.Xna.Framework.Input.GamePad.GetState(this.playerIndex);

            if (this.currentState.IsConnected)
            {
                this.buttonA.Update(this.currentState.Buttons.A == ButtonState.Pressed, seconds);
                this.buttonB.Update(this.currentState.Buttons.B == ButtonState.Pressed, seconds);
                this.buttonBack.Update(this.currentState.Buttons.Back == ButtonState.Pressed, seconds);
                this.buttonBig.Update(this.currentState.Buttons.BigButton == ButtonState.Pressed, seconds);
                this.buttonLeftShoulder.Update(this.currentState.Buttons.LeftShoulder == ButtonState.Pressed, seconds);
                this.buttonRightShoulder.Update(this.currentState.Buttons.RightShoulder == ButtonState.Pressed, seconds);
                this.buttonStart.Update(this.currentState.Buttons.Start == ButtonState.Pressed, seconds);
                this.buttonX.Update(this.currentState.Buttons.X == ButtonState.Pressed, seconds);
                this.buttonY.Update(this.currentState.Buttons.Y == ButtonState.Pressed, seconds);

                this.stickLeft.Update(this.currentState.Buttons.LeftStick == ButtonState.Pressed,
                    this.currentState.ThumbSticks.Left, seconds);
                this.stickRight.Update(this.currentState.Buttons.RightStick == ButtonState.Pressed,
                    this.currentState.ThumbSticks.Right, seconds);

                this.dPad.Update(this.currentState.DPad.Left == ButtonState.Pressed,
                                 this.currentState.DPad.Right == ButtonState.Pressed,
                                 this.currentState.DPad.Up == ButtonState.Pressed,
                                 this.currentState.DPad.Down == ButtonState.Pressed,
                                 seconds);

                this.triggerLeft.Update(this.currentState.Triggers.Left, seconds);
                this.triggerRight.Update(this.currentState.Triggers.Right, seconds);
            }
        }
    }
}