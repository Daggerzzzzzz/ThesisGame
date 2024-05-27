using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/ItemEffect/HealEffect")]
public class HealEffect : ItemEffectSO
{
    [Range(0f, 1f)] 
    [SerializeField] 
    private float healPercent;
    
    public override void ExecuteEffect(Vector2 spawnPosition, EntityStats playerStats)
    {
        int healAmount = Mathf.RoundToInt(playerStats.CalculateMaxHealthValue() * healPercent);
        playerStats.IncreaseHealthBy(healAmount * playerStats.TotalDamage);
    }
}
