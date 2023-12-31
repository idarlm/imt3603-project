﻿using FX;
using FX.Visual;
using FX.Visual.Effects;
using PlayerMovement;
using UnityEditor;
using UnityEngine;

namespace AIController.ChaseBehaviour
{
    class CaptureState : AIState
    {
        private static readonly int IsGrabbing = Animator.StringToHash("isGrabbing");
        private float _grabTimer;
        

        public override AIStateLabel GetLabel()
        {
            return AIStateLabel.Chasing;
        }
        
        
        public override void Enter(AIContext context)
        {
            context.RatAnimator.SetBool(IsGrabbing, true);
            AIInteractionFXManager.Instance.OnPlayerGrabbed();
        }
        
        
        public override void Exit(AIContext context)
        {
            context.RatAnimator.SetBool(IsGrabbing, false);
        }

        
        public override float GetSpeedPercentage(AIContext context)
        {
            return context.Agent.velocity.magnitude / context.RunSpeed;
        }
        

        public override void Update(AIContext context)
        {
            _grabTimer += Time.deltaTime;
            context.PlayerMovement.transform.position = context.StateMachine.attackDetector.transform.position;

            if (_grabTimer >= 4f)
            {
                var cage = context.StateMachine.cage;
                cage.Reset();
                
                context.PlayerMovement.transform.position = cage.GetPlayerTargetPosition();
                context.PlayerMovement.Freeze = false;
                
                context.Agent.enabled = false;
                context.Agent.transform.position = cage.GetAITargetPosition();
                context.Agent.enabled = true;
                
                var tempIdle = new AIController.IdleBehaviour.IdleState();
                tempIdle.SetCountdown(2f, AIStateLabel.Patrolling);
                context.StateMachine.ChangeState(tempIdle);
                AIInteractionFXManager.Instance.OnPlayerPlacedInCage();
            }
        }
    }
}