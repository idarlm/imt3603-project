using UnityEngine;

namespace PlayerMovement
{
    // GroundedState defines the behaviour when the player is on the ground.
    // This includes allowing the player to move around and potentially jump.
    internal class PlayerGroundedState : PlayerMovementState
    {
        public override void Enter(PlayerMovementSystem context)
        {
        }

        public override void Exit(PlayerMovementSystem context)
        {
        }

        public override void Update(PlayerMovementSystem context)
        {
            var handler = context.Handler;
            
            var dt = Time.fixedDeltaTime;

            // inherit velocity from previous frame
            var movement = context.Velocity * dt;

            // Don't preserve upwards velocity while grounded.
            // Prevents a bug that can fling the player into the air.
            movement.y = Mathf.Min(0, movement.y);

            var fwd = context.Forward;
            var rgt = context.Right;
            var input = context.inputVector;
            var settings = context.GetStanceSettings();

            // calculate acceleration
            float speedFactor = Mathf.Clamp01(settings.speed - handler.Velocity.magnitude);
            var acceleration = fwd * input.y;
            acceleration += rgt * input.x;
            acceleration *= settings.acceleration * speedFactor * dt * dt;

            movement += acceleration;

            // calculate deceleration
            var deceleration = handler.Velocity.normalized - acceleration.normalized;
            deceleration *= settings.deceleration * dt;

            deceleration = deceleration.sqrMagnitude > handler.Velocity.sqrMagnitude
                ? handler.Velocity
                : deceleration;

            movement -= deceleration * dt;

            if (handler.ShouldStick)
            {
                movement += Vector3.down;
            }

            handler.Move(movement);
        }

        public override void HandleInput(PlayerMovementSystem context)
        {
            var handler = context.Handler;
            bool grounded = handler.Grounded || handler.ShouldStick;

            if (!grounded || context.jumpInput)
            {
                context.ChangeState(new PlayerFallingState());
                return;
            }
        }
    }
}