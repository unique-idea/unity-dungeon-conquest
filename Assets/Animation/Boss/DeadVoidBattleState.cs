using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadVoidBattleState : EnemyState
{
    private Transform player;
    private EnemyDeadVoid enemyDeadVoid;
    private int moveDir;
    public DeadVoidBattleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeadVoid _enemyDeadVoid) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemyDeadVoid = _enemyDeadVoid;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(player != null)
        {
          //  Debug.Log("Player is not null");
        }
       // Debug.Log("Run in battle State");
        if (enemyDeadVoid.IsPlayerDetected())
        {
          //  Debug.Log("Player Detected");
            stateTimer = enemyDeadVoid.battleTime;

            if (enemyDeadVoid.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemyDeadVoid.attackState);
                }
                else
                {
                    stateMachine.ChangeState(enemyDeadVoid.idleState);
                }
            }
        }

        if (player.position.x > enemyDeadVoid.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemyDeadVoid.transform.position.x)
        {
            moveDir = -1;
        }

        enemyDeadVoid.SetVelocity(enemyDeadVoid.moveSpeed * moveDir, rb.velocity.y);
    }

    private bool CanAttack()
    {
        if (Time.time >= enemyDeadVoid.lastTimeAttack + enemyDeadVoid.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemyDeadVoid.lastTimeAttack = Time.time;
            return true;
        }
        return false;
    }
}
