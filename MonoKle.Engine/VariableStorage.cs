using MonoKle.Configuration;
using Microsoft.Extensions.Logging;

namespace MonoKle.Engine
{
    /// <summary>
    /// Class containing variable systems.
    /// </summary>
    public class VariableStorage : ILogged
    {
        /// <summary>
        /// The default file path.
        /// </summary>
        public const string DefaultFilePath = "settings.ini";

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableStorage"/> class.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        internal VariableStorage(ILogger logger)
        {
            Logger = logger;
            Variables = new CVarSystem(logger);
            VariablePopulator = new CVarFileLoader(Variables);
        }

        /// <summary>
        /// Gets or sets the logger instance.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Gets the variable populator.
        /// </summary>
        /// <value>
        /// The variable populator.
        /// </value>
        public CVarFileLoader VariablePopulator { get; private set; }

        /// <summary>
        /// Gets the variables.
        /// </summary>
        /// <value>
        /// The variables.
        /// </value>
        public CVarSystem Variables { get; private set; }

        /// <summary>
        /// Loads the default settings from the default path (<see cref="DefaultFilePath"/>).
        /// </summary>
        /// <returns>True if default path contained a setting file; otherwise false.</returns>
        public bool LoadDefaultVariables() => VariablePopulator.LoadFile(DefaultFilePath).Successes == 1;
    }
}