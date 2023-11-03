using UnityEngine;

namespace PlayerMovement
{

    [RequireComponent(typeof(CharacterController))]
    public class MovementHandler : MonoBehaviour
    {
        /// <summary>
        /// The CharacterController to use when performing movement.
        /// </summary>
        public CharacterController Controller { get; private set; }

        /// <summary>
        /// The velocity of last Move operation.
        /// </summary>
        public Vector3 Velocity => Controller.velocity;

        /// <summary>
        /// The previous velocity value.
        /// This can be useful when needing to check the velocity from
        /// before an event happened, such as when the player has landed.
        /// </summary>
        public Vector3 OldVelocity { get; private set; }

        /// <summary>
        /// Is the player currently on the ground?
        /// </summary>
        public bool Grounded => Controller.isGrounded || _groundCheck;

        /// <summary>
        /// Should the player stick to the ground?
        /// </summary>
        public bool ShouldStick { get; private set; }

        /// <summary>
        /// The slope angle of the surface the player is standing on.
        /// </summary>
        public float GroundAngle { get => Vector3.Angle(_groundNormal, Vector3.up); }

        /// <summary>
        /// The normal vector of the surface the player is standing on.
        /// </summary>
        public Vector3 GroundNormal { get => _groundNormal; }

        /// <summary>
        /// The distance to use when checking if the player should
        /// stick to the ground.
        /// </summary>
        [Tooltip("The distance to use when checking if the player should stick to the ground.")]
        public float StickyThreshold = 0.4f;
        /// <summary>
        /// The radius to use when checking 
        /// if the player should stick to the ground.
        /// </summary>
        [Tooltip("The radius to use when checking if the player should stick to the ground.")]
        public float StickyRadius = 0.2f;

        // Private fields
        private Vector3 _groundNormal = Vector3.up;
        private bool _groundCheck;

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
            // Update ground normal
            if (hit.controller.collisionFlags.HasFlag(CollisionFlags.Below))
            {
                // Get ground normal from collission
                _groundNormal = hit.normal;
                var ang = Vector3.Angle(_groundNormal, Vector3.up);
                
                // Check if there is a more reasonable ground normal.
                // This removes a lot of volatility when walking over
                // sharp edges, such as when climbing stairs.
                if(Physics.Raycast(
                    origin: transform.position, 
                    direction: Vector3.down, 
                    maxDistance: Controller.height * 0.5f + StickyThreshold,
                    hitInfo: out var rchit
                    ))
                {
                    _groundNormal = ang < Vector3.Angle(rchit.normal, Vector3.up) 
                        ? _groundNormal 
                        : rchit.normal;
                }
            }
        }
        
        /// <summary>
        /// Moves the player and updates state.
        /// </summary>
        /// <param name="movement">Vector to move by.</param>
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

            // Perform additional ground check - Prevents bug when landing in slopes
            Ray r = new(transform.position, Vector3.down);
            float dist = (Controller.height * 0.5f) + Controller.skinWidth - Controller.radius;
            _groundCheck = ShouldStick = Physics.SphereCast(
                ray: r, 
                radius: Controller.radius, 
                maxDistance: dist,
                layerMask: LayerMask.NameToLayer("Player"));

            // Check if we should stick to the ground
            dist = (Controller.height * 0.5f) + StickyThreshold;
            ShouldStick = Physics.SphereCast(r, StickyRadius, dist);
        }

        /// <summary>
        /// Use the supplied velocity during the next Move call.
        /// The velocity will be multiplied by deltaTime during the call to Move.
        /// 
        /// When called multiple times during one frame,
        /// the latest supplied value will be used.
        /// </summary>
        /// <param name="velocity">Velocity to use during next move.</param>
        public void SetVelocity(Vector3 velocity)
        {
            _setVelocity = true;
            _setVelocityVector = velocity;
        }

        /// <summary>
        /// Add the supplied velocity during the next call to Move.
        /// The velocity will be multiplied by deltaTime during the call to Move.
        /// 
        /// When called multiple times in one frame,
        /// the sum of all added velocities will be used.
        /// </summary>
        /// <param name="velocity"></param>
        public void AddVelocity(Vector3 velocity)
        {
            _addVelocity = true;
            _addVelocityVector += velocity;
        }
        
    }

}