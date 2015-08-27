namespace MonoKle.State
{
    /// <summary>
    /// Container class containing data for the next state and the string represented identifier for it.
    /// </summary>
    public class StateSwitchData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateSwitchData"/> class.
        /// </summary>
        /// <param name="nextState">The string identifier for the next state. May be null.</param>
        /// <param name="nextState">The string identifier for the previous state. May be null.</param>
        /// <param name="data">The data to pass to the next state. May be null.</param>
        public StateSwitchData(string nextState, string previousState, object data)
        {
            this.NextState = nextState;
            this.Data = data;
        }

        /// <summary>
        /// Gets the data passed to the next state. May be null.
        /// </summary>
        public object Data
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the string identifier for the next state. May be null.
        /// </summary>
        public string NextState
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the string identifier for the previous state. May be null.
        /// </summary>
        public string PreviousState
        {
            get;
            private set;
        }
    }
}