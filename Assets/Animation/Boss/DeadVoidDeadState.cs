using UnityEngine;

public class DeadVoidDeadState : EnemyState
{
    private EnemyDeadVoid enemyDeadVoid;
    public DeadVoidDeadState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeadVoid _enemyDeadVoid) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemyDeadVoid = _enemyDeadVoid;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.StopAllBGM();
        AudioManager.instance.AllowBGM();
        AudioManager.instance.StopSFXWithTime(25);
        AudioManager.instance.PlaySFXNormal(30);
        AudioManager.instance.PlayBGM(1);
        //Debug.Log("Dead");
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

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 5);
        }
    }
}
