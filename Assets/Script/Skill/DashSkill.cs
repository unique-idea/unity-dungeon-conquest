using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    [SerializeField] private UISkillTreeSlot dashUnlockButton;
    public bool dashUnlocked { get; private set; }


    [Header("Clone on dash")]
    [SerializeField] private UISkillTreeSlot cloneOnDashUnlockButton;
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on arrival")]
    [SerializeField] private UISkillTreeSlot cloneOnArrivalDashUnlockButton;
    public bool cloneOnArrivalUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
        base.Start();
    }

    private void UnlockDash()
    {
       // Debug.Log(dashUnlockButton.unlocked);
        if (dashUnlockButton.unlocked)
        {
            dashUnlocked = true;
        }
    }

    private void UnlockCloneOnDash()
    {
        if(cloneOnDashUnlockButton.unlocked)
        {
            cloneOnDashUnlocked = true;
        }
    }

    private void UnlockCloneOnArrival()
    {
        if(cloneOnArrivalDashUnlockButton.unlocked)
        {
            cloneOnArrivalUnlocked = true;
        }
    }

    public void CreateCloneOnDashStart()
    {
        if (cloneOnDashUnlocked)
        {
           SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if (cloneOnArrivalUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }
}
