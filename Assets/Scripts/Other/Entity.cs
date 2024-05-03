using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator OnAnim { get; private set; }
    public Rigidbody2D OnRb { get; set; }
    public EntityFx OnEntityFx { get; private set; }
    public EntityStats OnEntityStats { get; private set; }
    public CapsuleCollider2D OnCapsuleCollider2D { get; private set; }
    #endregion

    public Vector2 OnMovementDirection { get; private set; }
    public Vector2 OnAttackDirection { get; private set; }

    [Header("Collision Info")] 
    public Transform attackCheck;
    public float attackCheckRadius;

    [Header("Knockback Info")]
    [SerializeField]
    protected float knockbackDuration;
    [SerializeField]
    protected bool isKnocked;
    [SerializeField] 
    protected float thrust;
    private Vector2 difference;
    
    public Vector2 animatorDirection;
    
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    
    protected virtual void Awake()
    {
        OnAnim = GetComponentInChildren<Animator>();
        OnRb = GetComponent<Rigidbody2D>();
        OnEntityFx = GetComponent<EntityFx>();
        OnEntityStats = GetComponent<EntityStats>();
        OnCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Start()
    {
        
    }
    
    protected virtual void Update()
    {
        
    }

    public void DamageEffect()
    {
        OnEntityFx.StartCoroutine("FlashEffects");
        //StartCoroutine("HitKnockback");
    }
    
    public void SetVelocity(float xInput, float yInput, float entitySpeed)
    {
        if (isKnocked)
        {
            return;
        }
        OnMovementDirection = new Vector2(xInput, yInput) * entitySpeed;
        OnRb.velocity = new Vector2(OnMovementDirection.x, OnMovementDirection.y);
        if (OnMovementDirection != Vector2.zero)
        {
            SetAnimator(OnMovementDirection);
        }
    }

    public void SetAnimator(Vector2 movementInput)
    {
        OnAnim.SetFloat(MoveX, movementInput.x);
        OnAnim.SetFloat(MoveY, movementInput.y);
        animatorDirection = movementInput;
    }

    public void SetZeroVelocity()
    {
        if (isKnocked)
        {
            return;
        }

        OnRb.velocity = Vector2.zero;
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        difference = (OnRb.transform.position - Player.OnTransformPosition.position);
        difference = difference.normalized * thrust;
        OnRb.AddForce(difference, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public void AttackDirection(Vector2 movementInput)
    {
        if (Mathf.Abs(movementInput.x) > Mathf.Abs(movementInput.y))
        {
            if (movementInput.x > 0)
            {
                OnAttackDirection = new Vector2(1.2f, -0.3f);
            }
            if (movementInput.x < 0)
            {
                OnAttackDirection = new Vector2(-1.2f, -0.3f);
            }
        }
        if (Mathf.Abs(movementInput.x) < Mathf.Abs(movementInput.y))
        {
            if (movementInput.y > 0)
            {
                OnAttackDirection = new Vector2(0f, 0.6f);
            }
            if (movementInput.y < 0)
            {
                OnAttackDirection = new Vector2(0f, -1f);
            }
        }
    }

    public virtual void EntityDeath()
    {
        
    }
    
    public virtual void SlowEntityBy(float slowPercent, float slowDuration)
    {
        
    }

    protected virtual void ReturnToNormalSpeed()
    {
        OnAnim.speed = 1;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(OnAnim), OnAnim);
        HelperUtilities.ValidateCheckNullValue(this, nameof(OnRb), OnRb);
        HelperUtilities.ValidateCheckNullValue(this, nameof(OnEntityFx), OnEntityFx);
    }
#endif
}
