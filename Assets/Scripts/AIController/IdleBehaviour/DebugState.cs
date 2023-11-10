using System;
using Unity.VisualScripting;
using UnityEngine;

namespace AIController.IdleBehaviour
{
    class DebugState : AIState
    {
        private static readonly int IsDebugging = Animator.StringToHash("isDebugging");
        private float _idleTime;
        private AIStateLabel _nextState;
        private bool _temporary;
        private float _seenPlayerInSeconds;

        public override AIStateLabel GetLabel()
        {
            return AIStateLabel.Debug;
        }
        public override void Enter(AIContext context)
        {
            
            context.StateMachine.IKController.DisableLookAt();
            context.ratAnimator.SetBool(IsDebugging, true);
            context.Agent.isStopped = true;
        }

        public override void Exit(AIContext context)
        {
            context.ratAnimator.SetBool(IsDebugging, false);
        }
        
        
        public override float GetSpeedPercentage(AIContext context)
        {
            return 1f;
        }
        
        public override void Update(AIContext context)
        {
            context.stimuli = 0;
            CanSeeLimb(context,HumanBodyBones.LeftHand);
            CanSeeLimb(context,HumanBodyBones.RightHand);
            CanSeeLimb(context,HumanBodyBones.Head);
            CanSeeLimb(context,HumanBodyBones.Chest);
            
        }
    }
}
