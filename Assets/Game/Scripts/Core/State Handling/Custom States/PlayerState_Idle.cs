using GDLib.Comms;
using GDLib.State;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : State, ISubscriber
{
    float speedMod;
    Transform thisTransform;
    Dictionary<string, object> blackboard;
    VirtualInput input;

    public PlayerState_Idle(FSM fsm) : base(fsm) { }

    public override void OnStateEntry(Dictionary<string, object> blackboard)
    {
        Debug.Log("Entering Idle state.");
        this.blackboard = blackboard;

        object obj;
        if (blackboard.TryGetValue("speedMod", out obj))
            speedMod = (float)obj;

        if (blackboard.TryGetValue("thisTransform", out obj))
            thisTransform = (Transform)obj;

        if (blackboard.TryGetValue("virtualInput", out obj))
            input = (VirtualInput)obj;
    }

    public override void OnStateExit(Dictionary<string, object> blackboard) { }

    public override void UpdateState(Dictionary<string, object> blackboard)
    {
        Vector3 local = speedMod * Time.deltaTime * input.inputAxisVector;
        thisTransform.position += local;

        if (Input.GetKeyDown(KeyCode.Space))
            fsm.SetState(new PlayerState_InFlight(fsm), blackboard);
    }

    public bool Receive(Message msg)
    {
        if (msg.MessageType == MessageLibrary.Collision2DEvent)
        {
            MSG_Collision2D coll = (MSG_Collision2D)msg;
            if (coll.collisionType == MSG_Collision2D.COLL_TYPE.ENTER)
            {
                fsm.SetState(new PlayerState_Bumped(fsm), blackboard);
                return true;
            }
        }

        return false;
    }
}