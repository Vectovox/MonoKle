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
        private Camera2D camera = new Camera2D(new MPoint2(800, 600)) { MinScale = 0.5f };
        private PrimitiveBatch2D primitive2D;
        private string stateSwitchMessage = string.Empty;

        public DemoStateOne() : base("stateOne")
        {
        }

        public class AnimationInstance
        {
            Texture2D Texture;
            public int framesPerSecond;
            public int frameSize;

            public void GetSource()
            {
                // TODO:
                // Gör manifest till 1st class citizen
                // # FIL #
                // myPony textures/atlas2.png 50,50,500,50 10 20 
                //
                // # KOD #
                // TextureStorage.GetTexture("myPony") ->
                // { Texture2D 'atlas2', SourceRectangle '50,50,500,50', Frames: 10, FPS, 20 } 
                //    .Animate(frameNumber)
                //    .Animate(timeSpent)
                // { Texture2D 'atlas2', SourceRectangle 250, 50, 50, 50}
                //
                // 
            }
        }

        public override void Draw(TimeSpan deltaTime)
        {
            primitive2D.Begin(camera.TransformMatrix);
            primitive2D.DrawLine(new Vector2(80, 200), new Vector2(200, 80), Color.Red, Color.Blue);
            primitive2D.DrawLine(new Vector2(380, 500), new Vector2(500, 380), Color.Red, Color.Blue);
            primitive2D.End();

            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, camera.TransformMatrix);

            sb.Draw(MGame.TextureStorage.Error, new Vector2(50, 50), Color.White);
            sb.Draw(MGame.TextureStorage.White, new Vector2(150, 50), Color.Red);

            var testBoxRect = new MRectangleInt(250, 250, 64, 64);
            bool testBoxMouseWithin = testBoxRect.Contains(camera.TransformInv(MGame.Mouse.Position.Coordinate.ToMVector2()).ToMPoint2());
            sb.Draw(MGame.TextureStorage["testbox"].Texture, testBoxRect, testBoxMouseWithin ? Color.Red : Color.White);

            sb.Draw(MGame.TextureStorage["animation"].Texture, new Vector2(340, 0), Color.White);

            // TODO: Overload for sb that can take in a MTexture and do this automatically
            sb.Draw(MGame.TextureStorage["orange"].Texture, new Vector2(540, 0), MGame.TextureStorage["orange"].AtlasRectangle, Color.White);
            sb.Draw(MGame.TextureStorage["red"].Texture, new Vector2(640, 0), MGame.TextureStorage["red"].AtlasRectangle, Color.White);
            sb.Draw(MGame.TextureStorage["green"].Texture, new Vector2(640, -50), MGame.TextureStorage["green"].AtlasRectangle, Color.White);
            sb.Draw(MGame.TextureStorage["blue"].Texture, new Vector2(690, -100), MGame.TextureStorage["blue"].AtlasRectangle, Color.White);

            Font font = MGame.FontStorage.GetAsset("data/Fonts/testfont.mfnt");

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
                 new Vector2(50, 350), Color.Green, (float)MGame.TotalGameTime.TotalSeconds, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Test rotation
            sb.DrawString(font, "Rotating",
                 new Vector2(0, 0), Color.Green, (float)MGame.TotalGameTime.TotalSeconds, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Rotation with scale
            sb.DrawString(font, "Rotating scale",
                 new Vector2(350, 350), Color.Green, (float)MGame.TotalGameTime.TotalSeconds, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            // Rotation with scale and origin
            Vector2 orig = font.MeasureString("Rotating origin scale") * 0.5f;
            sb.DrawString(font, "Rotating origin scale",
                 new Vector2(550, 150), Color.Green, (float)MGame.TotalGameTime.TotalSeconds, orig, 2f, SpriteEffects.None, 0f);

            // XXX
            Vector2 o = font.MeasureString(ti.Text) * 0.5f;
            sb.DrawString(font, ti.Text, new Vector2(400, 300), Color.Green, (float)MGame.TotalGameTime.TotalSeconds, o, 1f, SpriteEffects.None, 0f);

            string s = "Testin size";
            sb.DrawString(MGame.FontStorage.DefaultValue, s, new Vector2(450, 350), Color.Green);
            sb.DrawString(MGame.FontStorage.DefaultValue, s, new Vector2(450, 350 + MGame.FontStorage.DefaultValue.MeasureString(s).Y), Color.Green);
            sb.DrawString(MGame.FontStorage.DefaultValue, s, new Vector2(450, 350 + 2 * MGame.FontStorage.DefaultValue.MeasureString(s).Y), Color.Green);

            string s2 = "Testin size\nLol";
            sb.DrawString(MGame.FontStorage.DefaultValue, s2, new Vector2(0, 0), Color.Green);
            sb.DrawString(MGame.FontStorage.DefaultValue, s2, new Vector2(0, 0 + MGame.FontStorage.DefaultValue.MeasureString(s2).Y), Color.Green);
            sb.DrawString(MGame.FontStorage.DefaultValue, s2, new Vector2(0, 0 + 2 * MGame.FontStorage.DefaultValue.MeasureString(s2).Y), Color.Green);

            sb.DrawString(MGame.FontStorage.DefaultValue, stateSwitchMessage, new Vector2(0, 700), Color.Green);

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
            sb.Draw(MGame.TextureStorage.White, new Rectangle(MGame.Mouse.Position.Coordinate.X, MGame.Mouse.Position.Coordinate.Y, 3, 3), Color.Black);
            sb.End();
        }

        public override void Update(TimeSpan deltaTime)
        {
            if (MGame.Console.IsOpen == false)
            {
                if (MGame.Keyboard.IsKeyHeld(Keys.Escape, TimeSpan.FromSeconds(1)))
                {
                    MGame.GameInstance.Exit();
                }

                if (MGame.Keyboard.IsKeyPressed(Keys.Space))
                {
                    MGame.StateSystem.SwitchState("stateTwo", null);
                    MGame.AudioStorage.GetAsset("Data/Sounds/santa.wav").Play(1f, 0f, 0f);
                }

                if (MGame.Keyboard.AreKeysHeld(new Keys[] { Keys.R, Keys.T }, MonoKle.Input.CollectionQueryBehavior.All))
                {
                    MGame.Console.WriteLine("R + T held.");
                }

                if (MGame.Keyboard.AreKeysHeld(new Keys[] { Keys.LeftShift, Keys.RightShift }, MonoKle.Input.CollectionQueryBehavior.Any))
                {
                    MGame.Console.WriteLine("Any shift held.");
                }

                if (MGame.Keyboard.IsKeyHeld(Keys.I))
                {
                    camera.SetPosition(camera.Position + new MVector2(0, -3));
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.K))
                {
                    camera.SetPosition(camera.Position + new MVector2(0, 3));
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.J))
                {
                    camera.SetPosition(camera.Position + new MVector2(-3, 0));
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.L))
                {
                    camera.SetPosition(camera.Position + new MVector2(3, 0));
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.U))
                {
                    camera.SetRotation(camera.Rotation + 0.05f);
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.O))
                {
                    camera.SetRotation(camera.Rotation - 0.05f);
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.Y))
                {
                    camera.SetScale(camera.Scale + 0.01f);
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.H))
                {
                    camera.SetScale(camera.Scale - 0.01f);
                }

                if (MGame.TouchScreen.Pinch.TryGetValues(out var pinchOrigin, out var pinchFactor))
                {
                    camera.ScaleTo(pinchOrigin.ToMVector2(), pinchFactor * 10);
                }
                else if (MGame.TouchScreen.Drag.TryGetDelta(out var dragDelta))
                {
                    camera.TranslateCameraSpace(-dragDelta.ToMVector2());
                }

                if (MGame.Keyboard.IsKeyPressed(Keys.F2))
                {
                    MGame.GraphicsManager.Resolution = new MPoint2(1280, 720);
                }
                else if (MGame.Keyboard.IsKeyPressed(Keys.F3))
                {
                    MGame.GraphicsManager.Resolution = new MPoint2(800, 600);
                }

                if (MGame.Keyboard.IsKeyPressed(Keys.F12))
                {
                    // CRASH ON PURPOSE
                    object o = null;
                    o.Equals(o);
                }

                if (MGame.Keyboard.IsKeyPressed(Keys.N))
                {
                    Logger.Global.Log("I am logging");
                }

                if (MGame.Keyboard.IsKeyPressed(Keys.R))
                {
                    camera.SetPosition(MVector2.Zero, 100);
                }

                ti.Update();
                if (MGame.TouchScreen.Hold.IsTriggered)
                {
                    MGame.StateSystem.SwitchState("stateTwo", null);
                }
            }

            camera.Update(deltaTime);
            timer.Update(deltaTime);
        }

        KeyboardTextInput ti = new KeyboardTextInput(new KeyboardCharacterInput(new KeyboardTyper(MGame.Keyboard, TimeSpan.FromSeconds(0.5), TimeSpan.FromMilliseconds(50))));

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
            stateSwitchMessage = (string)data.Data ?? string.Empty;
            MGame.Console.WriteLine($"State one activated! Message: {stateSwitchMessage}");
            MGame.Console.WriteLine(MGame.TextureStorage.LoadFromManifest("Data/assets.manifest") + " textures loaded.");
            MGame.Console.WriteLine(MGame.FontStorage.LoadFromManifest("Data/assets.manifest") + " fonts loaded.");
            MGame.Console.WriteLine(MGame.EffectStorage.LoadFromManifest("Data/assets.manifest") + " effects loaded.");
            MGame.Console.WriteLine(MGame.AudioStorage.LoadFromManifest("Data/assets.manifest") + " sounds loaded.");
            sb = new SpriteBatch(MGame.GraphicsManager.GraphicsDevice);
            timer.Reset();
            primitive2D = new PrimitiveBatch2D(MGame.GraphicsManager.GraphicsDevice);
        }
    }
}
