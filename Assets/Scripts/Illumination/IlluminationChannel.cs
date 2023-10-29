using System;
using UnityEngine;

namespace Illumination
{
    public sealed class IlluminationChannel
    {
         //TODO: Make event driven
        private static PlayerIllumination _illumination;
        
        public event Action<Vector3, HumanBodyBones> OnIlluminationRequest;

        private static IlluminationChannel _instance;
        
        private IlluminationChannel() {}
        
        public static IlluminationChannel Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log("IlluminationChannel instantiated");
                    _instance = new IlluminationChannel();
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
            }
        }

        private float GetIlluminationNoUpdate(HumanBodyBones limb)
        {
            switch (limb)
            {
                case HumanBodyBones.Head:
                    return _illumination.HeadIllumination;
                case HumanBodyBones.Chest:
                    return _illumination.ChestIllumination;
                case HumanBodyBones.LeftHand:  
                    return _illumination.LeftHandIllumination;
                case HumanBodyBones.RightHand:
                    return _illumination.RightHandIllumination;
            }
            return 0.0f;
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