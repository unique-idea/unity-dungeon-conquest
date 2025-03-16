using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleSkill : Skill
{

    [SerializeField] private UISkillTreeSlot blackHoleUnlockButton;
    public bool blackHoleUnlocked { get; private set; }
    [SerializeField] private int  amountOfAttack;
    [SerializeField] private float cloneCoolDown;
    [SerializeField] private float blackHoleDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;


    BlackHoleController currentBlackHole;

    private void UnlockBlackHole()
    {
        if (blackHoleUnlockButton.unlocked)
        {
            blackHoleUnlocked = true;
        }
    }
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);

        currentBlackHole = newBlackHole.GetComponent<BlackHoleController>();

        currentBlackHole.SetUpBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttack, cloneCoolDown, blackHoleDuration);

        AudioManager.instance.PlaySFX(3, player.transform);
        AudioManager.instance.PlaySFX(5, player.transform);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
    }

    public bool SkillCompleted()
    {

        if (!currentBlackHole)
        {
            return false;
        }

        if(currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }


        return false;
    }

    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockBlackHole();
    }
}
