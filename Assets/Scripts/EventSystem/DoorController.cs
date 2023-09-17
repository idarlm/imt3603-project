using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace EventSystem
{
    public class DoorController : MonoBehaviour
    {
        private TriggerCollection _triggerStatus;
        
        public int[] triggerIDs;
        public float openTime = 1.0f;
        public float closeTime = 3.0f;
        public Vector3 endPositionOffset;
        private Vector3 _startPosition;
        private bool _opening;
        private bool _moving;

        
        private float _animationState;


        private void Awake()
        {
            _triggerStatus = new TriggerCollection(triggerIDs);
        }
        private void Start()
        {
            _startPosition = transform.position;
            WorldEvents.current.OnDoorwayTriggerEnter += OnDoorwayOpen;
            WorldEvents.current.OnDoorwayTriggerExit += OnDoorwayClose;
        }

        private void Update()
        {
            if (_moving)
            {
                _animationState = Math.Clamp(
                    _animationState + Time.deltaTime * (_opening? 1 / openTime : -1 / closeTime), 0, 1);
                transform.position = Vector3.Lerp(
                    _startPosition, _startPosition + endPositionOffset, _animationState);
                _moving = (transform.position != _startPosition);
            }
        }

        private void OnDoorwayClose(int id)
        {
            _opening = !_triggerStatus.SetStatus(id, false);
        }

        private void OnDoorwayOpen(int id)
        {
            _triggerStatus.SetStatus(id, true);
            _moving = _opening = _triggerStatus.AllActive();
        }
    }
}