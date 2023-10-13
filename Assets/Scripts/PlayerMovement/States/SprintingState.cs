using UnityEngine;

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

        public override void Update(PlayerMovementSystem context)
        {
            base.Update(context);

            _jumpTimer -= Time.deltaTime;
        }

    }
}