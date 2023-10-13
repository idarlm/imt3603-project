using UnityEngine;

namespace PlayerMovement
{
    internal class WalkingState : PlayerGroundedState
    {
        private float _jumpTimer = 0;
        private bool _shouldJump = false;

        // Set possible state transitions when walking
        public override void HandleInput(PlayerMovementSystem context)
        {
            base.HandleInput(context);

            var input = context.Input;

            if (input.crouch)
            {
                context.ChangeState(new CrouchingState());
            }

            if (input.jump)
            {
                _jumpTimer = context.jumpDelay;
                _shouldJump = true;

                context.FireEvent(PlayerMovementEvent.PrepareJump);
            }

            if (_jumpTimer <= 0 && _shouldJump)
            {
                var i = context.Input;
                i.jump = true;
                context.Input = i;
                context.ChangeState(new PlayerFallingState());
            }

            if (input.sprint)
            {
                context.ChangeState(new SprintingState());
            }

            if (input.push)
            {
                var ray = new Ray(context.transform.position, context.Forward);

                if (Physics.SphereCast(ray: ray, radius: 0.5f, maxDistance: 1f, hitInfo: out var hit))
                {
                    var rb = hit.rigidbody;
                    if (rb)
                    {
                        context.pushTarget = rb;
                        context.ChangeState(new PushingObjectState());
                    }
                }
            }
        }

        public override void Update(PlayerMovementSystem context)
        {
            base.Update(context);

            _jumpTimer -= Time.deltaTime;
        }
    }
}