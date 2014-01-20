using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoKle;
using MonoKle.Assets;
using MonoKle.Core;
using MonoKle.Graphics;
using MonoKle.Input;
using MonoKle.Messaging;
using MonoKle.State;
using System;

namespace WindowsDemo
{
    public class DemoStateOne : GameState
    {
        private SpriteBatch sb;
        private Timer timer = new Timer(5);
        private Camera2D camera = new Camera2D(new Vector2Int32(800, 600));

        public override void Draw(double time)
        {
            MonoKleGame.PrimitiveDrawer.Draw2DLine(new Vector2(250, 250), new Vector2(200, 200), Color.Red);
            MonoKleGame.PrimitiveDrawer.Draw2DLine(new Vector2(250, 550), new Vector2(500, 500), Color.Red, Color.Blue);

            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, camera.GetTransformMatrix());

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


            string s = "Testin size";
            MonoKleGame.FontManager.DefaultFont.DrawString(sb, s, new Vector2(450, 350), Color.Green);
            MonoKleGame.FontManager.DefaultFont.DrawString(sb, s, new Vector2(450, 350 + MonoKleGame.FontManager.DefaultFont.MeasureString(s).Y), Color.Green);
            MonoKleGame.FontManager.DefaultFont.DrawString(sb, s, new Vector2(450, 350 + 2 * MonoKleGame.FontManager.DefaultFont.MeasureString(s).Y), Color.Green);

            string s2 = "Testin size\nLol";
            MonoKleGame.FontManager.DefaultFont.DrawString(sb, s2, new Vector2(0, 0), Color.Green);
            MonoKleGame.FontManager.DefaultFont.DrawString(sb, s2, new Vector2(0, 0 + MonoKleGame.FontManager.DefaultFont.MeasureString(s2).Y), Color.Green);
            MonoKleGame.FontManager.DefaultFont.DrawString(sb, s2, new Vector2(0, 0 + 2 * MonoKleGame.FontManager.DefaultFont.MeasureString(s2).Y), Color.Green);

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
            if (MonoKleGame.Keyboard.IsKeyPressed(Keys.F1))
            {
                MonoKleGame.Console.IsOpen = !MonoKleGame.Console.IsOpen;
            }

            if(MonoKleGame.Console.IsOpen == false)
            {

                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.Escape, 1))
                {
                    MonoKleGame.GetInstance().Exit();
                }

                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.Space))
                {
                    MonoKleGame.StateManager.SwitchState(new StateSwitchData("stateTwo", null));
                }

                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.I))
                {
                    camera.SetPosition(camera.GetPosition() + new Vector2(0, -3));
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.K))
                {
                    camera.SetPosition(camera.GetPosition() + new Vector2(0, 3));
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.J))
                {
                    camera.SetPosition(camera.GetPosition() + new Vector2(-3, 0));
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.L))
                {
                    camera.SetPosition(camera.GetPosition() + new Vector2(3, 0));
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.U))
                {
                    camera.SetRotation(camera.GetRotation() + 0.05f);
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.O))
                {
                    camera.SetRotation(camera.GetRotation() - 0.05f);
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.Y))
                {
                    camera.SetScale(camera.GetScale() + 0.01f);
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.H))
                {
                    camera.SetScale(camera.GetScale() - 0.01f);
                }

                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.F12))
                {
                    // CRASH ON PURPOSE
                    object o = null; o.Equals(o);
                }

                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.M))
                {
                    MonoKleGame.MessagePasser.SendMessage("testChannel", new MessageEventArgs("I AM HELLO"));
                    MonoKleGame.MessagePasser.SendMessage("noChannel", new MessageEventArgs("I AM NOT HELLO"));
                }
            }
            
            camera.Update(seconds);
            timer.Update(seconds);
        }

        public void Test(object sender, MessageEventArgs args)
        {
            Console.WriteLine(args.Data as string);
        }

        public void ConsoleMessage(object sender, MessageEventArgs args)
        {
            string data = args.Data as string;

            if(data.Equals("reset timer"))
            {
                timer.Reset();
            }
        }

        public override void Activated(StateSwitchData data)
        {
            MonoKleGame.MessagePasser.Subscribe("testChannel", Test);
            MonoKleGame.MessagePasser.Subscribe("noChannel", Test);
            MonoKleGame.MessagePasser.Unsubscribe("noChannel", Test);
            MonoKleGame.MessagePasser.Subscribe("CONSOLE", ConsoleMessage);
            Console.WriteLine("State one activated! Message: " + (string)data.Data);
            Console.WriteLine(MonoKleGame.TextureManager.Load("Assets\\Textures", true) + " textures loaded.");
            Console.WriteLine(MonoKleGame.FontManager.Load("Assets\\Fonts", true) + " fonts loaded.");
            sb = new SpriteBatch(MonoKleGame.GraphicsManager.GetGraphicsDevice());
            MonoKleGame.PrimitiveDrawer.Camera = camera;
            timer.Reset();
            MonoKleGame.ScriptInterface.LoadScripts("testscripts.ms");
        }
    }
}
