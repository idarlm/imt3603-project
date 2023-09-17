using UnityEngine;

namespace PlayerInput
{
    
    public interface IPlayerInput
    {
        public interface IButton
        {
            public bool Pressed();
            public bool Released();
            public bool Held();
        }
        
        Vector2 LeftJoystickXY();
        Vector2 RightJoystickXY();
        IButton LeftPad();
        IButton RightPad();
        IButton UpPad();
        IButton DownPad();
        IButton LeftAction();
        IButton TopAction();
        IButton RightAction();
        IButton BottomAction();
    }
}