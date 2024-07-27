using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("<color=green>Entering Idle State</color>");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("<color=red>Entering Idle State</color>");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if(player.IsMoving)
        {
            player.StateMachine.ChangeState(player.MoveState);
        }

        //if not moving go back idle?
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
