using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBattleState : EnemyState
{
    protected EnemySlime enemySlime;
    private Transform player;
    private int moveDir;
    public SlimeBattleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemySlime) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySlime = _enemySlime;
    }


    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemySlime.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemySlime.IsPlayerDetected())
        {
            stateTimer = enemySlime.battleTime;

            if (enemySlime.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemySlime.attackState);
                }
            }

        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 15)
            {
                stateMachine.ChangeState(enemySlime.idleState);
            }
        }

        if (player.position.x > enemySlime.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemySlime.transform.position.x)
        {
            moveDir = -1;
        }

        enemySlime.SetVelocity(enemySlime.moveSpeed * moveDir, rb.velocity.y);
    }

    private bool CanAttack()
    {
        if (Time.time >= enemySlime.lastTimeAttack + enemySlime.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemySlime.lastTimeAttack = Time.time;
            return true;
        }
        return false;
    }
}
