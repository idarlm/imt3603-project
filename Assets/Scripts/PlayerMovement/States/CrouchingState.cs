using PlayerMovement;

internal class CrouchingState : PlayerGroundedState
{
    public override void Enter(PlayerMovementSystem context)
    {
        context.shouldCrouch = true;
        stanceSettings = context.GetStanceSettings();
    }

    public override void Exit(PlayerMovementSystem context)
    {
        context.shouldCrouch = false;
    }

    public override void HandleInput(PlayerMovementSystem context)
    {
        base.HandleInput(context);
        var input = context.Input;

        if (input.crouch || input.jump)
        {
            context.ChangeState(new WalkingState());
        }
    }

}