using Pathing;
using UnityEngine;

namespace AIController.PatrolBehaviour
{
    internal class WaypointState : AIState
    { 
        private float _swapDistance = 2.0f;
        private float _squareSwapDistance;
        private bool _reverseOrder = false;
        public override string GetName()
        {
            return "waypointState";
        }
        public override void Enter(AIContext context)
        {
            _squareSwapDistance = _swapDistance * _swapDistance;
            context.Agent.destination = context.TargetWaypoint.GetPosition();
        }

        public override void Exit(AIContext context)
        {
        }

        public override void HandleInput(AIContext context)
        {
            
        }


        public bool IsCloseToWaypoint(AIContext context)
        {
            return (context.Agent.transform.position - 
                    context.TargetWaypoint.GetPosition()).sqrMagnitude < _squareSwapDistance;
        }
        
        public override void Update(AIContext context)
        {
            if (IsCloseToPlayer(context, 10.0f) && CanSeePlayer(context) )
            {
                context.StateMachine.ChangeState(StateFactory.CreateState("SimpleFollowerState"));
            }
            else
            {
                if (IsCloseToWaypoint(context))
                {
                    if (context.TargetWaypoint.isEndpoint)
                    {
                        _reverseOrder = !_reverseOrder;
                    }
                    if (!_reverseOrder)
                    {
                        context.TargetWaypoint = context.TargetWaypoint.GetNext();
                    }
                    else
                    {
                        context.TargetWaypoint = context.TargetWaypoint.GetPrevious();
                    }
                    context.Agent.destination = context.TargetWaypoint.GetPosition();
                }
            }
            
        }
    }
}