using PlayerMovement;
using UnityEngine;

internal class WalkingState : PlayerGroundedState
{
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
            context.ChangeState(new PlayerFallingState());
        }

        if (input.sprint)
        {
            context.ChangeState(new SprintingState());
        }

        if (input.push)
        {
            var ray = new Ray(context.transform.position, context.Forward);
            
            if(Physics.SphereCast(ray: ray, radius: 0.5f, maxDistance: 1f, hitInfo: out var hit))
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
}