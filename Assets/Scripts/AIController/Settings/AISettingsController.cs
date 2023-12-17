using System;
using PlayerMovement;
using UnityEngine;

namespace AIController.Settings
{
    public class AISettingsController : MonoBehaviour
    {
        [SerializeField] private float maxDetectionRange = 30f;
        [SerializeField] private float playerMovementDetectionBonus = 20f;
        [SerializeField] [Range(0,100)] private float hearingBonus = 5f;
        [SerializeField] private float playerDetectionThreshold = 0.5f;
        [SerializeField] private PlayerMovementSystem player;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] [Range(0,20)] private float runSpeed = 6.0f;
        [SerializeField] [Range(0,20)] private float walkSpeed = 3.0f;
        [SerializeField] [Range(0,180)] private float verticalFOV = 90f;
        [SerializeField] [Range(0,180)] private float horizontalFOV = 180f;
        [SerializeField] [Range(1,10)] private float attackDistance = 2f;
        
        void Start()
        {
            var settingsManager = AISettingsManager.Instance;
            settingsManager.MaxDetectionRange = maxDetectionRange;
            settingsManager.PlayerMovementDetectionBonus = playerMovementDetectionBonus;
            settingsManager.PlayerDetectionThreshold = playerDetectionThreshold;
            settingsManager.Player = player;
            settingsManager.PlayerAnimator = playerAnimator;
            settingsManager.RunSpeed = runSpeed;
            settingsManager.WalkSpeed = walkSpeed;
            settingsManager.VerticalFOV = verticalFOV;
            settingsManager.HorizontalFOV = horizontalFOV;
            settingsManager.AttackDistance = attackDistance;
            settingsManager.HearingBonus = hearingBonus;
        }

        private void OnDrawGizmos()
        {
            var settingsManager = AISettingsManager.Instance;
            settingsManager.VerticalFOV = verticalFOV;
            settingsManager.HorizontalFOV = horizontalFOV;
            settingsManager.MaxDetectionRange = maxDetectionRange;
            settingsManager.PlayerDetectionThreshold = playerDetectionThreshold;
        }
    }
}