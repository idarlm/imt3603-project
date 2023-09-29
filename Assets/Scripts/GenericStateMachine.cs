
public class GenericStateMachine<TContext>
{
    private IState<TContext> _currentState;

    private bool _changeState;
    private IState<TContext> _nextState;

    public IState<TContext> CurrentState => _currentState;

    public void Update(TContext context)
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
    
    public void SetState(IState<TContext> nextState)
    {
        _changeState = true;
        _nextState = nextState;
    }
}
