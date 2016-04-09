namespace MonoKle.Scripting
{
    /// <summary>
    /// The result of executing a script.
    /// </summary>
    public struct ScriptExecution
    {
        public object Result;
        public bool Success;
        public string Message;

        public ScriptExecution(object result, bool success, string message)
        {
            this.Result = result;
            this.Success = success;
            this.Message = message;
        }
    }
}
