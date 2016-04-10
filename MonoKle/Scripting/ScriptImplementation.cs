namespace MonoKle.Scripting
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Implementation class of an executable script.
    /// </summary>
    public abstract class ScriptImplementation
    {
        /// <summary>
        /// The method used for execution.
        /// </summary>
        public MethodInfo ExecuteMethod;

        /// <summary>
        /// Gets the type of the return.
        /// </summary>
        /// <value>
        /// The type of the return.
        /// </value>
        public Type ReturnType => this.ExecuteMethod?.ReturnType ?? typeof(void);

        /// <summary>
        /// Gets a value indicating whether this returns value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it returns value; otherwise <c>false</c>.
        /// </value>
        public bool ReturnsValue => this.ExecuteMethod != null ? this.ExecuteMethod.ReturnType != typeof(void) : false;

        /// <summary>
        /// Executes with the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public ScriptExecution Execute(params object[] args)
        {
            if (this.ExecuteMethod != null)
            {
                object res = null;
                string message = null;
                try
                {
                    res = this.ExecuteMethod.Invoke(this, args);
                }
                catch (Exception e)
                {
                    message = "Exception: " + e.InnerException?.Message ?? e.Message;
                }

                return new ScriptExecution(res, message == null, message ?? "");
            }
            else
            {
                return new ScriptExecution(null, false, "Execution method not defined.");
            }
        }
    }
}