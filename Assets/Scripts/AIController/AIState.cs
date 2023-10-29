﻿using Illumination;
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

        protected bool PlayerIsBehind(AIContext context, float distance)
        {
            var agentToPlayer = context.Target.position - context.Agent.transform.position;
            var cosine = Vector3.Dot(context.Agent.transform.forward.normalized,
                agentToPlayer.normalized);
            return cosine < 0 && agentToPlayer.magnitude < distance;
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

            var rawStimuli = context.PlayerIllumination.GetIllumination(limbPosition, bone);
            var attenuatedStimuli = OcularSimulator.AttenuateByDistance(playerRay.distance, rawStimuli);
            attenuatedStimuli = OcularSimulator.AttenuateByFOV(context.StateMachine.visionTransform.forward,
                (limbPosition - thisPosition), attenuatedStimuli);
            return (limbPosition - thisPosition).magnitude - playerRay.distance < 0.55 && attenuatedStimuli > context.Alertness;
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