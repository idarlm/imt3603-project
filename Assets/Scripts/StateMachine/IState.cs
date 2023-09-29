// Generic state interface meant to be paired with GenericStateMachine
// Context is injected with each function call, instead of at creation.
//
// This allows for reuse of state instances
// in cases where the state behaviour is not inherently "stateful".


namespace StateMachine
{
    public interface IState<TContext>
    {
        void Enter(TContext context);
        void Exit(TContext context);
        void HandleInput(TContext context);
        void Update(TContext context);
    }
}
