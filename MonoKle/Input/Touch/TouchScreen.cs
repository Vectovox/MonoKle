using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using MonoKle.Configuration;
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

        private readonly Dictionary<GestureType, PinchAction> _pinchActions = new Dictionary<GestureType, PinchAction>
        {
            { GestureType.Pinch, new PinchAction() },
        };

        private readonly IMouse _mouse;

        public TouchScreen(IMouse mouse)
        {
            _mouse = mouse;
        }

        [CVar("touch_virtual")]
        public bool VirtualTouch { get; set; }

        public IPressAction Tap => _pressActions[GestureType.Tap];

        public IPressAction DoubleTap => _pressActions[GestureType.DoubleTap];

        public IPressAction Hold => _pressActions[GestureType.Hold];

        public IDragAction Drag => _dragActions[GestureType.FreeDrag];

        public IPinchAction Pinch => _pinchActions[GestureType.Pinch];

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
            _pinchActions.Values.ForEach(action => action.Reset());

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
                else if (_pinchActions.TryGetValue(gesture.GestureType, out var pinchAction))
                {
                    // Current position
                    float currentDistance = Vector2.Distance(gesture.Position, gesture.Position2);

                    // Previous position
                    Vector2 firstPrevious = gesture.Position - gesture.Delta;
                    Vector2 secondPrevious = gesture.Position2 - gesture.Delta2;
                    float previousDistance = Vector2.Distance(firstPrevious, secondPrevious);

                    // Calculate origin and factor
                    var coordinate = (firstPrevious + secondPrevious) * 0.5f;
                    var factor = (currentDistance - previousDistance) / previousDistance;

                    pinchAction.Set(coordinate.ToPoint(), factor);
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

            if (_mouse.ScrollDirection == MouseScrollDirection.Down)
            {
                _pinchActions[GestureType.Pinch].Set(_mouse.Position.Coordinate, -0.01f);
            }
            else if (_mouse.ScrollDirection == MouseScrollDirection.Up)
            {
                _pinchActions[GestureType.Pinch].Set(_mouse.Position.Coordinate, 0.01f);
            }
        }
    }
}
