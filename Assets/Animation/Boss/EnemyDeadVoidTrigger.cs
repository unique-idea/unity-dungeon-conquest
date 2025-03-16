using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadVoidTrigger : EnemyAnimationTrigger
{
    private EnemyDeadVoid enemyDeadVoid => GetComponentInParent<EnemyDeadVoid>();
    private void Relocate() => enemyDeadVoid.FindPosition();

    private void MakeInvisible()
    {
        enemyDeadVoid.fx.MakeTransparent(true);
        AudioManager.instance.PlaySFX(29, null);
    }
    private void MakeVisible()
    {
        enemyDeadVoid.fx.MakeTransparent(false);
        AudioManager.instance.StopSFX(29);
    }
}
