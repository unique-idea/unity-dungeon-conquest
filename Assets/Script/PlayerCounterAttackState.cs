using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public PlayerCounterAttackState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.animator.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 2;
                    player.animator.SetBool("SuccessfulCounterAttack", true);

                    player.skill.parry.UseSkill();
                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.parry.MakeMirageOnParry(hit.transform);
                    }
                }
            }
            if(triggerCalled == true)
            {
                player.animator.SetBool("SuccessfulCounterAttack", false);
            }
            if (stateTimer <= 0 || triggerCalled)
            {
                /*  Debug.Log("stopped");
                  player.animator.SetBool("SuccessfulCounterAttack", false);*/
                stateMachine.ChangeState(player.ideState); 
            }
        }
    }
}
