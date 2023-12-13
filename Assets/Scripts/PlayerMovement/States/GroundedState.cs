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
            var input = context.Input.joystick;

            // targetDirection is the desired direction of movement
            var targetDirection = input.x * context.CameraRight
                + input.y * context.CameraForward;

            // delta is the difference between the desired velocity and
            // the current velocity and is used to scale the acceleration.
            // It is equivalent to the Proportional part of a PID controller.
            // We don't really need the Integral and Derivative parts for this.
            var delta = stanceSettings.speed * targetDirection - context.HorizontalVelocity;
            if (delta.sqrMagnitude > 1.0f) delta.Normalize();

            movement += stanceSettings.acceleration * Time.deltaTime * Time.deltaTime * delta;

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
            movement += Time.deltaTime * Time.deltaTime * context.gravity * Vector3.down;
            handler.Move(movement);

            // rotate direction of player model
            context.Forward = Vector3.RotateTowards(
                current: context.Forward, 
                target: context.HorizontalVelocity.normalized, 
                maxRadiansDelta: context.turnRate * Mathf.Deg2Rad * Time.deltaTime,
                maxMagnitudeDelta: 0f);
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

    }
}