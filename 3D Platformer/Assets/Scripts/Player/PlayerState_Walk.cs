using UnityEngine;

public class PlayerState_Walk : StateBase
{
    public override string Name => "PLAYER_MOVE";

    private Player_FSM component;

    private float walkSpeed = 3f;

    public PlayerState_Walk(Player_FSM p)
    {
        component = p;
    }

    protected override void OnEnter()
    {
        Debug.Log("<color=green>Entering " + Name + "</color>");
        component.SetCurrentSpeed(walkSpeed);
        component.SetCurrentAnimation("Walk");
    }

    protected override void OnExit()
    {
        Debug.Log("<color=red>Exiting " + Name + "</color>");
    }

    protected override void OnUpdate(float deltaTime)
    {
    }
}