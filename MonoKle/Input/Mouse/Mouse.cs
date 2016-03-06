namespace MonoKle.Input.Mouse
{
    using Microsoft.Xna.Framework.Input;
    using Attributes;
    using System;

    /// <summary>
    /// Class representing the current state of a mouse.
    /// </summary>
    public class Mouse : IMouse, IUpdateable
    {
        private Button left = new Button();
        private Button middle = new Button();
        private InputPosition position = new InputPosition();
        private int previousScrollValue = 0;
        private Button right = new Button();
        private MouseScrollDirection scrollDirection = MouseScrollDirection.None;
        private MRectangleInt virtualRegion = new MRectangleInt();
        private Button x1 = new Button();
        private Button x2 = new Button();

        /// <summary>
        /// Initializes a new instance of the <see cref="Mouse"/> class.
        /// </summary>
        public Mouse() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mouse"/> class.
        /// </summary>
        /// <param name="virtualMouseRegion">The virtual mouse region.</param>
        public Mouse(MRectangleInt virtualMouseRegion) : this()
        {
            this.virtualRegion = virtualMouseRegion;
        }

        /// <summary>
        /// Gets the left button.
        /// </summary>
        /// <value>
        /// The left button.
        /// </value>
        public IPressable Left => this.left;

        /// <summary>
        /// Gets the middle button.
        /// </summary>
        /// <value>
        /// The middle button.
        /// </value>
        public IPressable Middle => this.middle;

        /// <summary>
        /// Gets the position relative the upper left corner of the game window.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public IInputPosition Position => this.position;

        /// <summary>
        /// Gets the right button.
        /// </summary>
        /// <value>
        /// The right button.
        /// </value>
        public IPressable Right => this.right;

        /// <summary>
        /// Gets the scroll direction.
        /// </summary>
        /// <value>
        /// The scroll direction.
        /// </value>
        public MouseScrollDirection ScrollDirection => this.scrollDirection;

        /// <summary>
        /// Gets or sets whether the virtual mouse is enabled.
        /// </summary>
        [PropertyVariable("mouse_isvirtual")]
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
        public MRectangleInt VirtualRegion { get { return this.virtualRegion; } set { this.virtualRegion = value; } }

        /// <summary>
        /// Gets the first extra button.</summary>
        /// <value>
        /// The first extra button.
        /// </value>
        public IPressable X1 => this.x1;

        /// <summary>
        /// Gets the second extra button.</summary>
        /// <value>
        /// The second extra button.
        /// </value>
        public IPressable X2 => this.x2;

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
                    return this.left;

                case MouseButton.Middle:
                    return this.middle;

                case MouseButton.Right:
                    return this.right;

                case MouseButton.XButton1:
                    return this.x1;

                case MouseButton.XButton2:
                    return this.x2;

                default:
                    throw new ArgumentException("Non supported mouse button value provided.");
            }
        }

        /// <summary>
        /// Updates the component with the specified seconds since last update.
        /// </summary>
        /// <param name="seconds">The amount of seconds since last update.</param>
        public void Update(double seconds)
        {
            MouseState currentState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            // Buttons
            this.left.Update(currentState.LeftButton == ButtonState.Pressed, seconds);
            this.middle.Update(currentState.MiddleButton == ButtonState.Pressed, seconds);
            this.right.Update(currentState.RightButton == ButtonState.Pressed, seconds);
            this.x1.Update(currentState.XButton1 == ButtonState.Pressed, seconds);
            this.x2.Update(currentState.XButton2 == ButtonState.Pressed, seconds);

            // Scroll wheel
            var scrollValue = currentState.ScrollWheelValue;
            this.scrollDirection = scrollValue > previousScrollValue ? MouseScrollDirection.Up :
                (scrollValue < previousScrollValue ? MouseScrollDirection.Down : MouseScrollDirection.None);
            this.previousScrollValue = scrollValue;

            // Mouse position
            MPoint2 mousePosition = new MPoint2(currentState.X, currentState.Y);
            if (this.IsVirtual)
            {
                // Update the virtual mouse using the mouse movement from the region center and center the actual mouse again
                var center = this.virtualRegion.Center.ToMPoint2();
                mousePosition = this.position.Value + (mousePosition - center);
                mousePosition = this.virtualRegion.Clamp(mousePosition);
                Microsoft.Xna.Framework.Input.Mouse.SetPosition(center.X, center.Y);
            }
            this.position.Update(mousePosition, seconds);
        }
    }
}