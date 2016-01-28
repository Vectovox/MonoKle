#region Using Statements
using MonoKle;
using MonoKle.Core.Geometry;
using MonoKle.Engine;
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
            using (MGame game = MBackend.Initialize(true))
            {
                MBackend.StateManager.AddState(new DemoStateOne());
                MBackend.StateManager.AddState(new DemoStateTwo());
                MBackend.StateManager.SwitchState("stateOne", null);
                MBackend.GraphicsManager.SetScreenSize(new MPoint2(800, 600));
                game.Run();
            }
        }
    }
}
