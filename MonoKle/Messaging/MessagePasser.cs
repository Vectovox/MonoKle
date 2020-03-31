using System;
using System.Collections.Generic;

namespace MonoKle.Messaging
{
    /// <summary>
    /// Class handling message parsing.
    /// </summary>
    public class MessagePasser
    {
        private Dictionary<string, HashSet<EventHandler<MessageEventArgs>>> handlersByChannel = new Dictionary<string, HashSet<EventHandler<MessageEventArgs>>>();

        /// <summary>
        /// Sends a message on a given channel.
        /// </summary>
        /// <param name="channelID">The channel to send the message on.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="sender">The sender.</param>
        public void SendMessage(string channelID, MessageEventArgs message, object sender)
        {
            if (handlersByChannel.ContainsKey(channelID))
            {
                foreach (EventHandler<MessageEventArgs> handler in handlersByChannel[channelID])
                {
                    handler.Invoke(sender, message);
                }
            }
        }

        /// <summary>
        /// Sends a message on a given channel, with the sender reported as this instance of <see cref="MessagePasser"/>.
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="message"></param>
        public void SendMessage(string channelID, MessageEventArgs message) => SendMessage(channelID, message, this);

        /// <summary>
        /// Subscribes the given handler to receive messages on the provided channel.
        /// </summary>
        /// <param name="channelID">The channel to subscribe to.</param>
        /// <param name="handler">The handler to subscribe.</param>
        public void Subscribe(string channelID, EventHandler<MessageEventArgs> handler)
        {
            if (handlersByChannel.ContainsKey(channelID) == false)
            {
                handlersByChannel.Add(channelID, new HashSet<EventHandler<MessageEventArgs>>());
            }

            HashSet<EventHandler<MessageEventArgs>> handlers = handlersByChannel[channelID];
            if (handlers.Contains(handler))
            {
                // TODO: REPORT ERROR
            }
            else
            {
                handlers.Add(handler);
            }
        }

        /// <summary>
        /// Unsubscribes the given handler from messages on the provided channel.
        /// </summary>
        /// <param name="channelID">The channel to unsubscribe from.</param>
        /// <param name="handler">The handler to unsubscribe.</param>
        public void Unsubscribe(string channelID, EventHandler<MessageEventArgs> handler)
        {
            if (handlersByChannel.ContainsKey(channelID) && handlersByChannel[channelID].Contains(handler))
            {
                handlersByChannel[channelID].Remove(handler);
            }
            else
            {
                // TODO: REPORT NO UNSUBSCRIBE
            }
        }
    }
}