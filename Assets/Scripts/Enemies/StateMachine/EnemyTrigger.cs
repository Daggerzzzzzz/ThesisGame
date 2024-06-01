using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] 
    private GameObject darkPillar;
    
    private Enemy OnEnemy => GetComponentInParent<Enemy>();
    private bool attackOnce;
    
    private void EnemyAnimation()
    {
        OnEnemy.AnimationTriggerForEnemy();
        ResetAttack();
    }
    
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(OnEnemy.attackCheck.position, OnEnemy.attackCheckRadius);
        
        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Player Trigger Collider") && !attackOnce)
            {
                PlayerStats target = hit.GetComponentInParent<PlayerStats>();
                OnEnemy.OnEntityStats.DoDamage(target, gameObject);
                attackOnce = true;
            }
        }
    }

    private void MagicTrigger()
    {
        GameObject newDarkPillar = Instantiate(darkPillar, PlayerManager.Instance.player.transform.position, Quaternion.identity);
        newDarkPillar.GetComponent<DarkPillarController>().SetUpDarkPillar(OnEnemy.OnEntityStats, gameObject);
    }

    private void ResetAttack()
    {
        attackOnce = false;
    }
}
