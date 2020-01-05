using System;

namespace MonoKle.Input
{
    /// <summary>
    /// Class providing the state of a button.
    /// </summary>
    /// <seealso cref="IPressable" />
    public class Button : IPressable
    {
        private bool wasDown;

        /// <summary>
        /// Gets the held time.
        /// </summary>
        /// <value>
        /// The held time.
        /// </value>
        public TimeSpan HeldTime { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is down.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is down; otherwise, <c>false</c>.
        /// </value>
        public bool IsDown { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is held.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is held; otherwise, <c>false</c>.
        /// </value>
        public bool IsHeld => IsDown && wasDown;

        /// <summary>
        /// Gets a value indicating whether this instance is pressed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pressed; otherwise, <c>false</c>.
        /// </value>
        public bool IsPressed => IsDown && !wasDown;

        /// <summary>
        /// Gets a value indicating whether this instance is released.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is released; otherwise, <c>false</c>.
        /// </value>
        public bool IsReleased => !IsDown && wasDown;

        /// <summary>
        /// Gets a value indicating whether this instance is up.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is up; otherwise, <c>false</c>.
        /// </value>
        public bool IsUp => !IsDown;

        /// <summary>
        /// Gets a value indicating whether this instance has been held for at least the provided amount of time.
        /// </summary>
        /// <param name="duration">The time to have been held.</param>
        /// <returns>
        /// True if held for at least the provided amount of duration; otherwise false.
        /// </returns>
        public bool IsHeldFor(TimeSpan duration) => HeldTime >= duration;

        public virtual void Update(bool down, TimeSpan deltaTime)
        {
            wasDown = IsDown;
            IsDown = down;
            HeldTime = down ? HeldTime + deltaTime : TimeSpan.Zero;
        }
    }
}
