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
    private float swordSummonDelay;

    [Header("Base Upgrade")] 
    [SerializeField]
    private SkillTreeSlot blackholeUnlockButton;
    public bool BaseUpgradeUnlock { get; private set; }

    [Header("Second Upgrade")] 
    [SerializeField]
    private SkillTreeSlot blackholeSecondUpgradeButton;
    public bool SecondUpgradeUnlock { get; private set; }
    
    [Header("Third Upgrade")] 
    [SerializeField]
    private SkillTreeSlot blackholeThirdUpgradeButton;
    public bool ThirdUpgradeUnlock { get; private set; }
    
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

    protected override void UseSkill()
    {
        base.UseSkill();
        
        BlackholeUpgrades(BaseUpgradeUnlock, SecondUpgradeUnlock, ThirdUpgradeUnlock);
        
        newBlackhole = Instantiate(blackholePrefab, player.transform.position, quaternion.identity);

        currentBlackhole = newBlackhole.GetComponent<BlackholeSkillController>();
        
        currentBlackhole.SetupBlackhole( maximumSize, speedOfGrowth, amountOfSwords, swordSummonDelay, angleIncrement, player);
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
            BaseUpgradeUnlock = true;
        }
    }
    
    private void UnlockSecondUpgrade()
    {
        if (blackholeSecondUpgradeButton.unlocked)
        {
            SecondUpgradeUnlock = true;
        }
    }
    
    private void UnlockThirdUpgrade()
    {
        if (blackholeThirdUpgradeButton.unlocked)
        {
            ThirdUpgradeUnlock = true;
        }
    }

    protected override void CheckUnlocked()
    {
        UnlockBaseUpgrade();
        UnlockSecondUpgrade();
        UnlockThirdUpgrade();
    }
}
