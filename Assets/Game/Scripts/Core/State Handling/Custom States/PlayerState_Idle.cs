using GDLib.Comms;
using GDLib.State;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : State, ISubscriber
{
    float speedMod;
    int currentHealth;
    GameObject thisGameObject;

    Transform thisTransform;
    VirtualInput input;
    MessageBroker localMsgBroker;
    Dictionary<string, object> blackboard;

    public PlayerState_Idle(FSM fsm) : base(fsm) { }

    public override void OnStateEntry(Dictionary<string, object> blackboard)
    {
        Debug.Log("Entering Idle state.");
        this.blackboard = blackboard;

        object obj;
        if (blackboard.TryGetValue("speedMod", out obj))
            speedMod = (float)obj;

        if (blackboard.TryGetValue("currentHealth", out obj))
            currentHealth = (int)obj;

        if (blackboard.TryGetValue("thisTransform", out obj))
            thisTransform = (Transform)obj;

        if (blackboard.TryGetValue("thisGameObject", out obj))
            thisGameObject = (GameObject)obj;

        if (blackboard.TryGetValue("virtualInput", out obj))
            input = (VirtualInput)obj;

        if (blackboard.TryGetValue("localMsgBroker", out obj))
            localMsgBroker = (MessageBroker)obj;

        thisGameObject.layer = LayerMask.NameToLayer("Hurtable");

        localMsgBroker.RegisterSubscriber(MessageLibrary.CollisionEvent, this);
        localMsgBroker.RegisterSubscriber(MessageLibrary.OnHit, this);
    }

    public override void OnStateExit(Dictionary<string, object> blackboard)
    {
        localMsgBroker.RemoveSubscriber(MessageLibrary.CollisionEvent, this);
        localMsgBroker.RemoveSubscriber(MessageLibrary.OnHit, this);
    }

    public override void UpdateState(Dictionary<string, object> blackboard)
    {
        Vector3 local = speedMod * Time.deltaTime * input.inputAxisVector;
        thisTransform.position += local;

        if (Input.GetKeyDown(KeyCode.Space))
            fsm.SetState(new PlayerState_InFlight(fsm), blackboard);
    }

    public bool Receive(Message msg)
    {
        if (msg.MessageType == MessageLibrary.CollisionEvent)
        {
            fsm.SetState(new PlayerState_Bumped(fsm), blackboard);
            return true;
        }
        else if (msg.MessageType == MessageLibrary.OnHit)
        {
            MSG_OnHit oh = (MSG_OnHit)msg;
            currentHealth -= oh.damage;
            Debug.Log(currentHealth);

            if (blackboard.ContainsKey("currentHealth"))
                blackboard["currentHealth"] = currentHealth;
            else
                blackboard.Add("currentHealth", currentHealth);

            fsm.SetState(new PlayerState_Bumped(fsm), blackboard);
            return true;
        }

        return false;
    }
}