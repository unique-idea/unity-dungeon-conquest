using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttackState : EnemyState
{
    protected EnemySlime enemySlime;
    public SlimeAttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemySlime) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySlime = _enemySlime;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemySlime.ZeroVelocity();

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemySlime.battleState);
        }
    }
}
