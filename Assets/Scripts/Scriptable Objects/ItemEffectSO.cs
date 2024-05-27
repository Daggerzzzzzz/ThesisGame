using UnityEngine;

public class ItemEffectSO : ScriptableObject
{
   public virtual void ExecuteEffect(Vector2 spawnPosition, EntityStats entityStats)
   {
      Debug.Log("Effect Execute");
   }
}
