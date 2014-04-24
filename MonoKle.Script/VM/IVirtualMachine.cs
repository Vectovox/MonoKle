namespace MonoKle.Script.VM
{
    using MonoKle.Script.Common.Script;
    using MonoKle.Script.VM.Event;
    using System.Collections.Generic;

    /// <summary>
    /// Virtual Machine interface.
    /// </summary>
    public interface IVirtualMachine
    {
        /// <summary>
        /// Executes all scripts on a given channel.
        /// </summary>
        /// <param name="channel">The channel to execute scripts from.</param>
        /// <returns>Result of the channel execution.</returns>
        ChannelResult ExecuteChannel(string channel);

        /// <summary>
        /// Executes all scripts on a given channel with the provided parameters.
        /// </summary>
        /// <param name="channel">The channel to execute scripts from.</param>
        /// <param name="parameters">Parameters to proide each script.</param>
        /// <returns>Result of the channel execution.</returns>
        ChannelResult ExecuteChannel(string channel, object[] parameters);

        /// <summary>
        /// Executes the script with the provided name.
        /// </summary>
        /// <param name="script">Name of the script to execute.</param>
        /// <returns>Result of the execution.</returns>
        ExecutionResult ExecuteScript(string script);

        /// <summary>
        /// Executes the script with the provided name.
        /// </summary>
        /// <param name="script">Name of the script to execute.</param>
        /// <param name="parameters">Parameters to give the script.</param>
        /// <returns>Result of the execution.</returns>
        ExecutionResult ExecuteScript(string script, object[] parameters);

        /// <summary>
        /// Loads the provided scripts and returns the amount loaded.
        /// </summary>
        /// <param name="scripts">Collection of scripts.</param>
        /// <returns>Amount of loaded scripts.</returns>
        int LoadScripts(ICollection<ByteScript> scripts);

        /// <summary>
        /// Loads the provided script and returns if successful.
        /// </summary>
        /// <param name="script">Script.</param>
        /// <returns>True if script was loaded, otherwise false.</returns>
        bool LoadScript(ByteScript script);

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
