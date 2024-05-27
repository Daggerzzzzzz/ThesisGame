using System.Collections;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    private Animator anim;
    
    private static readonly int Open = Animator.StringToHash("Open");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger(Open);
            StartCoroutine(DestroyChest());
        }
    }

    private IEnumerator DestroyChest()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
