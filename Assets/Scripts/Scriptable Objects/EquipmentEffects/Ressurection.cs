using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/ItemEffect/Ressurection")]
public class Ressurection : ItemEffectSO
{
    [Range(0f, 1f)] 
    [SerializeField] 
    private float healPercent;
    
    public override void ExecuteEffect(Vector2 spawnPosition, EntityStats playerStats)
    {
        if (Inventory.Instance.UseArmor())
        {
            int healAmount = Mathf.RoundToInt(playerStats.CalculateMaxHealthValue() * healPercent);
            playerStats.IncreaseHealthBy(healAmount);   
            Inventory.Instance.firstTimeUsed = false;
        }
    }
}
