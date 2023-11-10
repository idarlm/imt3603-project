using LabMaterials;
using Pathing;
using UnityEngine;

namespace AIController.PatrolBehaviour
{
    class WaypointState : AIState
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
            context.Agent.speed = context.walkSpeed;
            if (context.TargetWaypoint)
            {
                context.Agent.destination = context.TargetWaypoint.GetPosition();
            }
            else
            {
                context.Agent.destination = context.startPosition;
            }
            
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
            return context.Agent.velocity.magnitude / context.walkSpeed;
        }


        public bool IsCloseToWaypoint(AIContext context)
        {
            return (context.Agent.transform.position - 
                    context.TargetWaypoint.GetPosition()).sqrMagnitude < _squareSwapDistance;
        }
        
        public override void Update(AIContext context)
        {
            context.ratAnimator.SetFloat(MovementPercentage, GetSpeedPercentage(context));
            if (context.TargetWaypoint)
            {
                context.ratAnimator.SetFloat(MovementPercentage, GetSpeedPercentage(context));
                if (IsCloseToPlayer(context, 30.0f) && CanLocatePlayer(context) )
                {
                    var idle = new AIController.IdleBehaviour.IdleState();
                    idle.SetCountdown(2f, AIStateLabel.Patrolling);
                    context.StateMachine.ChangeState(idle);
                }
                else
                {
                    if (IsCloseToWaypoint(context))
                    {
                        var oldWaypoint = context.TargetWaypoint;
                        if (context.TargetWaypoint.isEndpoint)
                        {
                            context._reverseOrder = !context._reverseOrder;
                        }
                        if (!context._reverseOrder)
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
            else
            {
                if (Vector3.ProjectOnPlane(context.startPosition - context.Agent.transform.position, Vector3.up).sqrMagnitude < 1f)
                {
                    context.StateMachine.ChangeState(StateFactory.CreateState(AIStateLabel.Idle));
                }
            }
        }
    }
}