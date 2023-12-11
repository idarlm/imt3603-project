using System;
using Illumination;
using Pathing;
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
        [SerializeField] private AIStateLabel currentStateSerialized;
        private IState<AIContext> _currentState;
        private AIContext _context = new AIContext();
        [SerializeField] private Waypoint entryWaypoint;
        [SerializeField] private Transform target;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Animator ratAnimator;
        [SerializeField] private float walkSpeed = 3.0f;
        [SerializeField] private float runSpeed = 6.0f;
        public AttackDetector attackDetector;
        public Transform visionTransform;
        public RatIKController IKController;
        [FormerlySerializedAs("FOV")] public float horizontalFOV;
        public float verticalFOV;
        
        private void Start()
        {
            var sound = GetComponent<AudioSource>();
            sound.time = Random.Range(0, sound.clip.length);
            _context = new AIContext
            {
                PlayerIllumination = IlluminationManager.Instance,
                PlayerAnimator = playerAnimator,
                StateMachine = this,
                TargetWaypoint = entryWaypoint,
                Agent = GetComponent<NavMeshAgent>(),
                Target = target.transform,
                Alertness = 3.0f,
                ratAnimator = ratAnimator,
                walkSpeed =  walkSpeed,
                runSpeed = runSpeed,
                startPosition = transform.position
            };
            ChangeState(StateFactory.CreateState(currentStateSerialized));
        }

        public void ChangeState(AIState nextState)
        {
            base.ChangeState(nextState);
            currentStateSerialized = nextState.GetLabel();
        }

        private void Update()
        {
            Execute(_context);
        }

        private void OnDrawGizmos()
        {
            Handles.Label(visionTransform.position + Vector3.up, currentStateSerialized.ToString());
            Handles.Label((visionTransform.position + (Vector3.up * 1.5f)), _context.stimuli.ToString());
        }

        private void OnDrawGizmosSelected()
        {
            if (this.entryWaypoint != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, entryWaypoint.transform.position);
                Gizmos.DrawWireSphere(entryWaypoint.transform.position, 1f);
            }
            
            Handles.DrawWireArc(visionTransform.position, visionTransform.up, visionTransform.forward, horizontalFOV/2, 5);
            Handles.DrawWireArc(visionTransform.position, visionTransform.up, visionTransform.forward, -horizontalFOV/2, 5);
            Handles.DrawLine(visionTransform.position, visionTransform.position +  Quaternion.AngleAxis(horizontalFOV/2, visionTransform.up) * visionTransform.forward * 5);
            Handles.DrawLine(visionTransform.position, visionTransform.position +  Quaternion.AngleAxis(-horizontalFOV/2, visionTransform.up) * visionTransform.forward * 5);
            
            Handles.DrawWireArc(visionTransform.position, visionTransform.right, visionTransform.forward, verticalFOV/2, 5);
            Handles.DrawWireArc(visionTransform.position, visionTransform.right, visionTransform.forward, -verticalFOV/2, 5);
            Handles.DrawLine(visionTransform.position, visionTransform.position +  Quaternion.AngleAxis(verticalFOV/2, visionTransform.right) * visionTransform.forward * 5);
            Handles.DrawLine(visionTransform.position, visionTransform.position +  Quaternion.AngleAxis(-verticalFOV/2, visionTransform.right) * visionTransform.forward * 5);
        }
        
    }
    
}