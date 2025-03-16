using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMoveState : SlimeGroundedState
{
    public SlimeMoveState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemySlime) : base(_enemy, _stateMachine, _animBoolName, _enemySlime)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(23, enemy.transform);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemySlime.SetVelocity(enemySlime.moveSpeed * enemySlime.facingDir, rb.velocity.y);
        if (enemySlime.IsWallDetected() || !enemySlime.IsGroundDetected())
        {
            enemySlime.Flip();
            stateMachine.ChangeState(enemySlime.idleState);
        }
    }
}
