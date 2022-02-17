using Microsoft.Xna.Framework.Input;
using System;
using MonoKle.Configuration;

namespace MonoKle.Input.Mouse
{
    /// <summary>
    /// Class representing the current state of a mouse.
    /// </summary>
    public class Mouse : IMouse, IUpdateable
    {
        private readonly Button _left = new Button();
        private readonly Button _middle = new Button();
        private readonly InputPosition _position = new InputPosition();
        private int _previousScrollValue = 0;
        private readonly Button _right = new Button();
        private readonly Button _x1 = new Button();
        private readonly Button _x2 = new Button();

        /// <summary>
        /// Initializes a new instance of the <see cref="Mouse"/> class.
        /// </summary>
        public Mouse() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mouse"/> class.
        /// </summary>
        /// <param name="virtualMouseRegion">The virtual mouse region.</param>
        public Mouse(MRectangleInt virtualMouseRegion) => VirtualRegion = virtualMouseRegion;

        public IPressable Left => _left;
        public IPressable Middle => _middle;
        public IInputPosition Position => _position;
        public IPressable Right => _right;
        public MouseScrollDirection ScrollDirection { get; private set; } = MouseScrollDirection.None;
        public IPressable X1 => _x1;
        public IPressable X2 => _x2;
        public MRectangleInt VirtualRegion { get; set; } = new MRectangleInt();

        /// <summary>
        /// Gets or sets whether the virtual mouse is enabled.
        /// </summary>
        [CVar("mouse_isvirtual")]
        public bool IsVirtual
        {
            get;
            set;
        }

        public bool WasActivated { get; private set; }

        public IPressable GetButton(MouseButton button) => button switch
        {
            MouseButton.Left => _left,
            MouseButton.Middle => _middle,
            MouseButton.Right => _right,
            MouseButton.XButton1 => _x1,
            MouseButton.XButton2 => _x2,
            _ => throw new ArgumentException("Non supported mouse button value provided."),
        };

        public void Update(TimeSpan timeDelta)
        {
            var currentState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            // Buttons
            _left.Update(currentState.LeftButton == ButtonState.Pressed, timeDelta);
            _middle.Update(currentState.MiddleButton == ButtonState.Pressed, timeDelta);
            _right.Update(currentState.RightButton == ButtonState.Pressed, timeDelta);
            _x1.Update(currentState.XButton1 == ButtonState.Pressed, timeDelta);
            _x2.Update(currentState.XButton2 == ButtonState.Pressed, timeDelta);

            // Scroll wheel
            var scrollValue = currentState.ScrollWheelValue;
            ScrollDirection = scrollValue > _previousScrollValue ? MouseScrollDirection.Up :
                (scrollValue < _previousScrollValue ? MouseScrollDirection.Down : MouseScrollDirection.None);
            _previousScrollValue = scrollValue;

            // Mouse position
            var mousePosition = new MPoint2(currentState.X, currentState.Y);
            if (IsVirtual)
            {
                // Update the virtual mouse using the mouse movement from the region center and center the actual mouse again
                var center = VirtualRegion.Center.ToMPoint2();
                mousePosition = _position.Coordinate + (mousePosition - center);
                mousePosition = VirtualRegion.Clamp(mousePosition);
                Microsoft.Xna.Framework.Input.Mouse.SetPosition(center.X, center.Y);
            }
            _position.Update(mousePosition);

            WasActivated = _left.IsDown || _middle.IsDown || _right.IsDown || _x1.IsDown || _x2.IsDown
                || ScrollDirection != MouseScrollDirection.None || _position.Delta != MPoint2.Zero;
        }
    }
}
