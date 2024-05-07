using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Armor")]
public class ArmorDataSO : EquipmentDataSO
{
    public int vitality;
    public int armor;

    private int maxHealth;
    
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        
        playerStats.vitality.AddModifiers(vitality);
        playerStats.maxHealth.AddModifiers(vitality * 5);
        playerStats.armor.AddModifiers(armor);
        
        playerStats.CalculateMaxHealthValue();
    }
    
    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        
        playerStats.vitality.RemoveModifiers(vitality);
        playerStats.maxHealth.RemoveModifiers(vitality * 5);
        playerStats.armor.RemoveModifiers(armor);
        
        playerStats.CalculateMaxHealthValue();
    }
    
    public override string GetDescription()
    {
        sb.Length = 0;
        minDescLength = 0;
        
        AddItemDescription(vitality, "Vitality");
        AddItemDescription(armor, "Armor");

        if (minDescLength < 5)
        {
            for (int i = 0; i < 5 - minDescLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }
        
        return sb.ToString();
    }
}
