using Microsoft.Xna.Framework.Input.Touch;
using MonoKle.Engine;

namespace Demo.Domain
{
    public static class Boilerplate
    {
        public static void ConfigureStates()
        {
            MGame.StateSystem.AddState("stateOne", new DemoStateOne(), false);
            MGame.StateSystem.AddState("stateTwo", new DemoStateTwo(), false);
            MGame.StateSystem.SwitchState("stateOne");
            MGame.TouchScreen.EnabledGestures = GestureType.Tap | GestureType.DoubleTap | GestureType.FreeDrag | GestureType.Hold | GestureType.Pinch;
        }
    }
}
