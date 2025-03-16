using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    private Enemy enemy => GetComponentInParent<Enemy>();

    private void AnimationTrigger()
    {
      //  Debug.Log("Animation called");
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Player>() != null)
            {
               PlayerStats target = hit.GetComponent<PlayerStats>();
                if (GetComponentInParent<EnemySkeleton>() != null)
                {
                    AudioManager.instance.PlaySFX(22, null);
                }
                if(GetComponentInParent<EnemySlime>() != null)
                {
                    AudioManager.instance.PlaySFX(24, null);
                }
                if (GetComponentInParent<EnemyDeadVoid>() != null)
                {
                    AudioManager.instance.PlaySFX(26, null);
                }
                enemy.stats.DoDamage(target);
   
            }
        }
    }

    private void OpenCounterWindow()
    {
        enemy.OpenCounterAttackWindow();
    }

    private void CloseCounterWindow()
    {
        enemy.CloseCounterAttackWindow();
    }
}
