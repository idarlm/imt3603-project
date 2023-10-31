using System;
using UnityEngine;

namespace Illumination
{
    public sealed class IlluminationManager
    {
         //TODO: Make event driven
        private static PlayerIllumination _illumination;

        public const float ReferenceIllumination = 100f;
        
        public event Action<Vector3, HumanBodyBones> OnIlluminationRequest;

        private static IlluminationManager _instance;
        
        private IlluminationManager() {}
        
        public static IlluminationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new IlluminationManager();
                    _illumination = new PlayerIllumination();
                }
                return _instance;
            }
        }
        
        public void AddIllumination(float illumination, HumanBodyBones limb)
        {
            switch (limb)
            {
                case HumanBodyBones.Head: _illumination.HeadIllumination += illumination;
                    break;
                case HumanBodyBones.Chest: _illumination.ChestIllumination += illumination;
                    break;
                case HumanBodyBones.LeftHand: _illumination.LeftHandIllumination += illumination;
                    break;
                case HumanBodyBones.RightHand: _illumination.RightHandIllumination += illumination;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(limb), limb, null);
            }
        }

        private float GetIlluminationNoUpdate(HumanBodyBones limb)
        {
            return limb switch
            {
                HumanBodyBones.Head => _illumination.HeadIllumination,
                HumanBodyBones.Chest => _illumination.ChestIllumination,
                HumanBodyBones.LeftHand => _illumination.LeftHandIllumination,
                HumanBodyBones.RightHand => _illumination.RightHandIllumination,
                _ => 0.0f
            };
        }

        public void ResetIllumination()
        {
            _illumination = new PlayerIllumination();
        }

        public float GetIllumination(Vector3 limbPosition, HumanBodyBones limb)
        {
            var illumination = GetIlluminationNoUpdate(limb);
            
            if (illumination >= -1.0f) //TODO: Dirty flag
            {
                OnIlluminationRequest?.Invoke(limbPosition, limb);
                return GetIlluminationNoUpdate(limb);
            }
            else
            {
                return GetIlluminationNoUpdate(limb);
            }
        }
    }
}