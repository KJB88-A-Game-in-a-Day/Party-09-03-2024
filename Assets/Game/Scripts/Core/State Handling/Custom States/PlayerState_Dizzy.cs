using GDLib.Comms;
using GDLib.State;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerState_Dizzy : State, ISubscriber
{
    float dizzyTimeMax = 0;
    float dizzyCounter = 0.0f;
    int currentHealth;

    EmotionLibrary emotionLib;
    SpriteRenderer emotionDisplay;
    MessageBroker localMsgBroker;
    GameObject thisGameObject;

    Dictionary<string, object> blackboard;
    public PlayerState_Dizzy(FSM fsm) : base(fsm) { }

    public override void OnStateEntry(Dictionary<string, object> blackboard)
    {
        Debug.Log("Entering Dizzy state.");

        this.blackboard = blackboard;

        if (ServiceLocator.RequestService("emotionLibrary", out IService service))
            emotionLib = (EmotionLibrary)service;

        object obj;
        if (blackboard.TryGetValue("dizzyTimeMax", out obj))
            dizzyTimeMax = (float)obj;

        if (blackboard.TryGetValue("emotionDisplay", out obj))
            emotionDisplay = (SpriteRenderer)obj;

        if (blackboard.TryGetValue("localMsgBroker", out obj))
            localMsgBroker = (MessageBroker)obj;

        if (blackboard.TryGetValue("thisGameObject", out obj))
            thisGameObject = (GameObject)obj;

        if (blackboard.TryGetValue("currentHealth", out obj))
            currentHealth = (int)obj;

        thisGameObject.layer = LayerMask.NameToLayer("Hurtable");

        localMsgBroker.RegisterSubscriber(MessageLibrary.CollisionEvent, this);


        emotionDisplay.enabled = true;
        emotionDisplay.sprite = emotionLib.Dizzy;
    }

    public override void OnStateExit(Dictionary<string, object> blackboard)
    {
        emotionDisplay.enabled = false;
        localMsgBroker.RemoveSubscriber(MessageLibrary.CollisionEvent, this);
        localMsgBroker.RemoveSubscriber(MessageLibrary.OnHit, this);
    }

    public override void UpdateState(Dictionary<string, object> blackboard)
    {
        if (dizzyCounter >= dizzyTimeMax)
            fsm.SetState(new PlayerState_Idle(fsm), blackboard);

        dizzyCounter += Time.deltaTime;
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