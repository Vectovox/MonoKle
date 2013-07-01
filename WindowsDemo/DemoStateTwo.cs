using Microsoft.Xna.Framework.Input;
using MonoKle;
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
            if (MonoKleGame.Keyboard.IsKeyHeld(Keys.Escape, 1))
            {
                MonoKleGame.GetInstance().Exit();
            }

            if (MonoKleGame.Keyboard.IsKeyPressed(Keys.Space))
            {
                MonoKleGame.StateManager.NextState = "stateOne";
            }
        }

        public override void Activated()
        {
            Console.WriteLine("State two activated!");
        }
    }
}
