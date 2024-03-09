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

        public abstract void OnStateEntry(Blackboard blackboard);
        public abstract void OnStateExit(Blackboard blackboard);

        public abstract void UpdateState(Blackboard blackboard);
    }
}