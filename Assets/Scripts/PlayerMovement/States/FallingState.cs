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

            // preserve velocity and accelerate downwards
            Vector3 movement = handler.Velocity * dt;
            movement += Vector3.down * (context.gravity * dt * dt);

            // Slide if we are in a steep slope
            if (handler.Grounded)
            {
                // prevent the player from sliding upwards.
                // this stops the player from being launched into
                // the air due to preserving velocity added by the
                // collision resolver.
                movement.y = Mathf.Min(0f, movement.y);

                var acceleration = Vector3.ProjectOnPlane(Vector3.down, handler.GroundNormal);
                acceleration *= context.gravity * dt * dt;

                //remove the vertical component of the acceleration
                //since it is already handled earlier.
                //also helps prevent a bug that slingshots the player.
                acceleration = Vector3.ProjectOnPlane(acceleration, Vector3.up);

                movement += acceleration;
            }

            handler.Move(movement);
        }

        public override void HandleInput(PlayerMovementSystem context)
        {
            var handler = context.Handler;
            if (handler.Grounded && handler.GroundAngle <= handler.Controller.slopeLimit)
            {
                context.ChangeState(new WalkingState());
            }
        }
    }
}