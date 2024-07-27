using UnityEngine;

public class Player : MonoBehaviour, IPlayerInputCheckable
{
    private float currentSpeed;

    public Rigidbody RB { get; set; }

    #region State Machine Variables
    public PlayerStateMachine StateMachine { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerMoveState MoveState { get; set; }
    public PlayerSprintState SprintState { get; set; }
    public bool IsMoving { get; set; }
    public bool IsSprinting { get; set; }
    #endregion

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        SprintState = new PlayerSprintState(this, StateMachine);
    }

    private void Start()
    {
        RB = GetComponent<Rigidbody>();

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentPlayerState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentPlayerState.PhysicsUpdate();
    }

    #region movement
    public void MovePlayer(Vector3 direction)
    {
        RB.velocity = direction * currentSpeed;
    }

    public void SetMovingStatus(bool isMoving, float moveSpeed)
    {
        IsMoving = isMoving;
        currentSpeed = moveSpeed;
    }

    public void SetSprintingStatus(bool isSprinting, float sprintSpeed)
    {
        IsSprinting = isSprinting;
        currentSpeed = sprintSpeed;
    }
    #endregion

    #region Animation Triggers
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentPlayerState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        PlayerDamaged,
        PlayerFootstepsSound
    }
    #endregion
}
