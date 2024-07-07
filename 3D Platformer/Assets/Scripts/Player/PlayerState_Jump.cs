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
        Debug.Log("<color=green>Entering " + Name + "</color>");
        component.SetCurrentAnimation("Jump");
    }

    protected override void OnExit()
    {
        Debug.Log("<color=red>Exiting " + Name + "</color>");
    }

    protected override void OnUpdate(float deltaTime)
    {
    }
}