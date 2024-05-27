using UnityEngine;

public class ZombieTrigger : MonoBehaviour
{
    private EnemyZombie OnEnemyZombie => GetComponentInParent<EnemyZombie>();
    private bool attackOnce;
    
    private void EnemyAnimation()
    {
        OnEnemyZombie.AnimationTriggerForEnemy();
        ResetAttack();
    }
    
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(OnEnemyZombie.attackCheck.position, OnEnemyZombie.attackCheckRadius);
        
        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Player Trigger Collider") && !attackOnce)
            {
                PlayerStats target = hit.GetComponentInParent<PlayerStats>();
                OnEnemyZombie.OnEntityStats.DoDamage(target, gameObject);
                attackOnce = true;
            }
        }
    }

    private void ResetAttack()
    {
        attackOnce = false;
    }
}
