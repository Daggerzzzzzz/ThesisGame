using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skill
{
    [Header("Blackhole Info")]
    [SerializeField] 
    private GameObject blackholePrefab;
    [SerializeField] 
    private float maximumSize;
    [SerializeField]
    private float speedOfGrowth;
    [SerializeField]
    private float swordAttackCooldown;

    [Header("Base Upgrade")] 
    [SerializeField]
    private SkillTreeSlot blackholeUnlockButton;
    public bool baseUpgradeUnlock { get; private set; }

    [Header("Second Upgrade")] 
    [SerializeField]
    private SkillTreeSlot blackholeSecondUpgradeButton;
    public bool secondUpgradeUnlock { get; private set; }
    
    [Header("Third Upgrade")] 
    [SerializeField]
    private SkillTreeSlot blackholeThirdUpgradeButton;
    public bool thirdUpgradeUnlock { get; private set; }
    
    private int angleIncrement;
    private int amountOfSwords;

    private BlackholeSkillController currentBlackhole;
    private GameObject newBlackhole;
    
    protected override void Start()
    {
        base.Start();
        
        blackholeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBaseUpgrade);
        blackholeSecondUpgradeButton.GetComponent<Button>().onClick.AddListener(UnlockSecondUpgrade);
        blackholeThirdUpgradeButton.GetComponent<Button>().onClick.AddListener(UnlockThirdUpgrade);
    }

    protected override void Update()
    {
        base.Update();
    }
    
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        
        BlackholeUpgrades(baseUpgradeUnlock, secondUpgradeUnlock, thirdUpgradeUnlock);
        
        newBlackhole = Instantiate(blackholePrefab, player.transform.position, quaternion.identity);

        currentBlackhole = newBlackhole.GetComponent<BlackholeSkillController>();
        
        currentBlackhole.SetupBlackhole( maximumSize, speedOfGrowth, amountOfSwords, swordAttackCooldown, angleIncrement, player);
    }

    public bool BlackholeIsFinished()
    {
        if (currentBlackhole == null)
        {
            return false;
        }
        
        if (currentBlackhole.PlayerCanExitUltimate)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    private void BlackholeUpgrades(bool baseUpgrade, bool secondUpgrade, bool thirdUpgrade)
    {
        if (thirdUpgrade)
        {
            angleIncrement = 30;
            amountOfSwords = 360 / 30;
        }
        else if (secondUpgrade)
        {
            angleIncrement = 45;
            amountOfSwords = 360 / 45;
        }
        else if (baseUpgrade)
        {
            angleIncrement = 60;
            amountOfSwords = 360 / 60;
        }
    }

    private void UnlockBaseUpgrade()
    {
        if (blackholeUnlockButton.unlocked)
        {
            baseUpgradeUnlock = true;
        }
    }
    
    private void UnlockSecondUpgrade()
    {
        if (blackholeSecondUpgradeButton.unlocked)
        {
            secondUpgradeUnlock = true;
        }
    }
    
    private void UnlockThirdUpgrade()
    {
        if (blackholeThirdUpgradeButton.unlocked)
        {
            thirdUpgradeUnlock = true;
        }
    }
}
