using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace EventSystem
{
    public class WeightedTriggerArea : MonoBehaviour
    {
        public int id; // id of the trigger
        [SerializeField] private float weightThreshold = 0.5f; // Mass required to trigger area
        private float _currentWeight; // The mass currently colliding with area

        
        // Only results in an event if the mass goes above weightThreshold. 
        public void OnTriggerEnter(Collider other)
        {
            var collidingObject = other.GetComponent<Rigidbody>();
            if (collidingObject != null)
            {
                if (_currentWeight + collidingObject.mass >= weightThreshold && _currentWeight < weightThreshold)
                {
                    WorldEvents.current.DoorwayTriggerEnter(id);
                }
                _currentWeight += collidingObject.mass;
            }
        }
        
        
        // Only results in an event if the mass moves below weightThreshold
        public void OnTriggerExit(Collider other)
        {   
            var collidingObject = other.GetComponent<Rigidbody>();
            if (collidingObject != null)
            {
                if (_currentWeight - collidingObject.mass < weightThreshold && _currentWeight >= collidingObject.mass)
                {
                    WorldEvents.current.DoorwayTriggerExit(id);
                }
                _currentWeight -= collidingObject.mass;
            }

           
        }
    }
}