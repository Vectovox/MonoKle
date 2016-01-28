using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoKle;
using MonoKle.Asset;
using MonoKle.Core;
using MonoKle.Core.Geometry;
using MonoKle.Engine;
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

            sb.Draw(MBackend.TextureStorage.DefaultValue, new Vector2(50, 50), Color.White);
            sb.Draw(MBackend.TextureStorage.White, new Vector2(150, 50), Color.Red);

            sb.Draw(MBackend.TextureStorage.GetAsset("assets\\textures\\testbox.png"), new Vector2(250, 250), Color.White);

            // Test timer
            MBackend.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Timer: " + timer.GetTimeLeft() + " (" + timer.Duration + ") Done? " + timer.IsDone(),
                new Vector2(50, 150), Color.Green);

            // Test linebreak
            MBackend.FontStorage.GetAsset("TESTFONT").DrawString(sb, "ABCDEF\nabcdef",
                new Vector2(50, 250), Color.Green);

            // Test scale
            MBackend.FontStorage.GetAsset("TESTFONT").DrawString(sb, "ABCDEF\nabcdef",
                new Vector2(250, 250), Color.Green, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            // Test rotation
            MBackend.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Rotating",
                new Vector2(50, 350), Color.Green, (float)MBackend.TotalGameTime.TotalSeconds, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Test rotation
            MBackend.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Rotating",
                new Vector2(0, 0), Color.Green, (float)MBackend.TotalGameTime.TotalSeconds, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Rotation with scale
            MBackend.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Rotating scale",
                new Vector2(350, 350), Color.Green, (float)MBackend.TotalGameTime.TotalSeconds, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            // Rotation with scale and origin
            Vector2 orig = MBackend.FontStorage.GetAsset("TESTFONT").MeasureString("Rotating origin scale") * 0.5f;
            MBackend.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Rotating origin scale",
                new Vector2(550, 150), Color.Green, (float)MBackend.TotalGameTime.TotalSeconds, orig, 2f, SpriteEffects.None, 0f);

            // XXX
            Vector2 o = MBackend.FontStorage.GetAsset("TESTFONT").MeasureString("<=+=>") * 0.5f;
            MBackend.FontStorage.GetAsset("TESTFONT").DrawString(sb, "<=+=>",
                new Vector2(400, 300), Color.Green, (float)MBackend.TotalGameTime.TotalSeconds, o, 1f, SpriteEffects.None, 0f);


            string s = "Testin size";
            MBackend.FontStorage.DefaultValue.DrawString(sb, s, new Vector2(450, 350), Color.Green);
            MBackend.FontStorage.DefaultValue.DrawString(sb, s, new Vector2(450, 350 + MBackend.FontStorage.DefaultValue.MeasureString(s).Y), Color.Green);
            MBackend.FontStorage.DefaultValue.DrawString(sb, s, new Vector2(450, 350 + 2 * MBackend.FontStorage.DefaultValue.MeasureString(s).Y), Color.Green);

            string s2 = "Testin size\nLol";
            MBackend.FontStorage.DefaultValue.DrawString(sb, s2, new Vector2(0, 0), Color.Green);
            MBackend.FontStorage.DefaultValue.DrawString(sb, s2, new Vector2(0, 0 + MBackend.FontStorage.DefaultValue.MeasureString(s2).Y), Color.Green);
            MBackend.FontStorage.DefaultValue.DrawString(sb, s2, new Vector2(0, 0 + 2 * MBackend.FontStorage.DefaultValue.MeasureString(s2).Y), Color.Green);

            // Test size measurements.
            Vector2 pos = new Vector2(50, 450);
            MBackend.FontStorage.GetAsset("TESTFONT").DrawString(sb, "One-",
                pos, Color.Green);
            pos.X += MBackend.FontStorage.GetAsset("TESTFONT").MeasureString("One-").X;

            MBackend.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Two",
                pos, Color.Red);

            pos.Y -= MBackend.FontStorage.GetAsset("TESTFONT").MeasureString("Three").Y;
            MBackend.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Three",
                pos, Color.Orange);

            pos.X += MBackend.FontStorage.GetAsset("TESTFONT").MeasureString("Three").X;
            MBackend.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Four",
                pos, Color.Blue, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            pos.X += MBackend.FontStorage.GetAsset("TESTFONT").MeasureString("Four", 2f).X;
            MBackend.FontStorage.GetAsset("TESTFONT").DrawString(sb, "Five",
                pos, Color.Black);

            sb.End();
        }

        public override void Update(double seconds)
        {
            if(MBackend.Console.IsOpen == false)
            {

                if (MBackend.Keyboard.IsKeyHeld(Keys.Escape, 1))
                {
                    MBackend.GameInstance.Exit();
                }

                if (MBackend.Keyboard.IsKeyPressed(Keys.Space))
                {
                    MBackend.StateManager.SwitchState("stateTwo", null);
                }

                if (MBackend.Keyboard.IsKeyHeld(Keys.I))
                {
                    camera.SetPosition(camera.GetPosition() + new Vector2(0, -3));
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.K))
                {
                    camera.SetPosition(camera.GetPosition() + new Vector2(0, 3));
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.J))
                {
                    camera.SetPosition(camera.GetPosition() + new Vector2(-3, 0));
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.L))
                {
                    camera.SetPosition(camera.GetPosition() + new Vector2(3, 0));
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.U))
                {
                    camera.SetRotation(camera.GetRotation() + 0.05f);
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.O))
                {
                    camera.SetRotation(camera.GetRotation() - 0.05f);
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.Y))
                {
                    camera.SetScale(camera.GetScale() + 0.01f);
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.H))
                {
                    camera.SetScale(camera.GetScale() - 0.01f);
                }

                if(MBackend.Keyboard.IsKeyPressed(Keys.F2))
                {
                    MBackend.GraphicsManager.SetScreenSize(new MPoint2(1280, 720));
                }
                else if(MBackend.Keyboard.IsKeyPressed(Keys.F3))
                {
                    MBackend.GraphicsManager.SetScreenSize(new MPoint2(800, 600));
                }

                if (MBackend.Keyboard.IsKeyPressed(Keys.F12))
                {
                    // CRASH ON PURPOSE
                    object o = null; o.Equals(o);
                }

                if (MBackend.Keyboard.IsKeyPressed(Keys.M))
                {
                    MBackend.MessagePasser.SendMessage("testChannel", new MessageEventArgs("I AM HELLO"));
                    MBackend.MessagePasser.SendMessage("noChannel", new MessageEventArgs("I AM NOT HELLO"));
                }

                if(MBackend.Keyboard.IsKeyPressed(Keys.N))
                {
                    Logger.Global.Log("I am logging");
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
            MBackend.MessagePasser.Subscribe("testChannel", Test);
            MBackend.MessagePasser.Subscribe("noChannel", Test);
            MBackend.MessagePasser.Unsubscribe("noChannel", Test);
            MBackend.MessagePasser.Subscribe("CONSOLE", ConsoleMessage);
            MBackend.Console.WriteLine("State one activated! Message: " + (string)data.Data);
            MBackend.Console.WriteLine(MBackend.TextureStorage.LoadFiles("Assets\\Textures", true).Successes + " textures loaded.");
            MBackend.Console.WriteLine(MBackend.FontStorage.LoadFiles("Assets\\Fonts", true).Successes + " fonts loaded.");
            MBackend.Console.WriteLine(MBackend.EffectStorage.LoadFiles("Assets\\Effects", true).Successes + " effects loaded.");
            sb = new SpriteBatch(MBackend.GraphicsManager.GetGraphicsDevice());
            timer.Reset();
            //MonoKleGame.ScriptInterface.AddScriptSources("TestScripts.ms", false);
            //MonoKleGame.ScriptInterface.CompileSources();
            this.primitive2D = new PrimitiveBatch2D(MBackend.GraphicsManager.GetGraphicsDevice());
            //MonoKleGame.EffectStorage.LoadFiles("shader.fx");
        }
    }
}
