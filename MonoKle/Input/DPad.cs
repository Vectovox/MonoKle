namespace MonoKle.Input
{
    /// <summary>
    /// Class providing the state of a DPad.
    /// </summary>
    /// <seealso cref="IDPad" />
    public class DPad : IDPad
    {
        private Button up = new Button();
        private Button down = new Button();
        private Button left = new Button();
        private Button right = new Button();

        /// <summary>
        /// Gets the down direction.
        /// </summary>
        /// <value>
        /// The down direction.
        /// </value>
        public IPressable Down => this.down;
        /// <summary>
        /// Gets the left direction.
        /// </summary>
        /// <value>
        /// The left direction.
        /// </value>
        public IPressable Left => this.left;
        /// <summary>
        /// Gets the right direction.
        /// </summary>
        /// <value>
        /// The right direction.
        /// </value>
        public IPressable Right => this.right;
        /// <summary>
        /// Gets the up direction.
        /// </summary>
        /// <value>
        /// The up direction.
        /// </value>
        public IPressable Up => this.up;

        /// <summary>
        /// Updates the state of the <see cref="DPad" />.
        /// </summary>
        /// <param name="leftDown">True if the left-direction is down.</param>
        /// <param name="rightDown">True if the right-direction is down.</param>
        /// <param name="upDown">True if the up-direction is down.</param>
        /// <param name="downDown">True if the down-direction is down.</param>
        /// <param name="deltaTime">Time in seconds since last update.</param>
        public virtual void Update(bool leftDown, bool rightDown, bool upDown, bool downDown, double deltaTime)
        {
            this.up.Update(upDown, deltaTime);
            this.down.Update(downDown, deltaTime);
            this.left.Update(leftDown, deltaTime);
            this.right.Update(rightDown, deltaTime);
        }
    }
}
