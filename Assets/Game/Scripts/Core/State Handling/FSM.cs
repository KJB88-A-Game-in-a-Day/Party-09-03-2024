using System.Collections.Generic;
using UnityEngine;

namespace GDLib.State
{
    /// <summary>
    /// Finite State Machine.
    /// It holds the currently updating state using UpdateState.
    /// Current State can be changed by calling SetState, which will handle transition internally.
    /// </summary>
    public class FSM
    {
        protected State currentState;

        public FSM() { }

        public void SetState(State state, Dictionary<string, object> blackboard)
        {
           currentState?.OnStateExit(blackboard);

            currentState = state;
            currentState.OnStateEntry(blackboard);
        }

        public void UpdateFSM(Dictionary<string, object> blackboard)
        {
            currentState?.UpdateState(blackboard);
        }
    };
}