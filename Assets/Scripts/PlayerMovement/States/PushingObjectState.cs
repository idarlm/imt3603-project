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

        float maxDist = 0.5f;
        float minDist = 0.2f;
        float maxAngle = 45f;
        float distance = 1f;
        float surfaceDistance = 1f;
        float angle = 0f;

        float pushForce = 75f;

        readonly float turnRate = 1f;
        readonly float speed = 3.5f;

        float prevRotError = 0f;

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
            distance = Vector3.Distance(_target.position, context.transform.position);

            context.Forward = fwd;  // update orientation of player

            // Get normal vector of the surface we are pushing on
            var col = _target.gameObject.GetComponent<BoxCollider>();
            var ray = new Ray(context.transform.position, targetDir);
            if (col.Raycast(ray, out var hit, 5f))
            {
                surfaceDistance = hit.distance - context.Handler.Controller.radius;
                angle = Vector3.SignedAngle(fwd, -hit.normal, Vector3.up);
            }

            if (surfaceDistance > maxDist || Mathf.Abs(angle) > maxAngle || _target.velocity.magnitude > speed)
            {
                _detachFromTarget = true;
            }

            // MOVING THE OBJECT
            // torque is determined by a sort of PID controller but without the I.
            // so a PD controller.
            // force is determined using only the error. so a P controller.

            // calculate force.
            // desired position is current position rotated by turnRate,
            // and translated by forward direction * speed.
            var rotatedDir = Vector3.RotateTowards(fwd, rgt, input.x * turnRate * Time.deltaTime, 0).normalized;
            var desiredPos = rotatedDir * (distance/* + 0.2f - surfaceDistance*/);     // rotation
            desiredPos += rotatedDir * speed * input.y * Time.deltaTime; // translation
            desiredPos += context.transform.position;

            var posError = desiredPos - _target.position;
            posError *= 20f;

            posError = posError.magnitude > 1 ? posError.normalized : posError;

            var forceF = Mathf.Clamp01(speed - _target.velocity.magnitude);
            _target.AddForce(posError * pushForce * forceF);

            // calculate torque.
            // desired angle is always 0.
            // real angle is the angle between the inverted normal vector of 
            // the surface being pushed and fwd.
            var rotError = angle; // error
            rotError *= 1f;

            var rotROC = rotError - prevRotError; // rate of change estimate
            rotROC *= 0.5f;

            prevRotError = rotError;

            _target.AddTorque(Vector3.up * -Mathf.Clamp(rotError + rotROC, -1f, 1f) * pushForce);

            // Move player along with pushed object
            var movement = Vector3.Project(_target.velocity, fwd.normalized) * Time.deltaTime;

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