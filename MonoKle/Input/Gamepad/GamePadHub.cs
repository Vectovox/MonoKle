namespace MonoKle.Input.Gamepad {
    using System;

    /// <summary>
    /// Class providing methods to get gamepads.
    /// </summary>
    public class GamePadHub : IGamePadHub, IUpdateable {
        private GamePad playerFour = new GamePad(Microsoft.Xna.Framework.PlayerIndex.Four);
        private GamePad playerOne = new GamePad(Microsoft.Xna.Framework.PlayerIndex.One);
        private GamePad playerThree = new GamePad(Microsoft.Xna.Framework.PlayerIndex.Three);
        private GamePad playerTwo = new GamePad(Microsoft.Xna.Framework.PlayerIndex.Two);

        /// <summary>
        /// Gets the player four.
        /// </summary>
        /// <value>
        /// The player four.
        /// </value>
        public IGamePad PlayerFour => playerFour;

        /// <summary>
        /// Gets the player one.
        /// </summary>
        /// <value>
        /// The player one.
        /// </value>
        public IGamePad PlayerOne => playerOne;

        /// <summary>
        /// Gets the player three.
        /// </summary>
        /// <value>
        /// The player three.
        /// </value>
        public IGamePad PlayerThree => playerThree;

        /// <summary>
        /// Gets the player two.
        /// </summary>
        /// <value>
        /// The player two.
        /// </value>
        public IGamePad PlayerTwo => playerTwo;

        /// <summary>
        /// Gets the game pad for the provided player.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns>
        /// A gamepad.
        /// </returns>
        public IGamePad GetGamePad(Microsoft.Xna.Framework.PlayerIndex playerIndex) {
            switch (playerIndex) {
                case Microsoft.Xna.Framework.PlayerIndex.One:
                    return playerOne;

                case Microsoft.Xna.Framework.PlayerIndex.Two:
                    return playerTwo;

                case Microsoft.Xna.Framework.PlayerIndex.Three:
                    return playerThree;

                case Microsoft.Xna.Framework.PlayerIndex.Four:
                    return playerFour;

                default:
                    throw new ArgumentException("Invalid player index provided.");
            }
        }

        public void Update(TimeSpan timeDelta) {
            playerOne.Update(timeDelta);
            playerTwo.Update(timeDelta);
            playerThree.Update(timeDelta);
            playerFour.Update(timeDelta);
        }
    }
}
