using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    [field: SerializeField]
    public UnityEvent<bool> OnAttack { get; set; }

    [SerializeField]
    private bool isAttacking;
    [SerializeField]
    private SwordHitBox swordHitBox;
    [SerializeField]
    private int swordDamage = 1;

    //private PlayerInput playerInput;

    private bool attackOnce = false;

    Vector2 attackDirection;

    private void Awake() 
    {
        //playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }
    private void Start() 
    {
       swordHitBox.StopAttack(); 
    }
    private void PlayerAttack() 
    {
        if (isAttacking)
        {
            StartAttacking(); 
        }
        OnAttack?.Invoke(isAttacking);
    }

    public void TryAttaking()
    {
        isAttacking = true;
        PlayerAttack();
    }
    public void StopAttacking()
    {
        isAttacking = false;
        PlayerAttack();
    }
    private void StartAttacking()
    {
        DisableInput();
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            if (attackDirection.x > 0)
            {
                swordHitBox.AttackRight();
            }
            else
            {
                swordHitBox.AttackLeft();
            }
        }
        else
        {
            if (attackDirection.x != 0)
            {
                if (attackDirection.x > 0)
                {
                    swordHitBox.AttackRight();
                }
                if (attackDirection.x < 0)
                {
                    swordHitBox.AttackLeft();
                }
            }
            else if (attackDirection.y > 0)
            {
                swordHitBox.AttackUp();
            }
            else
            {
                swordHitBox.AttackDown();
            }
        }
        StartCoroutine(AttackDelay());
    }
    public void PlayerDirection(Vector2 movementInput)
    {
        if (movementInput != Vector2.zero)
        {
            attackDirection = movementInput.normalized;
        }
    }
    void DisableInput()
    { 
        //playerInput.enabled = false;
    }
    void EnableInput()
    {
        //playerInput.enabled = true;
    }
    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.7f);
        swordHitBox.StopAttack();
        EnableInput();
    }
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            if(!attackOnce)
            {
                HitObstacle();
                attackOnce = true;
            }
            else
            {
                return;
            }
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(!attackOnce)
            {
                var hittable = collision.GetComponent<IHittable>();
                hittable?.GetHit(swordDamage, gameObject);
                HitEnemy();
                attackOnce = true;
            }
            else
            {
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        attackOnce = false;
    }

    private void HitEnemy()
    {
        //Debug.Log("Hitting Enemy");
    }

    private void HitObstacle()
    {
        //Debug.Log("Hitting Obstacle");
    }
}
