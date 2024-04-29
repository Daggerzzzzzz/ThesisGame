using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;

public class HealthBar : MonoBehaviour
{
    private Material material;
    private EntityStats entityStats;
    
    private static readonly int Health = Shader.PropertyToID("_Health");

    private void Awake()
    {
        material = GetComponentInChildren<SpriteRenderer>().material;
        entityStats = GetComponentInParent<EntityStats>();
    }
    
    public void UpdateHealthUI()
    {
        int maxHealth = entityStats.CalculateMaxHealthValue();
        int currentHealth = entityStats.currentHealth;
        
        float normalizedHealth = (float)currentHealth / maxHealth;
        
        material.SetFloat(Health, normalizedHealth);
    }
}
