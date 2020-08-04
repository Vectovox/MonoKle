using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoKle.Attributes;
using MoreLinq;
using System;
using System.Collections.Generic;

namespace MonoKle.Input.Touch
{
    public class TouchScreen : ITouchScreen
    {
        private readonly Dictionary<GestureType, TouchAction> _actionDictionary = new Dictionary<GestureType, TouchAction>
        {
            { GestureType.Tap, new TouchAction() },
            { GestureType.DoubleTap, new TouchAction() },
            { GestureType.Hold, new TouchAction() }
        };

        private readonly Button _mouseButton = new Button();

        [PropertyVariable("t_virtual_touch")]
        public bool VirtualTouch { get; set; }

        public ITouchAction Tap => GetActionInternal(GestureType.Tap);

        public ITouchAction DoubleTap => GetActionInternal(GestureType.DoubleTap);

        public ITouchAction Hold => GetActionInternal(GestureType.Hold);

        public ITouchAction GetAction(GestureType gestureType) => GetActionInternal(gestureType);

        private TouchAction GetActionInternal(GestureType type) => _actionDictionary.TryGetValue(type, out var action)
            ? action
            : throw new InvalidOperationException($"The touch action {type} is not supported yet.");

        public void Update(TimeSpan timeDelta)
        {
            // Reset all actions
            _actionDictionary.Values.ForEach(action => action.Reset());

            // Set actions again if their conditions are met
            UpdateTouchInput();

            // If enabled, we set actions from mouse input as well
            if (VirtualTouch)
            {
                UpdateMouseInput(timeDelta);
            }
        }

        private void UpdateTouchInput()
        {
            // Iterate all available gestures and act on them
            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();

                // Update action gestures
                if (_actionDictionary.TryGetValue(gesture.GestureType, out var action))
                {
                    action.Set(gesture.Position.ToPoint());
                }
            }
        }

        private void UpdateMouseInput(TimeSpan timeDelta)
        {
            MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            _mouseButton.Update(mouseState.LeftButton == ButtonState.Pressed, timeDelta);

            if (_mouseButton.IsHeldForOnce(TimeSpan.FromSeconds(1)))
            {
                GetActionInternal(GestureType.Hold).Set(mouseState.Position);
            }
            else if (_mouseButton.IsPressed)
            {
                GetActionInternal(GestureType.Tap).Set(mouseState.Position);
            }
        }
    }
}
