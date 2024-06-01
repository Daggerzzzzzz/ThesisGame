using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/ItemEffect/GetsugaTenshou")]
public class GetsugaTenshou : ItemEffectSO
{
    [SerializeField] 
    private GameObject getsugaPrefab;
    
    public override void ExecuteEffect(Vector2 spawnPosition, EntityStats entityStats)
    {
        Player player = PlayerManager.Instance.player;
        GameObject newGetsuga = Instantiate(getsugaPrefab, player.transform.position, player.transform.rotation);
        newGetsuga.GetComponent<GetsugaController>().SetupGetsuga(spawnPosition.x, spawnPosition.y);
    }
}
