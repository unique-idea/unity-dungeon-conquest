using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private UISkillTreeSlot parryUnlockButton;
    public bool parryUnlocked {  get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UISkillTreeSlot restoreUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentage;
    public bool restoreUnlocked { get; private set; }

    [Header("Parry with mirage")]
    [SerializeField] private UISkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get; private set; }


    public override void UseSkill()
    {
        base.UseSkill();
        AudioManager.instance.PlaySFX(20, null);
        if(restoreUnlocked )
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }

    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMigrate);
    }

    private void UnlockParry()
    {
        if(parryUnlockButton.unlocked)
        {
            parryUnlocked = true; 
        }
    }

    private void UnlockParryRestore()
    {
        if(restoreUnlockButton.unlocked)
        { 
            restoreUnlocked = true;
        }
    }

    private void UnlockParryWithMigrate()
    {
        if(parryWithMirageUnlockButton.unlocked)
        {
            parryWithMirageUnlocked = true;
        }
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestore();
        UnlockParryWithMigrate();
    }
    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
        {
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
        }
    }
}
