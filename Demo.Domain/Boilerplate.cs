using Microsoft.Xna.Framework.Input.Touch;
using MonoKle.Engine;

namespace Demo.Domain
{
    public static class Boilerplate
    {
        public static void ConfigureStates()
        {
            MGame.StateSystem.AddState(new DemoStateOne());
            MGame.StateSystem.AddState(new DemoStateTwo());
            MGame.StateSystem.SwitchState("stateOne", null);
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap | GestureType.FreeDrag | GestureType.Hold;
        }
    }
}
