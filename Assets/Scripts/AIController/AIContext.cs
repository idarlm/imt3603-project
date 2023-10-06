using Pathing;
using UnityEngine;
using UnityEngine.AI;

namespace AIController
{
    public class AIContext
    {
        public NavMeshAgent Agent { set; get; }
        public AIStateMachine StateMachine;
        public Transform Target { set; get; }
        public Vector3[] PatrolWaypoints { set; get; }
        public Waypoint TargetWaypoint;
        public Vector3 LastKnownTargetPosition;
        public float TimeSincePlayerSeen;
    }
}