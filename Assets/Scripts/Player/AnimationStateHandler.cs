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

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            _characterAnimator.SetBool(IsMoving, _playerController.Velocity.magnitude != 0);
            var frontVector = Vector3.ProjectOnPlane(_playerController.Velocity.normalized, Vector3.up);
            
            if (frontVector != Vector3.zero)
            {
                transform.forward = frontVector;
            }
        }
    }
}

