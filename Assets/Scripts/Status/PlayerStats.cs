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

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int damage, GameObject sender)
    {
        base.TakeDamage(damage, sender);
        EquipmentDataSO currentArmor;
        
        currentArmor = Inventory.Instance.GetEquipment(EquipmentType.ARMOR);
        
        if (currentArmor.itemName == "Thornmail")
        {
            Inventory.Instance.GetEquipment(EquipmentType.ARMOR).UseEffect(sender.transform.position, sender.GetComponentInParent<EnemyStats>());
        }
        
        player.DamageEffect(sender);
    }

    protected override void EntityDeath()
    {
        base.EntityDeath();
        player.EntityDeath();
    }
    
    public void LoadData(GameData data)
    {
        strength.baseValue = data.strength;
        agility.baseValue = data.agility;
        intelligence.baseValue = data.intelligence;
        vitality.baseValue = data.vitality;
        currentHealth = data.currentHealth;
        
        Inventory.Instance.UIStatSlotUpdate();
    }

    public void SaveData(ref GameData data)
    {
        data.strength = strength.GetValue();
        data.agility = agility.GetValue();
        data.intelligence = agility.GetValue();
        data.vitality = vitality.GetValue();
        data.currentHealth = currentHealth;
    }
}
