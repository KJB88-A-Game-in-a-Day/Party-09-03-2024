using GDLib.Comms;

public class MSG_OnHit : Message
{
    public int damage;

    public MSG_OnHit(string messageType, int damage) : base(messageType)
        => this.damage = damage;
}
