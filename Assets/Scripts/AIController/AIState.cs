using System;
using Illumination;
using StateMachine;
using UnityEngine;

namespace AIController
{
    /// <summary>
    /// Label indicating the use case of a particular state.
    /// </summary>
    public enum AIStateLabel
    {
        Chasing,
        Patrolling,
        Idle,
        Debug
    }
    
       
    /// <summary>
    /// Base class for other AIStates. Provides methods that are utilized by several states, such as
    /// functions allowing visual detection of a player character.
    /// </summary>
    public abstract class AIState : IState<AIContext>
    {
        /// <summary>
        /// Returns the AIStateLabel associated with the particular AIState implementation.
        /// </summary>
        /// <returns></returns>
        public virtual AIStateLabel GetLabel()
        {
            return AIStateLabel.Idle;
        }
        

        
        /// <summary>
        /// Checks whether the player is within a sphere with radius equal to thresholdDistance
        /// </summary>
        /// <param name="context">Current AIContext</param>
        /// <param name="thresholdDistance">radius of sphere where player presence is detected</param>
        /// <returns></returns>
        protected bool IsCloseToPlayer(AIContext context, float thresholdDistance)
        {
            var squareDistanceThreshold = thresholdDistance * thresholdDistance;
            return squareDistanceThreshold > Vector3.SqrMagnitude(context.Agent.transform.position - context.Target.position);
        }

        
        /// <summary>
        /// Returns true if there is a clear line of sight from the AI's visualTransform object and the player's
        /// hands, head or chest, provided said limb is sufficiently lit.
        /// </summary>
        /// <param name="context">Current AIContext</param>
        /// <returns></returns>
        protected bool CanLocatePlayer(AIContext context)
        {
            context.Stimuli = 0;
            
            CanSeeLimb(context, HumanBodyBones.Head);
            CanSeeLimb(context, HumanBodyBones.Chest);
            CanSeeLimb(context, HumanBodyBones.LeftHand);
            CanSeeLimb(context, HumanBodyBones.RightHand);
            var movementBonus = context.MotionDetectionBonus * 
                                context.PlayerMovement.CurrentSpeed /
                                  context.PlayerMovement.standingSettings.speed;
            context.Stimuli *= Mathf.Max(movementBonus, 1);
            return context.Stimuli > context.DetectionThreshold;
        }

        
        /// <summary>
        /// Returns true if the player is behind the AI, defined as any position any that would project onto the
        /// negative region of the AI's forward axis.
        /// </summary>
        /// <param name="context">Current AIContext</param>
        /// <param name="distance">How close the player has to be in game units</param>
        /// <returns>True if the player is behind the AI within the given distance</returns>
        protected bool PlayerIsBehind(AIContext context, float distance)
        {
            var transform = context.Agent.transform;
            var agentToPlayer = context.Target.position - transform.position;
            var cosine = Vector3.Dot(
                transform.forward.normalized,
                agentToPlayer.normalized);
            return cosine < 0 && agentToPlayer.magnitude < distance;
        }

        
        /// <summary>
        /// Checks whether a limb is in line of sight, and whether the limb provides enough visual stimuli
        /// for the AI to be detected.
        /// </summary>
        /// <param name="context">current AIContext</param>
        /// <param name="bone">which bone to check for visibility</param>
        /// <returns></returns>
        protected bool CanSeeLimb(AIContext context, HumanBodyBones bone)
        {
            var observationPosition = context.StateMachine.visionTransform.position;
            var limbPosition = context.PlayerAnimator.GetBoneTransform(bone).position;// context.Target.position;
            Physics.Raycast(observationPosition , (limbPosition-observationPosition) , out var playerRay);
            
            var rawStimuli = context.PlayerIllumination.GetIllumination(limbPosition, bone);
            var attenuatedStimuli = OcularSimulator.AttenuateByDistance((observationPosition-limbPosition).magnitude, rawStimuli);
            attenuatedStimuli = OcularSimulator.AttenuateByFOV(
                context.StateMachine.visionTransform, 
                (limbPosition - observationPosition), 
                context.HorizontalFOV, 
                attenuatedStimuli
                );
            var canSeeLimb = (limbPosition - observationPosition).magnitude - playerRay.distance < 0.55;
            Debug.DrawLine(observationPosition, limbPosition, Color.white * (attenuatedStimuli / context.Alertness));
            if(canSeeLimb)
            {
                context.Stimuli += attenuatedStimuli;
            }
        
            return canSeeLimb;
        }

        /// <summary>
        /// Returns the current movement speed relative to the maximum possible movement speed for the current state
        /// </summary>
        /// <param name="context">The current AI context</param>
        /// <returns>float - percentage of movement speed in range [0, 1] </returns>
        public abstract float GetSpeedPercentage(AIContext context);

        public abstract void Enter(AIContext context);

        public abstract void Exit(AIContext context);

        public virtual void HandleInput(AIContext context) {}

        public abstract void Update(AIContext context);
    }
}