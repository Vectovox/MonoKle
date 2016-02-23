using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoKle.Engine;
using MonoKle.Graphics.Primitives;
using MonoKle.State;
using System;

namespace WindowsDemo
{
    public class DemoStateTwo : GameState
    {
        private PrimitiveBatch3D primitive3D;

        private Vector3 camPos = new Vector3(0f, 0f, 500f);

        public DemoStateTwo()
            : base("stateTwo")
        {
        }

        public override void Draw(double time)
        {
            Matrix view = Matrix.CreateLookAt(camPos, camPos + new Vector3(0f, 0f, -1f), Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                MBackend.GraphicsManager.Resolution.X / MBackend.GraphicsManager.Resolution.Y,
                1.0f, 10000.0f
                );

            this.primitive3D.Begin(view * projection);
            this.primitive3D.DrawLine(new Vector3(100, 100, 0), new Vector3(200, 300, 400), Color.Green, Color.Red);
            this.primitive3D.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, 300), Color.Green, Color.Red);
            this.primitive3D.End();
        }


        public override void Update(double seconds)
        {
            if (MBackend.Keyboard.IsKeyHeld(Keys.Escape, 1))
            {
                MBackend.GameInstance.Exit();
            }

            if (MBackend.Keyboard.IsKeyHeld(Keys.I))
            {
                camPos.Y += 5;
            }
            if (MBackend.Keyboard.IsKeyHeld(Keys.K))
            {
                camPos.Y -= 5;
            }
            if (MBackend.Keyboard.IsKeyHeld(Keys.L))
            {
                camPos.X += 5;
            }
            if (MBackend.Keyboard.IsKeyHeld(Keys.J))
            {
                camPos.X -= 5;
            }

            if (MBackend.Keyboard.IsKeyPressed(Keys.Space))
            {
                MBackend.StateSystem.SwitchState("stateOne", "HELLO!");
            }
        }

        protected override void Activated(StateSwitchData data)
        {
            Console.WriteLine("State two activated! Message: " + (string)data.Data);
            this.primitive3D = new PrimitiveBatch3D(MBackend.GraphicsManager.GraphicsDevice);
        }
    }
}