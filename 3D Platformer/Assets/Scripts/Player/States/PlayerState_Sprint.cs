using UnityEngine;

public class PlayerState_Sprint : StateBase
{
    public override string Name => "PLAYER_SPRINT";

    private Player_FSM component;

    private float sprintSpeed = 6f;

    public PlayerState_Sprint(Player_FSM p)
    {
        component = p;
    }

    protected override void OnEnter()
    {
        //component.DepleteStamina();
        component.SetCurrentSpeed(sprintSpeed);
        component.SetCurrentAnimation("Run");
    }

    protected override void OnExit()
    {
    }

    protected override void OnUpdate(float deltaTime)
    {
        component.DepleteStamina();
    }
}