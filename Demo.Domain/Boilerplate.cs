using MonoKle.Engine;

namespace Demo.Domain
{
    public static class Boilerplate
    {
        public static void ConfigureStates()
        {
            MonoKleGame.StateSystem.AddState(new DemoStateOne());
            MonoKleGame.StateSystem.AddState(new DemoStateTwo());
            MonoKleGame.StateSystem.SwitchState("stateOne", null);
        }
    }
}
