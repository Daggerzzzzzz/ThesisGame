using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum StatType
{
    STRENGTH,
    AGILITY,
    VITALITY,
    INTELLIGENCE,
    DAMAGE,
    CRITCHANCE,
    CRITICALDAMAGE,
    HEALTH,
    ARMOR,
    DODGE,
    BURNDAMAGE,
    FREEZEDAMAGE,
    SHOCKDAMAGE
}
    
public class EntityStats : MonoBehaviour
{
    [SerializeField] 
    private float statusEffectsDuration = 5;
    
    [Header("Main Stats")]
    public Stats strength; //1 point increase damage by 1 and crit damage by 1%
    public Stats agility; //1 point increase dodge by 1% and crit chance by 1%
    public Stats vitality; //1 point increase health by 5 points, can decrease damage from status ailments
    public Stats intelligence; //1 point increase damage of status effects
    
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
    public bool isDead;
    
    public UnityEvent onHealthChanged;
    private EntityFx entityFx;
    private bool isVulnerable;
    public int TotalDamage { get; private set; }
    public bool isInvincible { get; internal set; }

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

            if (currentHealth <= 0 && !isDead)
            {
                EntityDeath();
            }
            
            burnDamageTimer = burnDamageCooldown;
        }
    }

    public virtual void DoDamage(EntityStats entityStats, GameObject sender)
    {
        if(AvoidAttack(entityStats))
        {
            return;
        }

        TotalDamage = damage.GetValue() + strength.GetValue();

        if (CritEntity())
        {
            TotalDamage = CalculateCriticalDamage(TotalDamage);
        }

        TotalDamage = CalculateTargetsArmor(entityStats, TotalDamage);
        entityStats.TakeDamage(TotalDamage, sender);
        StatusAilments(entityStats, sender);
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

    public virtual void TakeDamage(int _damage, GameObject sender)
    {
        if (isInvincible)
        {
            return;
        }
        
        DecreaseHealthBy(_damage);
        
        if (currentHealth <= 0 && !isDead)
        {
            EntityDeath();
        }
    }

    private void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
        {
            _damage = Mathf.RoundToInt(_damage * 1.5f);
        }

        if (_damage > 0)
        {
            entityFx.CreateInformationText(_damage.ToString());
        }
        
        currentHealth -= _damage;

        onHealthChanged?.Invoke();
    }

    public void IncreaseHealthBy(int amount)
    {
        currentHealth += amount;

        if (currentHealth > CalculateMaxHealthValue())
        {
            currentHealth = CalculateMaxHealthValue();
        }
        
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

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCriticalDamage = (criticalDamage.GetValue() + strength.GetValue()) * .01f;
        
        float critDamage = _damage * totalCriticalDamage;
        
        return Mathf.RoundToInt(critDamage);
    }
    
    public int CalculateMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    public virtual void StatusAilments(EntityStats target, GameObject sender)
    {
        if (isInvincible)
        {
            return;
        }
        
        int fireDamage = burnDamage.GetValue();
        int iceDamage = freezeDamage.GetValue();
        int electricDamage = shockDamage.GetValue();

        int totalDamage = fireDamage + iceDamage + electricDamage + intelligence.GetValue();
        totalDamage -= target.vitality.GetValue() * 3;
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        
        target.TakeDamage(totalDamage, sender);

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

    protected virtual void ApplyStatusAilments(bool burn, bool freeze, bool shock)
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
        }
    }

    public void MakeVulnerable(float duration)
    {
        StartCoroutine(VulnerableTimeDelay(duration));
    }

    private IEnumerator VulnerableTimeDelay(float duration)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(duration);

        isVulnerable = false;
    }

    private void SetupBurnDamage(int _damage)
    {
        burnDamageOverTime = _damage;
    }
    
    private void SetupShockDamage(int _damage)
    {
        lightningDamage = _damage;
    }
    
    protected virtual void EntityDeath()
    {
        isDead = true;
    }

    public void MakeInvincible(float seconds)
    {
        if (!isDead)
        {
            StartCoroutine(InvincibleDelay(seconds));
        }
    }

    private IEnumerator InvincibleDelay(float seconds)
    {
        isInvincible = true;
        yield return new WaitForSeconds(seconds);
        isInvincible = false;
    }

    public Stats StatToGet(StatType statType)
    {
        return statType switch
        {
            StatType.STRENGTH => strength,
            StatType.AGILITY => agility,
            StatType.VITALITY => vitality,
            StatType.INTELLIGENCE => intelligence,
            StatType.DAMAGE => damage,
            StatType.CRITCHANCE => criticalChance,
            StatType.CRITICALDAMAGE => criticalDamage,
            StatType.HEALTH => maxHealth,
            StatType.ARMOR => armor,
            StatType.DODGE => dodge,
            StatType.BURNDAMAGE => burnDamage,
            StatType.FREEZEDAMAGE => freezeDamage,
            StatType.SHOCKDAMAGE => shockDamage,
            _ => null
        };
    }
}
