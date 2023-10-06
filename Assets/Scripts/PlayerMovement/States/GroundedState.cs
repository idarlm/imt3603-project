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

            var movement = handler.Velocity * Time.deltaTime;
            movement.y = Mathf.Min(movement.y, 0);

            movement += CalculateAcceleration(context);
            movement -= CalculateDeceleration(context);

            if (handler.ShouldStick)
            {
                movement += Vector3.down;
            }

            handler.Move(movement);
        }

        public override void HandleInput(PlayerMovementSystem context)
        {
            var input = context.Input;
            var handler = context.Handler;
            bool grounded = handler.Grounded || handler.ShouldStick;

            if (!grounded || input.jump)
            {
                context.ChangeState(new PlayerFallingState());
                return;
            }

            if (input.push)
            {
                // assign pushTarget
                Ray r = new(context.transform.position, context.Forward);

                var hits = Physics.SphereCastAll(
                    ray: r,
                    radius: 0.2f,
                    maxDistance: 2f,
                    layerMask: LayerMask.NameToLayer("Player")
                );

                foreach ( var hit in hits )
                {
                    if (hit.rigidbody)
                    {
                        context.pushTarget = hit.rigidbody;
                        context.ChangeState(new PushingObjectState());
                    }
                }
            }
        }

        private Vector3 CalculateAcceleration(PlayerMovementSystem context)
        {
            var dt = Time.fixedDeltaTime;
            var input = context.Input.joystick;
            var settings = context.GetStanceSettings();

            // joystick deadzone
            input = input.sqrMagnitude > 0.1f ? input : Vector2.zero;
            input.Normalize();

            // calculate new direction
            var currentDir = context.Forward;
            var targetDirection = (context.CameraForward * input.y
                                  +context.CameraRight   * input.x).normalized;

            var dir = Vector3.RotateTowards(currentDir, targetDirection, context.turnRate * Mathf.Deg2Rad * dt, 0f);
            context.Forward = dir;

            // return acceleration in new direction
            return dir * (settings.acceleration * input.sqrMagnitude * dt * dt);
        }

        private Vector3 CalculateDeceleration(PlayerMovementSystem context)
        {
            var dt = Time.fixedDeltaTime;
            var input = context.Input.joystick;
            var settings = context.GetStanceSettings();

            // limit speed
            var horizontalVelocity = Vector3.ProjectOnPlane(context.Velocity, Vector3.up);
            var speedLimitF = Mathf.Clamp(horizontalVelocity.sqrMagnitude / (Mathf.Pow(settings.speed, 2)), 0, 1f);

            var deceleration = context.Velocity * (settings.acceleration * speedLimitF);

            // calculate decel direction
            var decelDir = context.Forward * (input.sqrMagnitude > 0.1f ? 1 : 0) - horizontalVelocity.normalized;

            deceleration -= decelDir * settings.deceleration;

            if((deceleration * dt).sqrMagnitude > context.Velocity.sqrMagnitude)
            {
                deceleration = context.Velocity / dt;
            }

            // return deceleration
            return deceleration * dt * dt;
        }

    }
}