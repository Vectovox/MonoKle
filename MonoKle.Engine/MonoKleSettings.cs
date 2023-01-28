using MonoKle.Configuration;

namespace MonoKle.Engine
{
    public class MonoKleSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether keyboard is enabled.
        /// </summary>
        [CVar("keyboard_enabled")]
        public bool KeyboardEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gamepad is enabled.
        /// </summary>
        [CVar("gamepad_enabled")]
        public bool GamePadEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse is enabled.
        /// </summary>
        [CVar("mouse_enabled")]
        public bool MouseEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether touch input is enabled.
        /// </summary>
        [CVar("touch_enabled")]
        public bool TouchEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether console is enabled.
        /// </summary>
        [CVar("console_enabled")]
        public bool ConsoleEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse cursor is visible inside the application.
        /// </summary>
        [CVar("mouse_visible")]
        public bool MouseVisible { get { return MGame.GameInstance.IsMouseVisible; } set { MGame.GameInstance.IsMouseVisible = value; } }
    }
}
