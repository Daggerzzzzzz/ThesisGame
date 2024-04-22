using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rigidbody;
    private Player player;

    private bool canRotate;
    private bool isReturning;
    private bool canChangeSwordPosition;
    
    private Vector2 playerFacingDirection;
    
    private float throwSword;
    private float swordSpeed;
    
    private static readonly int Rotation = Animator.StringToHash("rotation");
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    private static readonly int Stuck = Animator.StringToHash("stuck");

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        circleCollider = GetComponentInChildren<CircleCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        canRotate = true;
    }

    private void Start()
    {
        canChangeSwordPosition = true;
    }

    private void Update()
    {
        if (canRotate)
        {
            rigidbody.velocity = playerFacingDirection.normalized * throwSword;
        }

        if (canChangeSwordPosition)
        {
            if (player.animatorDirection != Vector2.zero)
            {
                anim.SetFloat(MoveX, player.animatorDirection.x);
                anim.SetFloat(MoveY, player.animatorDirection.y);
                canChangeSwordPosition = false;
            }
        }
        
        
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, swordSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.ClearNewSword();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning)
        {
            return;
        }
        
        canRotate = false;
        circleCollider.enabled = false;
        
        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        
        transform.parent = other.transform;
        anim.SetBool(Rotation, false);
        anim.SetBool(Stuck, true);
    }
    
    public void SetupSword(Vector2 movementDirection, float throwForce, float returnSpeed, Player player)
    {
        this.player = player;
        throwSword = throwForce;
        swordSpeed = returnSpeed;
        playerFacingDirection = movementDirection;
        rigidbody.velocity = movementDirection;
        anim.SetBool(Rotation, true);
    }

    public void ReturnSword()
    {
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
        anim.SetBool(Stuck, false);
        ResetAnimator();
    }
    
    private void ResetAnimator()
    {
        canChangeSwordPosition = true;
    }
}
