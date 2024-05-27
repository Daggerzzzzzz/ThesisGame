using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/ItemEffect/DamageReflect")]
public class DamageReflectEffect : ItemEffectSO
{
   [Range(0f, 1f)] 
   [SerializeField] 
   private float damageReflectPercent;
   
   public override void ExecuteEffect(Vector2 spawnPosition, EntityStats enemyStats)
   {
      PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
      int damageAmount = Mathf.RoundToInt(playerStats.maxHealth.GetValue() * damageReflectPercent);
      Debug.Log(damageAmount);
      enemyStats.TakeDamage(damageAmount, PlayerManager.Instance.player.gameObject);
   }
}
