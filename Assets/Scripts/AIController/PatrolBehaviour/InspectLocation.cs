using AIController.IdleBehaviour;
using FX;
using UnityEngine;

namespace AIController.PatrolBehaviour
{
    public class InspectLocation : AIState
    {
        private float _swapDistance = 2.0f;
        private float _squareSwapDistance;
        private static readonly int IsPatrolling = Animator.StringToHash("isPatrolling");
        private static readonly int MovementPercentage = Animator.StringToHash("movementPercentage");

        public override AIStateLabel GetLabel()
        {
            return AIStateLabel.Patrolling;
        }
        public override void Enter(AIContext context)
        {
            _squareSwapDistance = _swapDistance * _swapDistance;
            context.Agent.speed = context.WalkSpeed;
            context.Agent.destination = context.LastKnownTargetPosition;
            context.RatAnimator.SetBool(IsPatrolling, true);
        }

        public override void Exit(AIContext context)
        {
            context.RatAnimator.SetBool(IsPatrolling, false);
        }

        public override void HandleInput(AIContext context)
        {
            
        }
        
        public override float GetSpeedPercentage(AIContext context)
        {
            return context.Agent.velocity.magnitude / context.WalkSpeed;
        }


        public bool IsCloseToWaypoint(AIContext context)
        {
            return (context.Agent.transform.position - 
                    context.TargetWaypoint.GetPosition()).sqrMagnitude < _squareSwapDistance;
        }
        
        public override void Update(AIContext context)
        {
            context.RatAnimator.SetFloat(MovementPercentage, GetSpeedPercentage(context));

            if (IsCloseToPlayer(context, context.MaxDetectionRange) && CanLocatePlayer(context))
            {
                var idle = new IdleState();
                idle.SetCountdown(2f, AIStateLabel.Patrolling);
                AIInteractionFXManager.Instance.OnPlayerNearlyDetected();
                context.StateMachine.ChangeState(idle);
            }

            // Arrived at location to inspect
            if (Vector3.SqrMagnitude(context.Agent.transform.position - context.LastKnownTargetPosition) < _squareSwapDistance)
            {
                var idle = new IdleState();
                idle.SetCountdown(4f, AIStateLabel.Patrolling);
                context.Alertness = 1;
                context.StateMachine.ChangeState(idle);
            }
        }
    }
}