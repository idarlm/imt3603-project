using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
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

            public PlayerMovementSystem MovementSystem { get; private set; }
            public MovementHandler Handler { get; private set; }

            public abstract void Enter();        // Called when entering state
            public abstract void Exit();         // Called when exiting state
            public abstract void Update();       // Called every update cycle
            public abstract void HandleInput();  // Used to handle inputs/state transitions
        }
        
        public float Speed = 5f;
        public float Gravity = -10f;

        private MovementHandler _handler;

        private Vector3 _inputVector = Vector3.zero;

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
            _inputVector = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }

        private void FixedUpdate()
        {
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
                var movement = MovementSystem._inputVector * MovementSystem.Speed * dt;
                movement += Vector3.up * (MovementSystem.Gravity * dt * dt);

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
                movement += Vector3.up * (MovementSystem.Gravity * dt * dt);
                
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