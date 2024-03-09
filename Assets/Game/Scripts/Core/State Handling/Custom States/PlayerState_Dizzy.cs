using GDLib.Comms;
using GDLib.State;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerState_Dizzy : State, ISubscriber
{
    float dizzyTimeMax = 0;
    EmotionLibrary emotionLib;
    SpriteRenderer emotionDisplay;

    float dizzyCounter = 0.0f;

    Dictionary<string, object> blackboard;
    public PlayerState_Dizzy(FSM fsm) : base(fsm) { }

    public override void OnStateEntry(Dictionary<string, object> blackboard)
    {
        Debug.Log("Entering Dizzy state.");

        this.blackboard = blackboard;

        if (ServiceLocator.RequestService("emotionLibrary", out IService service))
            emotionLib = (EmotionLibrary)service;

        object obj;
        if (blackboard.TryGetValue("dizzyTimeMax", out  obj))
            dizzyTimeMax = (float)obj;

        if (blackboard.TryGetValue("emotionDisplay", out  obj))
            emotionDisplay = (SpriteRenderer)obj;

        emotionDisplay.enabled = true;
        emotionDisplay.sprite = emotionLib.Dizzy;
    }

    public override void OnStateExit(Dictionary<string, object> blackboard)
        => emotionDisplay.enabled = false;

    public override void UpdateState(Dictionary<string, object> blackboard)
    {
        if (dizzyCounter >= dizzyTimeMax)
            fsm.SetState(new PlayerState_Idle(fsm), blackboard);

        dizzyCounter += Time.deltaTime;
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