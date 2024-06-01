using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] 
    private GameObject dropPrefab;
    [SerializeField] 
    private ItemDataSO[] possibleDrop;
    [SerializeField] 
    private List<ItemDataSO> dropList = new();
    
    public virtual void GenerateDrop()
    {
        foreach (var drop in possibleDrop)
        {
            float randomValue = UnityEngine.Random.Range(0f, 1f);
            if (randomValue <= drop.dropChance)
            {
                dropList.Add(drop);
            }
        }

        for (int i = 0; i < dropList.Count; i++)
        {
            DropItem(dropList[i]);
        }
    }
    
    private void DropItem(ItemDataSO itemDataSo)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, quaternion.identity);
        newDrop.GetComponent<ItemObject>().SetupItem(itemDataSo);
    }
}
