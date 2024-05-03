using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

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
    
    [Header("Blackhole Upgrades")]
    [SerializeField] 
    private bool baseUpgrade;
    [SerializeField]
    private bool secondUpgrade;
    [SerializeField]
    private bool thirdUpgrade;
    
    [SerializeField]
    private int angleIncrement;
    [SerializeField]
    private int amountOfSwords;

    private BlackholeSkillController currentBlackhole;
    
    protected override void Start()
    {
        base.Start();
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
        
        BlackholeUpgrades(baseUpgrade, secondUpgrade, thirdUpgrade);
        
        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, quaternion.identity);

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
        if (baseUpgrade)
        {
            angleIncrement = 60;
            amountOfSwords = 360 / 60;
        }
        else if (secondUpgrade)
        {
            angleIncrement = 45;
            amountOfSwords = 360 / 45;
        }
        else
        {
            angleIncrement = 30;
            amountOfSwords = 360 / 30;
        }
    }
}
