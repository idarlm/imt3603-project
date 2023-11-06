using System;
using AIController.ChaseBehaviour;
using AIController.IdleBehaviour;
using AIController.PatrolBehaviour;

namespace AIController
{



    public static class StateFactory
    {
        public static AIState CreateState(AIStateLabel stateName)
        {
            switch (stateName)
            {
                case AIStateLabel.Patrolling: return new WaypointState();
                case AIStateLabel.Chasing: return new SimpleChaseState();
                case AIStateLabel.Idle: return new IdleState();
                case AIStateLabel.Debug: return new DebugState();
                default: throw new Exception("No such chase state");
            }
        }
    }
}