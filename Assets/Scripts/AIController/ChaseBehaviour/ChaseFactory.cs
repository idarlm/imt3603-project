using System;
using StateMachine;

namespace AIController.ChaseBehaviour
{
    public static class ChaseFactory
    {
        public static IAIState CreateChaseState(string stateName)
        {
            switch (stateName)
            {
                case "simpleFollower": return new SimpleFollowerState();
                default: throw new Exception("No such chase state");
            }
            
        }
    }
}