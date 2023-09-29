using UnityEngine;

namespace PlayerMovement
{
    // FallingState defines the behaviour when the player is not on the ground
    // This is mainly applying gravity and letting the player jump.
    internal class PlayerFallingState : PlayerMovementState
    {
        private MovementHandler Handler;
        private PlayerMovementSystem MovementSystem;
        
        public override void Enter(PlayerMovementSystem context)
        {
            MovementSystem = context;
            Handler = context.Handler;
            
            MovementSystem.Falling = true;

            // Add velocity upwards if jump flag is set
            if (MovementSystem.jumpInput)
            {
                var newVelocity = Handler.Velocity;
                newVelocity.y = MovementSystem.jumpSpeed;
                Handler.SetVelocity(newVelocity);

                // Raise Jumped event
                //MovementSystem.Jumped?.Invoke(MovementSystem, MovementSystem.GetEventArgs());
            }

            //MovementSystem.StartFalling?.Invoke(MovementSystem, MovementSystem.GetEventArgs());
        }

        public override void Exit(PlayerMovementSystem context)
        {
            MovementSystem.Falling = false;
            //MovementSystem.Landed?.Invoke(MovementSystem, MovementSystem.GetEventArgs());
        }

        public override void Update(PlayerMovementSystem context)
        {
            float dt = Time.fixedDeltaTime;
            Vector3 movement = Handler.Velocity * dt;
            movement += Vector3.down * (MovementSystem.gravity * dt * dt);

            Handler.Move(movement);
        }

        public override void HandleInput(PlayerMovementSystem context)
        {
            if (Handler.Grounded)
            {
                MovementSystem.ChangeState(new PlayerGroundedState());
            }
        }
    }
}