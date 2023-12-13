using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UIElements;

namespace PlayerMovement
{
    internal class SprintingState : PlayerGroundedState
    {
        private float _jumpTimer = 0;
        private bool _shouldJump = false;

        public override void Enter(PlayerMovementSystem context)
        {
            base.Enter(context);
            stanceSettings.speed = context.sprintSpeed;
        }

        internal override void OnStanceChanged(object sender, PlayerMovementEventArgs args)
        {
            base.OnStanceChanged(sender, args);
            stanceSettings.speed = (sender as PlayerMovementSystem).sprintSpeed; // OBS! ingen null check
        }

        public override void HandleInput(PlayerMovementSystem context)
        {
            base.HandleInput(context);
            var input = context.Input;

            if (!input.sprint)
            {
                context.ChangeState(new WalkingState());
                return;
            }

            if (input.jump)
            {
                _shouldJump = true;
                _jumpTimer = context.jumpDelay;

                context.FireEvent(PlayerMovementEvent.PrepareJump);
            }

            if (_shouldJump && _jumpTimer <= 0)
            {
                input.jump = true;
                context.Input = input;

                context.ChangeState(new PlayerFallingState());
            }
        }

        /// <summary>
        /// In the sprint state we want the movement to behave a little differently.
        /// The character should not be able to turn as nimbly as they can when walking.
        /// There will also be a stamina bar that prevents the player from running excessively.
        /// These things will discourage the player from abusing the sprint mechanic.
        /// </summary>
        /// <param name="context"></param>
        public override void Update(PlayerMovementSystem context)
        {
            //base.Update(context);
            var handler = context.Handler;
            var input = context.Input;

            // acceleration
            var targetVelocity = input.joystick.x * context.CameraRight
                + input.joystick.y * context.CameraForward;
            targetVelocity = Vector3.RotateTowards(context.HorizontalVelocity.normalized, targetVelocity, Mathf.Deg2Rad * Time.deltaTime * context.turnRate, 0f) * context.sprintSpeed;
            var delta = targetVelocity - context.HorizontalVelocity;

            var movement = context.Velocity;
            movement += Time.deltaTime * 0.5f * stanceSettings.acceleration * delta;

            if(handler.ShouldStick)
                movement.y = -1f;

            handler.Move(movement * Time.deltaTime);

            context.Forward = context.HorizontalVelocity.normalized;

            _jumpTimer -= Time.deltaTime;
        }

    }
}