using System;

namespace MonoKle.Input.Mouse
{
    /// <summary>
    /// Interface providing polling functionality for mouse input.
    /// </summary>
    public interface IMouse : IActivatableInput
    {
        /// <summary>
        /// Gets or sets whether the virtual mouse is enabled.
        /// </summary>
        bool IsVirtual { get; set; }

        /// <summary>
        /// Gets or sets the region of the screen used for the virtual mouse.
        /// </summary>
        /// <value>
        /// The virtual screen region.
        /// </value>
        MRectangleInt VirtualRegion { get; set; }

        /// <summary>
        /// Gets the position relative the upper left corner of the game window.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        IInputPosition Position { get; }


        /// <summary>
        /// Gets the provided mouse button.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <exception cref="ArgumentException">Non supported mouse button value provided.</exception>
        IPressable GetButton(MouseButton button);

        /// <summary>
        /// Gets the left button.
        /// </summary>
        /// <value>
        /// The left button.
        /// </value>
        IPressable Left { get; }

        /// <summary>
        /// Gets the middle button.
        /// </summary>
        /// <value>
        /// The middle button.
        /// </value>
        IPressable Middle { get; }

        /// <summary>
        /// Gets the right button.
        /// </summary>
        /// <value>
        /// The right button.
        /// </value>
        IPressable Right { get; }

        /// <summary>
        /// Gets the first extra button.</summary>
        /// <value>
        /// The first extra button.
        /// </value>
        IPressable X1 { get; }

        /// <summary>
        /// Gets the second extra button.</summary>
        /// <value>
        /// The second extra button.
        /// </value>
        IPressable X2 { get; }

        /// <summary>
        /// Gets the scroll direction.
        /// </summary>
        /// <value>
        /// The scroll direction.
        /// </value>
        MouseScrollDirection ScrollDirection { get; }
    }
}