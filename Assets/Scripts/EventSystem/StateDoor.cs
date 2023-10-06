using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Utility;

namespace EventSystem
{
    public class StateDoorController : MonoBehaviour
    {
        private TriggerCollection _triggerStatus;
        public DoorState CurrentState;
        
        public int[] triggerIDs;
        public float openTime = 1.0f;
        public float closeTime = 3.0f;
        public Vector3 endPositionOffset;
        private Vector3 _startPosition;
        
        
        protected float _animationState;


        public void ChangeState(DoorState newState)
        {
            CurrentState = newState;
        }

        private void Awake()
        {
            CurrentState = new StaticDoorState(this);
            _triggerStatus = new TriggerCollection(triggerIDs);
        }
        
        private void Start()
        {
            _startPosition = transform.position;
            WorldEvents.current.OnDoorwayTriggerEnter += OnDoorwayOpen;
            WorldEvents.current.OnDoorwayTriggerExit += OnDoorwayClose;
        }

        private void FixedUpdate()
        {
            CurrentState?.Execute();
        }

        
        private void OnDoorwayClose(int id)
        {
            ChangeState(new ClosingState(this));
        }

        private void OnDoorwayOpen(int id)
        {
            ChangeState(new OpeningState(this));
        }

        public abstract class DoorState
        {
            protected StateDoorController Door;
            public DoorState(StateDoorController door)
            {
                Door = door;
            }
            public virtual void Enter() {}
            public virtual void Exit() {}
            public virtual void Execute() {}
        }
        
        public class StaticDoorState : DoorState
        {
            public StaticDoorState(StateDoorController door) : base(door)
            {
                Door = door;
            }
            public override void Enter() {}
            public override void Exit() {}
            public override void Execute() {}
        }
        
        private class OpeningState : DoorState
        {
            public OpeningState(StateDoorController door) : base(door) {}
            public override void Enter() {}
            public override void Exit() {}
            public override void Execute()
            {
                Door._animationState = Math.Clamp(
                    Door._animationState + Time.deltaTime * 1/Door.openTime, 0, 1);
                Door.transform.position = Vector3.LerpUnclamped(
                    Door._startPosition, Door._startPosition + Door.endPositionOffset, Easing.InOutCubic(Door._animationState));
                if (Door._animationState >= 1.0f)
                {
                    Door.ChangeState(new StaticDoorState(Door));
                }
            }
        }
        
        private class ClosingState : DoorState
        {
            public ClosingState(StateDoorController door) : base(door) {}
            public override void Enter() {}
            public override void Exit() {}
            public override void Execute()
            {
                Door._animationState = Math.Clamp(
                    Door._animationState + Time.deltaTime * -1/Door.closeTime, 0, 1);
                Door.transform.position = Vector3.LerpUnclamped(
                    Door._startPosition, Door._startPosition + Door.endPositionOffset, Easing.InOutCubic(Door._animationState));
                if (Door._animationState <= 0.0f)
                {
                    Door.ChangeState(new StaticDoorState(Door));
                }
            }
        }
        
    }
}