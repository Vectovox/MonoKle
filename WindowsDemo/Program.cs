namespace WindowsDemo {
    using System;
    using MonoKle.Engine;

    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            using (MGame game = MBackend.Initialize()) {
                MBackend.StateSystem.AddState(new DemoStateOne());
                MBackend.StateSystem.AddState(new DemoStateTwo());
                MBackend.StateSystem.SwitchState("stateOne", null);
                game.Run();
            }
        }
    }
}