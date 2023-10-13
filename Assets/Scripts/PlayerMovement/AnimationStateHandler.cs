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
            _playerController.StartFalling += OnStartFalling;
            _playerController.Landed += OnLanded;

            _characterAnimator.SetBool(IsJumping, false);
            _characterAnimator.SetBool(IsFalling, _playerController.Falling);
        }

        // Update is called once per frame
        void Update()
        {            
            var frontVector = _playerController.Forward;
            var playerSpeed = _playerController.CurrentSpeed;
            _characterAnimator.SetBool(IsMoving, playerSpeed != 0);
            _characterAnimator.SetFloat(MovementSpeedPercentage, playerSpeed / 5.0f);
            if (frontVector != Vector3.zero)
            {
                transform.forward = frontVector;
            }
        }

        void OnLanded(object sender, PlayerMovementEventArgs args)
        {
            _characterAnimator.SetBool(IsJumping, false);
            _characterAnimator.SetBool(IsFalling, false);
        }

        void OnStartFalling(object sender, PlayerMovementEventArgs args)
        {
            _characterAnimator.SetBool(IsJumping, args.Jumping);
            _characterAnimator.SetBool(IsFalling, true);
        }
    }
}

