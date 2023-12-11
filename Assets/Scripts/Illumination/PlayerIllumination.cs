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

        public static PlayerIllumination operator+ (PlayerIllumination lhs, PlayerIllumination rhs)
        {
            var sum = lhs;
            lhs.RightHandIllumination += rhs.RightHandIllumination;
            lhs.LeftHandIllumination += rhs.LeftHandIllumination;
            lhs.HeadIllumination += rhs.HeadIllumination;
            lhs.ChestIllumination += rhs.ChestIllumination;
            return lhs;
        }
    }
}