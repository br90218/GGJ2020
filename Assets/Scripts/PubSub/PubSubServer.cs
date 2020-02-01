namespace PubSubCommunication
{
    using System;
    using System.Collections.Generic;
    using PubSubMessages;

    /// <summary>
    /// Defines that class that allows you to publish or subscribe to message.
    /// This is where the magic happens. You need one and only one of these per message set.
    /// </summary>
    public sealed class PubSubServer
    {
        private static readonly PubSubServer instance = new PubSubServer();
        private Dictionary<Type, List<Action<BaseMessage>>> subscriptions = new Dictionary<Type, List<Action<BaseMessage>>>();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static PubSubServer()
        {
        }

        private PubSubServer()
        {
        }

        public static PubSubServer Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Allows a component to subscribe to a message type.
        /// </summary>
        /// <param name="messageType">Type of message that the component wants to subscribe to.</param>
        /// <param name="call">Function to call when message is processed.</param>
        public void Subscribe(Type messageType, Action<BaseMessage> call)
        {
            //Logger.Log(messageType);
            List<Action<BaseMessage>> calls;
            if (subscriptions.TryGetValue(messageType, out calls))
            {
                calls.Add(call);
            }
            else
            {
                calls = new List<Action<BaseMessage>>();
                calls.Add(call);
                subscriptions[messageType] = calls;
            }
        }

        /// <summary>
        /// Allows a component to unsubscribe from a message type.
        /// </summary>
        /// <param name="messageType">Type of message that the component wants to unsubscribe from.</param>
        /// <param name="call">Function to call when message is processed.</param>
        public void Unsubscribe(Type messageType, Action<BaseMessage> call)
        {
            List<Action<BaseMessage>> calls;
            if (subscriptions.TryGetValue(messageType, out calls))
            {
                calls.Remove(call);
            }
        }

        /// <summary>
        /// Function that is called when a component wants to publish a message.
        /// </summary>
        /// <param name="message">The message that the component wants to send.</param>
        public void Publish(BaseMessage message)
        {
            //Logger.Log(message.GetType());

            List<Action<BaseMessage>> calls;
            
            if (subscriptions.TryGetValue(message.GetType(), out calls))
            {
                 foreach (Action<BaseMessage> call in calls)
                {
                    call(message);
                }
            }  
        }
    }
}
