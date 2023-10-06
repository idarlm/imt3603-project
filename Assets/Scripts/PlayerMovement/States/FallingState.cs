using UnityEngine;

namespace PlayerMovement
{
    // FallingState defines the behaviour when the player is not on the ground
    // This is mainly applying gravity and letting the player jump.
    internal class PlayerFallingState : PlayerMovementState
    {
        public override void Enter(PlayerMovementSystem context)
        {
            var handler = context.Handler;
            
            context.Falling = true;

            // Add velocity upwards if jump flag is set
            if (context.Input.jump)
            {
                var newVelocity = handler.Velocity;
                newVelocity.y = context.jumpSpeed;
                handler.SetVelocity(newVelocity);

                // Raise Jumped event
                //MovementSystem.Jumped?.Invoke(MovementSystem, MovementSystem.GetEventArgs());
            }

            //MovementSystem.StartFalling?.Invoke(MovementSystem, MovementSystem.GetEventArgs());
        }

        public override void Exit(PlayerMovementSystem context)
        {
            context.Falling = false;
            //MovementSystem.Landed?.Invoke(MovementSystem, MovementSystem.GetEventArgs());
        }

        public override void Update(PlayerMovementSystem context)
        {
            var handler = context.Handler;
            
            float dt = Time.fixedDeltaTime;
            Vector3 movement = handler.Velocity * dt;
            movement += Vector3.down * (context.gravity * dt * dt);

            handler.Move(movement);
        }

        public override void HandleInput(PlayerMovementSystem context)
        {
            if (context.Handler.Grounded)
            {
                context.ChangeState(new PlayerGroundedState());
            }
        }
    }
}