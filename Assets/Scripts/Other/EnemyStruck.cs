using UnityEngine;

public class EnemyStruck : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.4f);
    }
}
