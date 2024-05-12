using UnityEngine;

public class KunaiSkillController : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D circleCollider;
    private Player player;

    private bool canExplode;
    private bool canGrow;
    private bool canTeleport;
    private bool canMultiStack;
    
    private float speedOfGrowth = 3;

    private bool pressedTwice;

    private static readonly int CanExplode = Animator.StringToHash("canExplode");
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        canTeleport = true;
    }

    public void SetupKunai(bool canExplode, bool canTeleport,bool pressedTwice, bool canMultiStack, Player player)
    {
        this.canExplode = canExplode;
        this.canTeleport = canTeleport;
        this.pressedTwice = pressedTwice;
        this.canMultiStack = canMultiStack;
        this.player = player;
    }

    private void Update()
    {
        Debug.Log(pressedTwice);
        
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), speedOfGrowth * Time.deltaTime);
        }

        if (canTeleport && pressedTwice)
        {
            KunaiExplosion();
        }

        if (canExplode && pressedTwice)
        {
            KunaiExplosion();
        }

        if (canMultiStack && pressedTwice)
        {
            KunaiExplosion();
        }
    }

    public void KunaiExplosion()
    {
        if (canExplode || canMultiStack)
        {
            canGrow = true;
            anim.SetBool(CanExplode, true);
        }
        else
        {
            KunaiDestroy();
        }
    }
    
    public void KunaiDestroy()
    {
        Destroy(gameObject);
    }

    public void ExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);

        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Debug.Log(hit.name);
                player.OnEntityStats.StatusAilments(hit.GetComponent<EntityStats>());
            }
        }
    }
}
