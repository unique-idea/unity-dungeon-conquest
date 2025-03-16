using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashedState : PlayerState
{
    public PlayerDashedState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        float xOffset;

        player.skill.dash.CreateCloneOnDashStart();

        /*     if (Random.Range(0, 100) > 50)
             {
                 xOffset = 2;
             }
             else
             {
                 xOffset = -2;
             }
             player.skill.clone.CreateClone(player.transform, new Vector3(xOffset, 0)); */
        AudioManager.instance.PlaySFX(19, null);
        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();
        player.skill.dash.CreateCloneOnDashOver();
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if(!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.ideState);
        }

        player.fx.CreateAfterImage();
    }
}
