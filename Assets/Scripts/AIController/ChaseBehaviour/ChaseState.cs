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
        private float _grabTimer;
        private bool _grabbingPlayer;
        private PlayerMovementSystem _player;

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
                _player = player;
                _grabbingPlayer = true;
            }
        }
        
        public override void Enter(AIContext context)
        {
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
            if (_grabbingPlayer)
            {
                context.RatAnimator.SetBool(IsGrabbing, true);
                // _grabbingPlayer = false;
                _grabTimer += Time.deltaTime;
                _player.transform.position = context.StateMachine.attackDetector.transform.position;
                if (_grabTimer >= 4f)
                {
                    var cage = context.StateMachine.cage;
                    _player.transform.position = cage.GetPlayerTargetPosition();
                    _player.Freeze = false;
                    context.RatAnimator.SetBool(IsGrabbing, false);
                    
                    context.Agent.transform.position = cage.GetAITargetPosition();
                    cage.ResetCage();
                    
                    context.StateMachine.ChangeState(StateFactory.CreateState(AIStateLabel.Patrolling));
                    PostProcessingQue.Instance.QueEffect(new FadeToColor(Color.white, 4f));
                }
            }
            else
            {
                context.RatAnimator.SetBool(IsGrabbing, false);
                context.StateMachine.IKController.SetLookAtTarget(context.Target.position);
                context.RatAnimator.SetFloat(MovementPercentage, GetSpeedPercentage(context));
                
                if (CanLocatePlayer(context))
                {
                    context.StateMachine.IKController.EnableLookAt();
                    context.Agent.destination = context.Target.position;
                    context.TimeSincePlayerSeen = 0.0f;
                    if (IsCloseToPlayer(context) && !context.RatAnimator.GetBool(IsAttacking))
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
                    // context.StateMachine.IKController.DisableLookAt();
                }
                if (context.TimeSincePlayerSeen > 5.0)
                {
                    context.StateMachine.ChangeState(StateFactory.CreateState(AIStateLabel.Patrolling));
                }
                _attackTimer -= Time.deltaTime;
                if (_attackTimer < 0f)
                {
                    context.RatAnimator.SetBool(IsAttacking, false);
                    context.RatAnimator.SetBool(IsGrabbing, false);
                    context.Agent.isStopped = false;
                }
                else
                {
                    context.Agent.transform.forward = Vector3.Lerp(
                        context.Agent.transform.forward,
                        Vector3.ProjectOnPlane(
                            context.Target.position - context.Agent.transform.position, Vector3.up),
                        Time.deltaTime * 5
                    );
                }
            }
        }
    }
}

