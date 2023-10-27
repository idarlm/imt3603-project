using UnityEngine;

namespace PlayerMovement
{
    /// <summary>
    /// PlayerGroundedState defines the common behaviour when the player is on the ground.
    /// This behaviour includes moving, and transitioning to the fallingState when appropriate.
    /// 
    /// This is class provides the base implementation for the walking, crouching and sprinting states.
    /// </summary>
    internal abstract class PlayerGroundedState : PlayerMovementState
    {
        /// <summary>
        /// The current stance settings.
        /// </summary>
        protected PlayerMovementSystem.StanceSettings stanceSettings;

        /// <summary>
        /// Did we land this frame?
        /// </summary>
        protected bool landed = false;

        public override void Enter(PlayerMovementSystem context)
        {
            // check if we were falling before entering state
            if (context.Falling)
            {
                context.Falling = false;
                context.FireEvent(PlayerMovementEvent.Landed);
                landed = true;
            }

            stanceSettings = context.GetStanceSettings();
            context.StanceChanged += OnStanceChanged;
        }

        public override void Exit(PlayerMovementSystem context)
        {
            context.StanceChanged -= OnStanceChanged;
        }

        internal virtual void OnStanceChanged(object sender, PlayerMovementEventArgs args)
        {
            var context = sender as PlayerMovementSystem;
            if (context != null)
            {
                stanceSettings = context.GetStanceSettings();
            }
        }

        public override void Update(PlayerMovementSystem context)
        {
            var handler = context.Handler;

            // preserve velocity from last update
            var movement = handler.Velocity * Time.deltaTime;
            movement.y = Mathf.Min(movement.y, 0);

            // calculate acceleration and deceleration
            movement += CalculateAcceleration(context);
            movement -= CalculateDeceleration(context);

            // apply movement penalty when landing
            if(landed)
            {
                movement -= movement * context.landingSpeedPenalty;
                landed = false;
            }

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

            if (!grounded || handler.GroundAngle > handler.Controller.slopeLimit)
            {
                context.ChangeState(new PlayerFallingState());
                return;
            }
        }

        private Vector3 CalculateAcceleration(PlayerMovementSystem context)
        {
            var dt = Time.fixedDeltaTime;
            var input = context.Input.joystick;

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
            return targetDirection * (stanceSettings.acceleration * input.sqrMagnitude * dt * dt);
        }

        private Vector3 CalculateDeceleration(PlayerMovementSystem context)
        {
            var dt = Time.fixedDeltaTime;
            var input = context.Input.joystick;

            // limit speed
            var horizontalVelocity = Vector3.ProjectOnPlane(context.Velocity, Vector3.up);
            var speedLimitF = Mathf.Clamp(horizontalVelocity.sqrMagnitude / (Mathf.Pow(stanceSettings.speed, 2)), 0, 1f);

            var deceleration = context.Velocity * (stanceSettings.acceleration * speedLimitF);

            // calculate decel direction
            var decelDir = context.Forward * (input.sqrMagnitude > 0.1f ? 1 : 0) - horizontalVelocity.normalized;

            deceleration -= decelDir * stanceSettings.deceleration;

            if((deceleration * dt).sqrMagnitude > context.Velocity.sqrMagnitude)
            {
                deceleration = context.Velocity / dt;
            }

            // return deceleration
            return deceleration * dt * dt;
        }
    }
}