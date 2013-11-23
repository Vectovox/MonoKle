using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoKle;
using MonoKle.Assets;
using MonoKle.Core;
using MonoKle.Graphics;
using MonoKle.Input;
using MonoKle.State;
using System;

namespace WindowsDemo
{
    public class DemoStateOne : GameState
    {
        private SpriteBatch sb;
        private Timer timer = new Timer(5);

        public override void Draw(double time)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

            sb.Draw(MonoKleGame.TextureManager.DefaultTexture, new Vector2(50, 50), Color.White);
            sb.Draw(MonoKleGame.TextureManager.WhiteTexture, new Vector2(150, 50), Color.Red);

            sb.Draw(MonoKleGame.TextureManager.GetTexture("testbox"), new Vector2(250, 250), Color.White);

            // Test timer
            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "Timer: " + timer.TimeLeft + " (" + timer.Duration + ") Done? " + timer.IsDone,
                new Vector2(50, 150), Color.Green);

            // Test linebreak
            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "ABCDEF\nabcdef",
                new Vector2(50, 250), Color.Green);

            // Test scale
            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "ABCDEF\nabcdef",
                new Vector2(250, 250), Color.Green, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            // Test rotation
            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "Rotating",
                new Vector2(50, 350), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Test rotation
            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "Rotating",
                new Vector2(0, 0), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Rotation with scale
            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "Rotating scale",
                new Vector2(350, 350), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            // Rotation with scale and origin
            Vector2 orig = MonoKleGame.FontManager.GetFont("TESTFONT").MeasureString("Rotating origin scale") * 0.5f;
            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "Rotating origin scale",
                new Vector2(550, 150), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, orig, 2f, SpriteEffects.None, 0f);

            // XXX
            Vector2 o = MonoKleGame.FontManager.GetFont("TESTFONT").MeasureString("<=+=>") * 0.5f;
            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "<=+=>",
                new Vector2(400, 300), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, o, 1f, SpriteEffects.None, 0f);


            // Test size measurements.
            Vector2 pos = new Vector2(50, 450);
            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "One-",
                pos, Color.Green);
            pos.X += MonoKleGame.FontManager.GetFont("TESTFONT").MeasureString("One-").X;

            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "Two",
                pos, Color.Red);

            pos.Y -= MonoKleGame.FontManager.GetFont("TESTFONT").MeasureString("Three").Y;
            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "Three",
                pos, Color.Orange);

            pos.X += MonoKleGame.FontManager.GetFont("TESTFONT").MeasureString("Three").X;
            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "Four",
                pos, Color.Blue, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            pos.X += MonoKleGame.FontManager.GetFont("TESTFONT").MeasureString("Four", 2f).X;
            MonoKleGame.FontManager.GetFont("TESTFONT").DrawString(sb, "Five",
                pos, Color.Black);

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

            timer.Update(seconds);
        }

        public override void Activated(StateSwitchData data)
        {
            Console.WriteLine("State one activated! Message: " + (string)data.Data);
            Console.WriteLine(MonoKleGame.TextureManager.Load("Assets\\Textures", true) + " textures loaded.");
            Console.WriteLine(MonoKleGame.FontManager.Load("Assets\\Fonts", true) + " fonts loaded.");
            sb = new SpriteBatch(MonoKleGame.GraphicsManager.GetGraphicsDevice());
            timer.Reset();
        }
    }
}
