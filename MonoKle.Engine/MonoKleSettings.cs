namespace MonoKle.Engine {
    using Attributes;

    public class MonoKleSettings {
        /// <summary>
        /// Gets or sets a value indicating whether keyboard is enabled.
        /// </summary>
        [PropertyVariableAttribute("keyboard_enabled")]
        public bool KeyboardEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gamepad is enabled.
        /// </summary>
        [PropertyVariableAttribute("gamepad_enabled")]
        public bool GamePadEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse is enabled.
        /// </summary>
        [PropertyVariableAttribute("mouse_enabled")]
        public bool MouseEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether console is enabled.
        /// </summary>
        [PropertyVariableAttribute("console_enabled")]
        public bool ConsoleEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse cursor is visible inside the application.
        /// </summary>
        [PropertyVariableAttribute("mouse_visible")]
        public bool MouseVisible { get { return MBackend.GameInstance.IsMouseVisible; } set { MBackend.GameInstance.IsMouseVisible = value; } }
    }
}
