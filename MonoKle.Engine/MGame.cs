using Microsoft.Xna.Framework;

namespace MonoKle.Engine
{
    /// <summary>
    /// Main game class for MonoKle.
    /// </summary>
    public class MGame : Game
    {
        internal MGame()
        {
        }

        protected override void Draw(GameTime gameTime) => MBackend.Draw(gameTime);

        protected override void Update(GameTime gameTime) => MBackend.Update(gameTime);
    }
}