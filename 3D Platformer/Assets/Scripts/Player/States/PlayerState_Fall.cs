using UnityEngine;

public class PlayerState_Fall : StateBase
{
    public override string Name => "PLAYER_FALL";

    private Player_FSM component;

    private float fallGravity = -20f;

    public PlayerState_Fall(Player_FSM p)
    {
        component = p;
    }

    protected override void OnEnter()
    {
        component.SetCurrentGravity(fallGravity);
    }

    protected override void OnExit()
    {
    }

    protected override void OnUpdate(float deltaTime)
    {
    }
}