using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{

    private EnemySkeleton enemy;
    public SkeletonDeadState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemy = _enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.animator.SetBool(enemy.lastAnimBoolName, true);
        enemy.animator.speed = 0;

        enemy.cd.enabled = false;

        stateTimer = .3f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 5);
        }
    }

}
