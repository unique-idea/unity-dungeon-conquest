using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeItemController : MonoBehaviour
{
    protected PlayerStats playerStats;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
    
        if (collision.GetComponent<Enemy>() != null)
        {
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();

            playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}
