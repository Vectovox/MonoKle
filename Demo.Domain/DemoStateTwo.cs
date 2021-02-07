using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoKle;
using MonoKle.Asset;
using MonoKle.Engine;
using MonoKle.Graphics;
using MonoKle.State;
using System;

namespace Demo.Domain
{
    public class DemoStateTwo : GameState
    {
        private PrimitiveBatch3D _primitive3D;
        private PrimitiveBatch2D _primitiveBatch2D;
        private SpriteBatch _spriteBatch;

        private Vector3 _camPos = new Vector3(0f, 0f, 500f);
        private RenderingArea2D _renderingArea;
        private string _tapString = "";

        public DemoStateTwo()
            : base("stateTwo")
        {
        }

        public override void Draw(TimeSpan time)
        {
            var view = Matrix.CreateLookAt(_camPos, _camPos + new Vector3(0f, 0f, -1f), Vector3.Up);
            var projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                MGame.GraphicsManager.GraphicsDevice.PresentationParameters.BackBufferWidth / MGame.GraphicsManager.GraphicsDevice.PresentationParameters.BackBufferHeight,
                1.0f, 10000.0f
                );

            _primitive3D.Begin(view * projection);
            _primitive3D.DrawLine(new Vector3(100, 100, 0), new Vector3(200, 300, 400), Color.Green, Color.Red);
            _primitive3D.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, 300), Color.Green, Color.Red);
            _primitive3D.End();

            _primitiveBatch2D.Begin();
            _primitiveBatch2D.DrawRenderingArea(_renderingArea);
            _primitiveBatch2D.End();

            _spriteBatch.Begin();
            MGame.FontStorage.Default.Draw(_spriteBatch, _tapString, Vector2.Zero, Color.White);
            _spriteBatch.End();
        }

        public override void Update(TimeSpan time)
        {
            if (MGame.Keyboard.IsKeyHeld(Keys.Escape, TimeSpan.FromSeconds(1)))
            {
                MGame.GameInstance.Exit();
            }

            if (MGame.Keyboard.IsKeyHeld(Keys.I))
            {
                _camPos.Y += 5;
            }
            if (MGame.Keyboard.IsKeyHeld(Keys.K))
            {
                _camPos.Y -= 5;
            }
            if (MGame.Keyboard.IsKeyHeld(Keys.L))
            {
                _camPos.X += 5;
            }
            if (MGame.Keyboard.IsKeyHeld(Keys.J))
            {
                _camPos.X -= 5;
            }

            if (MGame.Keyboard.IsKeyPressed(Keys.Space))
            {
                MGame.StateSystem.SwitchState("stateOne", "HELLO!");
            }

            if (MGame.TouchScreen.Tap.TryGetCoordinate(out var tapCoordinate))
            {
                _tapString = tapCoordinate.ToString();
            }

            if (MGame.TouchScreen.Hold.TryGetCoordinate(out var holdCoordinate))
            {
                MGame.StateSystem.SwitchState("stateOne", $"You switched on: {holdCoordinate}");
            }
        }

        protected override void Activated(StateSwitchData data)
        {
            Console.WriteLine("State two activated! Message: " + (string)data.Data);
            _primitive3D = new PrimitiveBatch3D(MGame.GraphicsManager.GraphicsDevice);
            _spriteBatch = new SpriteBatch(MGame.GraphicsManager.GraphicsDevice);
            _primitiveBatch2D = new PrimitiveBatch2D(MGame.GraphicsManager.GraphicsDevice);
            _renderingArea = new RenderingArea2D(new MPoint2(320, 200), MGame.GraphicsManager.Resolution);
            MGame.GraphicsManager.ResolutionChanged += GraphicsManager_ResolutionChanged;
        }

        private void GraphicsManager_ResolutionChanged(object sender, ResolutionChangedEventArgs e)
        {
            _renderingArea = new RenderingArea2D(new MPoint2(320, 200), MGame.GraphicsManager.Resolution);
        }
    }
}
