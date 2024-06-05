using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController characterController;

    [Header("Parameter")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private int jumpCount = 1;
    [SerializeField] private float gravity = -10f;
    [SerializeField] private float turnSmoothTime = 0.1f;

    private Vector3 velocity;
    private float turnSmoothVelocity;

    [Header("Private")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private int currentJumpCount;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isGliding = false;
    [SerializeField] private float timer;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        currentSpeed = walkSpeed;
        currentJumpCount = jumpCount;
    }

    private void Update()
    {
        HandleGravity();
        HandleMovementAndJump();

        if (isJumping)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                isGliding = true;
            }
        }

        if (Input.GetButtonDown("Jump"))
        {

            if (currentJumpCount > 0)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                currentJumpCount--;
                isJumping = true;
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            isGliding = false;
            timer = 0;
        }
    }

    private void HandleGravity()
    {
        isGrounded = characterController.isGrounded;

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        if (isGrounded)
        {
            currentJumpCount = jumpCount;
            isJumping = false;

            if(velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }
    }

    private void HandleMovementAndJump()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            currentSpeed = sprintSpeed;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            currentSpeed = walkSpeed;
        }

        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(inputHorizontal, 0.0f, inputVertical);
        movement.Normalize();

        // Apply horizontal movement
        characterController.Move(movement * currentSpeed * Time.deltaTime);

        if (movement.magnitude >= 0.1f)
        {
            // Calculate the target angle to rotate towards
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // Rotate the player towards the target angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Calculate the direction to move in
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Apply movement
            characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
        }

        // Apply vertical movement
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleGlide()
    {

    }
}