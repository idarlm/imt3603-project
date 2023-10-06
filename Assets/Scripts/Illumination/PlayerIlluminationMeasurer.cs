using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Illumination
{

    public class PlayerIllumination
    {
        public float HeadIllumination = -1;
        public float ChestIllumination = -1;
        public float LeftHandIllumination = -1;
        public float RightHandIllumination = -1;

        public PlayerIllumination(){}
        public PlayerIllumination(float head, float chest, float leftHand, float rightHand)
        {
            HeadIllumination = head;
            ChestIllumination = chest;
            LeftHandIllumination = leftHand;
            RightHandIllumination = rightHand;
        }

        public static PlayerIllumination operator+ (PlayerIllumination a, PlayerIllumination b)
        {
            var sum = a;
            a.RightHandIllumination += b.RightHandIllumination;
            a.LeftHandIllumination += b.LeftHandIllumination;
            a.HeadIllumination += b.HeadIllumination;
            a.ChestIllumination += b.ChestIllumination;
            return a;
        }
    }
    
    
    
    public class PlayerIlluminationMeasurer : MonoBehaviour
    {
        //TODO: Make event driven
        private PlayerIllumination _illumination;
        
        public event Action<HumanBodyBones> OnHeadIlluminationRequest;
        
        public void LateUpdate()
        {
            //Debug.Log("head: " + _illumination.HeadIllumination +"     chest:" + _illumination.ChestIllumination + "     leftHand:" + _illumination.LeftHandIllumination + "     rightHand:" + _illumination.RightHandIllumination);
            _illumination = new PlayerIllumination();
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

        public float GetIllumination(HumanBodyBones limb)
        {
            var illumination = GetIlluminationNoUpdate(limb);
            
            if (illumination >= -1.0f) //TODO: Dirty flag
            {
                OnHeadIlluminationRequest?.Invoke(limb);
                return GetIlluminationNoUpdate(limb);
            }
            else
            {
                return GetIlluminationNoUpdate(limb);
            }
        }
    }
}