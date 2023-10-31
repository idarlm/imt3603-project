using System;
using Unity.VisualScripting;
using UnityEngine;

namespace AIController.IdleBehaviour
{
    class IdleState : AIState
    {
        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        private float _idleTime;
        private AIStateLabel _nextState;
        private bool _temporary;
        private float _seenPlayerInSeconds;
        private static readonly int IsBehind = Animator.StringToHash("playerIsBehind");

        public override AIStateLabel GetLabel()
        {
            return AIStateLabel.Idle;
        }
        public override void Enter(AIContext context)
        {
            context.StateMachine.IKController.SetLookAtTarget(context.Target);
            context.StateMachine.IKController.DisableLookAt();
            context.ratAnimator.SetBool(IsIdle, true);
            context.Agent.isStopped = true;
        }

        public override void Exit(AIContext context)
        {
            context.StateMachine.IKController.DisableLookAt();
            context.ratAnimator.SetBool(IsIdle, false);
            context.Agent.isStopped = false;
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
            if (IsCloseToPlayer(context, 30f) && CanSeePlayer(context))
            {
                context.LastKnownTargetPosition = context.Target.position;
                _seenPlayerInSeconds += Time.deltaTime;
                if (_seenPlayerInSeconds > 1f)
                {
                    context.StateMachine.ChangeState(StateFactory.CreateState(AIStateLabel.Chasing));
                }
            }
            else
            {
                _seenPlayerInSeconds = Math.Max(_seenPlayerInSeconds -= Time.deltaTime, 0f);
            }

            if (IsCloseToPlayer(context, 3f) && PlayerIsBehind(context, 3f))
            {
                context.ratAnimator.SetBool(IsBehind, true);
            }
            else
            {
                context.ratAnimator.SetBool(IsBehind, false);
            }
            
            if (_temporary)
            {
                if (_idleTime < 0f)
                {
                    context.StateMachine.ChangeState(StateFactory.CreateState(_nextState));
                }
                _idleTime -= Time.deltaTime;
            }
            
            if (_seenPlayerInSeconds > 0.0f)
            {
                context.StateMachine.IKController.SetLookAtTarget(context.LastKnownTargetPosition);
                context.StateMachine.IKController.EnableLookAt();
            }
            else
            {
                context.StateMachine.IKController.DisableLookAt();
            }
        }
    }
}
