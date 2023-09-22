using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace EventSystem
{
    public class SlidingDoorController : MonoBehaviour
    {
        private TriggerCollection _triggerStatus;
        
        public int[] triggerIDs;
        public float openTime = 1.0f;
        public float closeTime = 3.0f;
        public Vector3 endPositionOffset;
        private Vector3 _startPosition;
        private bool _opening; // If true, the door is opening. if _opening && _moving, it is closing.
        private bool _moving; // If true, the door is not in its resting position
        private LTDescr _tween;

        
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
            _moving = true;
            _opening = !_triggerStatus.SetStatus(id, false);
            //var remainingTime = Time.time - LeanTween.descr(_tweenID).time;
            /*
            if (_tween != null && _animationState != 0.0f) _animationState -= 
                (openTime * _animationState - _tween.passed) / (openTime * _animationState);
            LeanTween.cancel(gameObject);
            _tween = LeanTween.move(gameObject, _startPosition, closeTime * _animationState);
            */
        }

        private void OnDoorwayOpen(int id)
        {
            _triggerStatus.SetStatus(id, true);
            _moving = _opening = _triggerStatus.AllActive();
            //var remainingTime = Time.time - LeanTween.descr(_tweenID).time;
            /*
            if (_tween != null && _animationState != 1.0f) _animationState -=
                (closeTime * (1.0f - _animationState) - _tween.passed) / closeTime * (1.0f-_animationState);
            LeanTween.cancel(gameObject);
            _tween = LeanTween
                .move(gameObject, _startPosition + endPositionOffset, openTime * (1.0f-_animationState))
                .setEaseInOutCubic();
                */
        }
    }
}