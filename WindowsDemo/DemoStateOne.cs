using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoKle;
using MonoKle.Assets;
using MonoKle.Core;
using MonoKle.Core.Geometry;
using MonoKle.Graphics;
using MonoKle.Graphics.Primitives;
using MonoKle.Input;
using MonoKle.Logging;
using MonoKle.Messaging;
using MonoKle.State;
using System;

namespace WindowsDemo
{
    public class DemoStateOne : GameState
    {
        private SpriteBatch sb;
        private Timer timer = new Timer(5);
        private Camera2D camera = new Camera2D(new MPoint2(800, 600));
        private PrimitiveBatch2D primitive2D;

        public DemoStateOne() : base("stateOne") { }

        public override void Draw(double time)
        {
            this.primitive2D.Begin(this.camera.GetTransformMatrix());
            this.primitive2D.DrawLine(new Vector2(80, 200), new Vector2(200, 80), Color.Red, Color.Blue);
            this.primitive2D.DrawLine(new Vector2(380, 500), new Vector2(500, 380), Color.Red, Color.Blue);
            this.primitive2D.End();

            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, camera.GetTransformMatrix());

            sb.Draw(MonoKleGame.TextureStorage.DefaultValue, new Vector2(50, 50), Color.White);
            sb.Draw(MonoKleGame.TextureStorage.WhiteTexture, new Vector2(150, 50), Color.Red);

            sb.Draw(MonoKleGame.TextureStorage.GetAsset("assets\\textures\\testbox.png"), new Vector2(250, 250), Color.White);

            // Test timer
            MonoKleGame.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Timer: " + timer.GetTimeLeft() + " (" + timer.Duration + ") Done? " + timer.IsDone(),
                new Vector2(50, 150), Color.Green);

            // Test linebreak
            MonoKleGame.FontStorage.GetAsset("TESTFONT").DrawString(sb, "ABCDEF\nabcdef",
                new Vector2(50, 250), Color.Green);

            // Test scale
            MonoKleGame.FontStorage.GetAsset("TESTFONT").DrawString(sb, "ABCDEF\nabcdef",
                new Vector2(250, 250), Color.Green, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            // Test rotation
            MonoKleGame.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Rotating",
                new Vector2(50, 350), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Test rotation
            MonoKleGame.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Rotating",
                new Vector2(0, 0), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Rotation with scale
            MonoKleGame.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Rotating scale",
                new Vector2(350, 350), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            // Rotation with scale and origin
            Vector2 orig = MonoKleGame.FontStorage.GetAsset("TESTFONT").MeasureString("Rotating origin scale") * 0.5f;
            MonoKleGame.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Rotating origin scale",
                new Vector2(550, 150), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, orig, 2f, SpriteEffects.None, 0f);

            // XXX
            Vector2 o = MonoKleGame.FontStorage.GetAsset("TESTFONT").MeasureString("<=+=>") * 0.5f;
            MonoKleGame.FontStorage.GetAsset("TESTFONT").DrawString(sb, "<=+=>",
                new Vector2(400, 300), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, o, 1f, SpriteEffects.None, 0f);


            string s = "Testin size";
            MonoKleGame.FontStorage.DefaultValue.DrawString(sb, s, new Vector2(450, 350), Color.Green);
            MonoKleGame.FontStorage.DefaultValue.DrawString(sb, s, new Vector2(450, 350 + MonoKleGame.FontStorage.DefaultValue.MeasureString(s).Y), Color.Green);
            MonoKleGame.FontStorage.DefaultValue.DrawString(sb, s, new Vector2(450, 350 + 2 * MonoKleGame.FontStorage.DefaultValue.MeasureString(s).Y), Color.Green);

            string s2 = "Testin size\nLol";
            MonoKleGame.FontStorage.DefaultValue.DrawString(sb, s2, new Vector2(0, 0), Color.Green);
            MonoKleGame.FontStorage.DefaultValue.DrawString(sb, s2, new Vector2(0, 0 + MonoKleGame.FontStorage.DefaultValue.MeasureString(s2).Y), Color.Green);
            MonoKleGame.FontStorage.DefaultValue.DrawString(sb, s2, new Vector2(0, 0 + 2 * MonoKleGame.FontStorage.DefaultValue.MeasureString(s2).Y), Color.Green);

            // Test size measurements.
            Vector2 pos = new Vector2(50, 450);
            MonoKleGame.FontStorage.GetAsset("TESTFONT").DrawString(sb, "One-",
                pos, Color.Green);
            pos.X += MonoKleGame.FontStorage.GetAsset("TESTFONT").MeasureString("One-").X;

            MonoKleGame.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Two",
                pos, Color.Red);

            pos.Y -= MonoKleGame.FontStorage.GetAsset("TESTFONT").MeasureString("Three").Y;
            MonoKleGame.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Three",
                pos, Color.Orange);

            pos.X += MonoKleGame.FontStorage.GetAsset("TESTFONT").MeasureString("Three").X;
            MonoKleGame.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Four",
                pos, Color.Blue, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            pos.X += MonoKleGame.FontStorage.GetAsset("TESTFONT").MeasureString("Four", 2f).X;
            MonoKleGame.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Five",
                pos, Color.Black);

            sb.End();
        }

        public override void Update(double seconds)
        {
            if(MonoKleGame.Console.IsOpen == false)
            {

                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.Escape, 1))
                {
                    MonoKleGame.GetInstance().Exit();
                }

                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.Space))
                {
                    MonoKleGame.StateManager.SwitchState("stateTwo", null);
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

                if(MonoKleGame.Keyboard.IsKeyPressed(Keys.F2))
                {
                    MonoKleGame.GraphicsManager.SetScreenSize(new MPoint2(1280, 720));
                }
                else if(MonoKleGame.Keyboard.IsKeyPressed(Keys.F3))
                {
                    MonoKleGame.GraphicsManager.SetScreenSize(new MPoint2(800, 600));
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

                if(MonoKleGame.Keyboard.IsKeyPressed(Keys.N))
                {
                    Logger.Global.Log("I am logging", this.GetType());
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

        protected override void Activated(StateSwitchData data)
        {
            MonoKleGame.MessagePasser.Subscribe("testChannel", Test);
            MonoKleGame.MessagePasser.Subscribe("noChannel", Test);
            MonoKleGame.MessagePasser.Unsubscribe("noChannel", Test);
            MonoKleGame.MessagePasser.Subscribe("CONSOLE", ConsoleMessage);
            MonoKleGame.Console.WriteLine("State one activated! Message: " + (string)data.Data);
            MonoKleGame.Console.WriteLine(MonoKleGame.TextureStorage.LoadFiles("Assets\\Textures", true).Successes + " textures loaded.");
            MonoKleGame.Console.WriteLine(MonoKleGame.FontStorage.LoadFiles("Assets\\Fonts", true).Successes + " fonts loaded.");
            MonoKleGame.Console.WriteLine(MonoKleGame.EffectStorage.LoadFiles("Assets\\Effects", true).Successes + " effects loaded.");
            sb = new SpriteBatch(MonoKleGame.GraphicsManager.GetGraphicsDevice());
            timer.Reset();
            //MonoKleGame.ScriptInterface.AddScriptSources("TestScripts.ms", false);
            //MonoKleGame.ScriptInterface.CompileSources();
            this.primitive2D = new PrimitiveBatch2D(MonoKleGame.GraphicsManager.GetGraphicsDevice());
            //MonoKleGame.EffectStorage.LoadFiles("shader.fx");
        }
    }
}
