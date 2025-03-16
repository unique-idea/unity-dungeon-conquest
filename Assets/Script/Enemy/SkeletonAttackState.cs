using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private EnemySkeleton enemySkeleton;
    public SkeletonAttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemy, _stateMachine, _animBoolName)
    {
        enemySkeleton = _enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }


    public override void Exit()
    {
        base.Exit();

        enemySkeleton.lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemySkeleton.ZeroVelocity();

        if(triggerCalled)
        {
            stateMachine.ChangeState(enemySkeleton.battelState);
        }
    }
}
