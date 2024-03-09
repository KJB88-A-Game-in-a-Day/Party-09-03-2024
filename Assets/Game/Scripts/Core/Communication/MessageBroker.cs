using System.Collections.Generic;

namespace GDLib.Comms
{
    // Message Broker to update other systems when this entity's state changes.
    public class MessageBroker : IService 
    {
        readonly Dictionary<string, List<ISubscriber>> subscribers = new Dictionary<string, List<ISubscriber>>();

        public void RegisterSubscriber(string msg, ISubscriber subscriber)
        {
            msg = msg.ToLower();
            if (!subscribers.ContainsKey(msg))
                subscribers.Add(msg, new List<ISubscriber> { subscriber });
            else
                subscribers[msg].Add(subscriber);
        }

        /// <summary>
        /// Remove a subscriber from listening for specific messages.
        /// </summary>
        /// <param name="msg">string: The message to listen for and send through.</param>
        /// <param name="subscriber">ISubscriber: The subscriber to send messages to.</param>
        public void RemoveSubscriber(string msg, ISubscriber subscriber)
        {
            msg = msg.ToLower();
            if (!subscribers.ContainsKey(msg))
                return;

            subscribers[msg].Remove(subscriber);
        }

        public void SendMessage(Message msg)
        {
            if (!subscribers.ContainsKey(msg.MessageType))
                return;

            foreach (ISubscriber subscriber in subscribers[msg.MessageType])
            {
                if (subscriber.Receive(msg))
                    return;
            }
        }
    };
}