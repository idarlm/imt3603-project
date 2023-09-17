

using UnityEngine;

namespace PlayerInput
{
    public class PSPlayerInput : IPlayerInput
    {
        class PS4Button : IPlayerInput.IButton
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
        private PS4Button _xButton;
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

        public IPlayerInput.IButton LeftPad()
        {
            return _leftDPad;
        }

        public IPlayerInput.IButton RightPad()
        {
            return _rightDPad;
        }

        public IPlayerInput.IButton UpPad()
        {
            return _topDPad;
        }

        public IPlayerInput.IButton DownPad()
        {
            return _downDPad;
        }

        public IPlayerInput.IButton LeftAction()
        {
            return _squareButton;
        }

        public IPlayerInput.IButton TopAction()
        {
            return _triangleButton;
        }

        public IPlayerInput.IButton RightAction()
        {
            return _circleButton;
        }

        public IPlayerInput.IButton BottomAction()
        {
            return _xButton;
        }
    }
}