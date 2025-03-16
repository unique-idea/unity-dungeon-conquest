using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattelState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemySkeleton;
    private int moveDir;
    public SkeletonBattelState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemy, _stateMachine, _animBoolName)
    {
        enemySkeleton = _enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        if(player.GetComponent<PlayerStats>().isDead){
            stateMachine.ChangeState(enemySkeleton.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(enemySkeleton.IsPlayerDetected())
        {
            stateTimer = enemySkeleton.battleTime;

            if(enemySkeleton.IsPlayerDetected().distance < enemy.attackDistance ) 
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemySkeleton.attackState);
                }
            }

        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 15)
            {
                stateMachine.ChangeState(enemySkeleton.idleState);
            }
        }

        if (player.position.x > enemySkeleton.transform.position.x)
        {
            moveDir = 1;
        }
        else if(player.position.x < enemySkeleton.transform.position.x)
        {
            moveDir = -1;
        }

        enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * moveDir, rb.velocity.y);
    }

    private bool CanAttack()
    {
        if(Time.time >= enemySkeleton.lastTimeAttack + enemySkeleton.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemySkeleton.lastTimeAttack = Time.time;
            return true;
        }
        return false;
    }
}
