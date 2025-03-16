using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    public int comboCounter { get; private set; }
    private float lastTimeAttacked;
    private float comboCoolDown = 2;
    public PlayerPrimaryAttack(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //AudioManager.instance.PlaySFX(2);

        xInput = 0;

        if(comboCounter > 2 || Time.time >= lastTimeAttacked + comboCoolDown)
        {
            comboCounter = 0;
        }

        player.animator.SetInteger("ComboCounter", comboCounter);
        //  player.animator.speed = 3;
        float attackDir = player.facingDir;
        if(xInput != 0)
        {
            attackDir = xInput;
        }

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = .2f;
    }

    public override void Exit()
    {
        base.Exit();

      //  player.animator.speed = 1;
        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
        {
            player.ZeroVelocity();
        }

        if(triggerCalled)
        {
            stateMachine.ChangeState(player.ideState);
        }
    }
}
