using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyStats : EntityStats
{
    private Enemy enemy;
    private ItemDrop itemDrop;
    private HealthBar healthBar;

    [Header("Level Details")] 
    [SerializeField]
    private int level = 1;
    [Range(0f, 1f)] 
    [SerializeField]
    private float percentageModifier = .4f;
    
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
        itemDrop = GetComponent<ItemDrop>();
        healthBar = GetComponentInChildren<HealthBar>();
    }

    private void ApplyModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(vitality);
        
        Modify(damage);
        Modify(criticalChance);
        Modify(criticalDamage);
        
        Modify(maxHealth);
        Modify(armor);
        Modify(dodge);
        
        Modify(burnDamage);
        Modify(freezeDamage);
        Modify(shockDamage);
    }

    private void Modify(Stats stats)
    {
        for (int i = 0; i < level; i++)
        {
            float modifier = stats.GetValue() * percentageModifier;
            stats.AddModifiers(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        enemy.DamageEffect();
    }

    protected override void EntityDeath()
    {
        base.EntityDeath();
        enemy.EntityDeath();
        StartCoroutine(DestroyUponDeath());
        healthBar.DisableSpriteRenderer();
    }

    private IEnumerator DestroyUponDeath()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        itemDrop.GenerateDrop();
    }
}
