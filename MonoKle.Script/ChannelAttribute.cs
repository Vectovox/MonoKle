namespace MonoKle.Script
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ChannelAttribute : Attribute
    {
        public string Channel { get; set; }

        public ChannelAttribute(string channel)
        {
            this.Channel = channel;
        }
    }
}
