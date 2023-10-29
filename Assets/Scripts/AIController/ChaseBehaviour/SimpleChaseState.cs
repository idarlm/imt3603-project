using StateMachine;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

namespace AIController.ChaseBehaviour
{
    class SimpleChaseState : AIState
    {
        private static readonly int IsChasing = Animator.StringToHash("isChasing");
        private static readonly int MovementPercentage = Animator.StringToHash("movementPercentage");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private float attackTimer;
        private bool grabbingPlayer;
        private static readonly int IsGrabbing = Animator.StringToHash("isGrabbing");

        public override AIStateLabel GetLabel()
        {
            return AIStateLabel.Chasing;
        }

        private void DetectGrab(float something)
        {
            if (attackTimer > 0f)
            {
                grabbingPlayer = true;
            }
        }
        
        public override void Enter(AIContext context)
        {
            context.Agent.speed = context.runSpeed;
            context.Agent.destination = context.Target.position;
            context.ratAnimator.SetBool(IsChasing, true);

            context.StateMachine.attackDetector.OnPlayerOverlap += DetectGrab;
        }

        public override void Exit(AIContext context)
        {
            context.ratAnimator.SetBool(IsChasing, false);
            context.StateMachine.attackDetector.OnPlayerOverlap -= DetectGrab;
        }

        public override void HandleInput(AIContext context)
        {
            
        }

        public override float GetSpeedPercentage(AIContext context)
        {
            return context.Agent.speed / context.runSpeed;
        }

        public override void Update(AIContext context)
        {
            context.ratAnimator.SetFloat(MovementPercentage, GetSpeedPercentage(context));
            if (CanSeePlayer(context))
            {
                context.Agent.destination = context.Target.position;
                context.TimeSincePlayerSeen = 0.0f;
                if (IsCloseToPlayer(context, 3f) && !context.ratAnimator.GetBool(IsAttacking))
                {
                    context.ratAnimator.SetBool(IsAttacking, true);
                    context.Agent.isStopped = true;
                    attackTimer = 2.0f;
                }
            }
            else
            {
                context.TimeSincePlayerSeen += Time.deltaTime;
            }
            if (context.TimeSincePlayerSeen > 3.0)
            {
                context.StateMachine.ChangeState(StateFactory.CreateState(AIStateLabel.Patrolling));
            }
            attackTimer -= Time.deltaTime;
            if (attackTimer < 0f)
            {
                context.ratAnimator.SetBool(IsAttacking, false);
                context.ratAnimator.SetBool(IsGrabbing, false);
                context.Agent.isStopped = false;
            }

            if (grabbingPlayer)
            {
                context.ratAnimator.SetBool(IsGrabbing, true);
                grabbingPlayer = false;
            }
            else
            {
                context.ratAnimator.SetBool(IsGrabbing, false);
            }
        }
    }
}

