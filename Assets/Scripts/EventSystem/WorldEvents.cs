using System;
using UnityEngine;

namespace EventSystem
{
    public class WorldEvents : MonoBehaviour
    {

        public static WorldEvents current;

        private void Awake()
        {
            current = this;
        }

        public event Action<int> OnDoorwayTriggerEnter;

        public void DoorwayTriggerEnter(int id)
        {
            OnDoorwayTriggerEnter?.Invoke(id);
        }

        public event Action<int> OnDoorwayTriggerExit;

        public void DoorwayTriggerExit(int id)
        {
            OnDoorwayTriggerExit?.Invoke(id);
        }
    }
}