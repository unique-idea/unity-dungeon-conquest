using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadVoidSpellCastState : EnemyState
{

    private int amountOfSpells;
    private float spellTimer;

    private EnemyDeadVoid enemyDeadVoid;
    public DeadVoidSpellCastState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeadVoid _enemyDeadVoid) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemyDeadVoid = _enemyDeadVoid;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfSpells = enemyDeadVoid.amountOfSpell;
        spellTimer = .5f;
        AudioManager.instance.PlaySFX(27, null);
    }

    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;

        if(CanCast())
        {
            enemyDeadVoid.CastSpell();
        }
       
        if(amountOfSpells <= 0)
        {
            stateMachine.ChangeState(enemyDeadVoid.teleportState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemyDeadVoid.lastTimeCast = Time.time;
        AudioManager.instance.StopSFX(27);
    }
    private bool CanCast()
    {
        if(amountOfSpells > 0 && spellTimer < 0)
        {
            amountOfSpells--;
            spellTimer = enemyDeadVoid.spellCoolDown;
            return true;
        }
        return false;
    }
}
