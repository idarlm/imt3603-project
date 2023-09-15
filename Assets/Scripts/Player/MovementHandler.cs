using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
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
            
            Controller.Move(movement);
            
            // Check if we should stick to the ground
            Ray r = new(transform.position, Vector3.down);
            float dist = (Controller.height * 0.5f) + StickyThreshold;
            ShouldStick = Physics.SphereCast(r, StickyRadius, dist);
        }

        public void SetVelocity(Vector3 velocity)
        {
            
        }

        public void AddVelocity(Vector3 velocity)
        {
            
        }
        
    }

}