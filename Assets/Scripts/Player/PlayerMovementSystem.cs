using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    [RequireComponent(typeof(MovementHandler))]
    public class PlayerMovementSystem : MonoBehaviour
    {
        private interface IMovementState
        {
            public void Enter();        // Called when entering state
            public void Exit();         // Called when exiting state
            public void Update();       // Called every update cycle
            public void HandleInput();  // Used to handle inputs/state transitions
        }
        
        public float Speed = 5f;
        public float Gravity = -10f;

        private MovementHandler _handler;

        private bool _changeState = false;
        private IMovementState _currentState;
        private IMovementState _nextState;
        
        // Unity messages
        private void Start()
        {
            _handler = GetComponent<MovementHandler>();
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {
            _currentState.HandleInput();

            if (_changeState)
            {
                _currentState.Exit();
                _currentState = _nextState;
                _currentState.Enter();
            }

            _currentState.Update();
        }
        
        // Private methods
        private void ChangeState(IMovementState newState)
        {
            _nextState = newState;
            _changeState = true;
        }
    }

}