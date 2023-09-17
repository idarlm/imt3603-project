using UnityEditor;
using UnityEngine;

namespace EventSystem
{
    public class WeightedTriggerArea : MonoBehaviour
    {
        public int id;
        [SerializeField] private float WeightThreshold = 0.5f;
        private float _currentWeight;

        
        public void OnTriggerEnter(Collider other)
        {
            var collidingObject = other.GetComponent<Rigidbody>();
            if (collidingObject != null)
            {
                if (_currentWeight + collidingObject.mass >= WeightThreshold && _currentWeight < WeightThreshold)
                {
                    WorldEvents.current.DoorwayTriggerEnter(id);
                }
                _currentWeight += collidingObject.mass;
            }
        }

        public void OnTriggerExit(Collider other)
        {   
            var collidingObject = other.GetComponent<Rigidbody>();
            if (collidingObject != null)
            {
                _currentWeight -= collidingObject.mass;
            }

            if (_currentWeight < WeightThreshold)
            {
                WorldEvents.current.DoorwayTriggerExit(id);
            }
        }
    }
}