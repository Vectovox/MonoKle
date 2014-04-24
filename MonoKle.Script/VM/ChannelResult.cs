namespace MonoKle.Script.VM
{
    using System.Collections.Generic;

    /// <summary>
    /// Result for execution of a channel.
    /// </summary>
    public class ChannelResult
    {
        /// <summary>
        /// Gets the results from the channel execution.
        /// </summary>
        /// <returns>Collection of results.</returns>
        public ICollection<ExecutionResult> ExecutionResult { get; private set; }
        
        /// <summary>
        /// Gets the channel name that was executed.
        /// </summary>
        /// <returns>Name of channel.</returns>
        public string ChannelName { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="ChannelResult"/>.
        /// </summary>
        /// <param name="channelName">Name of executed channel.</param>
        /// <param name="executionResult">Results of execution.</param>
        public ChannelResult(string channelName, ICollection<ExecutionResult> executionResult)
        {
            this.ChannelName = channelName;
            this.ExecutionResult = executionResult;
        }
    }
}
