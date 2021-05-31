using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoKle;
using MonoKle.Asset;
using MonoKle.Engine;
using MonoKle.Graphics;
using MonoKle.Input.Keyboard;
using MonoKle.Logging;
using MonoKle.State;
using System;

namespace Demo.Domain
{
    public class DemoStateOne : GameState
    {
        private SpriteBatch _spriteBatch;
        private readonly Timer _timer = new Timer(TimeSpan.FromSeconds(5));
        private GameDisplay2D<DynamicCamera2D> _gameDisplay;
        private PrimitiveBatch2D _primitive2D;
        private string _stateSwitchMessage = string.Empty;
        private MVector2 _lastInversionPosition;
        private MVector2 _errorBoxPosition = new Vector2(50, 50);
        private readonly KeyboardTextInput _textInput = new KeyboardTextInput(new KeyboardCharacterInput(new KeyboardTyper(MGame.Keyboard)))
        {
            MaxLength = 50,
        };
        private bool _outlineFont = false;

        public override void Draw(TimeSpan deltaTime)
        {
            // Draw scene to render target
            MGame.GraphicsManager.GraphicsDevice.SetRenderTarget(_gameDisplay.WorldRenderTarget);
            MGame.GraphicsManager.GraphicsDevice.Clear(Color.DimGray);

            // Test primitive batch
            _primitive2D.Begin(_gameDisplay.Camera.TransformMatrix);
            _primitive2D.DrawLine(new Vector2(80, 200), new Vector2(200, 80), Color.Red, Color.Blue);
            _primitive2D.DrawLine(new Vector2(380, 500), new Vector2(500, 380), Color.Red, Color.Blue);
            _primitive2D.End();

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default,
                RasterizerState.CullCounterClockwise, null, _gameDisplay.Camera.TransformMatrix);

            // Test animation
            _spriteBatch.Draw(MGame.TextureStorage["animation"].AnimateRow(_timer.Elapsed), new Vector2(0, -20), Color.White);

            // Test textures in general
            _spriteBatch.Draw(MGame.TextureStorage.Error, new MRectangleInt(0, 50, 16, 16), Color.White);
            _spriteBatch.Draw(MGame.TextureStorage.White, new MRectangleInt(16, 16).Translate(_errorBoxPosition.ToMPoint2()), Color.Red);
            _spriteBatch.Draw(MGame.TextureStorage["orange"], new Vector2(100, 50), Color.White);
            _spriteBatch.Draw(MGame.TextureStorage["red"], new Vector2(150, 50), Color.White);
            _spriteBatch.Draw(MGame.TextureStorage["green"], new Vector2(200, 50), Color.White);
            _spriteBatch.Draw(MGame.TextureStorage["blue"], new Vector2(250, 50), Color.White);

            // Test atlasing
            _spriteBatch.Draw(MGame.TextureStorage["colorAtlas"].GetCell(0, 0), new Vector2(500, 50), Color.White);
            _spriteBatch.Draw(MGame.TextureStorage["colorAtlas"].GetCell(1, 0), new Vector2(550, 50), Color.White);
            _spriteBatch.Draw(MGame.TextureStorage["colorAtlas"].GetCell(0, 1), new Vector2(600, 50), Color.White);
            _spriteBatch.Draw(MGame.TextureStorage["colorAtlas"].GetCell(1, 1), new Vector2(650, 50), Color.White);

            FontInstance font = _outlineFont ? MGame.FontStorage["testfont_o"] : MGame.FontStorage["testfont"];

            // Test non-gesture touch
            if (MGame.TouchScreen.Touch.Press.IsHeldFor(TimeSpan.FromMilliseconds(150)))
            {
                font.Draw(_spriteBatch, $"TOUCHDOWN: {MGame.TouchScreen.Touch.Press.HeldTime}: {MGame.TouchScreen.Touch.Position.Coordinate}", MVector2.Zero, Color.White);
            }

            // Test timer
            font.Draw(_spriteBatch, "Timer: " + _timer.TimeLeft + " (" + _timer.Duration + ") Done? " + _timer.IsDone,
                new Vector2(50, 150), Color.Green);

            Vector2 DrawTextBox(string text, MVector2 position, int fontSize = 32)
            {
                var newInstance = font.WithSize(fontSize);
                var size = newInstance.Measure(text);
                _spriteBatch.Draw(MGame.TextureStorage.White, new MRectangle(position, position + size).ToMRectangleInt(), Color.Gray);
                newInstance.Draw(_spriteBatch, text, position, Color.White);
                return size;
            }

            // Test linebreak
            font.WithLinePadding(-20).Draw(_spriteBatch, "LINE break\nwith padding", new Vector2(50, 250), Color.Green);

            // Test wrapping strings
            int wrapLength = (int)_errorBoxPosition.X;
            DrawTextBox(font.Wrap("Mmm... This is a too long, default string that should be wrapped appropriately", wrapLength), new MVector2(0, 650));
            _spriteBatch.Draw(MGame.TextureStorage.White, new MRectangleInt(0, 650, wrapLength, -50), new Color(1f, 1f, 1f, 0.3f));

            DrawTextBox(font.WithSize(48).Wrap("Mmm... This is a too long, bigger string that should be wrapped appropriately", wrapLength), new MVector2(0, 1150), 48);
            _spriteBatch.Draw(MGame.TextureStorage.White, new MRectangleInt(0, 1150, wrapLength, -50), new Color(1f, 1f, 1f, 0.3f));

            DrawTextBox(font.WithSize(24).Wrap("Mmm... This is a too long, smaller string that should be wrapped appropriately", wrapLength), new MVector2(0, 1850), 24);
            _spriteBatch.Draw(MGame.TextureStorage.White, new MRectangleInt(0, 1850, wrapLength, -50), new Color(1f, 1f, 1f, 0.3f));

            // Test color changing
            static Color ColorChanger(char token, Color original) => token switch
            {
                '1' => Color.Red,
                _ => original,
            };
            font.WithSize(64).Draw(_spriteBatch, "Test changing \\1\\color\\0\\ in text.", new Vector2(500, 500), Color.Green, ColorChanger);

            // Test size
            _spriteBatch.Draw(MGame.TextureStorage.White, new MRectangleInt(500, 250, 100, 64), Color.DarkGray);
            font.WithSize(64).Draw(_spriteBatch, "Text size test", new Vector2(500, 250), Color.Green);

            // Test rotation
            font.Draw(_spriteBatch, "Rotating text",
                 new Vector2(50, 350), Color.Green, (float)MGame.TotalGameTime.TotalSeconds, Vector2.Zero);

            // Rotation with sizing change
            font.WithSize(64).Draw(_spriteBatch, "Rotating scaled text",
                 new Vector2(350, 350), Color.Green, (float)MGame.TotalGameTime.TotalSeconds, Vector2.Zero);

            // Rotation with scale and origin
            Vector2 orig = font.WithSize(64).Measure("Rotating origin scale") * 0.5f;
            font.WithSize(64).Draw(_spriteBatch, "Rotating origin scale",
                 new Vector2(550, 150), Color.Green, (float)MGame.TotalGameTime.TotalSeconds, orig);

            // Text input test
            Vector2 o = font.Measure(_textInput.Text) * 0.5f;
            font.Draw(_spriteBatch, _textInput.Text, new Vector2(000, 600), Color.Green, 0f, o);

            MGame.FontStorage.Default.Draw(_spriteBatch, _stateSwitchMessage, new Vector2(0, 700), Color.Green);

            // Test size measurements.
            var sizeTestPos = new Vector2(50, 450);
            sizeTestPos.X += DrawTextBox("One", sizeTestPos).X;
            DrawTextBox("Two", sizeTestPos);
            const string threeString = "Three";
            sizeTestPos.Y -= font.Measure(threeString).Y;
            sizeTestPos.X += DrawTextBox(threeString, sizeTestPos).X;
            sizeTestPos.X += DrawTextBox("Four", sizeTestPos, 64).X;
            DrawTextBox("Five", sizeTestPos);
            _spriteBatch.End();

            // Draw "UI"
            MGame.GraphicsManager.GraphicsDevice.SetRenderTarget(_gameDisplay.UiRenderTarget);
            MGame.GraphicsManager.GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin();
            var boxLocation = _gameDisplay.WorldToUI(_errorBoxPosition);
            font.Draw(_spriteBatch, "  <- Error box", boxLocation.ToMVector2(), Color.White);
            _spriteBatch.Draw(MGame.TextureStorage.White,
                new MRectangleInt(64, 64).Translate(_gameDisplay.UiRenderingArea.Render.Width - 64, _gameDisplay.UiRenderingArea.Render.Height - 64),
                _gameDisplay.DisplayToUI(MGame.Mouse.Position.Coordinate).X >= _gameDisplay.UiRenderingArea.Render.Width - 64 ? Color.Tan : Color.Teal);
            _spriteBatch.End();

            // Render inverting stuff 
            using RenderTarget2D _inverterRenderTarget =
                new RenderTarget2D(MGame.GraphicsManager.GraphicsDevice, MGame.GraphicsManager.ResolutionWidth, MGame.GraphicsManager.ResolutionHeight);
            MGame.GraphicsManager.GraphicsDevice.SetRenderTarget(_inverterRenderTarget);

            _spriteBatch.Begin();
            _spriteBatch.Draw(MGame.TextureStorage.White,
                new MRectangle(100, 100)
                    .PositionCenter(_lastInversionPosition)
                    .ToMRectangleInt(), new Color(1f, 1f, 1f, 0.5f));
            _spriteBatch.End();

            // Combine scene and inverting stuff to backbuffer
            MGame.GraphicsManager.GraphicsDevice.SetRenderTarget(null);
            var effect = MGame.EffectStorage["inversion"];
            effect.Parameters["inverterTexture"].SetValue(_inverterRenderTarget);
            
            _spriteBatch.Begin(effect: effect);
            _spriteBatch.Draw(_gameDisplay.WorldRenderTarget, _gameDisplay.WorldRenderingArea.Display, Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin();
            _spriteBatch.Draw(_gameDisplay.UiRenderTarget, _gameDisplay.WorldRenderingArea.Display, Color.White);
            _spriteBatch.End();

            _primitive2D.Begin();
            _primitive2D.DrawRenderingArea(_gameDisplay.WorldRenderingArea);
            _primitive2D.End();
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
                    MGame.StateSystem.SwitchState("stateTwo");
                    MGame.AudioStorage["santa"].WithPitchVariation(0.2f).Play();
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
                    _gameDisplay.Camera.Position = _gameDisplay.Camera.Position + new MVector2(0, -3);
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.K))
                {
                    _gameDisplay.Camera.Position = _gameDisplay.Camera.Position + new MVector2(0, 3);
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.J))
                {
                    _gameDisplay.Camera.Position = _gameDisplay.Camera.Position + new MVector2(-3, 0);
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.L))
                {
                    _gameDisplay.Camera.Position = _gameDisplay.Camera.Position + new MVector2(3, 0);
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.U))
                {
                    _gameDisplay.Camera.Rotation += 0.05f;
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.O))
                {
                    _gameDisplay.Camera.Rotation -= 0.05f;
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.Y))
                {
                    _gameDisplay.Camera.Scale += 0.01f;
                }
                if (MGame.Keyboard.IsKeyHeld(Keys.H))
                {
                    _gameDisplay.Camera.Scale -= 0.01f;
                }

                if (MGame.TouchScreen.Pinch.TryGetValues(out var pinchOrigin, out var pinchFactor))
                {
                    var worldCoordinate = _gameDisplay.DisplayToWorld(pinchOrigin);
                    _gameDisplay.Camera.ZoomAroundTo(worldCoordinate, pinchFactor, TimeSpan.FromMilliseconds(75));
                }
                else if (MGame.TouchScreen.Drag.TryGetDelta(out var dragDelta))
                {
                    _gameDisplay.Camera.Position -= _gameDisplay.DisplayToWorldDelta(dragDelta);
                }
                else if (MGame.TouchScreen.Tap.TryGetCoordinate(out var tapCoordinate))
                {
                    _lastInversionPosition = tapCoordinate.ToMVector2();
                }
                else if (MGame.TouchScreen.Touch.Press.IsHeldFor(TimeSpan.FromMilliseconds(500)))
                {
                    _errorBoxPosition = _gameDisplay.DisplayToWorld(MGame.TouchScreen.Touch.Position.Coordinate);
                }

                if (MGame.Mouse.Right.IsPressed)
                {
                    _errorBoxPosition = _gameDisplay.DisplayToWorld(MGame.Mouse.Position.Coordinate);
                }

                if (MGame.Keyboard.IsKeyPressed(Keys.F2))
                {
                    MGame.GraphicsManager.Resolution = new MPoint2(1280, 720);
                }
                else if (MGame.Keyboard.IsKeyPressed(Keys.F3))
                {
                    MGame.GraphicsManager.Resolution = new MPoint2(800, 600);
                }
                else if (MGame.Keyboard.IsKeyPressed(Keys.F5))
                {
                    _outlineFont = !_outlineFont;
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
                    _gameDisplay.Camera.MoveTo(MVector2.Zero, 100);
                }

                _textInput.Update();
                if (MGame.TouchScreen.Hold.IsTriggered)
                {
                    MGame.StateSystem.SwitchState("stateTwo");
                }
            }

            _gameDisplay.Camera.Update(deltaTime);
            _timer.Update(deltaTime);
        }

        protected override void BeforeFirstActivation(StateSwitchData data)
        {
            base.BeforeFirstActivation(data);
            var camera = new DynamicCamera2D(MPoint2.Zero)
            {
                MinScale = 0.3f
            };
            _gameDisplay = new GameDisplay2D<DynamicCamera2D>(MGame.GraphicsManager, camera, new MPoint2(900, 600), new MPoint2(1500, 768));
            MGame.Console.WriteLine(MGame.TextureStorage.LoadFromManifest("Data/assets.manifest") + " textures loaded.");
            MGame.Console.WriteLine(MGame.FontStorage.LoadFromManifest("Data/assets.manifest") + " fonts loaded.");
            MGame.Console.WriteLine(MGame.EffectStorage.LoadFromManifest("Data/assets.manifest") + " effects loaded.");
            MGame.Console.WriteLine(MGame.AudioStorage.LoadFromManifest("Data/assets.manifest") + " sounds loaded.");
        }

        protected override void Activated(StateSwitchData data)
        {
            _stateSwitchMessage = data.HasData ? (string)data.Data : string.Empty;
            MGame.Console.WriteLine($"State one activated! Message: {_stateSwitchMessage}");
            _spriteBatch = new SpriteBatch(MGame.GraphicsManager.GraphicsDevice);
            _timer.Reset();
            _primitive2D?.Dispose();
            _primitive2D = new PrimitiveBatch2D(MGame.GraphicsManager.GraphicsDevice);
        }
    }
}
