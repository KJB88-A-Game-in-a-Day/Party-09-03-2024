namespace GDLib.State
{
    /// <summary>
    /// Finite State Machine.
    /// It holds the currently updating state using UpdateState.
    /// Current State can be changed by calling SetState, which will handle transition internally.
    /// </summary>
    public class FSM
    {
        private readonly Blackboard blackboard;
        protected Blackboard Blackboard => blackboard;
        protected State currentState;

        public FSM(State state, Blackboard blackboard)
        {
            this.blackboard = blackboard;
            SetState(state);
        }

        public void SetState(State state)
        {
            currentState.OnStateExit(blackboard);

            currentState = state;
            currentState.OnStateEntry(blackboard);
        }

        public void UpdateFSM() => currentState?.UpdateState(blackboard);
    };
}