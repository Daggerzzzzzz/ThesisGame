public class PlayerStats : EntityStats
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        player.DamageEffect();
    }

    protected override void EntityDeath()
    {
        base.EntityDeath();
        player.EntityDeath();
    }

    protected override void ApplyStatusAilments(bool burn, bool freeze, bool shock)
    {
        playerAttack = true;
        
        base.ApplyStatusAilments(burn, freeze, shock);
    }
}
