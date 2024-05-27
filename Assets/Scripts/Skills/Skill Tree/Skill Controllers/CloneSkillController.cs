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
    private SpriteRenderer srCloneAttackCheck;
    private Animator anim;

    private Vector2 dashDirection;
    private Vector2 hitBoxDirection;
    
    private static readonly int AttackNumber = Animator.StringToHash("attackNumber");
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");

    private void Awake()
    {
        srClone = GetComponent<SpriteRenderer>();
        srCloneAttackCheck = attackCheck.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (dashDirection != Vector2.zero)
        {
            cloneAttackDirection = dashDirection;
        }
       
        attackCheck.transform.localPosition = hitBoxDirection;
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        
        anim.SetFloat(MoveX, cloneAttackDirection.x);
        anim.SetFloat(MoveY, cloneAttackDirection.y);

        if (cloneTimer < 0)
        {
            srClone.color = new Color(1, 1, 1, srClone.color.a - (Time.deltaTime * colorVanishingSpeed));
            srCloneAttackCheck.color = new Color(1, 1, 1, srCloneAttackCheck.color.a - (Time.deltaTime * colorVanishingSpeed));
            if (srClone.color.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetUpClone(Vector2 _newTransform, float _cloneDuration, bool canAttack, Vector2 _dashDirection, Vector2 _hitBoxDirection)
    {
        if (canAttack)
        {
            anim.SetInteger(AttackNumber, 1);
        }
        transform.position = _newTransform;
        cloneTimer = _cloneDuration;
        dashDirection = _dashDirection;
        hitBoxDirection = _hitBoxDirection;
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
            if (hit.CompareTag("Enemy"))
            {
                PlayerManager.Instance.player.OnEntityStats.DoDamage(hit.GetComponent<EntityStats>(), gameObject);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        if (attackCheck != null)
        {
            Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
        }
    }
}
