using System.Collections;
using System.Collections.Generic;
using PlayerMovement;
using UnityEngine;

namespace Player
{
    public class AnimationStateHandler : MonoBehaviour
    {
        [SerializeField] private PlayerMovementSystem _playerController;

        [SerializeField] private Animator _characterAnimator;

        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int IsFalling = Animator.StringToHash("isFalling");
        private static readonly int IsJumping = Animator.StringToHash("isJumping");
        private static readonly int MovementSpeedPercentage = Animator.StringToHash("movementSpeedPercentage");

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            var notTouchingGround = _playerController.Falling;
            var movingUp = _playerController.Velocity.y > 0;
            _characterAnimator.SetBool(IsJumping, movingUp && notTouchingGround);
            _characterAnimator.SetBool(IsFalling, notTouchingGround);
            _characterAnimator.SetBool(IsFalling, notTouchingGround);
            
            var frontVector = _playerController.Forward;
            var playerSpeed = _playerController.CurrentSpeed;
            _characterAnimator.SetBool(IsMoving, playerSpeed != 0);
            _characterAnimator.SetFloat(MovementSpeedPercentage, playerSpeed / 5.0f);
            if (frontVector != Vector3.zero)
            {
                transform.forward = frontVector;
            }
        }
    }
}

