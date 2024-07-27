using UnityEngine;

public class PlayerState_Jump : StateBase
{
    public override string Name => "PLAYER_JUMP";

    private Player_FSM component;

    public PlayerState_Jump(Player_FSM p)
    {
        component = p;
    }

    protected override void OnEnter()
    {
        component.SetCurrentAnimation("Jump");
    }

    protected override void OnExit()
    {
    }

    protected override void OnUpdate(float deltaTime)
    {
    }
}