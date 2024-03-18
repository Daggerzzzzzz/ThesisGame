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
            var instanceToAdd = Instantiate(afterImagePrefab);
            instanceToAdd.transform.SetParent(transform); //set this gameobject as a child to the gameobject where the scrip is attached to
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance) //public to access this function by singleton
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
