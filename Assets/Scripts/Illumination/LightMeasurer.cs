using System;
using UnityEngine;

namespace Illumination
{
    public class LightMeasurer : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        private float _range;


        public void Start()
        {
            _range = GetComponent<Light>().range;
        }

        public void Update()
        {
            var position = transform.position;
            var playerPosition = playerAnimator.GetBoneTransform(HumanBodyBones.Head).position;
            var sqrDistance = (playerPosition - position).sqrMagnitude;
            if (sqrDistance < _range*_range)
            {
                var attenuatedIllumination = Math.Pow(_range * _range - sqrDistance  / (_range * _range), 4);
                Physics.Raycast(position , (playerPosition-position) , out var playerRay);
                if (playerRay.distance * playerRay.distance < sqrDistance-0.5f)
                {
                    Debug.DrawRay(position,(playerPosition-position));
                    Debug.Log( attenuatedIllumination );
                }
            }
        }
    }
}