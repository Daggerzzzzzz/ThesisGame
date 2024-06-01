using UnityEngine;

public class LaserController : MonoBehaviour
{
    private EntityStats entityStats;
    private Animator anim;
    
    private float moveX;
    private float moveY;
    
    private EntityStats enemyEntityStats;
    private GameObject enemyGameObject;
    
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        anim.SetFloat(MoveX, moveX);
        anim.SetFloat(MoveY, moveY);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EntityStats playerEntityStats = other.GetComponent<EntityStats>();
            enemyEntityStats.DoDamage(playerEntityStats, enemyGameObject);
        }
    }
    
    public void SetupLaser(float moveX, float moveY, EntityStats _entityStats, GameObject _enemyGameObject)
    {
        this.moveX = moveX;
        this.moveY = moveY;
        enemyEntityStats = _entityStats;
        enemyGameObject = _enemyGameObject;
    }
    
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
