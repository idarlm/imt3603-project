using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Illumination
{
    public class IlluminationSource : MonoBehaviour
    {
        private Light _light;
        private float _rangeSquared;
        [SerializeField] private float directionalRayCastDistance = 100f;
        [SerializeField] private float directionalIntensity = IlluminationManager.ReferenceIllumination;

        public void Start()
        {
            Activate();
        }

        private float AttenuateSquare(float squareDistanceFromLight)
        {
            return (float)Math.Pow((_rangeSquared - squareDistanceFromLight) / _rangeSquared, 2);
        }

        private void CalculateLimbIllumination(Vector3 limbPosition, HumanBodyBones limb)
        {
            switch (_light.type)
            {
                case LightType.Point:
                {
                    IlluminationManager.Instance.AddIllumination(
                        CalculatePointLightIllumination(limbPosition), 
                        limb);
                }
                    break;
                case LightType.Directional:
                {
                    IlluminationManager.Instance.AddIllumination(
                        CalculateDirectionalIllumination(limbPosition), 
                        limb);
                }
                    break;
                default:
                {
                    
                }
                    break;
            }
            
        }

        private static float AverageOfColor(Color color)
        {
            return (float)(0.33 * color.r + 0.33 * color.g + 0.33 * color.b);
        }

        private float CalculatePointLightIllumination(Vector3 limbPosition)
        {
            var position = transform.position;
            var sqrDistance = (limbPosition - position).sqrMagnitude;
            if (sqrDistance < _rangeSquared)
            {
                Physics.Raycast(position , (limbPosition-position) , out var playerRay);
                if (playerRay.distance + 0.5 - (limbPosition - position).magnitude > 0.0f)
                {
                    var contribution = AttenuateSquare(sqrDistance) * _light.intensity * AverageOfColor(_light.color);
                    Debug.DrawLine(position, playerRay.point, _light.color * (contribution/_light.intensity));
                    return contribution;
                }
            }
            return 0;
        }

        private float CalculateDirectionalIllumination(Vector3 limbPosition)
        {
            var position = limbPosition - _light.transform.forward * IlluminationManager.ReferenceIllumination;
            Physics.Raycast(position , (limbPosition-position) , out var playerRay);
            
            if (playerRay.distance + 0.5 - directionalRayCastDistance > 0.0f)
            {
                Debug.DrawLine(position, playerRay.point, _light.color * _light.intensity);
                return _light.intensity * AverageOfColor(_light.color) * directionalIntensity;
            }
            return 0;
        }

        
        public void Activate()
        {
            _light = GetComponent<Light>();
            if (_light)
            {
                IlluminationManager.Instance.OnIlluminationRequest += CalculateLimbIllumination;
                var range = GetComponent<Light>().range;
                _rangeSquared = range * range;
            }
        }

        public void Deactivate()
        {
            if (_light)
            {
                IlluminationManager.Instance.OnIlluminationRequest -= CalculateLimbIllumination;
            }
        }
    }
}