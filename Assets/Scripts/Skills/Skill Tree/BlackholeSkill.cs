using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BlackholeSkill : Skill
{
    [SerializeField] 
    private GameObject blackholePrefab;
    [SerializeField] 
    private float maximumSize;
    [SerializeField]
    private float speedOfGrowth;
    [SerializeField]
    private int amountOfSwords;
    [SerializeField]
    private float swordAttackCooldown;
    [SerializeField]
    private float angleIncrement;
    
    
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
        
        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, quaternion.identity);

        BlackholeSkillController newBlackholeSkillController = newBlackhole.GetComponent<BlackholeSkillController>();
        
        newBlackholeSkillController.SetupBlackhole( maximumSize, speedOfGrowth, amountOfSwords, swordAttackCooldown, angleIncrement);
    }
}
