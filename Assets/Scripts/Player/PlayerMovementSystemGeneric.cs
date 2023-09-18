using System;
using PlayerInput;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace PlayerMovement
{

    [RequireComponent(typeof(MovementHandler))]
    public class PlayerMovementSystemGeneric : MonoBehaviour
    {
        private abstract class MovementState
        {
            public MovementState(PlayerMovementSystemGeneric system)
            {
                MovementSystem = system;
                Handler = system._handler;
            }
            
            // reference to the base movement system
            protected PlayerMovementSystemGeneric MovementSystem { get; private set; }
            
            // reference to the attached movement handler
            protected MovementHandler Handler { get; private set; }

            public abstract void Enter();        // Called when entering state
            public abstract void Exit();         // Called when exiting state
            
            // Update is called every frame after input handling and state transitions.
            // This is where behaviour code such as movement logic should go.
            //
            // Example: In the falling state, Update is used to accelerate the player downwards.
            public abstract void Update();
            
            // HandleInput is called every frame before the Update method.
            // HandleInput should be used to perform state changes.
            //
            // Example: if we lose contact with ground we should change to the falling state.
            public abstract void HandleInput();
        }
        
        // Public properties
        public Vector3 Velocity => _handler.Velocity;               // current velocity
        public float CurrentSpeed => _handler.Velocity.magnitude;   // current speed

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
        public event EventHandler Falling;
        public event EventHandler Landed;
        public event EventHandler Jumped;
        
        // Fields 
        [SerializeField] private float speed = 5f;
        [SerializeField] private float gravity = 10f;
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float deceleration = 10f;
        [SerializeField] private float jumpSpeed = 5f;

        private IPlayerInput _playerInput;

        [SerializeField] private Transform cameraTransform;     // used to determine forward direction
        [SerializeField] private Transform interpolatedBody;    // used to smoothly move the body of the player
        private Vector3 _oldPosition;                           // used for interpolation
        
        private MovementHandler _handler;
        
        // state management
        private bool _changeState = false;
        private MovementState _currentState;
        private MovementState _nextState;
        
        // temporary input handling
        private Vector2 _inputVector = Vector2.zero;
        private bool _jump = false;
        
        // Unity messages
        private void Start()
        {
            // set up dependencies
            _playerInput = new PCPlayerInput();
            _handler = GetComponent<MovementHandler>();
            _currentState = new GroundedState(this);
            
            // temp
            Falling += (sender, e) => { Debug.Log("[PlayerMovementSystem] Falling"); };
            Landed += (sender, e) => { Debug.Log("[PlayerMovementSystem] Landed"); };
            Jumped += (sender, e) => { Debug.Log("[PlayerMovementSystem] Jumped"); };
        }

        private void Update()
        {
            // temporary input handling
            _inputVector = _playerInput.LeftJoystickXY();
            _jump |= _playerInput.Jump().IsPressed();
            
            // Interpolate body
            float t = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
            interpolatedBody.position = Vector3.Lerp(_oldPosition, transform.position, t);
            
            // Check if _currentState is null
            // To let us update code without having to reenter playmode.
            _currentState ??= new GroundedState(this);
        }

        private void FixedUpdate()
        {
            // set old position
            // used to interpolate body transform
            _oldPosition = transform.position;
            
            _currentState.HandleInput();
            
            // change active state if needed
            if (_changeState)
            {
                _currentState.Exit();
                _currentState = _nextState;
                _currentState.Enter();

                _changeState = false;
            }

            _currentState.Update();

            _jump = false; // temp
        }
        
        // Private methods
        private void ChangeState(MovementState newState)
        {
            _nextState = newState;
            _changeState = true;
        }
        
        
        // State classes
        
        // GroundedState defines the behaviour when the player is on the ground.
        // This includes allowing the player to move around and potentially jump.
        private class GroundedState : MovementState
        {
            public GroundedState(PlayerMovementSystemGeneric pms) : base(pms)
            {
            }
            
            public override void Enter()
            {
                // Raise Landed event
                // TODO: add event args
                MovementSystem.Landed?.Invoke(MovementSystem, EventArgs.Empty);
            }

            public override void Exit()
            {
                
            }

            public override void Update()
            {
                var dt = Time.fixedDeltaTime;
                
                // inherit velocity from previous frame
                var movement = Handler.Velocity * dt;
                
                // Don't allow player to preserve upwards
                // velocity while grounded.
                // Prevents a bug that can fling the player
                // into the air.
                movement.y = Mathf.Min(0, movement.y);
                
                var fwd = MovementSystem.Forward;
                var rgt = MovementSystem.Right;
                var input = MovementSystem._inputVector;

                // calculate acceleration
                float speedFactor = Mathf.Clamp01(MovementSystem.speed - Handler.Velocity.magnitude);
                var acceleration = fwd * input.y;
                acceleration += rgt * input.x;
                acceleration *= MovementSystem.acceleration * speedFactor * dt * dt;

                movement += acceleration;
                
                // calculate deceleration
                var deceleration = Handler.Velocity.normalized - acceleration.normalized;
                deceleration *= MovementSystem.deceleration * dt;

                deceleration = deceleration.sqrMagnitude > Handler.Velocity.sqrMagnitude
                    ? Handler.Velocity
                    : deceleration;

                movement -= deceleration * dt;

                if (Handler.ShouldStick)
                {
                    movement += Vector3.down;
                }

                Handler.Move(movement);
            }

            public override void HandleInput()
            {
                bool grounded = Handler.Grounded || Handler.ShouldStick;

                if (!grounded || MovementSystem._jump)
                {
                    MovementSystem.ChangeState(new FallingState(MovementSystem));
                }
            }
        }
        
        // FallingState defines the behaviour when the player is not on the ground
        // This is mainly applying gravity and letting the player jump.
        private class FallingState : MovementState
        {
            public FallingState(PlayerMovementSystemGeneric pms) : base(pms)
            {
            }


            public override void Enter()
            {
                // Add velocity upwards if jump flag is set
                if (MovementSystem._jump)
                {
                    var newVelocity = Handler.Velocity;
                    newVelocity.y = MovementSystem.jumpSpeed;
                    Handler.SetVelocity(newVelocity);
                    
                    // Raise Jumped event
                    MovementSystem.Jumped?.Invoke(MovementSystem, EventArgs.Empty);
                }
                
                // Raise Falling event
                // TODO: add event args
                MovementSystem.Falling?.Invoke(MovementSystem, EventArgs.Empty);
            }

            public override void Exit()
            {
                
            }

            public override void Update()
            {
                float dt = Time.fixedDeltaTime;
                Vector3 movement = Handler.Velocity * dt;
                movement += Vector3.down * (MovementSystem.gravity * dt * dt);
                
                Handler.Move(movement);
            }

            public override void HandleInput()
            {
                if (Handler.Grounded)
                {
                    MovementSystem.ChangeState(new GroundedState(MovementSystem));
                    return;
                }
            }
        }
        
    }

}