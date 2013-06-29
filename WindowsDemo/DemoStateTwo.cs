using Microsoft.Xna.Framework.Input;
using MonoKle.Input;
using MonoKle.State;
using System;

namespace WindowsDemo
{
    public class DemoStateTwo : GameState
    {
        public override void Draw(double time)
        {
        }

        public override void Update(double seconds)
        {
            if (KeyboardInput.IsKeyHeld(Keys.Escape, 1))
            {
                MonoKle.MonoKleGame.GetInstance().Exit();
            }

            if (KeyboardInput.IsKeyPressed(Keys.Space))
            {
                StateManager.NextState = "stateOne";
            }
        }

        public override void Activated()
        {
            Console.WriteLine("State two activated!");
        }
    }
}
