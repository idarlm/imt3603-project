using System;
using AIController.ChaseBehaviour;
using AIController.PatrolBehaviour;

namespace AIController
{
    public static class StateFactory
    {
        public static IAIState CreateState(string stateName)
        {
            switch (stateName)
            {
                case "WaypointState": return new WaypointState();
                case "SimpleFollowerState": return new SimpleFollowerState();
                default: throw new Exception("No such chase state");
            }
        }
    }
}