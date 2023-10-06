using UnityEngine;

namespace StateMachine
{
    public class StateMachineMono<TContext> : MonoBehaviour
    {
        // state management
        private bool _changeState;
        private IState<TContext> _currentState;
        private IState<TContext> _nextState;

        public IState<TContext> CurrentState => _currentState;

        public StateMachineMono()
        {
        }

        public StateMachineMono(IState<TContext> entry)
        {
            ChangeState(entry);
        }
    
        // Update current state
        public void Execute(TContext context)
        {
            _currentState?.HandleInput(context);

            if (_changeState)
            {
                var oldState = _currentState;   // used for stateChangedAction
            
                _currentState?.Exit(context);
                _currentState = _nextState;
                _currentState?.Enter(context);

                _changeState = false;
            }
        
            _currentState?.Update(context);
        }
    
        public void ChangeState(IState<TContext> nextState)
        {
            _changeState = true;
            _nextState = nextState;
        }
    }
}