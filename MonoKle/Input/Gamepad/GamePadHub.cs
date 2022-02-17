using Microsoft.Xna.Framework;
using System;

namespace MonoKle.Input.Gamepad
{
    /// <summary>
    /// Class providing gamepad instances for different players.
    /// </summary>
    public sealed class GamePadHub : IGamePadHub, IUpdateable
    {
        private readonly GamePad _playerFour = new GamePad(PlayerIndex.Four);
        private readonly GamePad _playerOne = new GamePad(PlayerIndex.One);
        private readonly GamePad _playerThree = new GamePad(PlayerIndex.Three);
        private readonly GamePad _playerTwo = new GamePad(PlayerIndex.Two);

        public IGamePad PlayerOne => _playerOne;
        public IGamePad PlayerThree => _playerThree;
        public IGamePad PlayerTwo => _playerTwo;
        public IGamePad PlayerFour => _playerFour;

        public bool WasActivated => _playerOne.WasActivated || _playerTwo.WasActivated || _playerThree.WasActivated || _playerFour.WasActivated;

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
