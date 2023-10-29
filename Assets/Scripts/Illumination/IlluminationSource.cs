using System;
using UnityEngine;

namespace Illumination
{
    public class IlluminationSource : MonoBehaviour
    {
        private float _range;
        private float _intensity;


        public void Start()
        {
            
            IlluminationChannel.Instance.OnIlluminationRequest += CalculateLimbIllumination;
            _range = GetComponent<Light>().range;
            _intensity = GetComponent<Light>().intensity;
        }

        public void CalculateLimbIllumination(Vector3 limbPosition, HumanBodyBones limb)
        {
            var attenuation = 0.0f;
            var position = transform.position;
            var sqrDistance = (limbPosition - position).sqrMagnitude;
            if (sqrDistance < _range*_range)
            {
                Physics.Raycast(position , (limbPosition-position) , out var playerRay);
                if (playerRay.distance + 0.5 - (limbPosition - position).magnitude > 0.0f)
                {
                    attenuation += (float)Math.Pow((_range * _range - sqrDistance)  / (_range * _range), 2);
                    Debug.DrawLine(position,playerRay.point);
                }
            }
            IlluminationChannel.Instance.AddIllumination(attenuation * _intensity, limb);
        }
    }
}