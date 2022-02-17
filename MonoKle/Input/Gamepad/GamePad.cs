using Microsoft.Xna.Framework;
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
        private readonly Button _buttonA = new Button();
        private readonly Button _buttonB = new Button();
        private readonly Button _buttonBack = new Button();
        private readonly Button _buttonBig = new Button();
        private readonly Button _buttonLeftShoulder = new Button();
        private readonly Button _buttonRightShoulder = new Button();
        private readonly Button _buttonStart = new Button();
        private readonly Button _buttonX = new Button();
        private readonly Button _buttonY = new Button();
        private GamePadState _currentState;
        private readonly DPad _dPad = new DPad();
        private readonly PlayerIndex _playerIndex;
        private readonly PressableStick _stickLeft = new PressableStick();
        private readonly PressableStick _stickRight = new PressableStick();
        private readonly Trigger _triggerLeft = new Trigger();
        private readonly Trigger _triggerRight = new Trigger();

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePad"/> class.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        public GamePad(PlayerIndex playerIndex)
        {
            _playerIndex = playerIndex;
            _currentState = new GamePadState();
        }

        public IPressable A => _buttonA;
        public IPressable B => _buttonB;
        public IPressable Back => _buttonBack;
        public IPressable Big => _buttonBig;
        public IDPad DPad => _dPad;
        public bool IsConnected => _currentState.IsConnected;
        public IPressable LeftShoulder => _buttonLeftShoulder;
        public IPressableStick LeftThumbstick => _stickLeft;
        public ITrigger LeftTrigger => _triggerLeft;
        public int PacketNumber => _currentState.PacketNumber;
        public PlayerIndex PlayerIndex => _playerIndex;
        public IPressable RightShoulder => _buttonRightShoulder;
        public IPressableStick RightThumbstick => _stickRight;
        public ITrigger RightTrigger => _triggerRight;
        public IPressable Start => _buttonStart;
        public IPressable X => _buttonX;
        public IPressable Y => _buttonY;

        public bool WasActivated { get; private set; }

        public void Update(TimeSpan timeDelta)
        {
            _currentState = Microsoft.Xna.Framework.Input.GamePad.GetState(_playerIndex);

            if (_currentState.IsConnected)
            {
                _buttonA.Update(_currentState.Buttons.A == ButtonState.Pressed, timeDelta);
                _buttonB.Update(_currentState.Buttons.B == ButtonState.Pressed, timeDelta);
                _buttonBack.Update(_currentState.Buttons.Back == ButtonState.Pressed, timeDelta);
                _buttonBig.Update(_currentState.Buttons.BigButton == ButtonState.Pressed, timeDelta);
                _buttonLeftShoulder.Update(_currentState.Buttons.LeftShoulder == ButtonState.Pressed, timeDelta);
                _buttonRightShoulder.Update(_currentState.Buttons.RightShoulder == ButtonState.Pressed, timeDelta);
                _buttonStart.Update(_currentState.Buttons.Start == ButtonState.Pressed, timeDelta);
                _buttonX.Update(_currentState.Buttons.X == ButtonState.Pressed, timeDelta);
                _buttonY.Update(_currentState.Buttons.Y == ButtonState.Pressed, timeDelta);

                _stickLeft.Update(_currentState.Buttons.LeftStick == ButtonState.Pressed,
                    _currentState.ThumbSticks.Left, timeDelta);
                _stickRight.Update(_currentState.Buttons.RightStick == ButtonState.Pressed,
                    _currentState.ThumbSticks.Right, timeDelta);

                _dPad.Update(_currentState.DPad.Left == ButtonState.Pressed,
                                 _currentState.DPad.Right == ButtonState.Pressed,
                                 _currentState.DPad.Up == ButtonState.Pressed,
                                 _currentState.DPad.Down == ButtonState.Pressed,
                                 timeDelta);

                _triggerLeft.Update(_currentState.Triggers.Left, timeDelta);
                _triggerRight.Update(_currentState.Triggers.Right, timeDelta);

                WasActivated = _buttonA.IsDown || _buttonB.IsDown || _buttonBack.IsDown || _buttonBig.IsDown || _buttonLeftShoulder.IsDown
                    || _buttonRightShoulder.IsDown || _buttonStart.IsDown || _buttonX.IsDown || _buttonY.IsDown || _stickLeft.Button.IsDown
                    || _stickLeft.Direction.LengthSquared > 0.25f || _stickRight.Button.IsDown || _stickRight.Direction.LengthSquared > 0.25f
                    || _dPad.Left.IsDown || _dPad.Right.IsDown || _dPad.Up.IsDown || _dPad.Down.IsDown || _triggerLeft.IsDown || _triggerRight.IsDown;
            }
            else
            {
                WasActivated = false;
            }
        }
    }
}
