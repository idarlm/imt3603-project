using System;
using PlayerMovement;
using UnityEngine;
using UnityEngine.Serialization;

namespace AIController.Settings
{
    public class AISettingsManager
    {
        private static AISettingsManager _instance;
        
        public static AISettingsManager Instance => _instance ??= new AISettingsManager();

        private float _maxDetectionRange = 5f;
        public float MaxDetectionRange
        {
            set => _maxDetectionRange = Math.Clamp(value, 0, 100);
            get => _maxDetectionRange;
        }
        
        private float _playerMovementDetectionBonus = 20f;
        public float PlayerMovementDetectionBonus
        {
            set => _playerMovementDetectionBonus = Math.Max(value, 1);
            get => _playerMovementDetectionBonus;
        }
        
        private float _playerDetectionThreshold = 0.5f;
        public float PlayerDetectionThreshold
        {
            set => _playerDetectionThreshold = Math.Max(value, 0);
            get => _playerDetectionThreshold;
        }
        
        public Animator PlayerAnimator { set; get; }
        public PlayerMovementSystem Player { set; get; }
        
        private float _runSpeed = 6.0f;
        public float RunSpeed
        {
            set => _runSpeed = Math.Max(value, 1);
            get => _runSpeed;
        }
        
        private float _walkSpeed = 3.0f;
        public float WalkSpeed
        {
            set => _walkSpeed = Math.Max(value, 0);
            get => _walkSpeed;
        }
        
        private float _verticalFOV = 90f;
        public float VerticalFOV
        {
            set => _verticalFOV = Math.Clamp(value, 0, 180);
            get => _verticalFOV;
        }
        
        private float _horizontalFOV = 180f;
        public float HorizontalFOV
        {
            set => _horizontalFOV = Math.Clamp(value, 0, 360);
            get => _horizontalFOV;
        }
        
        
    }
}