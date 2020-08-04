using MonoKle.Attributes;

namespace MonoKle.Engine
{
    public class MonoKleSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether keyboard is enabled.
        /// </summary>
        [Variable("keyboard_enabled")]
        public bool KeyboardEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gamepad is enabled.
        /// </summary>
        [Variable("gamepad_enabled")]
        public bool GamePadEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse is enabled.
        /// </summary>
        [Variable("mouse_enabled")]
        public bool MouseEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse is enabled.
        /// </summary>
        [Variable("touch_enabled")]
        public bool TouchEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether console is enabled.
        /// </summary>
        [Variable("console_enabled")]
        public bool ConsoleEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse cursor is visible inside the application.
        /// </summary>
        [Variable("mouse_visible")]
        public bool MouseVisible { get { return MGame.GameInstance.IsMouseVisible; } set { MGame.GameInstance.IsMouseVisible = value; } }
    }
}
