using AIController.IdleBehaviour;
using FX;
using FX.Visual;
using FX.Visual.Effects;
using PlayerMovement;
using StateMachine;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

namespace AIController.ChaseBehaviour
{
    class ChaseState : AIState
    {
        private static readonly int IsChasing = Animator.StringToHash("isChasing");
        private static readonly int MovementPercentage = Animator.StringToHash("movementPercentage");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int IsGrabbing = Animator.StringToHash("isGrabbing");
        private static readonly int LostSightOfPlayer = Animator.StringToHash("lostSightOfPlayer");
        private float _attackTimer;
        private AIContext _context;

        public override AIStateLabel GetLabel()
        {
            return AIStateLabel.Chasing;
        }
        
        
        public override void Update(AIContext context)
        {
            // Updates lookAt target and animation speed
            context.StateMachine.IKController.SetLookAtTarget(context.Target.position);
            context.RatAnimator.SetFloat(MovementPercentage, GetSpeedPercentage(context));
            
            if (CanLocatePlayer(context))
            {
                OnVisibleTarget(context);
                context.TimeSincePlayerSeen = 0.0f;
            }
            else
            {
                context.RatAnimator.SetBool(LostSightOfPlayer, true);
                context.TimeSincePlayerSeen += Time.deltaTime;
            }
            
            DisengageOnLostTarget(context, 5f);
            
            if (_attackTimer < 0f) // Attack animation is over. Bad code.
            {
                context.RatAnimator.SetBool(IsAttacking, false);
                context.Agent.isStopped = false;
            }
            else
            { // Rotates towards player
                var transform = context.Agent.transform;
                context.Agent.transform.forward = Vector3.Lerp(
                    transform.forward,
                    Vector3.ProjectOnPlane(
                        context.Target.position - transform.position, Vector3.up),
                    Time.deltaTime * 5
                );
            }
            
            _attackTimer -= Time.deltaTime;
        }
        

        private void DetectGrab(PlayerMovementSystem player)
        {
            if (_attackTimer > 0f && !AIInteractionFXManager.Instance.IsPlayerGrabbed())
            {
                player.Freeze = true;
                _context.StateMachine.ChangeState(StateFactory.CreateState(AIStateLabel.Capture));
            }
        }
        
        
        public override void Enter(AIContext context)
        {
            _context = context;
            
            AIInteractionFXManager.Instance.OnPlayerDetected();
            
            context.StateMachine.IKController.SetLookAtTarget(context.LastKnownTargetPosition);
            context.StateMachine.IKController.EnableLookAt();
            context.Agent.speed = context.RunSpeed;
            context.Agent.destination = context.Target.position;
            context.RatAnimator.SetBool(IsChasing, true);

            context.StateMachine.attackDetector.OnPlayerOverlap += DetectGrab;
        }
        
        
        public override void Exit(AIContext context)
        {
            AIInteractionFXManager.Instance.OnLostSightOfPlayer(); 
            
            context.StateMachine.IKController.DisableLookAt();
            context.RatAnimator.SetBool(IsChasing, false);
            context.StateMachine.attackDetector.OnPlayerOverlap -= DetectGrab;
            context.Agent.isStopped = false;
        }

        
        public override float GetSpeedPercentage(AIContext context)
        {
            return context.Agent.velocity.magnitude / context.RunSpeed;
        }
        
        
        private void DisengageOnLostTarget(AIContext context, float timeToGiveUp)
        {
            if (context.TimeSincePlayerSeen > timeToGiveUp)
            {
                context.StateMachine.ChangeState(StateFactory.CreateState(AIStateLabel.Patrolling));
            }
        }

        private void OnVisibleTarget(AIContext context)
        {
            context.StateMachine.IKController.EnableLookAt();
            context.Agent.destination = context.Target.position;
                
            // If close enough and not already attacking, or player already grabbed, attack.
            if (IsCloseToPlayer(context, context.AttackDistance) && 
                !context.RatAnimator.GetBool(IsAttacking))
            {
                if(AIInteractionFXManager.Instance.IsPlayerGrabbed())
                {
                    var state = new IdleState();
                    state.SetCountdown(1f, AIStateLabel.Chasing);
                    context.StateMachine.ChangeState(state);
                    return;
                }
                context.RatAnimator.SetBool(IsAttacking, true);
                context.Agent.isStopped = true;
                _attackTimer = 2.0f;
            }
            context.RatAnimator.SetBool(LostSightOfPlayer, false);
        }
        
        
       
    }
}

