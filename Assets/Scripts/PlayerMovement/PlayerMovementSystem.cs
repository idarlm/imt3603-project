using System;
using PlayerInput;
using Snapshot;
using StateMachine;
using UnityEngine;

namespace PlayerMovement
{
    /// <summary>
    /// Holds and controls state for the player character, 
    /// and uses MovementHandler to perform movement operations.
    /// </summary>
    [RequireComponent(typeof(MovementHandler))]
    public class PlayerMovementSystem : MonoBehaviour, ISnapshotable
    {
        internal struct InputValues
        {
            public bool jump, crouch, push, sprint;
            public Vector2 joystick;

            public void ClearFlags()
            {
                jump = crouch = push = false;
            }
        }

        /// <summary>
        /// StanceSettings contains a set of options that are used to adjust
        /// movement feel/functionality. This is used to provide different
        /// values for the player when crouching vs when standing.
        /// </summary>
        [Serializable]
        internal struct StanceSettings
        {
            public float speed;
            public float acceleration;
            public float deceleration;
            [Space(10)]
            public float controllerHeight;
            public Vector3 controllerCenter;
        }

        // Public properties
        /// <summary>
        /// The current velocity of the player.
        /// </summary>
        public Vector3 Velocity { get => _handler.Velocity; set => Handler.SetVelocity(value); }
        /// <summary>
        /// The current velocity of the player projected on the horizontal plane.
        /// </summary>
        public Vector3 HorizontalVelocity { get => Vector3.ProjectOnPlane(_handler.Velocity, Vector3.up); }
        /// <summary>
        /// The current horizontal speed of the player.
        /// </summary>
        public float CurrentSpeed { get => HorizontalVelocity.magnitude; }
        /// <summary>
        /// Is the player currently falling?
        /// </summary>
        public bool Falling { get; internal set; }
        /// <summary>
        /// The attached MovementHandler object.
        /// </summary>
        public MovementHandler Handler { get => _handler; }

        /// <summary>
        /// The forward direction of the player.
        /// </summary>
        public Vector3 Forward { get; set; }
        /// <summary>
        /// The forward direction of the camera, projected on the horizontal plane.
        /// </summary>
        public Vector3 CameraForward
        {
            get
            {
                if (cameraTransform)
                {
                    return Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
                }

                return transform.forward;
            }
        }
        /// <summary>
        /// The right direction of the camera, projected on the horizontal plane.
        /// </summary>
        public Vector3 CameraRight
        {
            get
            {
                if (cameraTransform)
                {
                    return Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
                }

                return transform.right;
            }
        }

        /// <summary>
        /// Disables the movement system and allows for position to be manipulated
        /// directly.
        /// 
        /// This also disables the CharacterContoller which means there is no collission
        /// detection until the movement system is unfrozen.
        /// </summary>
        public bool Freeze 
        { 
            get => _freeze; 
            set {
                _handler.SetControllerEnabled(!value);
                _freeze = value;
            }
        }

        // Events
        /// <summary>
        /// Fired when the player loses contact with the ground.
        /// </summary>
        public event EventHandler<PlayerMovementEventArgs> StartFalling;
        /// <summary>
        /// Fired when the player regains contact with the ground.
        /// </summary>
        public event EventHandler<PlayerMovementEventArgs> Landed;
        /// <summary>
        /// Fired when the player is preparing to jump.
        /// </summary>
        public event EventHandler<PlayerMovementEventArgs> PrepareJump;
        /// <summary>
        /// Fired when the player jumps. 
        /// The StartFalling event will normally be fired after this,
        /// unless the jump is blocked and the player does not lose contact
        /// with the ground.
        /// </summary>
        public event EventHandler<PlayerMovementEventArgs> Jumped;
        /// <summary>
        /// Fired when the player changes stance.
        /// </summary>
        public event EventHandler<PlayerMovementEventArgs> StanceChanged;

        // Fields
        [Tooltip("Should the player be able to jump?")]
        public bool enableJump = true;
        [Tooltip("Should the player be able to crouch?")]
        public bool enableCrouch = true;

        [Space(10)]

        [SerializeField] internal float gravity = 10f;
        [SerializeField] internal float jumpSpeed = 5f;
        [Tooltip("How long to wait until jumping after jump button is pressed.")]
        [SerializeField] internal float jumpDelay = 0f;
        [Tooltip("How much speed should the player lose when landing [0-1].")]
        [Range(0f, 1f)]
        [SerializeField] internal float landingSpeedPenalty = 0.5f;

        [Space(10)]

        [SerializeField] internal float sprintSpeed = 5f;
        [SerializeField] internal float turnRate = 180f;
        [SerializeField] internal float turnEventThreshold = 120f;

        [Space(10)]

        [Tooltip("StanceSettings to be used when the player is standing.")]
        [SerializeField] private StanceSettings standingSettings;
        [Tooltip("StanceSettings to be used when the player is crouching.")]
        [SerializeField] private StanceSettings crouchingSettings;

        [Space(10)]

        [Tooltip("When Camera Transform is assigned, the player will move based on camera direction.")]
        [SerializeField] private Transform cameraTransform;
        [Tooltip("Used to smoothly move child objects each frame, instead of only on fixed update tics.")]
        [SerializeField] private Transform interpolatedBody;
        private Vector3 _oldPosition; // used for interpolation
    
        // Fields ralating to object pushing
        [SerializeField] internal Rigidbody pushTarget;
        internal bool canPush = true;

        // dependencies
        private MovementHandler _handler;
        private GenericStateMachine<PlayerMovementSystem> _stateMachine;

        // input handling
        private IPlayerInput _inputController = new CombinedInput();
        private InputValues _inputValues;

        internal InputValues Input { get => _inputValues; set => _inputValues = value; }


        // stance
        private bool _crouching;
        internal bool shouldCrouch;

        // allow the system to be disabled
        private bool _freeze;
        
        // Internal Methods
        /// <summary>
        /// Change the selected movement state.
        /// </summary>
        /// <param name="newState">State to change to.</param>
        internal void ChangeState(PlayerMovementState newState)
        {
            // Debug.Log($"Changing movement state: {_stateMachine.CurrentState} -> {newState}");
            _stateMachine.ChangeState(newState);
        }

        /// <summary>
        /// Get the movement settings for the current stance.
        /// </summary>
        /// <returns>StanceSettings by reference</returns>
        internal ref StanceSettings GetStanceSettings()
        {
            if (_crouching)
                return ref crouchingSettings;

            return ref standingSettings;
        }

        // Set up references to dependencies
        private void Start()
        {
            // set up dependencies
            _handler = GetComponent<MovementHandler>();
            _stateMachine = new(new WalkingState());

            Forward = transform.forward;
        }
        
        // Handle player input and interpolation every frame
        private void Update()
        {
            if (_freeze)
            {
                return;
            }

            // input handling
            _inputValues.joystick = _inputController.LeftJoystickXY();
            _inputValues.jump    |= _inputController.Jump().IsPressed() && enableJump;
            _inputValues.push    |= canPush && _inputController.Interact().IsPressed();
            _inputValues.crouch  |= _inputController.Crouch().IsPressed() && enableCrouch;
            _inputValues.sprint   = _inputController.Sprint().IsHeld();

            // Interpolate body
            float t = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
            interpolatedBody.position = Vector3.Lerp(_oldPosition, transform.position, t);

            // Make sure state machine is not null
            _stateMachine ??= new(new WalkingState());
        }
        
        // Perform movement behaviour in sync with physics updates 
        private void FixedUpdate()
        {
            if (_freeze)
            {
                return;
            }

            // set old position
            // used to interpolate body transform
            _oldPosition = transform.position;

            // change stance
            if (_crouching != shouldCrouch)
            {
                ToggleStance();
            }
            
            // update state machine
            // the state machine performs all movement behaviour
            // like acceleration, etc...
            _stateMachine.Update(this);
            
            // clear input flags
            _inputValues.ClearFlags();
        }

        /// <summary>
        /// Try to uncrouch if we are crouching,
        /// or crouch if we are not.
        ///
        /// Uncrouching might not happen immediately
        /// if the space above the player is obstructed.
        /// The StanceChanged event should be used to get notified
        /// of stance changes.
        /// </summary>
        private void ToggleStance()
        {
            if (_crouching)
            {
                // Check if we are blocked from standing up
                var controller = _handler.Controller;

                Ray ray = new(transform.position, Vector3.up);
                var radius = controller.radius + controller.skinWidth;
                var dist = standingSettings.controllerHeight * 0.5f - radius;
                //Debug.DrawLine(transform.position, transform.position + Vector3.up * (dist + radius), Color.red);
                //Debug.DrawLine(transform.position, transform.position + Vector3.right * radius, Color.red);

                // Return if spherecast hits anything other than the player
                var hits = Physics.SphereCastAll(
                    ray,
                    radius,
                    dist,
                    LayerMask.NameToLayer("Player")
                );

                if (hits.Length > 0)
                {
                    return;
                }

                // Stand up
                _crouching = false;
                controller.height = standingSettings.controllerHeight;
                controller.center = standingSettings.controllerCenter;
            }
            else
            {
                // Crouch
                _crouching = true;
                var controller = _handler.Controller;
                controller.height = crouchingSettings.controllerHeight;
                controller.center = crouchingSettings.controllerCenter;
            }

            StanceChanged?.Invoke(this, GetEventArgs());
        }
        
        /// <summary>
        /// Get a new event args object.
        /// </summary>
        /// <returns>PlayerMovementEventArgs with prepopulated values.</returns>
        private PlayerMovementEventArgs GetEventArgs()
        {
            var args = new PlayerMovementEventArgs();

            args.Velocity = Velocity;
            args.Speed = CurrentSpeed;
            args.FallSpeed = -Mathf.Min(_handler.OldVelocity.y, 0);
            args.Crouching = _crouching;
            args.Falling = Falling;
            args.Jumping = _inputValues.jump;

            return args;
        }

        /// <summary>
        /// Fires the specified event.
        /// </summary>
        /// <param name="e">Event to fire</param>
        internal void FireEvent(PlayerMovementEvent e)
        {
            switch(e)
            {
                case PlayerMovementEvent.Landed:
                    Landed?.Invoke(this, GetEventArgs());
                    break;
                case PlayerMovementEvent.Jumped:
                    Jumped?.Invoke(this, GetEventArgs());
                    break;
                case PlayerMovementEvent.StartFalling:
                    StartFalling?.Invoke(this, GetEventArgs());
                    break;
                case PlayerMovementEvent.PrepareJump:
                    PrepareJump?.Invoke(this, GetEventArgs());
                    break;
                case PlayerMovementEvent.StanceChanged:
                    StanceChanged?.Invoke(this, GetEventArgs());
                    break;
                default:
                    Debug.LogWarning($"The event handler for \"{e}\" is not implemented.");
                    break;
            }
        }

        /// <summary>
        /// Creates a snapshot containing the current state of the object.
        /// </summary>
        /// <param name="ws"></param>
        public void OnMakeSnapshot(IWorldSnapshotWriter ws)
        {
            ws.AddSnapshotOf(this)
                .Add("velocity", Velocity)
                .Add("fwd", Forward);
        }

        /// <summary>
        /// Loads object state from a snapshot reader.
        /// </summary>
        /// <param name="ws"></param>
        public void OnLoadSnapshot(IWorldSnapshotReader ws)
        {
            // Disable CharacterController to allow for direct
            // manipulation of position values.
            Freeze = true;

            // load snapshot
            var snap = ws.LoadSnapshotOf(this);
            if (snap == null)
                return;

            // read values
            Handler.SetVelocity(snap.GetVector3("velocity"));
            Forward = snap.GetVector3("fwd");

            Freeze = false;
        }
    }
    
    public class PlayerMovementEventArgs : EventArgs
    {
        public bool Falling      { get; set; }
        public bool Jumping      { get; set; }
        public Vector3 Velocity  { get; set; }
        public float   Speed     { get; set; }
        public float   FallSpeed { get; set; }
        public bool    Crouching { get; set; }
    }

    internal enum PlayerMovementEvent
    {
        Landed,
        Jumped,
        StartFalling,
        StanceChanged,
        PrepareJump,
    }
    
}