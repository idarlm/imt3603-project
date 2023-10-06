using PlayerMovement;

internal class SprintingState : PlayerGroundedState
{
    public override void Enter(PlayerMovementSystem context)
    {
        stanceSettings = context.GetStanceSettings();
        stanceSettings.speed = context.sprintSpeed;
    }

    public override void Exit(PlayerMovementSystem context)
    {

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
            context.ChangeState(new PlayerFallingState());
        }
    }

}