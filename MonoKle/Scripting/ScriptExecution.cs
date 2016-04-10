namespace MonoKle.Scripting
{
    /// <summary>
    /// The result of executing a script.
    /// </summary>
    public struct ScriptExecution
    {
        /// <summary>
        /// The result of the execution.
        /// </summary>
        public object Result;

        /// <summary>
        /// Success of the execution.
        /// </summary>
        public bool Success;

        /// <summary>
        /// The error message of the execution.
        /// </summary>
        public string Message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptExecution"/> struct.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="message">The message.</param>
        public ScriptExecution(object result, bool success, string message)
        {
            this.Result = result;
            this.Success = success;
            this.Message = message;
        }
    }
}
