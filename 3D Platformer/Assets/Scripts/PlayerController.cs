using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum PlayerState { Idle, Walk, Sprint, Jump }
    private PlayerState state;
    private bool stateComplete;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;
    //[SerializeField] private float glidingGravity = -10f;
    private float currentGravity;

    [Header("In Air")]
    private Vector3 velocity;

    [Header("Move Parameters")]
    //[SerializeField] private float walkSpeed = 3f;
    //[SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float inputX;
    private float inputY;
    private Vector3 movement;
    private float currentSpeed;
    private float turnSmoothVelocity;

    [Header("Jump Parameters")]
    //[SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private int jumpCount = 1;
    private int currentJumpCount;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();
        HandleGravity();
        HandleMovement();
        //HandleSprint();
        HandleJump();

        if (stateComplete)
        {
            SelectState();
        }
        UpdateState();
    }

    private void HandleInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    private void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;

        if (characterController.isGrounded)
        {
            currentJumpCount = jumpCount;

            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }
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
                currentJumpCount--;
                //isJumping = true;
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            //isGliding = false;
            //timerInAir = 0;
        }
    }

    private void SelectState()
    {
        stateComplete = false;

        if (characterController.isGrounded)
        {
            if (movement.magnitude == 0)
            {
                state = PlayerState.Idle;
                StartIdle();
            }
            else if (Input.GetButton("Sprint"))
            {
                state = PlayerState.Sprint;
                StartSprint();
            }
            else
            {
                state = PlayerState.Walk;
                StartWalk();
            }
        }
        else
        {
            state = PlayerState.Jump;
            StartJump();
        }
    }

    private void UpdateState()
    {
        switch (state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Walk:
                UpdateWalk();
                break;
            case PlayerState.Sprint:
                UpdateSprint();
                break;
            case PlayerState.Jump:
                UpdateJump();
                break;
        }
    }

    private void StartIdle()
    {
        //animator play
    }

    private void StartSprint()
    {
        //animator play
    }

    private void StartWalk()
    {
        // animator play walk
    }

    private void StartJump()
    {
        // animator play jump
    }

    private void UpdateIdle()
    {
        if (movement.magnitude != 0)
        {

        }
    }

    private void UpdateWalk()
    {

    }

    private void UpdateSprint()
    {

    }

    private void UpdateJump()
    {

    }
}