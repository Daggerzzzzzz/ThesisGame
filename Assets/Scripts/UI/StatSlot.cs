using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatSlot : MonoBehaviour
{
    [SerializeField] 
    private string statName;
    [SerializeField] 
    private StatType statType;
    [SerializeField] 
    private TextMeshProUGUI statValueText;
    [SerializeField] 
    private TextMeshProUGUI statNameText;

    private void OnValidate()
    {
        gameObject.name = "Stat -" + statName;

        if (statNameText != null)
        {
            statNameText.text = statName;
        }
    }

    private void Start()
    {
        UpdateStatValue();
    }

    public void UpdateStatValue()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        
        if (playerStats != null)
        {
            statValueText.text = playerStats.StatToGet(statType).GetValue().ToString();
        }

        statValueText.text = statType switch
        {
            StatType.HEALTH => playerStats.CalculateMaxHealthValue().ToString(),
            StatType.DAMAGE => (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString(),
            StatType.CRITCHANCE => (playerStats.criticalChance.GetValue() + playerStats.agility.GetValue()).ToString(),
            StatType.CRITICALDAMAGE => (playerStats.criticalDamage.GetValue() + playerStats.strength.GetValue())
                .ToString(),
            StatType.EVASION => (playerStats.dodge.GetValue() + playerStats.agility.GetValue()).ToString(),
            StatType.BURNDAMAGE => (playerStats.burnDamage.GetValue() + playerStats.intelligence.GetValue()).ToString(),
            StatType.FREEZEDAMAGE => (playerStats.freezeDamage.GetValue() + playerStats.intelligence.GetValue())
                .ToString(),
            StatType.SHOCKDAMAGE => (playerStats.shockDamage.GetValue() + playerStats.intelligence.GetValue())
                .ToString(),
            _ => statValueText.text
        };
    }
}
