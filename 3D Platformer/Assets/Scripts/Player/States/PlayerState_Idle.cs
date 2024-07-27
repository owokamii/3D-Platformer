using UnityEngine;

public class PlayerState_Idle : StateBase
{
    public override string Name => "PLAYER_IDLE";

    private Player_FSM component;

    public PlayerState_Idle(Player_FSM p)
    {
        component = p;
    }

    protected override void OnEnter()
    {
        component.SetCurrentAnimation("Idle_A");
    }

    protected override void OnExit()
    {
    }

    protected override void OnUpdate(float deltaTime)
    {
    }
}