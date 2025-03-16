using UnityEngine;

public class DeadVoidIdleState : EnemyState
{
    private EnemyDeadVoid enemyDeadVoid;
    private Transform player;
    public DeadVoidIdleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeadVoid _enemeyDeadVoid) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemyDeadVoid = _enemeyDeadVoid;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(Vector2.Distance(player.transform.position, enemyDeadVoid.transform.position) < 7)
        {
            enemyDeadVoid.bossFightBegun = true;
        }

        if (stateTimer < 0 && enemyDeadVoid.bossFightBegun)
        {
            stateMachine.ChangeState(enemyDeadVoid.battleState);
        }
    }
}
