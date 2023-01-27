using MonoKle.Asset;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoKle.Console
{
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
        Color BackgroundColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the command broker. Used for executing console commands.
        /// </summary>
        CommandBroker CommandBroker { get; }

        /// <summary>
        /// Gets the log data.
        /// </summary>
        GameConsoleLogData Log { get; }

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
        uint Capacity { get; set; }

        /// <summary>
        /// Gets or sets the string identifier of the text font.
        /// </summary>
        FontInstance TextFont
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        int TextSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the toggle key.
        /// </summary>
        Keys ToggleKey
        {
            get;
            set;
        }
        Color CommandTextColour { get; set; }
    }
}