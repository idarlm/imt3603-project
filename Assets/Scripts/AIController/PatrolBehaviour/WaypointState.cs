using Pathing;
using UnityEngine;

namespace AIController.PatrolBehaviour
{
    internal class WaypointState : IAIState
    { 
        private float _swapDistance = 2.0f;
        private float _squareSwapDistance;
        private bool _reverseOrder = false;
        public string GetName()
        {
            return "waypointState";
        }
        public void Enter(AIContext context)
        {
            _squareSwapDistance = _swapDistance * _swapDistance;
            context.Agent.destination = context.TargetWaypoint.GetPosition();
        }

        public void Exit(AIContext context)
        {
        
        }

        public void HandleInput(AIContext context)
        {
            
        }


        public bool IsCloseToWaypoint(AIContext context)
        {
            return (context.Agent.transform.position - 
                    context.TargetWaypoint.GetPosition()).sqrMagnitude < _squareSwapDistance;
        }

        public void Update(AIContext context)
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