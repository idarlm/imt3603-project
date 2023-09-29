using UnityEngine;

namespace PlayerMovement
{
    // GroundedState defines the behaviour when the player is on the ground.
    // This includes allowing the player to move around and jump.
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

            var movement = CalculateMovement(context);

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

        private Vector3 CalculateMovement(PlayerMovementSystem context)
        {
            var dt = Time.fixedDeltaTime;
            var input = context.inputVector;
            var settings = context.GetStanceSettings();

            // calculate new direction
            var currentDir = context.Forward;
            var targetDirection = (context.CameraForward * input.y
                                  +context.CameraRight   * input.x).normalized;

            var dir = Vector3.RotateTowards(currentDir, targetDirection, context.turnRate * Mathf.Deg2Rad * dt, 0f);
            context.Forward = dir;

            // calculate speed
            var speed = Vector3.ProjectOnPlane(context.Velocity, Vector3.up).magnitude;

            if (speed < settings.speed * input.magnitude)
            {
                speed += settings.acceleration * dt;
            }
            else if (speed > 0)
            {
                speed -= settings.deceleration * dt;
                speed = speed < 0 ? 0 : speed;
            }

            return dir * (speed * dt);
        }

    }
}