using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoKle.Engine;
using MonoKle.Graphics;
using MonoKle.State;
using System;

namespace Demo.Domain
{
    public class DemoStateTwo : GameState
    {
        private PrimitiveBatch3D primitive3D;

        private Vector3 camPos = new Vector3(0f, 0f, 500f);

        public DemoStateTwo()
            : base("stateTwo")
        {
        }

        public override void Draw(TimeSpan time)
        {
            var view = Matrix.CreateLookAt(camPos, camPos + new Vector3(0f, 0f, -1f), Vector3.Up);
            var projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                MonoKleGame.GraphicsManager.Resolution.X / MonoKleGame.GraphicsManager.Resolution.Y,
                1.0f, 10000.0f
                );

            primitive3D.Begin(view * projection);
            primitive3D.DrawLine(new Vector3(100, 100, 0), new Vector3(200, 300, 400), Color.Green, Color.Red);
            primitive3D.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, 300), Color.Green, Color.Red);
            primitive3D.End();
        }


        public override void Update(TimeSpan time)
        {
            if (MonoKleGame.Keyboard.IsKeyHeld(Keys.Escape, TimeSpan.FromSeconds(1)))
            {
                MonoKleGame.GameInstance.Exit();
            }

            if (MonoKleGame.Keyboard.IsKeyHeld(Keys.I))
            {
                camPos.Y += 5;
            }
            if (MonoKleGame.Keyboard.IsKeyHeld(Keys.K))
            {
                camPos.Y -= 5;
            }
            if (MonoKleGame.Keyboard.IsKeyHeld(Keys.L))
            {
                camPos.X += 5;
            }
            if (MonoKleGame.Keyboard.IsKeyHeld(Keys.J))
            {
                camPos.X -= 5;
            }

            if (MonoKleGame.Keyboard.IsKeyPressed(Keys.Space))
            {
                MonoKleGame.StateSystem.SwitchState("stateOne", "HELLO!");
            }
        }

        protected override void Activated(StateSwitchData data)
        {
            Console.WriteLine("State two activated! Message: " + (string)data.Data);
            primitive3D = new PrimitiveBatch3D(MonoKleGame.GraphicsManager.GraphicsDevice);
        }
    }
}
