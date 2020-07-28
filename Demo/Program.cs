using MonoKle.Engine;
using System;

namespace WindowsDemo
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using var game = MonoKleGame.Initialize();
            MonoKleGame.StateSystem.AddState(new DemoStateOne());
            MonoKleGame.StateSystem.AddState(new DemoStateTwo());
            MonoKleGame.StateSystem.SwitchState("stateOne", null);
            game.Run();
        }
    }
}