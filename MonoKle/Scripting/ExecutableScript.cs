using System;
using System.Reflection;

namespace MonoKle.Scripting
{
    public abstract class ExecutableScript
    {
        private const string EXECUTE_METHOD_NAME = "Run";

        private MethodInfo executeMethod;
        private bool initialized;
        
        public void Initialize()
        {
            this.executeMethod = this.GetType().GetMethod(EXECUTE_METHOD_NAME,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public bool HasReturn => this.executeMethod != null ? this.executeMethod.ReturnType != typeof(void) : false;

        public ScriptExecution Execute(params object[] args)
        {
            if (this.executeMethod != null)
            {
                object res = null;
                string message = null;
                try
                {
                    res = this.executeMethod.Invoke(this, args);
                }
                catch (Exception e)
                {
                    message = e.Message;
                }

                return new ScriptExecution(res, message == null, message ?? "");
            }
            else
            {
                return new ScriptExecution(null, false, "Method not defined: " + EXECUTE_METHOD_NAME);
            }
        }
    }
}
