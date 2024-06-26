using UnityEngine;

public class SlashController : MonoBehaviour
{
    private Player player;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            player.OnEntityStats.DoDamage(other.GetComponent<EntityStats>(), gameObject);
        }
    }

    public void DeleteSlash()
    {
        Destroy(gameObject);
    }
    
    public void SetupSwordSlash(Player player)
    {
        this.player = player;
    }
}
