using System;

namespace MonoKle.Input
{
    /// <summary>
    /// Class providing the state of a DPad.
    /// </summary>
    /// <seealso cref="IDPad" />
    public class DPad : IDPad
    {
        private readonly Button up = new();
        private readonly Button down = new();
        private readonly Button left = new();
        private readonly Button right = new();

        /// <summary>
        /// Gets the down direction.
        /// </summary>
        /// <value>
        /// The down direction.
        /// </value>
        public IPressable Down => down;
        /// <summary>
        /// Gets the left direction.
        /// </summary>
        /// <value>
        /// The left direction.
        /// </value>
        public IPressable Left => left;
        /// <summary>
        /// Gets the right direction.
        /// </summary>
        /// <value>
        /// The right direction.
        /// </value>
        public IPressable Right => right;
        /// <summary>
        /// Gets the up direction.
        /// </summary>
        /// <value>
        /// The up direction.
        /// </value>
        public IPressable Up => up;

        /// <summary>
        /// Updates the state of the <see cref="DPad" />.
        /// </summary>
        /// <param name="leftDown">True if the left-direction is down.</param>
        /// <param name="rightDown">True if the right-direction is down.</param>
        /// <param name="upDown">True if the up-direction is down.</param>
        /// <param name="downDown">True if the down-direction is down.</param>
        /// <param name="deltaTime">Time since last update.</param>
        public virtual void Update(bool leftDown, bool rightDown, bool upDown, bool downDown, TimeSpan deltaTime)
        {
            up.Update(upDown, deltaTime);
            down.Update(downDown, deltaTime);
            left.Update(leftDown, deltaTime);
            right.Update(rightDown, deltaTime);
        }

        public MPoint2 PressedDirection()
        {
            int x = 0, y = 0;
            if (Left.IsPressed)
            {
                x = -1;
            }
            else if (Right.IsPressed)
            {
                x = 1;
            }

            if (Up.IsPressed)
            {
                y = -1;
            }
            else if (Down.IsPressed)
            {
                y = 1;
            }

            return new MPoint2(x, y);
        }

        public MPoint2 DownDirection()
        {
            int x = 0, y = 0;
            if (Left.IsDown)
            {
                x = -1;
            }
            else if (Right.IsDown)
            {
                x = 1;
            }

            if (Up.IsDown)
            {
                y = -1;
            }
            else if (Down.IsDown)
            {
                y = 1;
            }

            return new MPoint2(x, y);
        }
    }
}
