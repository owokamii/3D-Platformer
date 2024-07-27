using UnityEngine;
using TMPro;

public class Player_FSM : MonoBehaviour
{
    #region For All Variables
    [Header("References")]
    private StateMachine stateMachine;
    private PlayerController playerController;
    private Animator playerAnimator;

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
        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponentInChildren<Animator>();
        #endregion  Get Components

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
        StateBase.AddAnyTransition(playerMoveState, (timeInState) =>
        {
            return IsGrounded() && IsMoving() && !IsSprinting();
        });

        // Sprint
        StateBase.AddAnyTransition(playerSprintState, (timeInState) =>
        {
            return IsGrounded() && IsMoving() && IsSprinting();
        });

        // Idle
        StateBase.AddAnyTransition(playerIdleState, (timeInState) =>
        {
            return IsGrounded() && !IsMoving();
        });
        #endregion  Grounded

        #region     In Air
        // Jump
        StateBase.AddAnyTransition(playerJumpState, (timeInState) =>
        {
            return !IsGrounded() && IsJumping();
        });

        /*
        // Fall
        StateBase.AddAnyTransition(playerFallState, (timeInState) =>
        {
            return !IsGrounded() && IsFalling();
        });*/

        // Glide
        //*playerFallState.AddTransition(playerGlideState, (timeInState) =>
        //    return !IsGrounded() && IsGliding();
        //});
        #endregion  In Air
        #endregion For All Transitions

        // 3. Set the starting state
        #region     For Initial State
        stateMachine.SetInitialState(playerIdleState);
        #endregion  For Initial State
    }

    private void Update()
    {
        stateMachine.Tick(Time.deltaTime);

        PrintBools();
    }

    #region For State Actions
    public void SetCurrentGravity(float gravityValue)
    {
        playerController.GetCurrentGravity = gravityValue;
    }

    public void SetCurrentSpeed(float speedValue)
    {
        playerController.GetCurrentSpeed = speedValue;
    }

    public void SetCurrentAnimation(string animation)
    {
        playerAnimator.Play(animation);
    }

    public void DepleteStamina()
    {
        //stamina -= depleteRate * Time.deltaTime;
    }
    #endregion For State Actions

    #region For Transition Checks
    private bool IsGrounded()
    {
        return playerController.GetCharacterController.isGrounded;
    }

    private bool IsMoving()
    {
        return playerController.GetMovement.magnitude != 0;
    }

    private bool IsSprinting()
    {
        return Input.GetButton("Sprint");
    }

    private bool IsJumping()
    {
        return playerController.GetVelocity.y > 0;
    }

    private bool IsFalling()
    {
        return playerController.GetVelocity.y < 0;
    }

    /*private bool IsGliding()
    {
        return Input.GetButton("Jump");
    }*/

    /*private bool IsStaminaDepleted()
    {
        return stamina <= 0;
    }*/
    #endregion For Transition Checks


    #region For Visuals
    #endregion For Visuals

    #region     For Debug
    private void PrintBools()
    {
        stateText.text = stateMachine.CurrentStateName;

        movementText.text = "Movement: " + playerController.GetMovement.ToString();
        velocityText.text = "Velocity: " + playerController.GetVelocity.y.ToString();
        groundedText.text = "Grounded: " + IsGrounded().ToString();
        sprintingText.text = "Sprinting: " + IsSprinting().ToString();
        jumpingText.text = "Jumping: " + IsJumping().ToString();
        jumpCountText.text = "Current Jump Count: " + playerController.GetCurrentJumpCount.ToString();
        //glidingText.text = "Gliding: " + IsGliding().ToString();
        //staminaText.text = "Stamina: " + stamina.ToString();
    }
    #endregion  For Debug

}