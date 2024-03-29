using Microsoft.Xna.Framework;
using System;

namespace MonoKle.Input.Gamepad
{
    /// <summary>
    /// Class providing gamepad instances for different players.
    /// </summary>
    public sealed class GamePadHub : IGamePadHub, IUpdateable
    {
        private readonly GamePad _playerOne = new(PlayerIndex.One);
        private readonly GamePad _playerTwo = new(PlayerIndex.Two);
        private readonly GamePad _playerThree = new(PlayerIndex.Three);
        private readonly GamePad _playerFour = new(PlayerIndex.Four);

        public IGamePad PlayerOne => _playerOne;
        public IGamePad PlayerTwo => _playerTwo;
        public IGamePad PlayerThree => _playerThree;
        public IGamePad PlayerFour => _playerFour;

        public bool WasActivated => _playerOne.WasActivated || _playerTwo.WasActivated || _playerThree.WasActivated || _playerFour.WasActivated;

        public bool AnyDisconnected => _playerOne.WasDisconnected || _playerTwo.WasDisconnected || _playerThree.WasDisconnected || _playerFour.WasDisconnected;

        public IGamePad GetGamePad(PlayerIndex playerIndex) => playerIndex switch
        {
            PlayerIndex.One => _playerOne,
            PlayerIndex.Two => _playerTwo,
            PlayerIndex.Three => _playerThree,
            PlayerIndex.Four => _playerFour,
            _ => throw new ArgumentException("Invalid player index provided."),
        };

        public void Update(TimeSpan timeDelta)
        {
            _playerOne.Update(timeDelta);
            _playerTwo.Update(timeDelta);
            _playerThree.Update(timeDelta);
            _playerFour.Update(timeDelta);
        }
    }
}
