public static class MessageLibrary
{
    private readonly static string clientConnected = "clientConnected";
    private readonly static string statusUpdated = "statusUpdated";
    private readonly static string collisionEvent = "collisionEvent";
    private readonly static string onHit = "onHit";

    public static string ClientConnected => clientConnected.ToLower();
    public  static string StatusUpdated => statusUpdated.ToLower();
    public static string CollisionEvent => collisionEvent.ToLower();
    public static string OnHit => onHit.ToLower();
}