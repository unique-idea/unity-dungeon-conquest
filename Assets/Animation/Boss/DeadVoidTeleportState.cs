using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadVoidTeleportState : EnemyState
{
    private EnemyDeadVoid enemyDeadVoid;
    public DeadVoidTeleportState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemyDeadVoid _enemyDeadVoid) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemyDeadVoid = _enemyDeadVoid;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(29, null);
    }

    public override void Update()
    {
        base.Update();

       // Debug.Log("Trigger " + triggerCalled);
       if(triggerCalled)
        {
            if (enemyDeadVoid.CanDoSpellCast())
            {
                stateMachine.ChangeState(enemyDeadVoid.spellCastState);
            }
            else
            {
                stateMachine.ChangeState(enemyDeadVoid.battleState);
            }
        }
    }
}
