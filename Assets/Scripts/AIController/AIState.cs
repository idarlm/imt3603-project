using StateMachine;
using UnityEngine;

namespace AIController
{
    public abstract class IAIState : IState<AIContext>
    {
        public virtual string GetName()
        {
            return "";
        }
        protected float SqrDistanceToTarget(AIContext context, Vector3 targetPosition)
        {
            return (context.Agent.transform.position - targetPosition).sqrMagnitude;
        }
        protected bool IsCloseToPlayer(AIContext context, float thresholdDistance)
        {
            return SqrDistanceToTarget(context, context.Target.position) < thresholdDistance * thresholdDistance;
        }

        //TODO: More intelligent logic for player detection
        protected bool CanSeePlayer(AIContext context)
        {
            var thisPosition = context.Agent.transform.position;
            var playerPosition = context.Target.position;
            
            Physics.Raycast(thisPosition , (playerPosition-thisPosition) , out var playerRay);
            Debug.DrawLine(thisPosition, playerPosition);
            Debug.DrawRay(thisPosition,(playerPosition-thisPosition));
            //Debug.Log((playerPosition - thisPosition).magnitude - playerRay.distance);
            return (playerPosition - thisPosition).magnitude - playerRay.distance < 0.55;
        }

        public virtual void Enter(AIContext context)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Exit(AIContext context)
        {
            throw new System.NotImplementedException();
        }

        public virtual void HandleInput(AIContext context)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Update(AIContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}