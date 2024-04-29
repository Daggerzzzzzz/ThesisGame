using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EntityStats : MonoBehaviour
{
    [SerializeField] 
    private float statusEffectsDuration = 4;
    [SerializeField] 
    private GameObject lightningPrefab;
    
    [Header("Main Stats")]
    public Stats strength; //1 point increase damage by 1 and crit damage by 1%
    public Stats agility; //1 point increase dodge by 1% and crit chance by 1%
    public Stats vitality; //1 point increase health by 3 or 5 points, can decrease damage from status ailments
    
    [Header("Offensive Stats")] 
    public Stats damage;
    public Stats criticalChance;
    public Stats criticalDamage;

    [Header("Defensive Stats")] 
    public Stats maxHealth;
    public Stats armor;
    public Stats dodge;

    [Header("Status Effects")] 
    public Stats burnDamage;
    public Stats freezeDamage;
    public Stats shockDamage;

    public bool isBurning;
    public bool isFreezing;
    public bool isOnShock;

    private float burnTimer;
    private float freezeTimer;
    private float shockTimer;
    
    private float burnDamageCooldown = .3f;
    private float burnDamageTimer;
    private int burnDamageOverTime;
    private int lightningDamage;
    
    public int currentHealth;
    
    public UnityEvent onHealthChanged;
    private EntityFx entityFx;
    
    protected virtual void Start()
    {
        currentHealth = CalculateMaxHealthValue();
        criticalDamage.SetDefaultValue(150);
        entityFx = GetComponent<EntityFx>();
    }

    protected void Update()
    {
        burnTimer -= Time.deltaTime;
        freezeTimer -= Time.deltaTime;
        shockTimer -= Time.deltaTime;
        
        burnDamageTimer -= Time.deltaTime;

        if (burnTimer < 0)
        {
            isBurning = false;
        }

        if (freezeTimer < 0)
        {
            isFreezing = false;
        }

        if (shockTimer < 0)
        {
            isOnShock = false;
        }

        if (burnDamageTimer < 0 && isBurning)
        {
            DecreaseHealthBy(burnDamageOverTime);

            if (currentHealth < 0)
            {
                EntityDeath();
            }
            
            burnDamageTimer = burnDamageCooldown;
        }
    }

    public virtual void DoDamage(EntityStats entityStats)
    {
        if(AvoidAttack(entityStats))
        {
            return;
        }

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CritEntity())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CalculateTargetsArmor(entityStats, totalDamage);

        //entityStats.TakeDamage(totalDamage);
        StatusAilments(entityStats);
    }

    private int CalculateTargetsArmor(EntityStats entityStats, int totalDamage)
    {
        if (entityStats.isFreezing)
        {
            totalDamage -= Mathf.RoundToInt(entityStats.armor.GetValue() * 0.8f);
        }
        else
        {
            totalDamage -= entityStats.armor.GetValue();
        }
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool AvoidAttack(EntityStats entityStats)
    {
        int totalDodge = entityStats.dodge.GetValue() + entityStats.agility.GetValue();

        if (isOnShock)
        {
            totalDodge += 20;
        }

        if (Random.Range(0, 100) < totalDodge)
        {
            return true;
        }
        return false;
    }

    public virtual void TakeDamage(int damage)
    {
        DecreaseHealthBy(damage);
        
        Debug.Log(damage);

        if (currentHealth < 0)
        {
            EntityDeath();
        }
    }

    protected virtual void DecreaseHealthBy(int damage)
    {
        currentHealth -= damage;

        onHealthChanged?.Invoke();
    }

    private bool CritEntity()
    {
        int totalCriticalChance = criticalChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    private int CalculateCriticalDamage(int damage)
    {
        float totalCriticalDamage = (criticalDamage.GetValue() + strength.GetValue()) * .01f;
        
        float critDamage = damage * totalCriticalDamage;
        
        return Mathf.RoundToInt(critDamage);
    }
    
    public int CalculateMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    protected virtual void StatusAilments(EntityStats target)
    {
        int fireDamage = burnDamage.GetValue();
        int iceDamage = freezeDamage.GetValue();
        int electricDamage = shockDamage.GetValue();

        int totalDamage = fireDamage + iceDamage + electricDamage + strength.GetValue();
        totalDamage -= target.vitality.GetValue() * 3;
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        
        target.TakeDamage(totalDamage);

        if (Mathf.Max(fireDamage, iceDamage, electricDamage) <= 0)
        {
            return;
        }

        bool applyBurn = fireDamage > iceDamage && fireDamage > electricDamage;
        bool applyFreeze = iceDamage > fireDamage && iceDamage > electricDamage;
        bool applyShock = electricDamage > fireDamage && electricDamage > iceDamage;

        if (applyBurn)
        {
            target.SetupBurnDamage(Mathf.RoundToInt(fireDamage * .2f));
        }

        if (applyShock)
        {
            target.SetupShockDamage(Mathf.RoundToInt(electricDamage * .2f));
        }
        
        target.ApplyStatusAilments(applyBurn, applyFreeze, applyShock);
    }

    private void ApplyStatusAilments(bool burn, bool freeze, bool shock)
    {
        bool canBurn = !isBurning && !isOnShock && !isOnShock;
        bool canFreeze = !isBurning && !isOnShock && !isOnShock;
        bool canShock = !isBurning && !isFreezing;

        if (burn && canBurn)
        {
            isBurning = true;
            burnTimer = statusEffectsDuration;
            
            entityFx.BurnFx(statusEffectsDuration);
        }
        
        if (freeze && canFreeze)
        {
            float slowPercentage = .3f;
            isFreezing = true;
            freezeTimer = statusEffectsDuration;
            
            GetComponent<Entity>().SlowEntityBy(slowPercentage, statusEffectsDuration);
            entityFx.FreezeFx(statusEffectsDuration);
        }
        
        if (shock && canShock)
        {
            if (!isOnShock) 
            {
                isOnShock = true;
                shockTimer = statusEffectsDuration;
            
                entityFx.ShockFx(statusEffectsDuration);
            }
            else
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                float closestDistance = Mathf.Infinity;
                Transform closestEnemy = null;
                
                foreach (var hit in colliders)
                {
                    if (hit.CompareTag("Enemy") && transform.position != hit.transform.position)
                    {
                        float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                        if (distanceToEnemy < closestDistance)
                        {
                            closestDistance = distanceToEnemy;
                            closestEnemy = hit.transform;
                        }

                        if (closestEnemy == null)
                        {
                            closestEnemy = transform;
                        }
                    }
                }

                if (closestEnemy != null)
                {
                    GameObject newLightningPrefab = Instantiate(lightningPrefab, transform.position, Quaternion.identity);
                    newLightningPrefab.GetComponent<LightningController>().SetupLightning(lightningDamage, closestEnemy.GetComponent<EntityStats>());
                }
            }
        }
    }

    private void SetupBurnDamage(int damage)
    {
        burnDamageOverTime = damage;
    }
    
    private void SetupShockDamage(int damage)
    {
        lightningDamage = damage;
    }
    
    protected virtual void EntityDeath()
    {
        
    }
}
