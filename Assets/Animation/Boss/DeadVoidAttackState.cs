using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadVoidAttackState : EnemyState
{
    private EnemyDeadVoid enemyDeadVoid;
    public DeadVoidAttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeadVoid _enemyDeadVoid) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemyDeadVoid = _enemyDeadVoid;
    }
    public override void Enter()
    {
        base.Enter();

        enemyDeadVoid.chanceToTeleport += 5;
    }

    public override void Exit()
    {
        base.Exit();

        enemyDeadVoid.lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemyDeadVoid.ZeroVelocity();

        if (triggerCalled)
        {
            if(enemyDeadVoid.CanTeleport())
            {
                stateMachine.ChangeState(enemyDeadVoid.teleportState);
            }
            else
            {
                stateMachine.ChangeState(enemyDeadVoid.battleState);
            }

        }
    }
}
