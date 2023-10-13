using PlayerMovement;

internal class SprintingState : PlayerGroundedState
{
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
            context.ChangeState(new PlayerFallingState());
        }
    }

}