using UnityEngine;

public class PlayerStats : EntityStats, ISaveManager
{
    private Player player;

    public void AddStrength()
    {
        Modify(strength); 
    }
    
    public void AddVitality()
    {
        Modify(vitality);
    }
    
    public void AddAgility()
    {
        Modify(agility);
    }
    
    public void AddIntelligence()
    {
        Modify(intelligence);
    }
    
    private void Modify(Stats stats)
    {
        stats.AddModifiers(1);
        PlayerManager.Instance.statPoints--;
        Inventory.Instance.UIStatSlotUpdate();
    }

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int damage, GameObject sender)
    {
        base.TakeDamage(damage, sender);
        player.DamageEffect(sender);
    }

    protected override void EntityDeath()
    {
        base.EntityDeath();
        player.EntityDeath();
    }

    protected override void ApplyStatusAilments(bool burn, bool freeze, bool shock)
    {
        playerAttack = true;
        
        base.ApplyStatusAilments(burn, freeze, shock);
    }

    public void LoadData(GameData data)
    {
        strength.AddModifiers(data.strength);
        agility.AddModifiers(data.agility);
        intelligence.AddModifiers(data.intelligence);
        vitality.AddModifiers(data.vitality);
        CurrentHealth = data.currentHealth;
        
        Inventory.Instance.UIStatSlotUpdate();
    }

    public void SaveData(ref GameData data)
    {
        data.strength = strength.GetValue();
        data.agility = agility.GetValue();
        data.intelligence = agility.GetValue();
        data.vitality = vitality.GetValue();
        data.currentHealth = CurrentHealth;
    }
}
