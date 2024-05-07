using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerDropItem : MonoBehaviour
{
    [SerializeField] 
    private GameObject dropItemPrefab;
    
    [SerializeField] 
    private GameObject dropEquipmentPrefab;
    
    private GameObject targetGameObject;

    private void Start()
    {
        targetGameObject = GameObject.Find("Starting Center");
    }
    
    public void GenerateDrop()
    {
        Inventory inventory = Inventory.Instance;
        List<InventoryItem> currentStash = inventory.stash;

        foreach (InventoryItem stash in currentStash)
        {
            DropItem(stash.itemDataSo);
        }
    }
    
    public void DropItem(ItemDataSO itemDataSo)
    {
        GameObject newDrop = Instantiate(dropItemPrefab, transform.position, quaternion.identity);
        newDrop.GetComponent<ItemObject>().SetupItem(itemDataSo);
    }
    
    public void DropEquipment(EquipmentDataSO equipmentDataSo)
    {
        GameObject newDrop = Instantiate(dropEquipmentPrefab, equipmentDataSo.position, quaternion.identity);
        newDrop.transform.parent = targetGameObject.transform;
        newDrop.GetComponent<ItemObject>().SetupItem(equipmentDataSo);
    }
}
