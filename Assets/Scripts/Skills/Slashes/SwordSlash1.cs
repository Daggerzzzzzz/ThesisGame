using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash1 : MonoBehaviour
{
    [field:SerializeField]
    public List<Transform> targets { get; private set; } = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!targets.Contains(other.transform))
            {
                other.GetComponent<Enemy>().Damage();
            }
        }
    }

    public void DeleteSlash()
    {
        Destroy(gameObject);
    }
}
