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
        Debug.Log("<color=green>Entering " + Name + "</color>");
        component.SetCurrentGravity(glideGravity);
    }

    protected override void OnExit()
    {
        Debug.Log("<color=red>Exiting " + Name + "</color>");
    }

    protected override void OnUpdate(float deltaTime)
    {
    }
}