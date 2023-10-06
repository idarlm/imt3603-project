using AIController.ChaseBehaviour;
using AIController.PatrolBehaviour;
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
        
        
        private void Start()
        {
            this.
            _context = new AIContext
            {
                TargetWaypoint = entryWaypoint,
                Agent = GetComponent<NavMeshAgent>(),
                Target = target.transform
            };
            ChangeState(StateFactory.CreateState(currentStateSerialized));
        }

        private void ChangeState(IAIState nextState)
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