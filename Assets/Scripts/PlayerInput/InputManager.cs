using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInput;

namespace PlayerInput
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        private IPlayerInput _controllerScheme = new PCPlayerInput();
        
        public Action<IPlayerInput> OnControllerSchemeChange;
    
        
        public InputManager()
        {
            Instance = this;
        }

        public void SetControllerScheme(IPlayerInput controllerScheme)
        {
            _controllerScheme = controllerScheme;
            ControllerSchemeChange();
        }

        public IPlayerInput GetControllerScheme()
        {
            return _controllerScheme;
        }
        
        private void ControllerSchemeChange()
        {
            Debug.Log("InputManager invoking registered hooks");
            OnControllerSchemeChange?.Invoke(_controllerScheme);
        }
    }
}
