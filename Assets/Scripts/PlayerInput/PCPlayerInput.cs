

using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace PlayerInput
{
    public class PCPlayerInput : IPlayerInput
    {
        class PCButton : IPlayerInput.IButton
        {
            private readonly string _name;
            public bool IsPressed()
            {
                return Input.GetButtonDown(_name);
            }
            public bool IsReleased()
            {
                return Input.GetButtonUp(_name);
            }

            public bool IsHeld()
            {
                return Input.GetButton(_name);
            }

            public PCButton(string name)
            {
                _name = name;
            }
        }

        private PCButton _leftDPad;
        private PCButton _topDPad;
        private PCButton _rightDPad;
        private PCButton _downDPad;

        private PCButton _jumpButton = new PCButton("Jump");
        private PCButton _interactButton = new ("Fire2");
        private PCButton _circleButton;
        private PCButton _triangleButton;

        public Vector2 LeftJoystickXY()
        {
            var inputVector = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                inputVector += Vector2.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputVector -= Vector2.up;
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputVector -= Vector2.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputVector += Vector2.right;
            }
            return inputVector.normalized;
        }

        public Vector2 RightJoystickXY()
        {
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        public IPlayerInput.IButton Left()
        {
            return _leftDPad;
        }

        public IPlayerInput.IButton Right()
        {
            return _rightDPad;
        }

        public IPlayerInput.IButton Up()
        {
            return _topDPad;
        }

        public IPlayerInput.IButton Down()
        {
            return _downDPad;
        }

        public IPlayerInput.IButton Confirm()
        {
            return _circleButton;
        }

        public IPlayerInput.IButton Cancel()
        {
            return _triangleButton;
        }

        public IPlayerInput.IButton Interact()
        {
            return _circleButton;
        }

        public IPlayerInput.IButton Jump()
        {
            return _jumpButton;
        }
        
        public IPlayerInput.IButton Menu()
        {
            return _jumpButton;
        }
        
        public IPlayerInput.IButton Crouch()
        {
            return _jumpButton;
        }
        
        public IPlayerInput.IButton Sprint()
        {
            return _jumpButton;
        }
    }
}