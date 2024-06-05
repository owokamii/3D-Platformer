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
        Debug.Log("<color=green>Entering Bunny_Flee</color>");
        component.spriteRenderer.sprite = component.flee;
        component.slowingRadius = 0;
    }

    protected override void OnExit()
    {
        Debug.Log("<color=green>Exiting Bunny_Flee</color>");
    }

    protected override void OnUpdate(float deltaTime)
    {
    }
}