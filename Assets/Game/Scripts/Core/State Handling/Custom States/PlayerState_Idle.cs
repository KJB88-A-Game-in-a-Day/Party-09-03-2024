using GDLib.State;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : State
{
    float idleCounter = 0.0f;
    float idleTimeMax;

    float speedMod;
    Rigidbody2D rb;
    Transform thisTransform;
    public PlayerState_Idle(FSM fsm) : base(fsm) { }

    public override void OnStateEntry(Dictionary<string, object> blackboard)
    {
        object obj;
        if (blackboard.TryGetValue("idleTimeMax", out obj))
            idleTimeMax = (float)obj;

        if (blackboard.TryGetValue("speedMod", out obj))
            speedMod = (float)obj;

        if (blackboard.TryGetValue("rigidbody2D", out obj))
            rb = (Rigidbody2D)obj;

        if (blackboard.TryGetValue("thisTransform", out obj))
            thisTransform = (Transform)obj;
    }

    public override void OnStateExit(Dictionary<string, object> blackboard) { }

    public override void UpdateState(Dictionary<string, object> blackboard)
    {
        // Handle input
        float x = Input.GetAxis("Horizontal") * speedMod * Time.deltaTime;
        float y = Input.GetAxis("Vertical") * speedMod * Time.deltaTime;

        rb.MovePosition(thisTransform.position + new Vector3(x, y));

        if (Input.GetKeyDown(KeyCode.Space))
            fsm.SetState(new PlayerState_InFlight(fsm), blackboard);

        //if (idleCounter >= idleTimeMax)
        //    fsm.SetState(new PlayerState_Sleeping(fsm), blackboard); // Go to sleep!
        //else
        //    idleCounter += Time.deltaTime; // Accumulate time
    }
}