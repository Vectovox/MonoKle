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
        private SpriteBatch _spriteBatch;
        private Timer _timer = new Timer(TimeSpan.FromSeconds(5));
        private Camera2D _camera = new Camera2D(new MPoint2(800, 600)) { MinScale = 0.3f };
        private PrimitiveBatch2D _primitive2D;
        private string _stateSwitchMessage = string.Empty;

        public DemoStateOne() : base("stateOne") { }

        public override void Draw(TimeSpan deltaTime)
        {
            _primitive2D.Begin(_camera.TransformMatrix);
            _primitive2D.DrawLine(new Vector2(80, 200), new Vector2(200, 80), Color.Red, Color.Blue);
            _primitive2D.DrawLine(new Vector2(380, 500), new Vector2(500, 380), Color.Red, Color.Blue);
            _primitive2D.End();

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, _camera.TransformMatrix);


            _spriteBatch.Draw(MGame.TextureStorage["animation"].Animate(_timer.Elapsed), new Vector2(0, -20), Color.White);

            _spriteBatch.Draw(MGame.TextureStorage.Error, new Vector2(0, 50), Color.White);
            _spriteBatch.Draw(MGame.TextureStorage.White, new Vector2(50, 50), Color.Red);
            _spriteBatch.Draw(MGame.TextureStorage["orange"], new Vector2(100, 50), Color.White);
            _spriteBatch.Draw(MGame.TextureStorage["red"], new Vector2(150, 50), Color.White);
            _spriteBatch.Draw(MGame.TextureStorage["green"], new Vector2(200, 50), Color.White);
            _spriteBatch.Draw(MGame.TextureStorage["blue"], new Vector2(250, 50), Color.White);

            Font font = MGame.FontStorage.GetAsset("data/Fonts/testfont.mfnt");

            // Test timer
            _spriteBatch.DrawString(font, "Timer: " + _timer.TimeLeft + " (" + _timer.Duration + ") Done? " + _timer.IsDone,
                new Vector2(50, 150), Color.Green);

            // Test linebreak
            _spriteBatch.DrawString(font, "LINE\nbreak",
                 new Vector2(50, 250), Color.Green);

            // Test scale
            _spriteBatch.DrawString(font, "Scaled text",
                 new Vector2(250, 250), Color.Green, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            // Test rotation
            _spriteBatch.DrawString(font, "Rotating text",
                 new Vector2(50, 350), Color.Green, (float)MGame.TotalGameTime.TotalSeconds, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Rotation with scale
            _spriteBatch.DrawString(font, "Rotating scaled text",
                 new Vector2(350, 350), Color.Green, (float)MGame.TotalGameTime.TotalSeconds, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            // Rotation with scale and origin
            Vector2 orig = font.MeasureString("Rotating origin scale") * 0.5f;
            _spriteBatch.DrawString(font, "Rotating origin scale",
                 new Vector2(550, 150), Color.Green, (float)MGame.TotalGameTime.TotalSeconds, orig, 2f, SpriteEffects.None, 0f);

            // Text input test
            Vector2 o = font.MeasureString(_textInput.Text) * 0.5f;
            _spriteBatch.DrawString(font, _textInput.Text, new Vector2(000, 600), Color.Green, 0f, o, 1f, SpriteEffects.None, 0f);

            _spriteBatch.DrawString(MGame.FontStorage.DefaultValue, _stateSwitchMessage, new Vector2(0, 700), Color.Green);

            // Test size measurements.
            var pos = new Vector2(50, 450);
            _spriteBatch.DrawString(font, "One-", pos, Color.Green);
            pos.X += font.MeasureString("One-").X;

            _spriteBatch.DrawString(font, "Two", pos, Color.Red);

            pos.Y -= font.MeasureString("Three").Y;
            _spriteBatch.DrawString(font, "Three", pos, Color.Orange);

            pos.X += font.MeasureString("Three").X;
            _spriteBatch.DrawString(font, "Four", pos, Color.Blue, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            pos.X += font.MeasureString("Four", 2f).X;
            _spriteBatch.DrawString(font, "Five", pos, Color.Black);

            _spriteBatch.End();

            _spriteBatch.Begin();
            _spriteBatch.Draw(MGame.TextureStorage.White, new Rectangle(MGame.Mouse.Position.Coordinate.X, MGame.Mouse.Position.Coordinate.Y, 3, 3), Color.Black);
            _spriteBatch.End();
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
                    _camera.SetPosition(_camera.Position + new MVector2(0, -3));
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.K))
                {
                    _camera.SetPosition(_camera.Position + new MVector2(0, 3));
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.J))
                {
                    _camera.SetPosition(_camera.Position + new MVector2(-3, 0));
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.L))
                {
                    _camera.SetPosition(_camera.Position + new MVector2(3, 0));
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.U))
                {
                    _camera.SetRotation(_camera.Rotation + 0.05f);
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.O))
                {
                    _camera.SetRotation(_camera.Rotation - 0.05f);
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.Y))
                {
                    _camera.SetScale(_camera.Scale + 0.01f);
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.H))
                {
                    _camera.SetScale(_camera.Scale - 0.01f);
                }

                if (MGame.TouchScreen.Pinch.TryGetValues(out var pinchOrigin, out var pinchFactor))
                {
                    _camera.ScaleTo(pinchOrigin.ToMVector2(), pinchFactor * 10);
                }
                else if (MGame.TouchScreen.Drag.TryGetDelta(out var dragDelta))
                {
                    _camera.TranslateCameraSpace(-dragDelta.ToMVector2());
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
                    _camera.SetPosition(MVector2.Zero, 100);
                }

                _textInput.Update();
                if (MGame.TouchScreen.Hold.IsTriggered)
                {
                    MGame.StateSystem.SwitchState("stateTwo", null);
                }
            }

            _camera.Update(deltaTime);
            _timer.Update(deltaTime);
        }

        KeyboardTextInput _textInput = new KeyboardTextInput(new KeyboardCharacterInput(new KeyboardTyper(MGame.Keyboard, TimeSpan.FromSeconds(0.5), TimeSpan.FromMilliseconds(50))));

        public void Test(object sender, MessageEventArgs args) => Console.WriteLine(args.Data as string);

        public void ConsoleMessage(object sender, MessageEventArgs args)
        {
            string data = args.Data as string;

            if (data.Equals("reset timer"))
            {
                _timer.Reset();
            }
        }

        protected override void Activated(StateSwitchData data)
        {
            _stateSwitchMessage = (string)data.Data ?? string.Empty;
            MGame.Console.WriteLine($"State one activated! Message: {_stateSwitchMessage}");
            MGame.Console.WriteLine(MGame.TextureStorage.LoadFromManifest("Data/assets.manifest") + " textures loaded.");
            MGame.Console.WriteLine(MGame.FontStorage.LoadFromManifest("Data/assets.manifest") + " fonts loaded.");
            MGame.Console.WriteLine(MGame.EffectStorage.LoadFromManifest("Data/assets.manifest") + " effects loaded.");
            MGame.Console.WriteLine(MGame.AudioStorage.LoadFromManifest("Data/assets.manifest") + " sounds loaded.");
            _spriteBatch = new SpriteBatch(MGame.GraphicsManager.GraphicsDevice);
            _timer.Reset();
            _primitive2D = new PrimitiveBatch2D(MGame.GraphicsManager.GraphicsDevice);
        }
    }
}
