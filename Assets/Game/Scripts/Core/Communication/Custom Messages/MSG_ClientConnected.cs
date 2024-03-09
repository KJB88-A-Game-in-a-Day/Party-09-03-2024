using GDLib.Comms;

public class MSG_ClientConnected : Message
{
    public string clientName;

    public MSG_ClientConnected(string messageType, string clientName) : base(messageType)
        => this.clientName = clientName;
}