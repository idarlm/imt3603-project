using StateMachine;
using UnityEngine;

namespace AIController
{
    public class AIStateMachine : StateMachineMono<AIContext>
    {
        private IState<AIContext> _currentState;
        private AIContext _context;

        [SerializeField] private IState<AIContext>[] _states;
        public enum AIState
        {
            Chasing = 0,
            Patrolling = 1,
            Idle = 2
        }
        
        
        private void Start()
        {
            _currentState = _states[(int)AIState.Idle];
        }

        private void Update()
        {
            _currentState?.Update(_context);
        }
    }
}