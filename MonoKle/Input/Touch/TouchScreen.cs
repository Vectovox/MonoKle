using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoKle.Attributes;
using MonoKle.Input.Mouse;
using MoreLinq;
using System;
using System.Collections.Generic;

namespace MonoKle.Input.Touch
{
    public class TouchScreen : ITouchScreen
    {
        private readonly Dictionary<GestureType, PressAction> _pressActions = new Dictionary<GestureType, PressAction>
        {
            { GestureType.Tap, new PressAction() },
            { GestureType.DoubleTap, new PressAction() },
            { GestureType.Hold, new PressAction() },
        };

        private readonly Dictionary<GestureType, DragAction> _dragActions = new Dictionary<GestureType, DragAction>
        {
            { GestureType.FreeDrag, new DragAction() },
        };

        private IMouse _mouse;

        public TouchScreen(IMouse mouse)
        {
            _mouse = mouse;
        }

        [Variable("touch_virtual")]
        public bool VirtualTouch { get; set; }

        public IPressAction Tap => _pressActions[GestureType.Tap];

        public IPressAction DoubleTap => _pressActions[GestureType.DoubleTap];

        public IPressAction Hold => _pressActions[GestureType.Hold];

        public IDragAction Drag => _dragActions[GestureType.FreeDrag];

        public GestureType EnabledGestures
        {
            get { return TouchPanel.EnabledGestures; }
            set { TouchPanel.EnabledGestures = value; }
        }

        public void Update()
        {
            // Reset all actions
            _pressActions.Values.ForEach(action => action.Reset());
            _dragActions.Values.ForEach(action => action.Reset());

            // Set actions again if their conditions are met
            UpdateTouchInput();

            // If enabled, we set actions from mouse input as well
            if (VirtualTouch)
            {
                UpdateMouseInput();
            }
        }

        private void UpdateTouchInput()
        {
            // Iterate all available gestures and act on them
            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();

                // Update action gestures
                if (_pressActions.TryGetValue(gesture.GestureType, out var pressAction))
                {
                    pressAction.Set(gesture.Position.ToPoint());
                }
                else if (_dragActions.TryGetValue(gesture.GestureType, out var dragAction))
                {
                    dragAction.Set(gesture.Position.ToPoint(), gesture.Delta.ToPoint());
                }
            }
        }

        private void UpdateMouseInput()
        {
            if (_mouse.Left.IsHeldForOnce(TimeSpan.FromSeconds(1)))
            {
                _pressActions[GestureType.Hold].Set(_mouse.Position.Coordinate);
            }
            else if (_mouse.Left.IsPressed)
            {
                _pressActions[GestureType.Tap].Set(_mouse.Position.Coordinate);
            }

            if (_mouse.Right.IsHeldFor(TimeSpan.FromMilliseconds(50)))
            {
                _dragActions[GestureType.FreeDrag].Set(_mouse.Position.Coordinate, _mouse.Position.Delta);
            }
        }
    }
}
