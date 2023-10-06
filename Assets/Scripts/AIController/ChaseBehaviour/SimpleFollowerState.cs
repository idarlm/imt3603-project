using StateMachine;
using UnityEngine;

namespace AIController.ChaseBehaviour
{
    internal class SimpleFollowerState : AIState
    {
        public override string GetName()
        {
            return "simpleFactory";
        }
        public override void Enter(AIContext context)
        {
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

