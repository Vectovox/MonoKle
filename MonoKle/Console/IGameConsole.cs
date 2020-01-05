namespace MonoKle.Console
{
    using Asset.Font;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Interface for a game console.
    /// </summary>
    public interface IGameConsole
    {
        /// <summary>
        /// Gets or sets the area in which the console will be drawn.
        /// </summary>
        MRectangleInt Area
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>
        /// The color of the background.
        /// </value>
        Color BackgroundColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the command broker. Used for executing console commands.
        /// </summary>
        /// <value>
        /// The command broker.
        /// </value>
        CommandBroker CommandBroker
        {
            get;
        }

        /// <summary>
        /// Gets or sets the command text colour.
        /// </summary>
        /// <value>
        /// The command text colour.
        /// </value>
        Color CommandTextColour
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color that the text will be drawn with if no other colour is specified.
        /// </summary>
        Color DefaultTextColour
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the disabled text colour.
        /// </summary>
        /// <value>
        /// The disabled text colour.
        /// </value>
        Color DisabledTextColour
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error text colour.
        /// </summary>
        /// <value>
        /// The error text colour.
        /// </value>
        Color ErrorTextColour
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets wether the console is open.
        /// </summary>
        bool IsOpen
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum amount of entries to keep.
        /// </summary>
        int Size
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the length of the tabs.
        /// </summary>
        /// <value>
        /// The length of the tabs.
        /// </value>
        int TabLength
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the string identifier of the text font. If null, the default font will be used.
        /// </summary>
        Font TextFont
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale for the font.
        /// </summary>
        float TextScale
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the toggle key.
        /// </summary>
        /// <value>
        /// The toggle key.
        /// </value>
        Keys ToggleKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the warning text colour.
        /// </summary>
        /// <value>
        /// The warning text colour.
        /// </value>
        Color WarningTextColour
        {
            get;
            set;
        }

        /// <summary>
        /// Clears all history.
        /// </summary>
        void Clear();

        /// <summary>
        /// Writes the provided text with the colour <see cref="GameConsole.DefaultTextColour"/>.
        /// </summary>
        /// <param name="text">The text to write.</param>
        void WriteLine(string text);

        /// <summary>
        /// Writes the provided text with the given color.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="color">Color of the line.</param>
        void WriteLine(string text, Color color);
    }
}