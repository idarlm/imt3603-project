using UnityEngine;

namespace PlayerInput
{
    
    public interface IPlayerInput
    {
        public interface IButton
        {
            public bool IsPressed();
            public bool IsReleased();
            public bool IsHeld();
        }
        
        Vector2 LeftJoystickXY();
        Vector2 RightJoystickXY();
        IButton Left();
        IButton Right();
        IButton Up();
        IButton Down();
        IButton Confirm();
        IButton Cancel();
        IButton Interact();
        IButton Jump();
        IButton Menu();
    }
}