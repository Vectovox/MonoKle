using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoKle;
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
            sb.Draw(MonoKleGame.TextureManager.DefaultTexture, new Vector2(50, 50), Color.White);
            sb.Draw(MonoKleGame.TextureManager.WhiteTexture, new Vector2(150, 50), Color.Red);

            sb.Draw(MonoKleGame.TextureManager.GetTexture("testbox"), new Vector2(250, 250), Color.White);

            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "ABCDEF abcdef",
                new Vector2(50, 250), Color.Green);
            sb.End();
        }

        public override void Update(double seconds)
        {
            if (MonoKleGame.Keyboard.IsKeyHeld(Keys.Escape, 1))
            {
                MonoKleGame.GetInstance().Exit();
            }

            if (MonoKleGame.Keyboard.IsKeyPressed(Keys.Space))
            {
                MonoKleGame.StateManager.SwitchState(new StateSwitchData("stateTwo", null));
            }
        }

        public override void Activated(StateSwitchData data)
        {
            Console.WriteLine("State one activated! Message: " + (string)data.Data);
            Console.WriteLine(MonoKleGame.TextureManager.Load("Assets\\Textures", true) + " textures loaded.");
            Console.WriteLine(MonoKleGame.FontManager.Load("Assets\\Fonts", true) + " fonts loaded.");
            sb = new SpriteBatch(MonoKleGame.GraphicsManager.GetGraphicsDevice());
        }
    }
}
