using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoKle.Assets;
using MonoKle.Graphics;
using MonoKle.Input;
using MonoKle.State;
using System;

namespace WindowsDemo
{
    public class DemoStateOne : GameState
    {
        private SpriteBatch sb;

        public override void Draw(double time)
        {
            sb.Begin();
            sb.Draw(TextureManager.GetDefaultTexture(), new Vector2(50, 50), Color.White);
            sb.Draw(TextureManager.GetWhiteTexture(), new Vector2(150, 50), Color.Red);

            sb.Draw(TextureManager.GetTexture("testbox"), new Vector2(250, 250), Color.White);
            sb.End();
        }

        public override void Update(double seconds)
        {
            if (KeyboardInput.IsKeyHeld(Keys.Escape, 1))
            {
                MonoKle.MonoKleGame.GetInstance().Exit();
            }

            if (KeyboardInput.IsKeyPressed(Keys.Space))
            {
                StateManager.NextState = "stateTwo";
            }
        }

        public override void Activated()
        {
            Console.WriteLine("State one activated!");
            Console.WriteLine(TextureManager.Load("Assets\\", true) + " textures loaded.");
            sb = new SpriteBatch(GraphicsManager.GetGraphicsDevice());
        }
    }
}
