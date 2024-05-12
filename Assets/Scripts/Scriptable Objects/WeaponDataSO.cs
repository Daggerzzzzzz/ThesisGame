using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Weapon")]
public class WeaponDataSO : EquipmentDataSO
{
    [TextArea] 
    public string itemEffectDesc;
    
    public int strength;
    public int agility;
    public int intelligence;
    
    private int damage;
    private int criticalChance;
    private int criticalDamage;
    
    private int dodge;
    
    public int burnDamage;
    public int freezeDamage;
    public int shockDamage;
    
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        
        playerStats.strength.AddModifiers(strength);
        playerStats.agility.AddModifiers(agility);
        playerStats.intelligence.AddModifiers(intelligence);
        
        playerStats.damage.AddModifiers(strength + 1);
        playerStats.criticalChance.AddModifiers(agility * 1);
        playerStats.criticalDamage.AddModifiers(strength * 1);
        
        playerStats.dodge.AddModifiers(agility * 1);
        
        playerStats.burnDamage.AddModifiers(burnDamage + intelligence);
        playerStats.freezeDamage.AddModifiers(freezeDamage + intelligence);
        playerStats.shockDamage.AddModifiers(shockDamage + intelligence);
    }
    
    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        
        playerStats.strength.RemoveModifiers(strength);
        playerStats.agility.RemoveModifiers(agility);
        playerStats.intelligence.RemoveModifiers(intelligence);
        
        playerStats.damage.RemoveModifiers(strength + 1);
        playerStats.criticalChance.RemoveModifiers(agility * 1);
        playerStats.criticalDamage.RemoveModifiers(strength * 1);
        
        playerStats.dodge.RemoveModifiers(agility * 1);
        
        playerStats.burnDamage.RemoveModifiers(burnDamage + intelligence);
        playerStats.freezeDamage.RemoveModifiers(freezeDamage + intelligence);
        playerStats.shockDamage.RemoveModifiers(shockDamage + intelligence);
    }
    
    public override string GetDescription()
    {
        sb.Length = 0;
        minDescLength = 0;
        
        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        
        AddItemDescription(burnDamage, "Burn Damage");
        AddItemDescription(freezeDamage, "Freeze Damage");
        AddItemDescription(shockDamage, "Shock Damage");
        
        if (minDescLength < 5)
        {
            for (int i = 0; i < 5 - minDescLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        if (itemEffectDesc.Length > 0)
        {
            sb.Append(itemEffectDesc); 
        }
        
        return sb.ToString();
    }
}
