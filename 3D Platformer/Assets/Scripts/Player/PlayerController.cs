using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region For All Variables
    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;

    [Header("In Air")]
    private Vector3 velocity;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float depleteRate = 5f;

    [Header("Movement Parameters")]
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float inputX;
    private float inputY;
    private Vector3 movement;
    private float currentSpeed;
    private float turnSmoothVelocity;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpCount;
    #endregion For All Variables

    #region     Get Set
    public CharacterController GetCharacterController { get; set; }
    public Vector3 GetMovement { get; set; }
    public float GetCurrentSpeed { get; set; }
    public float GetCurrentStamina { get; set; }
    public Vector3 GetVelocity { get; set; }
    public float GetCurrentGravity { get; set; }
    public int GetCurrentJumpCount { get; set; }
    #endregion  Get Set

    private void Awake()
    {
        GetCharacterController = GetComponent<CharacterController>();

        GetCurrentGravity = gravity;
        GetCurrentStamina = maxStamina;
        GetCurrentJumpCount = jumpCount;
    }

    private void Update()
    {
        HandleGravity();
        HandleInput();
        HandleMovement();
        HandleJump();

        if(Input.GetKey(KeyCode.Q))
        {
            GetCurrentStamina -= depleteRate;

            if(GetCurrentStamina <= 0)
            {
                GetCurrentStamina = 0;
            }
        }
    }

    private void HandleGravity()
    {
        velocity.y += GetCurrentGravity * Time.deltaTime;

        if (GetCharacterController.isGrounded)
        {
            GetCurrentJumpCount = jumpCount;

            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }
        else
        {
            if (velocity.y < GetCurrentGravity)
            {
                velocity.y = GetCurrentGravity;
            }
        }

        GetVelocity = velocity;
    }

    private void HandleInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    private void HandleMovement()
    {
        GetMovement = new Vector3(inputX, 0.0f, inputY).normalized;

        if (GetMovement.magnitude >= 0.1f)
        {
            // player rotation
            float targetAngle = Mathf.Atan2(GetMovement.x,GetMovement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // horizontal movement
            GetCharacterController.Move(GetMovement * GetCurrentSpeed * Time.deltaTime);
        }

        // vertical movement
        GetCharacterController.Move(velocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (GetCurrentJumpCount > 0)
            {
                velocity.y = jumpForce;
                //velocity.y = Mathf.Sqrt(jumpHeight * jumpForce * currentGravity);
                GetCurrentJumpCount--;
            }
        }
    }
}