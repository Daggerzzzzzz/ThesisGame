using System.Collections;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    private EntityStats entityStats;
    
    private float lavaDamageTimer;
    private float lavaDamageCooldown = 0.5f;

    private void Start()
    {
        StartCoroutine(DestroyLava());
    }

    private void Update()
    {
        lavaDamageTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            entityStats = other.GetComponent<EntityStats>();
            PlayerManager.Instance.player.GetComponent<EntityStats>().DoDamage(entityStats, gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (lavaDamageTimer < 0)
            {
                Debug.Log("Damaging Enemy");
                entityStats = other.GetComponent<EntityStats>();
                PlayerManager.Instance.player.GetComponent<EntityStats>().DoDamage(entityStats, gameObject);
                lavaDamageTimer = lavaDamageCooldown;
            }
        }
    }

    private IEnumerator DestroyLava()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
