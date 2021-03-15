﻿using Microsoft.Xna.Framework.Input.Touch;
using MonoKle.Engine;

namespace Demo.Domain
{
    public static class Boilerplate
    {
        public static void ConfigureStates()
        {
            MGame.StateSystem.SwitchState("stateOne", new DemoStateOne());
            MGame.StateSystem.AddState("stateTwo", new DemoStateTwo());
            MGame.TouchScreen.EnabledGestures = GestureType.Tap | GestureType.DoubleTap | GestureType.FreeDrag | GestureType.Hold | GestureType.Pinch;
        }
    }
}
