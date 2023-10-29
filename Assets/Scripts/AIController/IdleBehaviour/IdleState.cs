using Unity.VisualScripting;
using UnityEngine;

namespace AIController.IdleBehaviour
{
    class IdleState : AIState
    {
        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        private float _idleTime = 0f;
        private AIStateLabel _nextState;
        private bool _temporary = false;

        public override AIStateLabel GetLabel()
        {
            return AIStateLabel.Idle;
        }
        public override void Enter(AIContext context)
        {
            context.ratAnimator.SetBool(IsIdle, true);
            context.Agent.isStopped = true;
            
        }

        public override void Exit(AIContext context)
        {
            context.ratAnimator.SetBool(IsIdle, false);
            context.Agent.isStopped = false;
        }

        public override void HandleInput(AIContext context)
        {
            
        }

        public void SetCountdown(float idleTime, AIStateLabel nextState)
        {
            _temporary = true;
            _nextState = nextState;
            _idleTime = idleTime;
        }
        
        public override float GetSpeedPercentage(AIContext context)
        {
            return 1f;
        }
        
        public override void Update(AIContext context)
        {
            if (this.IsCloseToPlayer(context, 30f) && CanSeePlayer(context))
            {
                context.StateMachine.ChangeState(StateFactory.CreateState(AIStateLabel.Chasing));
            }
            else if (_temporary)
            {
                if (_idleTime < 0f)
                {
                    context.StateMachine.ChangeState(StateFactory.CreateState(_nextState));
                }
                _idleTime -= Time.deltaTime;
            }
        }
    }
}
