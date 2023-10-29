using Illumination;
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
        public Animator PlayerAnimator;
        public Vector3[] PatrolWaypoints { set; get; }
        public Waypoint TargetWaypoint;
        public Vector3 LastKnownTargetPosition;
        public float TimeSincePlayerSeen;
        public IlluminationChannel PlayerIllumination;
        public float Alertness;
        public Animator ratAnimator;
        public float runSpeed;
        public float walkSpeed;
        public Vector3 startPosition;
    }
}