using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;


namespace AIController
{
    /// <summary>
    /// Provides methods for emulating visual impact of objects relative to a forward facing vector.
    /// </summary>
    public static class OcularSimulator
    {
        /**
         * Attenuates a value representing visual stimuli based on distance from the observer.
         * Approximates how perceived size of observed object changes with distance.
         */
        public static float AttenuateByDistance(float distanceToPlayer, float stimuli)
        {
            if (distanceToPlayer > 0)
            {
                return Math.Min(1f, 1f / distanceToPlayer) * stimuli;
            }
            return stimuli;
        }

        /**
         * Attenuates a visual stimuli by the angle between a vector from the observer to the observed object.
         * Simulates reduced visual impact of objects in peripheral vision.
         */
        public static float AttenuateByFOV(Transform headTransform, Vector3 fromObserverToObserved, float maxFOV, float stimuli)
        {
            var lookDirection = headTransform.forward;
            // var leftEye = Quaternion.AngleAxis(-50, Vector3.up) * lookDirection;
            // var rightEye = Quaternion.AngleAxis(50, Vector3.up) * lookDirection;
            //
            // var cosineLeft = Vector3.Dot(leftEye.normalized, fromObserverToObserved.normalized);
            // var cosineRight = Vector3.Dot(rightEye.normalized, fromObserverToObserved.normalized);
            // var cosineAdjustedHorizontal = Math.Cos((maxFOV/2) * Math.PI/180);
            //
            // var FOVAdjusted = (Math.Max(cosineLeft, cosineRight) - cosineAdjustedHorizontal) / (1 - cosineAdjustedHorizontal);
            // return (float)Math.Pow(Math.Max(FOVAdjusted * stimuli, 0f),2);
            
            
            // var verticalProjection = Vector3.ProjectOnPlane(fromObserverToObserved, headTransform.right);
            // var horizontalProjection = Vector3.ProjectOnPlane(fromObserverToObserved, headTransform.up);
            // var cosineThetaVertical = Vector3.Dot(lookDirection.normalized, verticalProjection.normalized);
            // var cosineThetaHorizontal = Vector3.Dot(lookDirection.normalized, horizontalProjection.normalized);
            // var cosineAdjustedHorizontal = Math.Cos((maxFOV/2) * Math.PI/180);
            // var cosineAdjustedVertical = Math.Cos(55 * Math.PI / 180);
            // var horizontalAdjusted = (cosineThetaHorizontal - cosineAdjustedHorizontal) / (1 - cosineAdjustedHorizontal);
            // var verticalAdjusted = (cosineThetaVertical - cosineAdjustedVertical) / (1 - cosineAdjustedVertical);
            // var finalProduct = Math.Max(horizontalAdjusted, 0) * Math.Max(verticalAdjusted, 0);
            // return (float)(finalProduct * stimuli);
            //TODO: Add vertical FOV
            fromObserverToObserved = Vector3.Scale(new Vector3(1,0.2f,1), fromObserverToObserved); 
            var cosineTheta = Vector3.Dot(lookDirection.normalized, fromObserverToObserved.normalized);
            var cosineAdjustedAngle = Math.Cos((maxFOV/2) * Math.PI/180);
            var FOVAdjusted = (cosineTheta - cosineAdjustedAngle) / (1 - cosineAdjustedAngle);  
            
            return (float)Math.Max(FOVAdjusted * stimuli, 0f);
            
            // var cosineTheta = Vector3.Dot(lookDirection.normalized, fromObserverToObserved.normalized);
            // return (float)Math.Max(cosineTheta * stimuli, 0f);
        }
        
    }

   
}