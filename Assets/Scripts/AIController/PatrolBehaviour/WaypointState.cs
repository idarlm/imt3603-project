using LabMaterials;
using Pathing;
using UnityEngine;

namespace AIController.PatrolBehaviour
{
    class WaypointState : AIState
    { 
        private float _swapDistance = 2.0f;
        private float _squareSwapDistance;
        private bool _reverseOrder = false;
        private static readonly int IsPatrolling = Animator.StringToHash("isPatrolling");
        private static readonly int MovementPercentage = Animator.StringToHash("movementPercentage");

        public override AIStateLabel GetLabel()
        {
            return AIStateLabel.Patrolling;
        }
        public override void Enter(AIContext context)
        {
            _squareSwapDistance = _swapDistance * _swapDistance;
            context.Agent.speed = context.walkSpeed;
            context.Agent.destination = context.TargetWaypoint.GetPosition();
            context.ratAnimator.SetBool(IsPatrolling, true);
        }

        public override void Exit(AIContext context)
        {
            context.ratAnimator.SetBool(IsPatrolling, false);
        }

        public override void HandleInput(AIContext context)
        {
            
        }
        
        public override float GetSpeedPercentage(AIContext context)
        {
            return context.Agent.speed / context.walkSpeed;
        }


        public bool IsCloseToWaypoint(AIContext context)
        {
            return (context.Agent.transform.position - 
                    context.TargetWaypoint.GetPosition()).sqrMagnitude < _squareSwapDistance;
        }
        
        public override void Update(AIContext context)
        {
            context.ratAnimator.SetFloat(MovementPercentage, GetSpeedPercentage(context));
            if (IsCloseToPlayer(context, 30.0f) && CanSeePlayer(context) )
            {
                context.StateMachine.ChangeState(StateFactory.CreateState(AIStateLabel.Chasing));
            }
            else
            {
                if (IsCloseToWaypoint(context))
                {
                    var oldWaypoint = context.TargetWaypoint;
                    if (context.TargetWaypoint.isEndpoint)
                    {
                        _reverseOrder = !_reverseOrder;
                    }
                    if (!_reverseOrder)
                    {
                        context.TargetWaypoint = context.TargetWaypoint.GetNext();
                    }
                    else
                    {
                        context.TargetWaypoint = context.TargetWaypoint.GetPrevious();
                    }
                    context.Agent.destination = context.TargetWaypoint.GetPosition();
                    if (oldWaypoint.isEndpoint)
                    {
                        var idleState = new AIController.IdleBehaviour.IdleState();
                        idleState.SetCountdown(5f, AIStateLabel.Patrolling);
                        context.StateMachine.ChangeState(idleState);
                    }
                }
            }
            
        }
    }
}