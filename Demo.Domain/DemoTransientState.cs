using Microsoft.Xna.Framework;
using MonoKle;
using MonoKle.Engine;
using MonoKle.Graphics;
using MonoKle.State;
using System;

namespace Demo.Domain
{
    public class DemoTransientState : GameState
    {
        private GameDisplay2D<DynamicCamera2D> _gameDisplay;

        public override void Draw(TimeSpan timeDelta)
        {
            MGame.GraphicsManager.GraphicsDevice.Clear(Color.Blue);
        }

        public override void Update(TimeSpan timeDelta)
        {
            if (MGame.Keyboard.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                MGame.StateSystem.SwitchState("stateOne");
            }
        }

        protected override void BeforeFirstActivation(StateSwitchData data)
        {
            _gameDisplay = new GameDisplay2D<DynamicCamera2D>(MGame.GraphicsManager, new DynamicCamera2D(MPoint2.Zero), new MPoint2(900, 600), new MPoint2(1500, 768));
        }
    }
}
