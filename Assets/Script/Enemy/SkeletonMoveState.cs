using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemy, _stateMachine, _animBoolName, _enemySkeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * enemySkeleton.facingDir, rb.velocity.y);
        if(enemySkeleton.IsWallDetected() || !enemySkeleton.IsGroundDetected())
        {
            enemySkeleton.Flip();
            stateMachine.ChangeState(enemySkeleton.idleState);
        }
    }
}
