public static class MessageLibrary
{
    private readonly static string clientConnected = "clientConnected";
    public static string ClientConnected => clientConnected.ToLower();

    private readonly static string statusUpdated = "statusUpdated";
    public  static string StatusUpdated => statusUpdated.ToLower();
}