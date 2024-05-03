using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] 
    private GameObject dropPrefab;
    [SerializeField] 
    private ItemDataSO itemDataSo;

    public void DropItem()
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, quaternion.identity);
        newDrop.GetComponent<ItemObject>().SetupItem(itemDataSo);
    }
}
