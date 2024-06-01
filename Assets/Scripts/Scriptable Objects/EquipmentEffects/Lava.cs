using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/ItemEffect/Lava")]
public class Lava : ItemEffectSO
{
    [SerializeField] 
    private GameObject lavaPrefab;

    private GameObject newLava;
    
    public override void ExecuteEffect(Vector2 spawnPosition, EntityStats entityStats)
    {
        if (Inventory.Instance.UseWeapon())
        {
            if (newLava != null)
            {
                Destroy(newLava);
            }
            
            Player player = PlayerManager.Instance.player;
            newLava = Instantiate(lavaPrefab, spawnPosition, player.transform.rotation);
        }
    }
}
