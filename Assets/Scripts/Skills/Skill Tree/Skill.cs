using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    
    protected float cooldownTimer;

    protected float tempCooldown;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.Instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            Debug.Log("Cooldown: " + cooldown);
            UseSkill();
            cooldownTimer = cooldown;
            Debug.Log("Skill Used");
            return true;
        }
        Debug.Log("Skill is on cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        
    }
}
