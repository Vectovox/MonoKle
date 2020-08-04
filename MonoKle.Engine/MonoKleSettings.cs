using MonoKle.Attributes;

namespace MonoKle.Engine
{
    public class MonoKleSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether keyboard is enabled.
        /// </summary>
        [PropertyVariable("keyboard_enabled")]
        public bool KeyboardEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gamepad is enabled.
        /// </summary>
        [PropertyVariable("gamepad_enabled")]
        public bool GamePadEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse is enabled.
        /// </summary>
        [PropertyVariable("mouse_enabled")]
        public bool MouseEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse is enabled.
        /// </summary>
        [PropertyVariable("touch_enabled")]
        public bool TouchEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether console is enabled.
        /// </summary>
        [PropertyVariable("console_enabled")]
        public bool ConsoleEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse cursor is visible inside the application.
        /// </summary>
        [PropertyVariable("mouse_visible")]
        public bool MouseVisible { get { return MonoKleGame.GameInstance.IsMouseVisible; } set { MonoKleGame.GameInstance.IsMouseVisible = value; } }
    }
}
