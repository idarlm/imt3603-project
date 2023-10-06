using StateMachine;
using UnityEngine;

namespace AIController.ChaseBehaviour
{
    internal class SimpleFollowerState : IAIState
    {
        public override string GetName()
        {
            return "simpleFactory";
        }
        public override void Enter(AIContext context)
        {
            Debug.Log("Entering " + GetName() + " state");
            context.Agent.destination = context.Target.position;
        }

        public override void Exit(AIContext context)
        {
            
        }

        public override void HandleInput(AIContext context)
        {
            
        }

        public override void Update(AIContext context)
        {
            if (CanSeePlayer(context))
            {
                context.Agent.destination = context.Target.position;
                context.TimeSincePlayerSeen = 0.0f;
            }
            else
            {
                context.TimeSincePlayerSeen += Time.deltaTime;
                //Debug.Log(context.TimeSincePlayerSeen + " seconds since player seen");
            }
            if (context.TimeSincePlayerSeen > 3.0)
            {
                context.StateMachine.ChangeState(StateFactory.CreateState("WaypointState"));
            }
        }
    }
}

