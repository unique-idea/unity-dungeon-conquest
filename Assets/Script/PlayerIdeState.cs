using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdeState : PlayerGroundedState
{
    public PlayerIdeState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.ZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (xInput != 0)
        {
            stateMachine.ChangeState(player.moveState);
        } 
    }
}
