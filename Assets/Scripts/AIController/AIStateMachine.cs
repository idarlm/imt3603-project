using System;
using AIController.Settings;
using Illumination;
using Pathing;
using PlayerMovement;
using StateMachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace AIController
{
    public class AIStateMachine : StateMachineMono<AIContext>
    {
        [SerializeField] private AIStateLabel startState;
        [SerializeField] private Waypoint entryWaypoint;
        [SerializeField] private Animator ratAnimator;
        [SerializeField] public Cage cage;

        private IState<AIContext> _currentState;
        private AIContext _context = new AIContext();
        
        public AttackDetector attackDetector;
        public Transform visionTransform;
        public RatIKController IKController;
        
        private void Start()
        {
            // Randomizes sound playback start position to avoid phase issues
            var sound = GetComponent<AudioSource>();
            sound.time = Random.Range(0, sound.clip.length);
            
            _context = new AIContext
            {
                MotionDetectionBonus = AISettingsManager.Instance.PlayerMovementDetectionBonus,
                PlayerIllumination = IlluminationManager.Instance,
                PlayerMovement = AISettingsManager.Instance.Player,
                PlayerAnimator = AISettingsManager.Instance.PlayerAnimator,
                StateMachine = this,
                TargetWaypoint = entryWaypoint,
                Agent = GetComponent<NavMeshAgent>(),
                Target = AISettingsManager.Instance.Player.transform,
                Alertness = 3.0f,
                RatAnimator = ratAnimator,
                WalkSpeed =  AISettingsManager.Instance.WalkSpeed,
                RunSpeed = AISettingsManager.Instance.RunSpeed,
                StartPosition = transform.position
            };
            
            ChangeState(StateFactory.CreateState(startState));
        }

        public void ChangeState(AIState nextState)
        {
            base.ChangeState(nextState);
            startState = nextState.GetLabel();
        }

        private void Update()
        {
            Execute(_context);
        }

        private void OnDrawGizmos()
        {
            var position = visionTransform.position;
            Handles.Label(position + Vector3.up, startState.ToString());
            Handles.Label(position + Vector3.up * 1.5f, _context.Stimuli.ToString());
        }

        private void OnDrawGizmosSelected()
        {
            if (entryWaypoint != null)
            {
                var waypointPosition = entryWaypoint.transform.position;
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, waypointPosition);
                Gizmos.DrawWireSphere(waypointPosition, 1f);
            }
            
            if (cage != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, cage.GetAITargetPosition());
                Gizmos.DrawWireSphere(cage.GetAITargetPosition(), 1f);
            }
            var position = visionTransform.position;
            var up = visionTransform.up;
            var forward = visionTransform.forward;
            var right = visionTransform.right;
            Handles.DrawWireArc(position, up, forward, _context.HorizontalFOV/2, 5);
            Handles.DrawWireArc(position, up, forward, -_context.HorizontalFOV/2, 5);
            Handles.DrawLine(position, position +  Quaternion.AngleAxis(_context.HorizontalFOV/2, up) * forward * 5);
            Handles.DrawLine(position, position +  Quaternion.AngleAxis(-_context.HorizontalFOV/2, up) * forward * 5);
            
            Handles.DrawWireArc(position, right, forward, _context.VerticalFOV/2, 5);
            Handles.DrawWireArc(position, right, forward, -_context.VerticalFOV/2, 5);
            Handles.DrawLine(position, position +  Quaternion.AngleAxis(_context.VerticalFOV/2, right) * forward * 5);
            Handles.DrawLine(position, position +  Quaternion.AngleAxis(-_context.VerticalFOV/2, right) * forward * 5);
            
            
        }
        
    }
    
}