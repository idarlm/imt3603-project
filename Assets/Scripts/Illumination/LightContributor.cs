using System;
using UnityEngine;

namespace Illumination
{
    public class LightContributor : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private PlayerIlluminationMeasurer playerIllumination;
        private float _range;
        private float _intensity;


        public void Start()
        {
            playerIllumination.OnHeadIlluminationRequest += CalculateLimbIllumination;
            _range = GetComponent<Light>().range;
            _intensity = GetComponent<Light>().intensity;
        }

        public void CalculateLimbIllumination(HumanBodyBones limb)
        {
            var attenuation = 0.0f;
            var position = transform.position;
            var playerPosition = playerAnimator.GetBoneTransform(limb).position;
            var sqrDistance = (playerPosition - position).sqrMagnitude;
            if (sqrDistance < _range*_range)
            {
                Physics.Raycast(position , (playerPosition-position) , out var playerRay);
                if (playerRay.distance + 0.5 - (playerPosition - position).magnitude > 0.0f)
                {
                    attenuation += (float)Math.Pow((_range * _range - sqrDistance)  / (_range * _range), 2);
                    Debug.DrawLine(position,playerRay.point);
                }
            }
            playerIllumination.AddIllumination(attenuation * _intensity, limb);
        }

        public void FixedUpdate()
        {
            // playerIllumination?.AddIllumination(new PlayerIllumination(
            //         head: CalculateLimbIllumination(HumanBodyBones.Head),
            //         chest: CalculateLimbIllumination(HumanBodyBones.Chest),
            //         leftHand: CalculateLimbIllumination(HumanBodyBones.LeftHand),
            //         rightHand: CalculateLimbIllumination(HumanBodyBones.RightHand)
            //     ));
        }
    }
}