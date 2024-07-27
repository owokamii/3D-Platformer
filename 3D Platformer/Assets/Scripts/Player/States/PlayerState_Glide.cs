using UnityEngine;

public class PlayerState_Glide : StateBase
{
    public override string Name => "PLAYER_GLIDE";

    private Player_FSM component;

    private float glideGravity = -10f;

    public PlayerState_Glide(Player_FSM p)
    {
        component = p;
    }

    protected override void OnEnter()
    {
        component.SetCurrentGravity(glideGravity);
    }

    protected override void OnExit()
    {
    }

    protected override void OnUpdate(float deltaTime)
    {
    }
}