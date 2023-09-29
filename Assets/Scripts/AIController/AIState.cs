using StateMachine;

namespace AIController
{
    public interface IAIState : IState<AIContext>
    {
        string GetName();
    }
}