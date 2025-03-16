using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroundedState : EnemyState
{
    protected Transform player;
    protected EnemySlime enemySlime;
    public SlimeGroundedState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemySlime) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySlime = _enemySlime;
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

        if (enemySlime.IsPlayerDetected() || Vector2.Distance(enemySlime.transform.position, player.position) < 2)
        {
            stateMachine.ChangeState(enemySlime.battleState);
        }
    }
}
