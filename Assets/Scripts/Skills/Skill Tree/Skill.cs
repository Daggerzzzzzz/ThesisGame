using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;

    private float cooldownTimer;
    private bool alreadyPlaySound;

    public float tempCooldown;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.Instance.player;
        
        CheckUnlocked();
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        return false;
    }

    protected virtual void UseSkill()
    {
        
    }

    protected virtual void CheckUnlocked()
    {
        
    }
}
