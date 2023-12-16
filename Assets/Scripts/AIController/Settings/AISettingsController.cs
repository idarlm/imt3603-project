using PlayerMovement;
using UnityEngine;

namespace AIController.Settings
{
    public class AISettingsController : MonoBehaviour
    {
        [SerializeField] private float maxDetectionRange = 5f;
        [SerializeField] private float playerMovementDetectionBonus = 20f;
        [SerializeField] private float playerDetectionThreshold = 0.5f;
        [SerializeField] private PlayerMovementSystem player;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private float runSpeed = 6.0f;
        [SerializeField] private float walkSpeed = 3.0f;
        [SerializeField] private float verticalFOV = 90f;
        [SerializeField] private float horizontalFOV = 180f;
        
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
        }
    }
}