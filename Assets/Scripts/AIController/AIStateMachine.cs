using System;
using AIController.Settings;
using FX;
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
        [FormerlySerializedAs("startState")] [SerializeField] private AIStateLabel currentStateLabel = AIStateLabel.Patrolling;
        [SerializeField] private Waypoint entryWaypoint;
        [SerializeField] private Animator ratAnimator;
        [SerializeField] public Cage cage;

        private IState<AIContext> _currentState;
        private AIContext _context = new AIContext();
        private float _coolDownTimer = 0;
        
        
        public float DetectionPercentage
        {
            get
            {
                if (_context == null || currentStateLabel == AIStateLabel.Chasing)
                {
                    return 0;
                }
                return Mathf.Clamp(_context.Stimuli / AISettingsManager.Instance.PlayerDetectionThreshold, 0,1);
            }
        }
        
        public AttackDetector attackDetector;
        public Transform visionTransform;
        public RatIKController IKController;
        
        private void Start()
        {
            AIInteractionFXManager.Instance.OnPlayerCapturedAction += OnPlayerCaptured;
            // Randomizes sound playback start position to avoid phase issues
            var sound = GetComponent<AudioSource>();
            sound.time = Random.Range(0, sound.clip.length);
            
            InitContext();
            ChangeState(StateFactory.CreateState(currentStateLabel));
        }

        private void OnDestroy()
        {
            AIInteractionFXManager.Instance.OnPlayerCapturedAction -= OnPlayerCaptured;
        }

        private void InitContext()
        {
            if (AISettingsManager.Instance.AllFieldsInitialized())
            {
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
                    StartPosition = transform.position,
                    HorizontalFOV = AISettingsManager.Instance.HorizontalFOV,
                    VerticalFOV = AISettingsManager.Instance.VerticalFOV,
                    MaxDetectionRange = AISettingsManager.Instance.MaxDetectionRange,
                    DetectionThreshold = AISettingsManager.Instance.PlayerDetectionThreshold,
                    AttackDistance = AISettingsManager.Instance.AttackDistance,
                    HearingBonus = AISettingsManager.Instance.HearingBonus,
                };
            }
            else
            {
                _context = null;
            }
        }

        private void OnPlayerCaptured()
        {
            ChangeState(StateFactory.CreateState(AIStateLabel.Patrolling));
            _context.Stimuli = 0;
            _coolDownTimer = 1f;
        }
        

        public void ChangeState(AIState nextState)
        {
            base.ChangeState(nextState);
            currentStateLabel = nextState.GetLabel();
        }

        private void Update()
        {
            if (_context == null)
            {
                InitContext();
                return;
            }

            if (_coolDownTimer > 0)
            {
                _coolDownTimer -= Time.deltaTime;
                return;
            }
            
            Execute(_context);
        }

        private void OnDrawGizmos()
        {
            var position = visionTransform.position;
            Handles.Label(position + Vector3.up, currentStateLabel.ToString());
            Handles.Label( position + Vector3.up * 1.5f, "Detection: " + Math.Round(_context.Stimuli * 100f / AISettingsManager.Instance.PlayerDetectionThreshold) + "%");
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
            
            var horizontalFOV = AISettingsManager.Instance.HorizontalFOV;
            var verticalFOV = AISettingsManager.Instance.VerticalFOV;
            
            Handles.DrawWireArc(position, up, forward, horizontalFOV/2, 5);
            Handles.DrawWireArc(position, up, forward, -horizontalFOV/2, 5);
            Handles.DrawLine(position, position +  Quaternion.AngleAxis(horizontalFOV/2, up) * forward * 5);
            Handles.DrawLine(position, position +  Quaternion.AngleAxis(-horizontalFOV/2, up) * forward * 5);
            
            Handles.DrawWireArc(position, right, forward, verticalFOV/2, 5);
            Handles.DrawWireArc(position, right, forward, -verticalFOV/2, 5);
            Handles.DrawLine(position, position +  Quaternion.AngleAxis(verticalFOV/2, right) * forward * 5);
            Handles.DrawLine(position, position +  Quaternion.AngleAxis(-verticalFOV/2, right) * forward * 5);
            
            Handles.DrawWireDisc(position, Vector3.up, AISettingsManager.Instance.MaxDetectionRange);
            
            
        }
        
    }
    
}