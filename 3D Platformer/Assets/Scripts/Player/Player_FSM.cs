using UnityEngine;
using TMPro;

public class Player_FSM : MonoBehaviour
{
    #region For All Variables
    private StateMachine stateMachine;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;
    private float currentGravity;

    [Header("In Air")]
    private Vector3 velocity;

    [Header("Stamina")]
    [SerializeField] private float stamina = 100f;
    [SerializeField] private float depleteRate = 5f;

    [Header("Movement Parameters")]
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float inputX;
    private float inputY;
    private Vector3 movement;
    private float currentSpeed;
    private float turnSmoothVelocity;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = -3f;
    [SerializeField] private int jumpCount = 1;
    private int currentJumpCount;

    #region     Debug Text
    [Header("Debug Text")]
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private TMP_Text movementText;
    [SerializeField] private TMP_Text velocityText;
    [SerializeField] private TMP_Text groundedText;
    [SerializeField] private TMP_Text sprintingText;
    [SerializeField] private TMP_Text jumpingText;
    [SerializeField] private TMP_Text jumpCountText;
    [SerializeField] private TMP_Text glidingText;
    [SerializeField] private TMP_Text staminaText;
    #endregion  Debug Text
    #endregion For All Variables

    private void Awake()
    {
        #region     Get Components
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        #endregion  Get Components

        currentJumpCount = jumpCount;
        currentGravity = gravity;

        stateMachine = new StateMachine();

        // 1. Create all possible states
        #region For All States
        var playerIdleState = new PlayerState_Idle(this);           // Idle
        var playerMoveState = new PlayerState_Walk(this);           // Move
        var playerSprintState = new PlayerState_Sprint(this);       // Sprint
        var playerJumpState = new PlayerState_Jump(this);           // Jump
        var playerFallState = new PlayerState_Fall(this);           // Fall
        var playerGlideState = new PlayerState_Glide(this);         // Glide
        #endregion For All States

        // 2. Set all transitions
        #region For All Transitions
        #region     Grounded
        // Move
        playerIdleState.AddTransition(playerMoveState, (timeInState) =>
        {
            return IsGrounded() && IsMoving();
        });
        playerSprintState.AddTransition(playerMoveState, (timeInState) =>
        {
            return (IsGrounded() && IsMoving() && !IsSprinting()) || IsStaminaDepleted();
        });
        playerJumpState.AddTransition(playerMoveState, (timeInState) =>
        {
            return IsGrounded() && IsMoving();
        });

        // Sprint
        playerMoveState.AddTransition(playerSprintState, (timeInState) =>
        {
            return IsGrounded() && IsMoving() && IsSprinting();
        });

        // Idle
        playerJumpState.AddTransition(playerIdleState, (timeInState) =>
        {
            return IsGrounded() && !IsMoving();
        });
        playerFallState.AddTransition(playerMoveState, (timeInState) =>
        {
            return IsGrounded() && IsMoving();
        });
        playerFallState.AddTransition(playerIdleState, (timeInState) =>
        {
            return IsGrounded() && !IsMoving();
        });
        playerSprintState.AddTransition(playerIdleState, (timeInState) =>
        {
            return IsGrounded() && !IsSprinting() && !IsMoving();
        });
        playerMoveState.AddTransition(playerIdleState, (timeInState) =>
        {
            return IsGrounded() && !IsMoving();
        });
        playerGlideState.AddTransition(playerIdleState, (timeInState) =>
        {
            return IsGrounded() && !IsMoving();
        });
        #endregion  Grounded

        #region     In Air
        // Jump
        playerIdleState.AddTransition(playerJumpState, (timeInState) =>
        {
            return IsGrounded() && IsJumping();
        });
        playerMoveState.AddTransition(playerJumpState, (timeInState) =>
        {
            return IsGrounded() && IsJumping();
        });
        playerSprintState.AddTransition(playerJumpState, (timeInState) =>
        {
            return IsGrounded() && IsJumping();
        });
        playerFallState.AddTransition(playerJumpState, (timeInState) =>
        {
            return !IsGrounded() && IsJumping();
        });

        // Fall
        playerJumpState.AddTransition(playerFallState, (timeInState) =>
        {
            return IsFalling();
        });
        playerGlideState.AddTransition(playerFallState, (timeInState) =>
        {
            return !IsGliding();
        });

        // Glide
        playerFallState.AddTransition(playerGlideState, (timeInState) =>
        {
            return !IsGrounded() && IsGliding();
        });
        #endregion  In Air
        #endregion For All Transitions

        // 3. Set the starting state
        #region     For Initial State
        stateMachine.SetInitialState(playerIdleState);
        #endregion  For Initial State
    }

    private void Update()
    {
        Debug.Log(currentGravity);
        // For Debug
        PrintBools();

        HandleGravity();
        HandleInput();
        HandleMovement();
        HandleJump();

        stateMachine.Tick(Time.deltaTime);
    }

    private void HandleGravity()
    {
        velocity.y += currentGravity * Time.deltaTime;

        if (characterController.isGrounded)
        {
            currentJumpCount = jumpCount;

            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }
        else
        {
            if(velocity.y < currentGravity)
            {
                velocity.y = currentGravity;
            }
        }
    }

    private void HandleInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    private void HandleMovement()
    {
        movement = new Vector3(inputX, 0.0f, inputY).normalized;

        if (movement.magnitude >= 0.1f)
        {
            // player rotation
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // horizontal movement
            characterController.Move(movement * currentSpeed * Time.deltaTime);
        }

        // vertical movement
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (currentJumpCount > 0)
            {
                velocity.y = jumpForce;
                //velocity.y = Mathf.Sqrt(jumpHeight * jumpForce * currentGravity);
                currentJumpCount--;
            }
        }
    }

    #region For State Actions
    public void SetCurrentGravity(float gravityValue)
    {
        currentGravity = gravityValue;
    }

    public void SetCurrentSpeed(float speedValue)
    {
        currentSpeed = speedValue;
    }

    public void SetCurrentAnimation(string animation)
    {
        animator.Play(animation);
    }

    public void DepleteStamina()
    {
        stamina -= depleteRate * Time.deltaTime;
    }
    #endregion For State Actions

    #region For Transition Checks
    private bool IsGrounded()
    {
        return characterController.isGrounded;
    }

    private bool IsMoving()
    {
        return movement.magnitude != 0;
    }

    private bool IsSprinting()
    {
        return Input.GetButton("Sprint");
    }

    private bool IsJumping()
    {
        return velocity.y > 0;
    }

    private bool IsFalling()
    {
        return velocity.y < 0;
    }

    private bool IsGliding()
    {
        return Input.GetButton("Jump");
    }

    private bool IsStaminaDepleted()
    {
        return stamina <= 0;
    }
    #endregion For Transition Checks

    #region For Visuals
    #endregion For Visuals

    #region     For Debug
    private void PrintBools()
    {
        stateText.text = stateMachine.CurrentStateName;
        movementText.text = "Movement: " + movement.ToString();
        velocityText.text = "Velocity: " + velocity.y.ToString();
        groundedText.text = "Grounded: " + IsGrounded().ToString();
        sprintingText.text = "Sprinting: " + IsSprinting().ToString();
        jumpingText.text = "Jumping: " + IsJumping().ToString();
        jumpCountText.text = "Current Jump Count: " + currentJumpCount.ToString();
        glidingText.text = "Gliding: " + IsGliding().ToString();
        staminaText.text = "Stamina: " + stamina.ToString();
    }
    #endregion  For Debug

}