using StateMachine;

namespace AIController.ChaseBehaviour
{
    internal class SimpleFollowerState : IAIState
    {
        public string GetName()
        {
            return "simpleFactory";
        }
        public void Enter(AIContext context)
        {

        }

        public void Exit(AIContext context)
        {
            
        }

        public void HandleInput(AIContext context)
        {
            
        }

        public void Update(AIContext context)
        {
            context.Agent.destination = context.Target.position;
        }
    }
}

