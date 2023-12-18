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
                case AIStateLabel.Patrolling: return new PatrolState();
                case AIStateLabel.Chasing: return new ChaseState();
                case AIStateLabel.Idle: return new IdleState();
                case AIStateLabel.Debug: return new DebugState();
                case AIStateLabel.Capture: return new CaptureState();
                default: throw new Exception("No such chase state");
            }
        }
    }
}