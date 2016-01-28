namespace WindowsDemo
{
    using MonoKle.Core.Geometry;
    using MonoKle.Engine;
    using System;

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
            using (MGame game = MBackend.Initialize(true))
            {
                MBackend.StateSystem.AddState(new DemoStateOne());
                MBackend.StateSystem.AddState(new DemoStateTwo());
                MBackend.StateSystem.SwitchState("stateOne", null);
                MBackend.GraphicsManager.SetScreenSize(new MPoint2(800, 600));
                game.Run();
            }
        }
    }
}