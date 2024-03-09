using GDLib.Comms;
using UnityEngine;

public class MSG_Collision2D : Message
{
    public enum COLL_TYPE
    {
        ENTER,
        EXIT
    }
    public COLL_TYPE collisionType;
    public Collider2D collider;

    public MSG_Collision2D(string messageType, COLL_TYPE collisionType, Collider2D collider) : base(messageType)
    {
        this.collisionType = collisionType;
        this.collider = collider;
    }
}