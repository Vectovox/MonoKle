namespace MonoKle.Input
{
    using System;

    /// <summary>
    /// Class providing methods to get gamepads.
    /// </summary>
    public class GamePadHub : IGamePadHub, IUpdateable
    {
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
        public IGamePad PlayerFour => this.playerFour;

        /// <summary>
        /// Gets the player one.
        /// </summary>
        /// <value>
        /// The player one.
        /// </value>
        public IGamePad PlayerOne => this.playerOne;

        /// <summary>
        /// Gets the player three.
        /// </summary>
        /// <value>
        /// The player three.
        /// </value>
        public IGamePad PlayerThree => this.playerThree;

        /// <summary>
        /// Gets the player two.
        /// </summary>
        /// <value>
        /// The player two.
        /// </value>
        public IGamePad PlayerTwo => this.playerTwo;

        /// <summary>
        /// Gets the game pad for the provided player.
        /// </summary>
        /// <param name="playerIndex">Index of the player.</param>
        /// <returns>
        /// A gamepad.
        /// </returns>
        public IGamePad GetGamePad(Microsoft.Xna.Framework.PlayerIndex playerIndex)
        {
            switch (playerIndex)
            {
                case Microsoft.Xna.Framework.PlayerIndex.One:
                    return this.playerOne;

                case Microsoft.Xna.Framework.PlayerIndex.Two:
                    return this.playerTwo;

                case Microsoft.Xna.Framework.PlayerIndex.Three:
                    return this.playerThree;

                case Microsoft.Xna.Framework.PlayerIndex.Four:
                    return this.playerFour;

                default:
                    throw new ArgumentException("Invalid player index provided.");
            }
        }

        /// <summary>
        /// Updates the component with the specified seconds since last update.
        /// </summary>
        /// <param name="seconds">The amount of seconds since last update.</param>
        public void Update(double seconds)
        {
            this.playerOne.Update(seconds);
            this.playerTwo.Update(seconds);
            this.playerThree.Update(seconds);
            this.playerFour.Update(seconds);
        }
    }
}