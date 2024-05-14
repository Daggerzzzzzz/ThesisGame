using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class PlayerDropItem : MonoBehaviour
{
    [SerializeField] 
    private GameObject dropItemPrefab;
    
    [Header("Weapon Prefab")]
    [SerializeField] 
    private GameObject zangetsu;
    [SerializeField] 
    private GameObject ryujinJakka;
    [SerializeField] 
    private GameObject gonryomaru;
    
    [Header("Armor Prefab")]
    [SerializeField] 
    private GameObject warmogs;
    [SerializeField] 
    private GameObject thornmail;
    [SerializeField] 
    private GameObject guardianAngel;
    [SerializeField]
    private GameObject targetGameObject;
    [SerializeField]
    private List<ItemObject> equipmentsGameObjects = new ();
    [SerializeField]
    private List<GameObject> gameObjectsToRemove = new ();

    private void Start()
    {
        targetGameObject = GameObject.FindGameObjectWithTag("Starting Center");
        equipmentsGameObjects = targetGameObject.GetComponentsInChildren<ItemObject>().ToList();
        RemoveMatchingGameObjects();
    }
    
    private void RemoveMatchingGameObjects()
    {
        foreach (InventoryItem equipment in Inventory.Instance.loadedEquipments)
        {
            foreach (ItemObject equipmentItemObject in equipmentsGameObjects)
            {
                if (equipmentItemObject.ItemDataSo.itemName == equipment.itemDataSo.itemName)
                {
                    gameObjectsToRemove.Add(equipmentItemObject.gameObject);
                }
            }
        }
        
        foreach (GameObject gameObjectToRemove in gameObjectsToRemove)
        {
            Destroy(gameObjectToRemove);
        }
        
        gameObjectsToRemove.Clear();
        equipmentsGameObjects.Clear();
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
        if (equipmentDataSo.equipmentType == EquipmentType.ARMOR)
        {
            switch (equipmentDataSo.itemName)
            {
                case "Warmogs":
                    EquipmentDropMethod(equipmentDataSo, warmogs);
                    break;
                case "Thornmail":
                    EquipmentDropMethod(equipmentDataSo, thornmail);
                    break;
                case "Guardian Angel":
                    EquipmentDropMethod(equipmentDataSo, guardianAngel);
                    break;
            }
        }
        else if (equipmentDataSo.equipmentType == EquipmentType.WEAPON)
        {
            switch (equipmentDataSo.itemName)
            {
                case "Zangetsu":
                    EquipmentDropMethod(equipmentDataSo, zangetsu);
                    break;
                case "Ryujin Jakka":
                    EquipmentDropMethod(equipmentDataSo, ryujinJakka);
                    break;
                case "Gonryomaru":
                    EquipmentDropMethod(equipmentDataSo, gonryomaru);
                    break;
            }
        }
    }

    private void EquipmentDropMethod(EquipmentDataSO equipmentDataSo, GameObject dropEquipmentPrefab)
    {
        GameObject newDrop = Instantiate(dropEquipmentPrefab, equipmentDataSo.position, quaternion.identity);
        newDrop.transform.parent = targetGameObject.transform;
        newDrop.GetComponent<ItemObject>().SetupItem(equipmentDataSo);
    }
}
