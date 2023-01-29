using MonoKle.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace MonoKle.Engine
{
    /// <summary>
    /// Class containing variable systems.
    /// </summary>
    public class VariableStorage
    {
        /// <summary>
        /// The default file path.
        /// </summary>
        public const string SettingsFile = "settings.ini";

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableStorage"/> class.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        internal VariableStorage(ILogger logger)
        {
            System = new CVarSystem(logger);
            Populator = new CVarFileLoader(System, logger);
        }

        /// <summary>
        /// Gets the variable populator.
        /// </summary>
        public CVarFileLoader Populator { get; }

        /// <summary>
        /// Gets the variables.
        /// </summary>
        public CVarSystem System { get; }

        /// <summary>
        /// Loads the default settings from the default path (<see cref="SettingsFile"/>).
        /// </summary>
        /// <returns>True if default path contained a setting file; otherwise false.</returns>
        public bool LoadSettings()
        {
            var settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFile);
            return Populator.LoadFile(settingsPath).Successes == 1;
        }
    }
}