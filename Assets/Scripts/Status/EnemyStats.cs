using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    private Enemy enemy;
    
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    
    protected override void Start()
    {
        base.Start();
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
    }
}
