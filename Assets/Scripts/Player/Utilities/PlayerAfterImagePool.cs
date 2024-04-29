using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : SingletonMonoBehavior<PlayerAfterImagePool>
{
    [SerializeField]
    private GameObject afterImagePrefab;
    private Queue<GameObject> availableObjects = new();

    private void Start()
    {
        GrowPool();
    }

    private void GrowPool()
    {
        for(int i = 0; i < 10; i++)
        {
            var instanceToAdd = Instantiate(afterImagePrefab, transform, true);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance) 
    {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }
    public GameObject GetFromPool()
    {
        if(availableObjects.Count == 0)
        {
            GrowPool();
        }
        var instance = availableObjects.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
