using System;

namespace MonoKle.State
{
    /// <summary>
    /// Container class containing data on state switch.
    /// </summary>
    public class StateSwitchData
    {
        private static readonly object _voidSwitchObject = new();

        /// <summary>
        /// Initializes a new instance of <see cref="StateSwitchData"/>, providing data to transfer upon state switch.
        /// </summary>
        /// <param name="nextState">The string identifier for the next state.</param>
        /// <param name="previousState">The string identifier for the previous state.</param>
        /// <param name="data">The data to pass to the next state.</param>
        public StateSwitchData(string nextState, string previousState, object data)
        {
            _data = data ?? throw new ArgumentNullException("Data may not be null!");
            NextState = nextState;
            PreviousState = previousState;
            HasData = true;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="StateSwitchData"/> without data to transfer.
        /// </summary>
        /// <param name="nextState">The string identifier for the next state.</param>
        /// <param name="previousState">The string identifier for the previous state.</param>
        public StateSwitchData(string nextState, string previousState) : this(nextState, previousState, _voidSwitchObject)
        {
            HasData = false;
        }

        /// <summary>
        /// Gets the data passed to the next state. Throws if <see cref="HasData"/> is false.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        public object Data
        {
            get
            {
                return HasData ? _data : throw new InvalidOperationException("No data present.");
            }
        }
        private readonly object _data;

        /// <summary>
        /// Gets whether any data is present.
        /// </summary>
        public bool HasData
        {
            get;
        }

        /// <summary>
        /// Gets the string identifier for the next state.
        /// </summary>
        public string NextState
        {
            get;
        }

        /// <summary>
        /// Gets the string identifier for the previous state.
        /// </summary>
        public string PreviousState
        {
            get;
        }
    }
}
