using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoKle;
using MonoKle.Asset.Font;
using MonoKle.Engine;
using MonoKle.Graphics;
using MonoKle.Input.Keyboard;
using MonoKle.Logging;
using MonoKle.Messaging;
using MonoKle.State;

namespace WindowsDemo {
    public class DemoStateOne : GameState {
        private SpriteBatch sb;
        private Timer timer = new Timer(TimeSpan.FromSeconds(5));
        private Camera2D camera = new Camera2D(new MPoint2(800, 600));
        private PrimitiveBatch2D primitive2D;

        public DemoStateOne() : base("stateOne") {
        }

        public override void Draw(TimeSpan deltaTime) {
            primitive2D.Begin(camera.TransformMatrix);
            primitive2D.DrawLine(new Vector2(80, 200), new Vector2(200, 80), Color.Red, Color.Blue);
            primitive2D.DrawLine(new Vector2(380, 500), new Vector2(500, 380), Color.Red, Color.Blue);
            primitive2D.End();

            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, camera.TransformMatrix);

            sb.Draw(MBackend.TextureStorage.DefaultValue, new Vector2(50, 50), Color.White);
            sb.Draw(MBackend.TextureStorage.White, new Vector2(150, 50), Color.Red);

            var testBoxRect = new MRectangleInt(250, 250, 64, 64);
            bool testBoxMouseWithin = testBoxRect.Contains(camera.TransformInv(MBackend.Mouse.Position.Value.ToMVector2()).ToMPoint2());
            sb.Draw(MBackend.TextureStorage.GetAsset("assets\\textures\\testbox.png"), testBoxRect, testBoxMouseWithin ? Color.Red : Color.White);

            Font font = MBackend.FontStorage.GetAsset("Assets\\Fonts\\testfont.mfnt");

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
                 new Vector2(50, 350), Color.Green, (float)MBackend.TotalGameTime.TotalSeconds, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Test rotation
            sb.DrawString(font, "Rotating",
                 new Vector2(0, 0), Color.Green, (float)MBackend.TotalGameTime.TotalSeconds, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Rotation with scale
            sb.DrawString(font, "Rotating scale",
                 new Vector2(350, 350), Color.Green, (float)MBackend.TotalGameTime.TotalSeconds, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            // Rotation with scale and origin
            Vector2 orig = font.MeasureString("Rotating origin scale") * 0.5f;
            sb.DrawString(font, "Rotating origin scale",
                 new Vector2(550, 150), Color.Green, (float)MBackend.TotalGameTime.TotalSeconds, orig, 2f, SpriteEffects.None, 0f);

            // XXX
            Vector2 o = font.MeasureString(ti.Text) * 0.5f;
            sb.DrawString(font, ti.Text, new Vector2(400, 300), Color.Green, (float)MBackend.TotalGameTime.TotalSeconds, o, 1f, SpriteEffects.None, 0f);

            string s = "Testin size";
            sb.DrawString(MBackend.FontStorage.DefaultValue, s, new Vector2(450, 350), Color.Green);
            sb.DrawString(MBackend.FontStorage.DefaultValue, s, new Vector2(450, 350 + MBackend.FontStorage.DefaultValue.MeasureString(s).Y), Color.Green);
            sb.DrawString(MBackend.FontStorage.DefaultValue, s, new Vector2(450, 350 + 2 * MBackend.FontStorage.DefaultValue.MeasureString(s).Y), Color.Green);

            string s2 = "Testin size\nLol";
            sb.DrawString(MBackend.FontStorage.DefaultValue, s2, new Vector2(0, 0), Color.Green);
            sb.DrawString(MBackend.FontStorage.DefaultValue, s2, new Vector2(0, 0 + MBackend.FontStorage.DefaultValue.MeasureString(s2).Y), Color.Green);
            sb.DrawString(MBackend.FontStorage.DefaultValue, s2, new Vector2(0, 0 + 2 * MBackend.FontStorage.DefaultValue.MeasureString(s2).Y), Color.Green);

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
            sb.Draw(MBackend.TextureStorage.White, new Rectangle(MBackend.Mouse.Position.Value.X, MBackend.Mouse.Position.Value.Y, 3, 3), Color.Black);
            sb.End();
        }

        public override void Update(TimeSpan deltaTime) {
            if (MBackend.Console.IsOpen == false) {
                if (MBackend.Keyboard.IsKeyHeld(Keys.Escape, TimeSpan.FromSeconds(1))) {
                    MBackend.GameInstance.Exit();
                }

                if (MBackend.Keyboard.IsKeyPressed(Keys.Space)) {
                    MBackend.StateSystem.SwitchState("stateTwo", null);
                }

                if (MBackend.Keyboard.AreKeysHeld(new Keys[] { Keys.R, Keys.T }, MonoKle.Input.CollectionQueryBehavior.All)) {
                    MBackend.Console.WriteLine("R + T held.");
                }

                if (MBackend.Keyboard.AreKeysHeld(new Keys[] { Keys.LeftShift, Keys.RightShift }, MonoKle.Input.CollectionQueryBehavior.Any)) {
                    MBackend.Console.WriteLine("Any shift held.");
                }

                if (MBackend.Keyboard.IsKeyHeld(Keys.I)) {
                    camera.SetPosition(camera.Position + new MVector2(0, -3));
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.K)) {
                    camera.SetPosition(camera.Position + new MVector2(0, 3));
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.J)) {
                    camera.SetPosition(camera.Position + new MVector2(-3, 0));
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.L)) {
                    camera.SetPosition(camera.Position + new MVector2(3, 0));
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.U)) {
                    camera.SetRotation(camera.Rotation + 0.05f);
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.O)) {
                    camera.SetRotation(camera.Rotation - 0.05f);
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.Y)) {
                    camera.SetScale(camera.Scale + 0.01f);
                }
                if (MBackend.Keyboard.IsKeyHeld(Keys.H)) {
                    camera.SetScale(camera.Scale - 0.01f);
                }

                if (MBackend.Keyboard.IsKeyPressed(Keys.F2)) {
                    MBackend.GraphicsManager.Resolution = new MPoint2(1280, 720);
                } else if (MBackend.Keyboard.IsKeyPressed(Keys.F3)) {
                    MBackend.GraphicsManager.Resolution = new MPoint2(800, 600);
                }

                if (MBackend.Keyboard.IsKeyPressed(Keys.F12)) {
                    // CRASH ON PURPOSE
                    object o = null;
                    o.Equals(o);
                }

                if (MBackend.Keyboard.IsKeyPressed(Keys.N)) {
                    Logger.Global.Log("I am logging");
                }


                ti.Update();

            }

            camera.Update(deltaTime);
            timer.Update(deltaTime);
        }

        KeyboardTextInput ti = new KeyboardTextInput(new KeyboardCharacterInput(new KeyboardTyper(MBackend.Keyboard, TimeSpan.FromSeconds(0.5), TimeSpan.FromMilliseconds(50))));

        public void Test(object sender, MessageEventArgs args) => Console.WriteLine(args.Data as string);

        public void ConsoleMessage(object sender, MessageEventArgs args) {
            string data = args.Data as string;

            if (data.Equals("reset timer")) {
                timer.Reset();
            }
        }

        protected override void Activated(StateSwitchData data) {
            MBackend.Console.WriteLine("State one activated! Message: " + (string)data.Data);
            MBackend.Console.WriteLine(MBackend.TextureStorage.LoadFilesGroup("Assets\\Textures", true, "agroup").Successes + " textures loaded.");
            MBackend.TextureStorage.LoadFileId("Assets\\Textures\\TestBox.png", "testbox", "mygroup");
            MBackend.Console.WriteLine(MBackend.FontStorage.LoadFiles("Assets\\Fonts", true).Successes + " fonts loaded.");
            MBackend.Console.WriteLine(MBackend.EffectStorage.LoadFiles("Assets\\Effects", true).Successes + " effects loaded.");
            MBackend.Console.WriteLine(MBackend.ScriptEnvironment.LoadFiles("Scripts", true).Successes + " scripts loaded.");
            sb = new SpriteBatch(MBackend.GraphicsManager.GraphicsDevice);
            timer.Reset();
            primitive2D = new PrimitiveBatch2D(MBackend.GraphicsManager.GraphicsDevice);
        }
    }
}
