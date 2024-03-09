using GDLib.Comms;

public class MSG_StatusUpdated : Message
{
    public string statusText;

    public MSG_StatusUpdated(string messageType, string statusText) : base(messageType)
        => this.statusText = statusText;
}