using GDLib.State;
using System.Collections.Generic;

public class PlayerState_Idle : State
{
    float idleCounter = 0.0f;
    float idleTimeMax;

    public PlayerState_Idle(FSM fsm) : base(fsm) { }

    public override void OnStateEntry(Dictionary<string, object> blackboard)
    {
        if (blackboard.TryGetValue("idleTimeMax", out object obj))
            idleTimeMax = (float)obj;
    }

    public override void OnStateExit(Dictionary<string, object> blackboard)
    {

    }

    public override void UpdateState(Dictionary<string, object> blackboard)
    {
        if (idleCounter <= idleTimeMax)
            fsm.SetState(new PlayerState_Sleeping(fsm), blackboard);

        // Handle input
    }
}