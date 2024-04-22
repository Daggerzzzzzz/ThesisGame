using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D CircleCollider;
    private BoxCollider2D BoxCollider;
    private Rigidbody2D rigidbody;
    private Player player;

    private bool canRotate;
    private bool isReturning;
    
    private Vector2 playerFacingDirection;
    
    private float throwSword;
    private float swordSpeed;
    private static readonly int Rotation = Animator.StringToHash("rotation");
    private static readonly int Back = Animator.StringToHash("back");
    private static readonly int Front = Animator.StringToHash("front");
    private static readonly int Left = Animator.StringToHash("left");
    private static readonly int Right = Animator.StringToHash("right");

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        CircleCollider = GetComponentInChildren<CircleCollider2D>();
        BoxCollider = GetComponentInChildren<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        canRotate = true;
    }

    private void Start()
    {
        Debug.Log("Start");
        if (player.transform.position.x > transform.position.x)
        {
            Debug.Log("Left");
            player.OnAnim.SetBool(Left, true);
        }
        else if (player.transform.position.x < transform.position.x)
        {
            Debug.Log("Right");
            player.OnAnim.SetBool(Right, true);
        }
        else if (player.transform.position.y > transform.position.y)
        {
            Debug.Log("Front");
            player.OnAnim.SetBool(Front, true);
        }
        else if (player.transform.position.y < transform.position.y)
        {
            Debug.Log("Back");
            player.OnAnim.SetBool(Back, true);
        }
    }

    private void Update()
    {
        if (canRotate)
        {
            rigidbody.velocity = playerFacingDirection.normalized * throwSword;
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
        CircleCollider.enabled = false;
        BoxCollider.enabled = false;
        
        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = other.transform;
        anim.SetBool(Rotation, false);
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
        ResetAnimator();
    }
    
    private void ResetAnimator()
    {
        player.OnAnim.SetBool(Left, false);
        player.OnAnim.SetBool(Right, false);
        player.OnAnim.SetBool(Front, false);
        player.OnAnim.SetBool(Back, false);
    }
}
