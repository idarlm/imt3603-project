

using UnityEngine;

namespace PlayerInput
{
    public class PSPlayerInput : IPlayerInput
    {
        class PS4Button : IPlayerInput.IButton
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

            public PS4Button(string name)
            {
                _name = name;
            }
        }

        private PS4Button _leftDPad;
        private PS4Button _topDPad;
        private PS4Button _rightDPad;
        private PS4Button _downDPad;

        private PS4Button _squareButton;
        private PS4Button _xButton = new ("Fire2");
        private PS4Button _circleButton;
        private PS4Button _triangleButton;

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
            return _squareButton;
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
            return _xButton;
        }

        public IPlayerInput.IButton Menu()
        {
            return _xButton;
        }
    }
}