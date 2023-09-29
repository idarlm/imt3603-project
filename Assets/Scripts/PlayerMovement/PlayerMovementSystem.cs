using System;
using PlayerInput;
using StateMachine;
using UnityEngine;

namespace PlayerMovement
{

    [RequireComponent(typeof(MovementHandler))]
    public class PlayerMovementSystem : MonoBehaviour
    {

        [Serializable]
        internal struct StanceSettings
        {
            public float speed;
            public float acceleration;
            public float deceleration;
            public float controllerHeight;
            public Vector3 controllerCenter;
        }

        // Public properties
        public Vector3 Velocity => _handler.Velocity; // current velocity
        public float CurrentSpeed => _handler.Velocity.magnitude; // current speed
        public bool Falling { get; internal set; }
        public MovementHandler Handler => _handler;

        public Vector3 Forward
        {
            get
            {
                if (cameraTransform)
                {
                    return Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
                }

                return transform.forward;
            }
        }

        public Vector3 Right
        {
            get
            {
                if (cameraTransform)
                {
                    return Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
                }

                return transform.right;
            }
        }

        // Events
        public event EventHandler<PlayerMovementEventArgs> StartFalling;
        public event EventHandler<PlayerMovementEventArgs> Landed;
        public event EventHandler<PlayerMovementEventArgs> Jumped;
        public event EventHandler<PlayerMovementEventArgs> StanceChanged;

        // Fields 
        [SerializeField] private StanceSettings standingSettings;
        [SerializeField] private StanceSettings crouchingSettings;
        [SerializeField] internal float gravity = 10f;
        [SerializeField] internal float jumpSpeed = 5f;
        [SerializeField] internal float sprintSpeed = 5f;

        [SerializeField] private Transform cameraTransform; // used to determine forward direction
        [SerializeField] private Transform interpolatedBody; // used to smoothly move the body of the player
        private Vector3 _oldPosition; // used for interpolation
        
        // dependencies
        private MovementHandler _handler;
        private GenericStateMachine<PlayerMovementSystem> _stateMachine;

        // input handling
        private IPlayerInput _playerInput = new CombinedInput();
        internal Vector2 inputVector = Vector2.zero;
        internal bool jumpInput;

        // stance
        private bool _crouching;
        private bool _shouldCrouch;
        
        // Public Methods
        internal void ChangeState(PlayerMovementState newState)
        {
            _stateMachine.ChangeState(newState);
        }

        internal ref StanceSettings GetStanceSettings()
        {
            if (_crouching)
                return ref crouchingSettings;

            return ref standingSettings;
        }

        // Unity messages
        private void Start()
        {
            // set up dependencies
            _handler = GetComponent<MovementHandler>();
            _stateMachine = new(new PlayerGroundedState());
        }

        private void Update()
        {
            // input handling
            inputVector = _playerInput.LeftJoystickXY();
            jumpInput |= _playerInput.Jump().IsPressed();
            _shouldCrouch ^= _playerInput.Crouch().IsPressed();

            // Interpolate body
            float t = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
            interpolatedBody.position = Vector3.Lerp(_oldPosition, transform.position, t);

            _stateMachine ??= new(new PlayerGroundedState());
        }

        private void FixedUpdate()
        {
            // set old position
            // used to interpolate body transform
            _oldPosition = transform.position;

            // change stance
            if (_crouching != _shouldCrouch)
            {
                ToggleStance();
            }
            
            // update state machine
            // the state machine performs all movement behaviour
            // like acceleration, etc...
            _stateMachine.Update(this);
            
            // clear input flags
            jumpInput = false; // temp
        }

        // Try to uncrouch if we are crouching,
        // or crouch if we are not.
        //
        // Uncrouching is more complicated because we need to check
        // for obstacles above the player.
        private void ToggleStance()
        {
            if (_crouching)
            {
                // Check if we are blocked from standing up
                var controller = _handler.Controller;

                Ray ray = new(transform.position, Vector3.up);
                var radius = controller.radius + controller.skinWidth;
                var dist = standingSettings.controllerHeight * 0.5f - radius;
                //Debug.DrawLine(transform.position, transform.position + Vector3.up * (dist + radius), Color.red);
                //Debug.DrawLine(transform.position, transform.position + Vector3.right * radius, Color.red);

                // Return if spherecast hits anything other than the player
                var hits = Physics.SphereCastAll(
                    ray,
                    radius,
                    dist,
                    LayerMask.NameToLayer("Player")
                );

                if (hits.Length > 0)
                {
                    return;
                }

                // Stand up
                _crouching = false;
                controller.height = standingSettings.controllerHeight;
                controller.center = standingSettings.controllerCenter;
            }
            else
            {
                // Crouch
                _crouching = true;
                var controller = _handler.Controller;
                controller.height = crouchingSettings.controllerHeight;
                controller.center = crouchingSettings.controllerCenter;
            }

            StanceChanged?.Invoke(this, GetEventArgs());
        }

        private PlayerMovementEventArgs GetEventArgs()
        {
            var args = new PlayerMovementEventArgs();

            args.Velocity = Velocity;
            args.Speed = CurrentSpeed;
            args.FallSpeed = -Mathf.Min(_handler.OldVelocity.y, 0);
            args.Crouching = _crouching;

            return args;
        }
    }
    
    public class PlayerMovementEventArgs : EventArgs
    {
        public bool Falling      { get; set; }
        public Vector3 Velocity  { get; set; }
        public float   Speed     { get; set; }
        public float   FallSpeed { get; set; }
        public bool    Crouching { get; set; }
    }
    
}