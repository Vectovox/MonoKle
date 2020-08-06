using MonoKle.Attributes;
using Microsoft.Xna.Framework.Input;
using System;

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

        /// <summary>
        /// Gets the left button.
        /// </summary>
        /// <value>
        /// The left button.
        /// </value>
        public IPressable Left => _left;

        /// <summary>
        /// Gets the middle button.
        /// </summary>
        /// <value>
        /// The middle button.
        /// </value>
        public IPressable Middle => _middle;

        /// <summary>
        /// Gets the position relative the upper left corner of the game window.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public IInputPosition Position => _position;

        /// <summary>
        /// Gets the right button.
        /// </summary>
        /// <value>
        /// The right button.
        /// </value>
        public IPressable Right => _right;

        /// <summary>
        /// Gets the scroll direction.
        /// </summary>
        /// <value>
        /// The scroll direction.
        /// </value>
        public MouseScrollDirection ScrollDirection { get; private set; } = MouseScrollDirection.None;

        /// <summary>
        /// Gets or sets whether the virtual mouse is enabled.
        /// </summary>
        [Variable("mouse_isvirtual")]
        public bool IsVirtual
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the region of the screen used for the virtual mouse.
        /// </summary>
        /// <value>
        /// The virtual mouse region.
        /// </value>
        public MRectangleInt VirtualRegion { get; set; } = new MRectangleInt();

        /// <summary>
        /// Gets the first extra button.</summary>
        /// <value>
        /// The first extra button.
        /// </value>
        public IPressable X1 => _x1;

        /// <summary>
        /// Gets the second extra button.</summary>
        /// <value>
        /// The second extra button.
        /// </value>
        public IPressable X2 => _x2;

        /// <summary>
        /// Gets the provided mouse button.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Non supported mouse button value provided.</exception>
        public IPressable GetButton(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return _left;

                case MouseButton.Middle:
                    return _middle;

                case MouseButton.Right:
                    return _right;

                case MouseButton.XButton1:
                    return _x1;

                case MouseButton.XButton2:
                    return _x2;

                default:
                    throw new ArgumentException("Non supported mouse button value provided.");
            }
        }

        public void Update(TimeSpan timeDelta)
        {
            MouseState currentState = Microsoft.Xna.Framework.Input.Mouse.GetState();

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
        }
    }
}
