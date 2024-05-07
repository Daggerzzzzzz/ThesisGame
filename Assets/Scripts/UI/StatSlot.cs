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
    }
}
