namespace MonoKle.Script
{
    using System;

    /// <summary>
    /// Channel attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ChannelAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the channel.
        /// </summary>
        /// <value>
        /// The channel.
        /// </value>
        public string Channel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelAttribute"/> class.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public ChannelAttribute(string channel)
        {
            this.Channel = channel;
        }
    }
}
