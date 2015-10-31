#region Using Statements
using MonoKle;
using System;

#endregion

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
        static void Main()
        {
            using (MonoKleGame game = MonoKleGame.GetInstance())
            {
                MonoKleGame.StateManager.AddState(new DemoStateOne());
                MonoKleGame.StateManager.AddState(new DemoStateTwo());
                MonoKleGame.StateManager.SwitchState("stateOne", null);
                MonoKleGame.GraphicsManager.SetScreenSize(new MonoKle.Core.Vector2DInteger(800, 600));
                game.Run();
            }
        }
    }
}
