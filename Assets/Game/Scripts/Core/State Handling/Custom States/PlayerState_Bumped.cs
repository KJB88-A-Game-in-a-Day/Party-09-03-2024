using GDLib.Comms;
using GDLib.State;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Bumped : State
{
    EmotionLibrary emotionLib;
    SpriteRenderer emotionDisplay;
    GameObject thisGameObject;

    float bumpedTimeMax;

    float bumpedCounter = 0.0f;

    public PlayerState_Bumped(FSM fsm) : base(fsm) { }

    public override void OnStateEntry(Dictionary<string, object> blackboard)
    {
        Debug.Log("Entering Bumped state.");
        if (ServiceLocator.RequestService("emotionLibrary", out IService service))
            emotionLib = (EmotionLibrary)service;

        object obj;
        if (blackboard.TryGetValue("bumpedTimeMax", out obj))
            bumpedTimeMax = (float)obj;

        if (blackboard.TryGetValue("emotionDisplay", out obj))
            emotionDisplay = (SpriteRenderer)obj;

        if (blackboard.TryGetValue("thisGameObject", out obj))
            thisGameObject = (GameObject)obj;

        emotionDisplay.enabled = true;
        emotionDisplay.sprite = emotionLib.Bumped;

        thisGameObject.layer = LayerMask.NameToLayer("NoHurt");
    }

    public override void OnStateExit(Dictionary<string, object> blackboard)
        => emotionDisplay.enabled = false;

    public override void UpdateState(Dictionary<string, object> blackboard)
    {
        Debug.Log("UPDATING!");
        if (bumpedCounter >= bumpedTimeMax)
            fsm.SetState(new PlayerState_Idle(fsm), blackboard);

        bumpedCounter += Time.deltaTime;
    }
}