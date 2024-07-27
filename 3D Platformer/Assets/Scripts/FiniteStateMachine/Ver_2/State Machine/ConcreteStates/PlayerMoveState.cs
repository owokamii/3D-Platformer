using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("<color=green>Entering Move State</color>");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("<color=red>Exiting Move State</color>");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if(player.IsSprinting)
        {
            player.StateMachine.ChangeState(player.SprintState);
        }
        else if (!player.IsMoving)
        {
            player.StateMachine.ChangeState(player.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}