using UnityEngine;

namespace StateMachine
{
    public abstract class StateMono<TContext> : MonoBehaviour
    {
        public virtual void Enter(TContext context){}
        public virtual void Exit(TContext context){}
        public virtual void HandleInput(TContext context){}
        public virtual void Execute(TContext context){}
    }
}