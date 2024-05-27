using System;
using System.Collections;
using UnityEngine;

public class GetsugaController : MonoBehaviour
{
    [SerializeField] 
    private int damage;
    
    private Animator anim;
    private EntityStats entityStats;

    private float moveX;
    private float moveY;
    
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
        if (other.CompareTag("Enemy"))
        {
            entityStats = other.GetComponent<EntityStats>();
            PlayerManager.Instance.player.GetComponent<EntityStats>().DoDamage(entityStats, gameObject);
        }
    }

    public void SetupGetsuga(float moveX, float moveY)
    {
        this.moveX = moveX;
        this.moveY = moveY;
    }
    
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
