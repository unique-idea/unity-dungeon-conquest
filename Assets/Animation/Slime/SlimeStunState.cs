using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunState : EnemyState
{
    private EnemySlime enemySlime;
    public SlimeStunState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemySlime) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySlime = _enemySlime;
    }

    public override void Enter()
    {
        base.Enter();
        enemySlime.fx.InvokeRepeating("DamageBlink", 0, 0.1f);

        stateTimer = enemySlime.stunDuration;
        rb.velocity = new Vector2(-enemySlime.facingDir * enemySlime.stunDirection.x, enemySlime.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemySlime.fx.Invoke("CancelColorChange", 0);
    }

    public override void Update()
    {
        base.Update();
        if(rb.velocity.y < .1f && enemySlime.IsGroundDetected())
        {
            enemySlime.animator.SetTrigger("StunFold");
           
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemySlime.idleState);
        }
    }
}
