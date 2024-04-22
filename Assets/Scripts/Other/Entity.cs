using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator OnAnim { get; private set; }
    public Rigidbody2D OnRb { get; set; }
    #endregion

    public Vector2 MovementDirection { get; set; }

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

    public EntityFx entityFx { get; private set; }
    public Vector2 animatorDirection;
    
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    
    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        OnAnim = GetComponentInChildren<Animator>();
        OnRb = GetComponent<Rigidbody2D>();
        entityFx = GetComponent<EntityFx>();
    }
    
    protected virtual void Update()
    {
        
    }

    public void Damage()
    {
        entityFx.StartCoroutine("FlashEffects");
        StartCoroutine("HitKnockback");
    }
    
    public void SetVelocity(float xInput, float yInput, float entitySpeed)
    {
        if (isKnocked)
        {
            return;
        }
        MovementDirection = new Vector2(xInput, yInput) * entitySpeed;
        OnRb.velocity = new Vector2(MovementDirection.x, MovementDirection.y);
        if (MovementDirection != Vector2.zero)
        {
            SetAnimator(MovementDirection);
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(OnAnim), OnAnim);
        HelperUtilities.ValidateCheckNullValue(this, nameof(OnRb), OnRb);
        HelperUtilities.ValidateCheckNullValue(this, nameof(entityFx), entityFx);
    }
#endif
}
