using System;
using FX;
using Unity.VisualScripting;
using UnityEngine;

namespace AIController.IdleBehaviour
{
    class IdleState : AIState
    {
        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        private float _idleTime;
        private AIStateLabel _nextState;
        private bool _idleStateIsTemporary;
        private float _seenPlayerInSeconds;
        

        public override AIStateLabel GetLabel()
        {
            return AIStateLabel.Idle;
        }
        public override void Enter(AIContext context)
        {
            context.StateMachine.IKController.SetLookAtTarget(context.Target);
            context.StateMachine.IKController.DisableLookAt();
            context.RatAnimator.SetBool(IsIdle, true);
            context.Agent.isStopped = true;
        }

        public override void Exit(AIContext context)
        {
            context.StateMachine.IKController.DisableLookAt();
            context.RatAnimator.SetBool(IsIdle, false);
            context.Agent.isStopped = false;
        }
        

        public void SetCountdown(float idleTime, AIStateLabel nextState)
        {
            _idleStateIsTemporary = true;
            _nextState = nextState;
            _idleTime = idleTime;
        }
        
        public override float GetSpeedPercentage(AIContext context)
        {
            return 1f;
        }

        
        
        public override void Update(AIContext context)
        {
            if (_idleStateIsTemporary)
            {
                if (_idleTime < 0f)
                {
                    context.StateMachine.ChangeState(StateFactory.CreateState(_nextState));
                }
                _idleTime -= Time.deltaTime;
            }

            if (IsCloseToPlayer(context, context.MaxDetectionRange) && CanLocatePlayer(context))
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

            CheckForPlayerBehind(context);
            
            
            if (_seenPlayerInSeconds > 0.0f)
            {
                AIInteractionFXManager.Instance.OnLostSightOfPlayerNear();
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
