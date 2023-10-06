using System;
using Illumination;
using Pathing;
using StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace AIController
{
    public class AIStateMachine : StateMachineMono<AIContext>
    {
        [SerializeField] private string currentStateSerialized;
        private IState<AIContext> _currentState;
        private AIContext _context;
        [SerializeField] private Waypoint entryWaypoint;

        [SerializeField] private string chaseState;

        [SerializeField] private Transform target;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private PlayerIlluminationMeasurer playerIllumination;
        
        private void Start()
        {
            this.
            _context = new AIContext
            {
                PlayerIllumination = playerIllumination,
                PlayerAnimator = playerAnimator,
                StateMachine = this,
                TargetWaypoint = entryWaypoint,
                Agent = GetComponent<NavMeshAgent>(),
                Target = target.transform,
                Alertness = 5.0f
            };
            ChangeState(StateFactory.CreateState(currentStateSerialized));
        }

        public void ChangeState(AIState nextState)
        {
            base.ChangeState(nextState);
            _currentState = nextState;
            _currentState.Enter(_context);
            currentStateSerialized = nextState.GetName();
        }

        private void Update()
        {
            _currentState?.Update(_context);
        }
    }
    
}