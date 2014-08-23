using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoKle;
using MonoKle.Graphics;
using MonoKle.Graphics.Primitives;
using MonoKle.Input;
using MonoKle.State;
using System;

namespace WindowsDemo
{
    public class DemoStateTwo : GameState
    {
        private PrimitiveBatch3D primitive3D;

        private Vector3 camPos = new Vector3(0f, 0f, 500f);

        public override void Draw(double time)
        {
            Matrix view = Matrix.CreateLookAt(camPos, camPos + new Vector3(0f, 0f, -1f), Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                MonoKleGame.GraphicsManager.ScreenSize.X / MonoKleGame.GraphicsManager.ScreenSize.Y,
                1.0f, 10000.0f
                );

            this.primitive3D.Begin(view * projection);
            this.primitive3D.DrawLine(new Vector3(100, 100, 0), new Vector3(200, 300, 400), Color.Green, Color.Red);
            this.primitive3D.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, 300), Color.Green, Color.Red);
            this.primitive3D.End();
        }

        public override void Update(double seconds)
        {
            if (MonoKleGame.Keyboard.IsKeyHeld(Keys.Escape, 1))
            {
                MonoKleGame.GetInstance().Exit();
            }

            if(MonoKleGame.Keyboard.IsKeyHeld(Keys.I))
            {
                camPos.Y += 5;
            }
            if(MonoKleGame.Keyboard.IsKeyHeld(Keys.K))
            {
                camPos.Y -= 5;
            }
            if(MonoKleGame.Keyboard.IsKeyHeld(Keys.L))
            {
                camPos.X += 5;
            }
            if(MonoKleGame.Keyboard.IsKeyHeld(Keys.J))
            {
                camPos.X -= 5;
            }

            if (MonoKleGame.Keyboard.IsKeyPressed(Keys.Space))
            {
                MonoKleGame.StateManager.SwitchState(new StateSwitchData("stateOne", "HELLO!"));
            }
        }

        protected override void Activated(StateSwitchData data)
        {
            Console.WriteLine("State two activated! Message: " + (string)data.Data);
            this.primitive3D = new PrimitiveBatch3D(MonoKleGame.GraphicsManager.GetGraphicsDevice());
        }
    }
}
