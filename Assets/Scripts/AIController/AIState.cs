using Illumination;
using StateMachine;
using UnityEngine;

namespace AIController
{
    public abstract class AIState : IState<AIContext>
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
            return CanSeeLimb(context, HumanBodyBones.Head) || 
                   CanSeeLimb(context, HumanBodyBones.Chest) ||
                   CanSeeLimb(context, HumanBodyBones.LeftHand) ||
                   CanSeeLimb(context, HumanBodyBones.RightHand);
        }

        protected bool CanSeeLimb(AIContext context, HumanBodyBones bone)
        {
            var thisPosition = context.Agent.transform.position;
            var playerPosition = context.PlayerAnimator.GetBoneTransform(bone).position;// context.Target.position;
            Physics.Raycast(thisPosition , (playerPosition-thisPosition) , out var playerRay);
            Debug.DrawLine(thisPosition, playerPosition);
            Debug.DrawRay(thisPosition,(playerPosition-thisPosition));
            if ((playerPosition - thisPosition).magnitude - playerRay.distance < 0.55 &&
                context.PlayerIllumination.GetIllumination(bone) > 5.0f)
            {
                Debug.Log( "Bone:" + bone + "    Illumination:" + context.PlayerIllumination.GetIllumination(bone));
            }
            return (playerPosition - thisPosition).magnitude - playerRay.distance < 0.55 && context.PlayerIllumination.GetIllumination(bone) > context.Alertness;
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