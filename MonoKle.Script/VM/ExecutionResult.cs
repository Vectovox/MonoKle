namespace MonoKle.Script.VM
{
    using System;
    using System.Text;

    /// <summary>
    /// Result of executing a script.
    /// </summary>
    public class ExecutionResult
    {
        /// <summary>
        /// Gets whether the execution was successful or not.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Gets the type of the return value.
        /// </summary>
        public Type ReturnType { get; private set; }

        /// <summary>
        /// Gets the returned value of the execution.
        /// </summary>
        public object ReturnValue { get; private set; }

        /// <summary>
        /// Gets the name of the executed script.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="ExecutionResult"/>.
        /// </summary>
        /// <param name="success">Success of the execution.</param>
        /// <param name="returnType">Type of returned value.</param>
        /// <param name="returnValue">Returned value.</param>
        /// <param name="name">Name of the executed script.</param>
        public ExecutionResult(bool success, Type returnType, object returnValue, string name)
        {
            this.Success = success;
            this.ReturnType = returnType;
            this.ReturnValue = returnValue;
            this.Name = name;
        }

        /// <summary>
        /// Creates a fail result for the script with the provided name.
        /// </summary>
        /// <returns>Failure result.</returns>
        public static ExecutionResult CreateFail(string scriptName)
        {
            return new ExecutionResult(false, null, null, scriptName);
        }

        /// <summary>
        /// Returns the string representation of the result.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Success: ");
            sb.Append(this.Success);
            if(this.Success)
            {
                sb.Append(" | Type: ");
                sb.Append(this.ReturnType.ToString());
                sb.Append(" | Value: ");
                sb.Append(this.ReturnValue);
            }
            return sb.ToString();
        }
    }
}