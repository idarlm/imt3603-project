using UnityEngine;

namespace PlayerMovement
{
    /// <summary>
    /// PushingObjectState defines the behaviour for when the player is pushing
    /// a rigidbody.
    /// </summary>
    internal class PushingObjectState : PlayerMovementState
    {
        private Rigidbody _target;
        private bool _detachFromTarget;

        float minDist = 0.2f;
        float maxDist = 0.5f;
        float maxAngle = 45f;
        float distance = 1f;
        float angle = 0f;

        readonly float turnRate = 1f;

        public override void Enter(PlayerMovementSystem context)
        {
            _target = context.pushTarget;
        }

        public override void Exit(PlayerMovementSystem context)
        {

        }

        public override void Update(PlayerMovementSystem context)
        {
            var input = context.Input.joystick;

            // Get directions based on position relative to target
            var targetDir = _target.position - context.transform.position;
            var fwd = Vector3.ProjectOnPlane(targetDir, Vector3.up);
            fwd.Normalize();
            var rgt = -Vector3.Cross(fwd, Vector3.up);

            context.Forward = fwd;  // update orientation of player

            // Get normal vector of the surface we are pushing on
            var col = _target.gameObject.GetComponent<BoxCollider>();
            var ray = new Ray(context.transform.position, targetDir);
            if (col.Raycast(ray, out var hit, 5f))
            {
                distance = hit.distance - context.Handler.Controller.radius;
                angle = Vector3.Angle(fwd, -hit.normal);
            }

            if (distance > maxDist || angle > maxAngle)
            {
                _detachFromTarget = true;
            }

            // Apply force to target
            var forceF = Mathf.Clamp01(context.GetStanceSettings().speed * 0.5f - context.CurrentSpeed);
            Vector3 force = fwd * (input.y * forceF * 20);
            _target.AddForce(force, ForceMode.Force);

            // Apply torque
            if (_target.angularVelocity.magnitude < turnRate)
                _target.AddTorque(-input.x * 20f * Vector3.up, ForceMode.Force);

            var movement = distance < minDist ? Vector3.zero : _target.velocity * Time.deltaTime;
            movement += input.x
                        * Time.deltaTime
                        * Vector3.Distance(_target.position, context.transform.position)
                        * _target.angularVelocity.magnitude
                        * rgt;

            if (context.Handler.ShouldStick)
            {
                movement += Vector3.down;
            }

            //movement = Vector3.Project(movement, fwd);

            context.Handler.Move(movement);
        }

        public override void HandleInput(PlayerMovementSystem context)
        {
            var handler = context.Handler;
            var grounded = handler.Grounded || handler.ShouldStick;

            if (!grounded)
            {
                context.ChangeState(new PlayerFallingState());
            }

            if (_detachFromTarget || context.Input.push)
            {
                context.ChangeState(new WalkingState());
            }
        }
    }
}