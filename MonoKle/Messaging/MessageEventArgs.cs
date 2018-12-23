namespace MonoKle.Messaging {
    using System;

    /// <summary>
    /// Event argument for message events.
    /// </summary>
    public class MessageEventArgs : EventArgs {
        /// <summary>
        /// Gets the data associated with a message event.
        /// </summary>
        public object Data {
            get; private set;
        }

        /// <summary>
        /// Creates a new instance of <see cref="MessageEventArgs"/>.
        /// </summary>
        /// <param name="data">The data to send in the event.</param>
        public MessageEventArgs(object data) {
            Data = data;
        }
    }
}