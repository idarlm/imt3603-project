

using UnityEngine;

namespace PlayerInput
{
    public class PCPlayerInput : IPlayerInput
    {
        class PCButton : IPlayerInput.IButton
        {
            private readonly string _name;
            public bool Pressed()
            {
                return Input.GetButtonDown(_name);
            }
            public bool Released()
            {
                return Input.GetButtonUp(_name);
            }

            public bool Held()
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
            return new Vector2(Input.GetAxis("Joystick1 Horizontal"), Input.GetAxis("Joystick1 Vertical"));
        }

        public Vector2 RightJoystickXY()
        {
            return new Vector2(Input.GetAxis("Joystick2 Horizontal"), Input.GetAxis("Joystick2 Vertical"));
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
    }
}