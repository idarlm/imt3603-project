using UnityEngine;

namespace PlayerMovement
{
    // GroundedState defines the behaviour when the player is on the ground.
    // This includes allowing the player to move around and potentially jump.
    internal class PlayerGroundedState : PlayerMovementState
    {
        MovementHandler Handler;
        PlayerMovementSystem MovementSystem;
        
        public override void Enter(PlayerMovementSystem context)
        {
            MovementSystem = context;
            Handler = context.Handler;
        }

        public override void Exit(PlayerMovementSystem context)
        {
        }

        public override void Update(PlayerMovementSystem context)
        {
            var dt = Time.fixedDeltaTime;

            // inherit velocity from previous frame
            var movement = Handler.Velocity * dt;

            // Don't preserve upwards velocity while grounded.
            // Prevents a bug that can fling the player into the air.
            movement.y = Mathf.Min(0, movement.y);

            var fwd = MovementSystem.Forward;
            var rgt = MovementSystem.Right;
            var input = MovementSystem.inputVector;
            var settings = MovementSystem.GetStanceSettings();

            // calculate acceleration
            float speedFactor = Mathf.Clamp01(settings.speed - Handler.Velocity.magnitude);
            var acceleration = fwd * input.y;
            acceleration += rgt * input.x;
            acceleration *= settings.acceleration * speedFactor * dt * dt;

            movement += acceleration;

            // calculate deceleration
            var deceleration = Handler.Velocity.normalized - acceleration.normalized;
            deceleration *= settings.deceleration * dt;

            deceleration = deceleration.sqrMagnitude > Handler.Velocity.sqrMagnitude
                ? Handler.Velocity
                : deceleration;

            movement -= deceleration * dt;

            if (Handler.ShouldStick)
            {
                movement += Vector3.down;
            }

            Handler.Move(movement);
        }

        public override void HandleInput(PlayerMovementSystem context)
        {
            bool grounded = Handler.Grounded || Handler.ShouldStick;

            if (!grounded || MovementSystem.jumpInput)
            {
                MovementSystem.ChangeState(new PlayerFallingState());
                return;
            }
        }
    }
}