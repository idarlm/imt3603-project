using System;
using UnityEngine;
using UnityEngine.UIElements;


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
            if (distanceToPlayer < 0)
            {
                return Math.Min(1f, 1f / distanceToPlayer) * stimuli;
            }
            return stimuli;
        }

        /**
         * Attenuates a visual stimuli by the angle between a vector from the observer to the observed object.
         * Simulates reduced visual impact of objects in peripheral vision.
         */
        public static float AttenuateByFOV(Vector3 lookDirection, Vector3 fromObserverToObserved, float maxFOV, float stimuli)
        {
            //TODO: Consider vertical vs horizontal FOV
            fromObserverToObserved = Vector3.Scale(new Vector3(1,0.2f,1), fromObserverToObserved); 
            var cosineTheta = Vector3.Dot(lookDirection.normalized, fromObserverToObserved.normalized);
            var cosineAdjustedAngle = Math.Cos((maxFOV/2) * Math.PI/180);
            var FOVAdjusted = (cosineTheta - cosineAdjustedAngle) / (1 - cosineAdjustedAngle);  
            
            return (float)Math.Max(FOVAdjusted * stimuli, 0f);
        }
    }

   
}