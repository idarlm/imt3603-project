using System;
using UnityEngine;

namespace PlayerMovement
{

    [RequireComponent(typeof(MovementHandler))]
    public class PlayerMovementSystem : MonoBehaviour
    {
        private abstract class MovementState
        {
            public MovementState(PlayerMovementSystem system)
            {
                MovementSystem = system;
                Handler = system._handler;
            }

            protected PlayerMovementSystem MovementSystem { get; private set; }
            protected MovementHandler Handler { get; private set; }

            public abstract void Enter();        // Called when entering state
            public abstract void Exit();         // Called when exiting state
            public abstract void Update();       // Called every update cycle
            public abstract void HandleInput();  // Used to handle inputs/state transitions
        }
        
        // Public properties
        public Vector3 Velocity => _handler.Velocity;
        public float CurrentSpeed => _handler.Velocity.magnitude;

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
        public float speed = 5f;
        public float gravity = 10f;

        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform interpolatedBody;
        private Vector3 _oldPosition;

        private MovementHandler _handler;

        private Vector2 _inputVector = Vector2.zero;

        private bool _changeState = false;
        private MovementState _currentState;
        private MovementState _nextState;
        
        // Unity messages
        private void Start()
        {
            _handler = GetComponent<MovementHandler>();
            _currentState = new GroundedState(this);
        }

        private void Update()
        {
            _inputVector = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            
            // Interpolate body
            float t = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
            interpolatedBody.position = Vector3.Lerp(_oldPosition, transform.position, t);
        }

        private void FixedUpdate()
        {
            _oldPosition = transform.position;
            
            _currentState.HandleInput();

            if (_changeState)
            {
                _currentState.Exit();
                _currentState = _nextState;
                _currentState.Enter();

                _changeState = false;
            }

            _currentState.Update();
        }
        
        // Private methods
        private void ChangeState(MovementState newState)
        {
            _nextState = newState;
            _changeState = true;
        }
        
        
        // State classes
        private class GroundedState : MovementState
        {
            public GroundedState(PlayerMovementSystem pms) : base(pms)
            {
            }
            
            public override void Enter()
            {
                Debug.Log("Enter grounded state");
            }

            public override void Exit()
            {
                Debug.Log("Exit grounded state");
            }

            public override void Update()
            {
                var dt = Time.fixedDeltaTime;
                var movement = MovementSystem.Forward * (MovementSystem._inputVector.y * MovementSystem.speed * dt);
                movement += MovementSystem.Right * (MovementSystem._inputVector.x * MovementSystem.speed * dt);
                movement += Vector3.down * (MovementSystem.gravity * dt * dt);

                if (Handler.ShouldStick)
                {
                    movement += Vector3.down;
                }
                
                Handler.Move(movement);
            }

            public override void HandleInput()
            {
                bool grounded = Handler.Grounded || Handler.ShouldStick;

                if (!grounded)
                {
                    MovementSystem.ChangeState(new FallingState(MovementSystem));
                }
            }
        }

        private class FallingState : MovementState
        {
            public FallingState(PlayerMovementSystem pms) : base(pms)
            {
            }


            public override void Enter()
            {
                Debug.Log("Entered FallingState");
            }

            public override void Exit()
            {
                Debug.Log("Exit GroundedState");
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