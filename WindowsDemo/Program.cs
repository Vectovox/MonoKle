#region Using Statements
using Microsoft.Xna.Framework;
using MonoKle;
using MonoKle.State;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace WindowsDemo
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        private static Game game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            StateManager.AddState("stateOne", new DemoStateOne());
            StateManager.AddState("stateTwo", new DemoStateTwo());
            StateManager.NextState = "stateOne";
            game = MonoKleGame.GetInstance();
            game.Run();
        }
    }
}
