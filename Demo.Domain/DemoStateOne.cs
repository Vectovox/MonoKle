using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoKle;
using MonoKle.Asset;
using MonoKle.Engine;
using MonoKle.Graphics;
using MonoKle.Input.Keyboard;
using MonoKle.Logging;
using MonoKle.Messaging;
using MonoKle.State;
using System;

namespace Demo.Domain
{
    public class DemoStateOne : GameState
    {
        private SpriteBatch sb;
        private Timer timer = new Timer(TimeSpan.FromSeconds(5));
        private Camera2D camera = new Camera2D(new MPoint2(800, 600));
        private PrimitiveBatch2D primitive2D;

        public DemoStateOne() : base("stateOne")
        {
        }

        public override void Draw(TimeSpan deltaTime)
        {
            primitive2D.Begin(camera.TransformMatrix);
            primitive2D.DrawLine(new Vector2(80, 200), new Vector2(200, 80), Color.Red, Color.Blue);
            primitive2D.DrawLine(new Vector2(380, 500), new Vector2(500, 380), Color.Red, Color.Blue);
            primitive2D.End();

            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, camera.TransformMatrix);

            sb.Draw(MonoKleGame.TextureStorage.DefaultValue, new Vector2(50, 50), Color.White);
            sb.Draw(MonoKleGame.TextureStorage.White, new Vector2(150, 50), Color.Red);

            var testBoxRect = new MRectangleInt(250, 250, 64, 64);
            bool testBoxMouseWithin = testBoxRect.Contains(camera.TransformInv(MonoKleGame.Mouse.Position.Value.ToMVector2()).ToMPoint2());
            sb.Draw(MonoKleGame.TextureStorage.GetAsset("textures/testbox.png"), testBoxRect, testBoxMouseWithin ? Color.Red : Color.White);

            Font font = MonoKleGame.FontStorage.GetAsset("Fonts/testfont.mfnt");

            // Test timer
            sb.DrawString(font, "Timer: " + timer.TimeLeft + " (" + timer.Duration + ") Done? " + timer.IsDone,
                new Vector2(50, 150), Color.Green);

            // Test linebreak
            sb.DrawString(font, "ABCDEF\nabcdef",
                 new Vector2(50, 250), Color.Green);

            // Test scale
            sb.DrawString(font, "ABCDEF\nabcdef",
                 new Vector2(250, 250), Color.Green, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            // Test rotation
            sb.DrawString(font, "Rotating",
                 new Vector2(50, 350), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Test rotation
            sb.DrawString(font, "Rotating",
                 new Vector2(0, 0), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Rotation with scale
            sb.DrawString(font, "Rotating scale",
                 new Vector2(350, 350), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            // Rotation with scale and origin
            Vector2 orig = font.MeasureString("Rotating origin scale") * 0.5f;
            sb.DrawString(font, "Rotating origin scale",
                 new Vector2(550, 150), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, orig, 2f, SpriteEffects.None, 0f);

            // XXX
            Vector2 o = font.MeasureString(ti.Text) * 0.5f;
            sb.DrawString(font, ti.Text, new Vector2(400, 300), Color.Green, (float)MonoKleGame.TotalGameTime.TotalSeconds, o, 1f, SpriteEffects.None, 0f);

            string s = "Testin size";
            sb.DrawString(MonoKleGame.FontStorage.DefaultValue, s, new Vector2(450, 350), Color.Green);
            sb.DrawString(MonoKleGame.FontStorage.DefaultValue, s, new Vector2(450, 350 + MonoKleGame.FontStorage.DefaultValue.MeasureString(s).Y), Color.Green);
            sb.DrawString(MonoKleGame.FontStorage.DefaultValue, s, new Vector2(450, 350 + 2 * MonoKleGame.FontStorage.DefaultValue.MeasureString(s).Y), Color.Green);

            string s2 = "Testin size\nLol";
            sb.DrawString(MonoKleGame.FontStorage.DefaultValue, s2, new Vector2(0, 0), Color.Green);
            sb.DrawString(MonoKleGame.FontStorage.DefaultValue, s2, new Vector2(0, 0 + MonoKleGame.FontStorage.DefaultValue.MeasureString(s2).Y), Color.Green);
            sb.DrawString(MonoKleGame.FontStorage.DefaultValue, s2, new Vector2(0, 0 + 2 * MonoKleGame.FontStorage.DefaultValue.MeasureString(s2).Y), Color.Green);

            // Test size measurements.
            var pos = new Vector2(50, 450);
            sb.DrawString(font, "One-", pos, Color.Green);
            pos.X += font.MeasureString("One-").X;

            sb.DrawString(font, "Two", pos, Color.Red);

            pos.Y -= font.MeasureString("Three").Y;
            sb.DrawString(font, "Three", pos, Color.Orange);

            pos.X += font.MeasureString("Three").X;
            sb.DrawString(font, "Four", pos, Color.Blue, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            pos.X += font.MeasureString("Four", 2f).X;
            sb.DrawString(font, "Five", pos, Color.Black);

            sb.End();

            sb.Begin();
            sb.Draw(MonoKleGame.TextureStorage.White, new Rectangle(MonoKleGame.Mouse.Position.Value.X, MonoKleGame.Mouse.Position.Value.Y, 3, 3), Color.Black);
            sb.End();
        }

        public override void Update(TimeSpan deltaTime)
        {
            if (MonoKleGame.Console.IsOpen == false)
            {
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.Escape, TimeSpan.FromSeconds(1)))
                {
                    MonoKleGame.GameInstance.Exit();
                }

                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.Space))
                {
                    MonoKleGame.StateSystem.SwitchState("stateTwo", null);
                }

                if (MonoKleGame.Keyboard.AreKeysHeld(new Keys[] { Keys.R, Keys.T }, MonoKle.Input.CollectionQueryBehavior.All))
                {
                    MonoKleGame.Console.WriteLine("R + T held.");
                }

                if (MonoKleGame.Keyboard.AreKeysHeld(new Keys[] { Keys.LeftShift, Keys.RightShift }, MonoKle.Input.CollectionQueryBehavior.Any))
                {
                    MonoKleGame.Console.WriteLine("Any shift held.");
                }

                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.I))
                {
                    camera.SetPosition(camera.Position + new MVector2(0, -3));
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.K))
                {
                    camera.SetPosition(camera.Position + new MVector2(0, 3));
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.J))
                {
                    camera.SetPosition(camera.Position + new MVector2(-3, 0));
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.L))
                {
                    camera.SetPosition(camera.Position + new MVector2(3, 0));
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.U))
                {
                    camera.SetRotation(camera.Rotation + 0.05f);
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.O))
                {
                    camera.SetRotation(camera.Rotation - 0.05f);
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.Y))
                {
                    camera.SetScale(camera.Scale + 0.01f);
                }
                if (MonoKleGame.Keyboard.IsKeyHeld(Keys.H))
                {
                    camera.SetScale(camera.Scale - 0.01f);
                }

                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.F2))
                {
                    MonoKleGame.GraphicsManager.Resolution = new MPoint2(1280, 720);
                }
                else if (MonoKleGame.Keyboard.IsKeyPressed(Keys.F3))
                {
                    MonoKleGame.GraphicsManager.Resolution = new MPoint2(800, 600);
                }

                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.F12))
                {
                    // CRASH ON PURPOSE
                    object o = null;
                    o.Equals(o);
                }

                if (MonoKleGame.Keyboard.IsKeyPressed(Keys.N))
                {
                    Logger.Global.Log("I am logging");
                }


                ti.Update();

            }

            camera.Update(deltaTime);
            timer.Update(deltaTime);
        }

        KeyboardTextInput ti = new KeyboardTextInput(new KeyboardCharacterInput(new KeyboardTyper(MonoKleGame.Keyboard, TimeSpan.FromSeconds(0.5), TimeSpan.FromMilliseconds(50))));

        public void Test(object sender, MessageEventArgs args) => Console.WriteLine(args.Data as string);

        public void ConsoleMessage(object sender, MessageEventArgs args)
        {
            string data = args.Data as string;

            if (data.Equals("reset timer"))
            {
                timer.Reset();
            }
        }

        protected override void Activated(StateSwitchData data)
        {
            MonoKleGame.Console.WriteLine("State one activated! Message: " + (string)data.Data);
            MonoKleGame.Console.WriteLine(MonoKleGame.TextureStorage.LoadFromManifest() + " textures loaded.");
            MonoKleGame.Console.WriteLine(MonoKleGame.FontStorage.LoadFromManifest() + " fonts loaded.");
            MonoKleGame.Console.WriteLine(MonoKleGame.EffectStorage.LoadFromManifest() + " effects loaded.");
            sb = new SpriteBatch(MonoKleGame.GraphicsManager.GraphicsDevice);
            timer.Reset();
            primitive2D = new PrimitiveBatch2D(MonoKleGame.GraphicsManager.GraphicsDevice);
        }
    }
}
