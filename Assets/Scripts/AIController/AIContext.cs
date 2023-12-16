using Illumination;
using Pathing;
using PlayerMovement;
using UnityEngine;
using UnityEngine.AI;

namespace AIController
{
    public class AIContext
    {
        public NavMeshAgent Agent { set; get; }
        public float MotionDetectionBonus;
        public AIStateMachine StateMachine;
        public PlayerMovementSystem PlayerMovement;
        public Transform Target { set; get; }
        public Animator PlayerAnimator;
        public Waypoint TargetWaypoint;
        public Vector3 LastKnownTargetPosition;
        public float TimeSincePlayerSeen;
        public IlluminationManager PlayerIllumination;
        public float Alertness;
        public Animator RatAnimator;
        public float RunSpeed;
        public float WalkSpeed;
        public Vector3 StartPosition;
        public bool ReverseOrder = false;
        public float Stimuli = 0;
        public float HorizontalFOV;
        public float VerticalFOV;
    }
}