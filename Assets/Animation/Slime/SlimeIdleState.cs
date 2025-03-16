using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdleState : SlimeGroundedState
{
    public SlimeIdleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemySlime) : base(_enemy, _stateMachine, _animBoolName, _enemySlime)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemySlime.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemySlime.moveState);
        }
    }
}
