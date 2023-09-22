using PlayerMovement;
using UnityEngine;

public class CrouchBody : MonoBehaviour
{
    public PlayerMovementSystem movementSystem;
    public float crouchingHeight = 0.5f;
    public float standingHeight = 1f;

    void Start()
    {
        movementSystem.StanceChanged += OnStanceChanged;
    }

    void OnStanceChanged(object sender, PlayerMovementEventArgs args)
    {
        bool crouching = args.Crouching;
        
        transform.localScale = new(
            1,
            crouching ? crouchingHeight : standingHeight,
            1);

        transform.localPosition = new(
            0,
            crouching ? -0.5f : 0f,
            0);
    }
}
