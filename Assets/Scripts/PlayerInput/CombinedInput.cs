

using System;
using System.Linq;
using UnityEditor.Build;
using UnityEngine;

namespace PlayerInput
{
    public class CombinedInput : IPlayerInput
    {
        public class CombinedButton : IPlayerInput.IButton
        {
            private string[] _names;
            public bool IsPressed()
            {
                return _names.Any(Input.GetButtonDown);
            }

            public bool IsReleased()
            {
                return _names.Any(Input.GetButtonUp);
            }

            public bool IsHeld()
            {
                return _names.Any(Input.GetButton);
            }

            public CombinedButton(string[] names)
            {
                _names = names;
            }
        }
        
        private IPlayerInput _gameController = new PSPlayerInput();
        private IPlayerInput _pcController = new PCPlayerInput();

        private CombinedButton _leftDPad;
        private CombinedButton _topDPad;
        private CombinedButton _rightDPad;
        private CombinedButton _downDPad;

        private CombinedButton _jumpButton = new (new string[]{"Jump"});
        private CombinedButton _interactButton = new (new string[]{"Fire2"});
        private CombinedButton _crouchButton = new (new string[]{"Fire1"});
        private CombinedButton _sprintButton;

        public Vector2 LeftJoystickXY()
        {
            return (_pcController.LeftJoystickXY() + _gameController.LeftJoystickXY());
        }

        public Vector2 RightJoystickXY()
        {
            return (_pcController.RightJoystickXY() + _gameController.RightJoystickXY()).normalized;
        }

        public IPlayerInput.IButton Left()
        {
            return _leftDPad;
        }
        public IPlayerInput.IButton Right(){
            return _leftDPad;
        }
        public IPlayerInput.IButton Up(){
            return _leftDPad;
        }
        public IPlayerInput.IButton Down(){
            return _leftDPad;
        }
        public IPlayerInput.IButton Confirm(){
            return _leftDPad;
        }
        public IPlayerInput.IButton Cancel(){
            return _leftDPad;
        }
        public IPlayerInput.IButton Interact(){
            return _interactButton;
        }
        public IPlayerInput.IButton Jump(){
            return _jumpButton;
        }
        public IPlayerInput.IButton Menu(){
            return _leftDPad;
        }
        
        public IPlayerInput.IButton Crouch()
        {
            return _crouchButton;
        }
        
        public IPlayerInput.IButton Sprint()
        {
            return _jumpButton;
        }
    }
}