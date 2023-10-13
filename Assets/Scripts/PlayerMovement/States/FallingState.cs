using UnityEngine;

namespace PlayerMovement
{
    /// <summary>
    /// PlayerFallingState defines the behaviour when the player is falling.
    /// This includes accelerating the player downwards and some state management.
    /// </summary>
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
                context.FireEvent(PlayerMovementEvent.Jumped);
            }

            context.FireEvent(PlayerMovementEvent.StartFalling);
        }

        public override void Exit(PlayerMovementSystem context)
        {
            
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
                context.ChangeState(new WalkingState());
            }
        }
    }
}