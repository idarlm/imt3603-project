using System;
using UnityEngine;


namespace AIController
{
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
        public static float AttenuateByFOV(Vector3 lookDirection, Vector3 fromObserverToObserved, float stimuli)
        {
            var cosineTheta = Vector3.Dot(lookDirection.normalized, fromObserverToObserved.normalized);
            return Math.Max(cosineTheta * stimuli, 0f);
        }
    }
}