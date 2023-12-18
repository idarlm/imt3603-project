using FX;
using FX.Effects;
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
        

        private void DetectGrab(PlayerMovementSystem player)
        {
            if (_attackTimer > 0f)
            {
                PostProcessingQue.Instance.QueEffect(new FadeToColor(Color.black, 4f));
                player.Freeze = true;
                _context.StateMachine.ChangeState(StateFactory.CreateState(AIStateLabel.Capture));
            }
        }
        
        
        public override void Enter(AIContext context)
        {
            _context = context;
            PostProcessingQue.Instance.QueEffect(new Fear(5));
            
            context.StateMachine.IKController.SetLookAtTarget(context.LastKnownTargetPosition);
            context.StateMachine.IKController.EnableLookAt();
            context.Agent.speed = context.RunSpeed;
            context.Agent.destination = context.Target.position;
            context.RatAnimator.SetBool(IsChasing, true);

            context.StateMachine.attackDetector.OnPlayerOverlap += DetectGrab;
        }
        
        
        public override void Exit(AIContext context)
        {
            context.StateMachine.IKController.DisableLookAt();
            context.RatAnimator.SetBool(IsChasing, false);
            context.StateMachine.attackDetector.OnPlayerOverlap -= DetectGrab;
            context.Agent.isStopped = false;
        }

        
        public override float GetSpeedPercentage(AIContext context)
        {
            return context.Agent.velocity.magnitude / context.RunSpeed;
        }
        
        
        public override void Update(AIContext context)
        {
            // Updates lookAt target and animation speed
            context.StateMachine.IKController.SetLookAtTarget(context.Target.position);
            context.RatAnimator.SetFloat(MovementPercentage, GetSpeedPercentage(context));
            
            if (CanLocatePlayer(context))
            {
                context.StateMachine.IKController.EnableLookAt();
                context.Agent.destination = context.Target.position;
                context.TimeSincePlayerSeen = 0.0f;
                
                if (IsCloseToPlayer(context, context.AttackDistance) && !context.RatAnimator.GetBool(IsAttacking))
                {
                    context.RatAnimator.SetBool(IsAttacking, true);
                    context.Agent.isStopped = true;
                    _attackTimer = 2.0f;
                }
                context.RatAnimator.SetBool(LostSightOfPlayer, false);
            }
            else
            {
                context.RatAnimator.SetBool(LostSightOfPlayer, true);
                context.TimeSincePlayerSeen += Time.deltaTime;
            }
            
            if (context.TimeSincePlayerSeen > 5.0)
            {
                context.StateMachine.ChangeState(StateFactory.CreateState(AIStateLabel.Patrolling));
            }
            
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
    }
}

