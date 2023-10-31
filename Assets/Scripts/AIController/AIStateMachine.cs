using System;
using Illumination;
using Pathing;
using StateMachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace AIController
{
    public class AIStateMachine : StateMachineMono<AIContext>
    {
        [SerializeField] private AIStateLabel currentStateSerialized;
        private IState<AIContext> _currentState;
        private AIContext _context;
        [SerializeField] private Waypoint entryWaypoint;
        [SerializeField] private Transform target;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Animator ratAnimator;
        [SerializeField] private float walkSpeed = 3.0f;
        [SerializeField] private float runSpeed = 6.0f;
        public AttackDetector attackDetector;
        public Transform visionTransform;
        public RatIKController IKController;
        public float FOV;
        
        private void Start()
        {
            var sound = GetComponent<AudioSource>();
            ;
            sound.time = Random.Range(0, sound.clip.length);
            _context = new AIContext
            {
                PlayerIllumination = IlluminationManager.Instance,
                PlayerAnimator = playerAnimator,
                StateMachine = this,
                TargetWaypoint = entryWaypoint,
                Agent = GetComponent<NavMeshAgent>(),
                Target = target.transform,
                Alertness = 5.0f,
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
        
    }
    
}