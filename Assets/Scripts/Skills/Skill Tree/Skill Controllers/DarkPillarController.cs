using UnityEngine;

public class DarkPillarController : MonoBehaviour
{
    private EntityStats enemyEntityStats;
    private GameObject enemyGameObject;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EntityStats playerEntityStats = other.GetComponent<EntityStats>();
            enemyEntityStats.DoDamage(playerEntityStats, enemyGameObject);
        }
    }

    public void SetUpDarkPillar(EntityStats _entityStats, GameObject _enemyGameObject)
    {
        enemyEntityStats = _entityStats;
        enemyGameObject = _enemyGameObject;
    }
    
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
