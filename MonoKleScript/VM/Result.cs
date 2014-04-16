namespace MonoKleScript.VM
{
    using System;
    using System.Text;

    /// <summary>
    /// Result of executing a script.
    /// </summary>
    public struct Result
    {
        /// <summary>
        /// Indicates whether the execution was successful or not.
        /// </summary>
        public bool success;

        /// <summary>
        /// The type of the return value.
        /// </summary>
        public Type returnType;

        /// <summary>
        /// Returned value of the execution.
        /// </summary>
        public object returnValue;

        /// <summary>
        /// Creates a new instance of result.
        /// </summary>
        /// <param name="success">Success of the execution.</param>
        /// <param name="returnType">Type of returned value.</param>
        /// <param name="returnValue">Returned value.</param>
        public Result(bool success, Type returnType, object returnValue)
        {
            this.success = success;
            this.returnType = returnType;
            this.returnValue = returnValue;
        }

        /// <summary>
        /// Fail result.
        /// </summary>
        /// <returns>Failure result.</returns>
        public static Result Fail
        {
            get { return new Result(false, null, null); }
        }

        /// <summary>
        /// Returns the string representation of the result.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Success: ");
            sb.Append(success);
            if(success)
            {
                sb.Append(" | Type: ");
                sb.Append(this.returnType.ToString());
                sb.Append(" | Value: ");
                sb.Append(this.returnValue);
            }
            return sb.ToString();
        }
    }
}