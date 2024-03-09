using GDLib.State;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class PlayerState_InFlight : State
{
    float speedMod;
    float maxPower;
    public PlayerState_InFlight(FSM fsm) : base(fsm) { }

    public override void OnStateEntry(Dictionary<string, object> blackboard)
    {
        object obj;
        if (blackboard.TryGetValue("speedMod", out obj))
            speedMod = (float)obj;

        if (blackboard.TryGetValue("maxPower", out  obj))
            maxPower = (float)obj;

        if (blackboard.TryGetValue("maxPower", out obj))
            maxPower = (float)obj;
    }

    public override void OnStateExit(Dictionary<string, object> blackboard)
    {

    }

    public override void UpdateState(Dictionary<string, object> blackboard) { }
}