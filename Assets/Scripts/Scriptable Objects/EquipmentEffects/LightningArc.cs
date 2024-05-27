using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/ItemEffect/LightningArc")]
public class LightningArc : ItemEffectSO
{
    [SerializeField] 
    private GameObject lightningPrefab;
    
    public override void ExecuteEffect(Vector2 spawnPosition, EntityStats entityStats)
    {
        Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);
    }
}
