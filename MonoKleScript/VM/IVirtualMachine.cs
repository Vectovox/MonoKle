namespace MonoKleScript.VM
{
    using MonoKleScript.Common.Script;
    using MonoKleScript.VM.Event;
    using System.Collections.Generic;

    /// <summary>
    /// Virtual machine interface.
    /// </summary>
    public interface IVirtualMachine
    {
        /// <summary>
        /// Executes the script with the provided name.
        /// </summary>
        /// <param name="script">Name of the script to execute.</param>
        /// <returns>Result of the execution.</returns>
        Result ExecuteScript(string script);

        /// <summary>
        /// Executes the script with the provided name.
        /// </summary>
        /// <param name="script">Name of the script to execute.</param>
        /// <param name="parameters">Parameters to give the script.</param>
        /// <returns>Result of the execution.</returns>
        Result ExecuteScript(string script, object[] parameters);

        /// <summary>
        /// Loads the provided scripts and returns the amount loaded.
        /// </summary>
        /// <param name="scripts">Collection of scripts.</param>
        /// <returns>Amount of loaded scripts.</returns>
        int LoadScripts(ICollection<ByteScript> scripts);

        /// <summary>
        /// Event fired when a script wants to print.
        /// </summary>
        event PrintEventHandler Print;

        /// <summary>
        /// Event fired when executing a script resulted in a runtime error.
        /// </summary>
        event RuntimeErrorEventHandler RuntimeError;
    }
}
