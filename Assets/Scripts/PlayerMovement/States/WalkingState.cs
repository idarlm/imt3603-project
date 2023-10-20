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

            // Crouching
            if (input.crouch)
            {
                context.ChangeState(new CrouchingState());
            }

            // Jumping
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

            // Sprinting
            if (input.sprint)
            {
                context.ChangeState(new SprintingState());
            }

            // Push object
            if (input.push)
            {
                var ray = new Ray(context.transform.position, context.Forward);
                var hits = Physics.SphereCastAll(
                    ray: ray,
                    radius: 1f,
                    maxDistance: 1.5f,
                    layerMask: LayerMask.NameToLayer("Player")
                    );

                foreach (var hit in hits)
                {
                    if (hit.rigidbody)
                    {
                        context.pushTarget = hit.rigidbody;
                        context.ChangeState(new PushingObjectState());
                        break;
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