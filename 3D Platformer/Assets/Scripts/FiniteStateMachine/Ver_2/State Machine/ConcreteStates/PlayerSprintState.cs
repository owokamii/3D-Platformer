using UnityEngine;

public class PlayerSprintState : PlayerState
{
    public PlayerSprintState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("<color=green>Entering Sprint State</color>");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("<color=red>Exiting Sprint State</color>");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (!player.IsSprinting)
        {
            player.StateMachine.ChangeState(player.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}