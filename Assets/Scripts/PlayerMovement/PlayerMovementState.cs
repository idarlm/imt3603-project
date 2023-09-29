
namespace PlayerMovement
{
    internal abstract class PlayerMovementState : IState<PlayerMovementSystem>
    {
        public abstract void Enter(PlayerMovementSystem context); // Called when entering state
        public abstract void Exit(PlayerMovementSystem context); // Called when exiting state

        // Update is called every frame after input handling and state transitions.
        // This is where behaviour code such as movement logic should go.
        //
        // Example: In the falling state, Update is used to accelerate the player downwards.
        public abstract void Update(PlayerMovementSystem context);

        // HandleInput is called every frame before the Update method.
        // HandleInput should be used to perform state changes.
        //
        // Example: if we lose contact with ground we should change to the falling state.
        public abstract void HandleInput(PlayerMovementSystem context);
    }
}