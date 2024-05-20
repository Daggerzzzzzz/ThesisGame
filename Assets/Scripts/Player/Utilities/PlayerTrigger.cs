using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private Player OnPlayer => GetComponentInParent<Player>();
    private bool attackOnce;
    
    private void PlayerAnimation()
    {
        OnPlayer.AnimationTriggerForPlayer();
        ResetAttack();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(OnPlayer.attackCheck.position, OnPlayer.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Enemy") && !attackOnce)
            {
                EnemyStats target = hit.GetComponent<EnemyStats>();
                OnPlayer.OnEntityStats.DoDamage(target,gameObject);
                attackOnce = true;
            }
        }
    }

    private void ResetAttack()
    {
        attackOnce = false;
    }

    private void ThrowSword()
    {
        SkillManager.Instance.Sword.CreateSword();
    }
}
