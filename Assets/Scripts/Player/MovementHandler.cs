using System;
using UnityEngine;

namespace PlayerMovement
{

    [RequireComponent(typeof(CharacterController))]
    public class MovementHandler : MonoBehaviour
    {
        // Public properties
        public CharacterController Controller { get; private set; }

        public Vector3 Velocity => Controller.velocity;
        public Vector3 OldVelocity { get; private set; }

        public bool Grounded => Controller.isGrounded;
        public bool ShouldStick { get; private set; }

        // Public fields
        public float StickyThreshold = 0.4f;
        public float StickyRadius = 0.2f;

        // Private fields
        private Vector3 _groundNormal = Vector3.up;

        private bool _addVelocity = false;
        private bool _setVelocity = false;
        private Vector3 _addVelocityVector;
        private Vector3 _setVelocityVector;

        // Unity messages
        private void Start()
        {
            Controller = GetComponent<CharacterController>();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.controller.collisionFlags.HasFlag(CollisionFlags.Below))
            {
                _groundNormal = hit.normal;
            }
        }
        
        // Public methods
        public void Move(Vector3 movement)
        {
            OldVelocity = Controller.velocity;

            if (_addVelocity)
            {
                movement += _addVelocityVector * Time.deltaTime;
                _addVelocityVector = Vector3.zero;
                _addVelocity = false;
            }

            if (_setVelocity)
            {
                movement = _setVelocityVector * Time.deltaTime;
                _setVelocity = false;
            }
            
            Controller.Move(movement);
            
            // Check if we should stick to the ground
            Ray r = new(transform.position, Vector3.down);
            float dist = (Controller.height * 0.5f) + StickyThreshold;
            ShouldStick = Physics.SphereCast(r, StickyRadius, dist);
        }

        public void SetVelocity(Vector3 velocity)
        {
            _setVelocity = true;
            _setVelocityVector = velocity;
        }

        public void AddVelocity(Vector3 velocity)
        {
            _addVelocity = true;
            _addVelocityVector += velocity;
        }
        
    }

}