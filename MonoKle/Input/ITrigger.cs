namespace MonoKle.Input
{
    /// <summary>
    /// Interface for a continuous-state trigger.
    /// </summary>
    public interface ITrigger : IPressable
    {
        /// <summary>
        /// Gets the continuous state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        float State { get; }
    }
}
