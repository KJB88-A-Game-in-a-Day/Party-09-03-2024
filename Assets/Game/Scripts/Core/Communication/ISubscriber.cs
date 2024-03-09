namespace GDLib.Comms
{
    /// <summary>
    /// Allows subscription to specific messages via the MessageBroker
    /// </summary>
    public interface ISubscriber 
    {
        /// <summary>
        /// Act on a received message that the class is subscribed to.
        /// </summary>
        /// <param name="msg">string: The message.</param>
        /// <returns>Bool: Consume message if true. Passthrough message if false.</returns>
        public bool Receive(Message msg);
    }
}