namespace MonoKle.Engine
{
    using Logging;
    using Variable;

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
        internal VariableStorage(Logger logger)
        {
            this.Logger = logger;
            this.Variables = new VariableSystem(logger);
            this.VariablePopulator = new VariablePopulator(this.Variables);
        }

        /// <summary>
        /// Gets or sets the logger instance.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public Logger Logger { get; set; }

        /// <summary>
        /// Gets the variable populator.
        /// </summary>
        /// <value>
        /// The variable populator.
        /// </value>
        public VariablePopulator VariablePopulator { get; private set; }

        /// <summary>
        /// Gets the variables.
        /// </summary>
        /// <value>
        /// The variables.
        /// </value>
        public VariableSystem Variables { get; private set; }

        /// <summary>
        /// Loads the default settings from the default path (<see cref="VariableStorage.DefaultFilePath"/>).
        /// </summary>
        /// <returns>True if default path contained a setting file; otherwise false.</returns>
        public bool LoadDefaultVariables() => this.VariablePopulator.LoadFile(VariableStorage.DefaultFilePath).Successes == 1;
    }
}