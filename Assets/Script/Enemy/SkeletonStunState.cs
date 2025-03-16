using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunState : EnemyState
{
    private EnemySkeleton enemySkeleton;
    public SkeletonStunState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySkeleton = _enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        enemySkeleton.fx.InvokeRepeating("DamageBlink", 0, 0.1f);

        stateTimer = enemySkeleton.stunDuration;
        rb.velocity = new Vector2(-enemySkeleton.facingDir * enemySkeleton.stunDirection.x, enemySkeleton.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemySkeleton.fx.Invoke("CancelColorChange", 0);
    }
   
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemySkeleton.idleState);
        }
    }
}
