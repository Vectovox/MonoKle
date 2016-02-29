namespace MonoKle.Engine
{
    using Attributes;

    public class MonoKleSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether keyboard is enabled.
        /// </summary>
        [PropertyVariableAttribute("m_keyboard_enabled")]
        public bool KeyboardEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gamepad is enabled.
        /// </summary>
        [PropertyVariableAttribute("m_gamepad_enabled")]
        public bool GamePadEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse is enabled.
        /// </summary>
        [PropertyVariableAttribute("m_mouse_enabled")]
        public bool MouseEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether console is enabled.
        /// </summary>
        [PropertyVariableAttribute("m_console_enabled")]
        public bool ConsoleEnabled { get; set; }
    }
}
