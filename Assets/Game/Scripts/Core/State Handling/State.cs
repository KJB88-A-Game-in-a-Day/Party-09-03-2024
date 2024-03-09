using System.Collections.Generic;

namespace GDLib.State
{
    /// <summary>
    /// Base class state for all states to derive from. Set as ScriptableObject to allow for modular building-block construction of extensible behaviours
    /// </summary>
    public abstract class State
    {
        protected FSM fsm;

        public State(FSM fsm)
            => this.fsm = fsm;

        public abstract void OnStateEntry(Dictionary<string, object> blackboard);
        public abstract void OnStateExit(Dictionary<string, object> blackboard);

        public abstract void UpdateState(Dictionary<string, object> blackboard);
    }
}