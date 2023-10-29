using StateMachine;
using UnityEngine;

namespace AIController.ChaseBehaviour
{
    class SimpleChaseState : AIState
    {
        private static readonly int IsChasing = Animator.StringToHash("isChasing");
        private static readonly int MovementPercentage = Animator.StringToHash("movementPercentage");

        public override AIStateLabel GetLabel()
        {
            return AIStateLabel.Chasing;
        }
        public override void Enter(AIContext context)
        {
            context.Agent.speed = context.runSpeed;
            context.Agent.destination = context.Target.position;
            context.ratAnimator.SetBool(IsChasing, true);
        }

        public override void Exit(AIContext context)
        {
            context.ratAnimator.SetBool(IsChasing, false);
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
            }
            else
            {
                context.TimeSincePlayerSeen += Time.deltaTime;
                //Debug.Log(context.TimeSincePlayerSeen + " seconds since player seen");
            }
            if (context.TimeSincePlayerSeen > 3.0)
            {
                context.StateMachine.ChangeState(StateFactory.CreateState(AIStateLabel.Patrolling));
            }
        }
    }
}

