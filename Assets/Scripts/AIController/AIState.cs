using Illumination;
using StateMachine;
using UnityEngine;

namespace AIController
{
    public enum AIStateLabel
    {
        Chasing,
        Patrolling,
        Idle
    }
    
    
    public abstract class AIState : IState<AIContext>
    {
        public virtual AIStateLabel GetLabel()
        {
            return AIStateLabel.Idle;
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
            return CanSeeLimb(context, HumanBodyBones.Head) || 
                   CanSeeLimb(context, HumanBodyBones.Chest) ||
                   CanSeeLimb(context, HumanBodyBones.LeftHand) ||
                   CanSeeLimb(context, HumanBodyBones.RightHand);
        }

        protected bool CanSeeLimb(AIContext context, HumanBodyBones bone)
        {
            var thisPosition = context.Agent.transform.position;
            var limbPosition = context.PlayerAnimator.GetBoneTransform(bone).position;// context.Target.position;
            Physics.Raycast(thisPosition , (limbPosition-thisPosition) , out var playerRay);
            Debug.DrawLine(thisPosition, limbPosition);
            Debug.DrawRay(thisPosition,(limbPosition-thisPosition));
            if ((limbPosition - thisPosition).magnitude - playerRay.distance < 0.55 &&
                context.PlayerIllumination.GetIllumination(limbPosition, bone) > 5.0f)
            {
                // Debug.Log( "Bone:" + bone + "    Illumination:" + context.PlayerIllumination.GetIllumination(limbPosition, bone));
            }
            return (limbPosition - thisPosition).magnitude - playerRay.distance < 0.55 && context.PlayerIllumination.GetIllumination(limbPosition, bone) > context.Alertness;
        }

        public virtual float GetSpeedPercentage(AIContext context)
        {
            throw new System.NotImplementedException();
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