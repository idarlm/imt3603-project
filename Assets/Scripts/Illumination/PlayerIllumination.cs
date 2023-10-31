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
}