using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    [SerializeField] 
    private float colorVanishingSpeed;
    [SerializeField] 
    private Transform attackCheck;
    [SerializeField] 
    private float attackCheckRadius = 0.7f;
    
    private float cloneTimer;
    private Vector2 cloneAttackDirection;
    private Vector2 cloneHitBoxDirection;

    private SpriteRenderer srClone;
    //private SpriteRenderer srCloneAttackCheck;
    private Animator anim;
    private GameObject playerReference;
    private Player player;
    
    private static readonly int AttackNumber = Animator.StringToHash("attackNumber");
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    private readonly HashSet<Enemy> attackedEnemies = new();

    private void Awake()
    {
        srClone = GetComponent<SpriteRenderer>();
        //srCloneAttackCheck = attackCheck.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerReference = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        if (playerReference != null)
        {
            player = playerReference.GetComponent<Player>();
        }
        
        if (player.dashDirection != Vector2.zero)
        {
            cloneAttackDirection = player.dashDirection;
            cloneHitBoxDirection = cloneAttackDirection;
        }

        if (cloneHitBoxDirection != Vector2.zero)
        {
            AttackDirection();
            attackCheck.transform.localPosition = cloneHitBoxDirection;
        }
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        
        anim.SetFloat(MoveX, cloneAttackDirection.x);
        anim.SetFloat(MoveY, cloneAttackDirection.y);

        if (cloneTimer < 0)
        {
            srClone.color = new Color(1, 1, 1, srClone.color.a - (Time.deltaTime * colorVanishingSpeed));
            //srCloneAttackCheck.color = new Color(1, 1, 1, srCloneAttackCheck.color.a - (Time.deltaTime * colorVanishingSpeed));
        }
    }

    public void SetUpClone(Vector2 _newTransform, float _cloneDuration, bool canAttack)
    {
        if (canAttack)
        {
            anim.SetInteger(AttackNumber, 1);
        }
        transform.position = _newTransform;
        cloneTimer = _cloneDuration;
    }
    
    private void PlayerAnimation()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (!attackedEnemies.Contains(enemy))
                {
                    enemy.DamageEffect();
                    attackedEnemies.Add(enemy);
                }
            }
        }
    }

    private void ResetColliders()
    {
        attackedEnemies.Clear();
    }

    private void AttackDirection()
    {
        if (cloneAttackDirection.x != 0)
        {
            if (cloneAttackDirection.x > 0)
            {
                cloneHitBoxDirection= new Vector2(0.25f, 0f);
            }
            if (cloneAttackDirection.x < 0)
            {
                cloneHitBoxDirection = new Vector2(-0.25f, 0f); 
            }
        }
        else if (cloneAttackDirection.y > 0)
        {
            cloneHitBoxDirection = new Vector2(0f, 0.15f);
        }
        else
        {
            cloneHitBoxDirection = new Vector2(0f, -0.25f);
        }
    }
}
