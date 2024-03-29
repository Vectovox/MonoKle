﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using MonoKle.Configuration;
using MonoKle.Input.Mouse;
using System;
using System.Collections.Generic;

namespace MonoKle.Input.Touch
{
    public class TouchScreen : ITouchScreen
    {
        private readonly Dictionary<GestureType, PressAction> _pressActions = new()
        {
            { GestureType.Tap, new PressAction() },
            { GestureType.DoubleTap, new PressAction() },
            { GestureType.Hold, new PressAction() },
        };

        private readonly DragAction _dragAction = new();
        private readonly PinchAction _pinchAction = new();

        private readonly IMouse _mouse;
        private readonly TouchInput _touchInput = new();

        /// <summary>
        /// Creates and initializes a new instance of <see cref="TouchScreen"/>.
        /// </summary>
        /// <param name="mouse">The mouse to use for virtual touch screen input.</param>
        public TouchScreen(IMouse mouse) => _mouse = mouse;

        [CVar("touch_virtual")]
        public bool VirtualTouch { get; set; }

        public IPressAction Tap => _pressActions[GestureType.Tap];

        public IPressAction DoubleTap => _pressActions[GestureType.DoubleTap];

        public IPressAction Hold => _pressActions[GestureType.Hold];

        public IDragAction Drag => _dragAction;

        public IPinchAction Pinch => _pinchAction;

        public ITouchInput Touch => _touchInput;

        public GestureType EnabledGestures
        {
            get { return TouchPanel.EnabledGestures; }
            set { TouchPanel.EnabledGestures = value; }
        }

        public void Update(TimeSpan timeDelta)
        {
            // Reset all actions
            foreach (var pressAction in _pressActions)
            {
                pressAction.Value.Reset();
            }
            _dragAction.Reset();
            _pinchAction.Reset();

            if (VirtualTouch)
            {
                UpdateMouseInput(timeDelta);
            }
            else
            {
                var anyGestures = UpdateGestures();
                UpdateTouchInput(timeDelta, anyGestures);
            }
        }

        private bool UpdateGestures()
        {
            // Iterate all available gestures and act on them
            var anyGestures = false;
            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();

                // Update action gestures
                if (_pressActions.TryGetValue(gesture.GestureType, out var pressAction))
                {
                    pressAction.Set(gesture.Position.ToPoint());
                }
                else if (gesture.GestureType == GestureType.FreeDrag)
                {
                    _dragAction.Set(gesture.Position.ToPoint(), gesture.Delta.ToPoint());
                }
                else if (gesture.GestureType == GestureType.Pinch)
                {
                    // Current
                    var currentDistance = Vector2.Distance(gesture.Position, gesture.Position2);

                    // Previous
                    var previousTouchA = gesture.Position - gesture.Delta;
                    var previousTouchB = gesture.Position2 - gesture.Delta2;
                    var previousDistance = Vector2.Distance(previousTouchA, previousTouchB);

                    // Calculate origin and factor
                    var origin = (previousTouchA + previousTouchB) * 0.5f;
                    var deltaFactor = 2 * (currentDistance - previousDistance) / previousDistance;   // Multiplied by two to get the full change
                    _pinchAction.Set(origin.ToPoint(), deltaFactor);
                }

                anyGestures = true;
            }

            return anyGestures;
        }

        private void UpdateTouchInput(TimeSpan timeDelta, bool anyGestures)
        {
            // Poll the screen state
            var state = TouchPanel.GetState();

            // Iterate to check if the screen is continuously touched
            bool touched = false;

            if (!anyGestures)
            {
                foreach (TouchLocation location in state)
                {
                    if (location.State == TouchLocationState.Moved)
                    {
                        // Touched so update the data
                        touched = true;
                        _touchInput.Update(timeDelta, location.Position.ToPoint());
                        break;
                    }
                }
            }

            // Reset in case nothing was recorded
            if (!touched)
            {
                _touchInput.Reset(timeDelta);
            }
        }

        private void UpdateMouseInput(TimeSpan delta)
        {
            if (_mouse.ScrollDirection == MouseScrollDirection.Down)
            {
                _pinchAction.Set(_mouse.Position.Coordinate, -0.1f);
            }
            else if (_mouse.ScrollDirection == MouseScrollDirection.Up)
            {
                _pinchAction.Set(_mouse.Position.Coordinate, 0.1f);
            }
            else if (_mouse.Right.IsHeldFor(TimeSpan.FromMilliseconds(50)))
            {
                _dragAction.Set(_mouse.Position.Coordinate, _mouse.Position.Delta);
            }
            else if (_mouse.Left.IsHeldForOnce(TimeSpan.FromSeconds(1)))
            {
                _pressActions[GestureType.Hold].Set(_mouse.Position.Coordinate);
            }
            else if (_mouse.Left.IsPressed)
            {
                _pressActions[GestureType.Tap].Set(_mouse.Position.Coordinate);
            }
            else if (_mouse.Left.IsDown)
            {
                _touchInput.Update(delta, _mouse.Position.Coordinate);
            }
            else
            {
                _touchInput.Reset(delta);
            }
        }
    }
}
